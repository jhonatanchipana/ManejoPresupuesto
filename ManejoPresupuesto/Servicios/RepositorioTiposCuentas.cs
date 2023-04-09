using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Exists(string nombre, int usuarioId, int id = 0);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {

            using (var con = new SqlConnection(connectionString))
            {
                var id = await con.QuerySingleAsync<int>(@"TiposCuentas_Insertar", 
                                                        new {usuarioId = tipoCuenta.UsuarioId, nombre =tipoCuenta.Nombre},
                                                        commandType: CommandType.StoredProcedure);
                tipoCuenta.Id = id;
            }
        }

        public async Task<bool> Exists(string nombre, int usuarioId, int id = 0)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas 
                                                                    WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId
                                                                            and Id <> @id;",
                                                                    new {nombre, usuarioId, id});
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var listado = await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre , Orden 
                                                            FROM TiposCuentas 
                                                            WHERE UsuarioId = @UsuarioId
                                                            ORDER BY Orden", new { usuarioId});
            return listado;
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            //execute asynce: solo ejecuta no trae nada
            await connection.ExecuteAsync(@"update tiposcuentas set nombre = @nombre where Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var tipoCuenta = await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"select id,nombre,orden 
                                                                from tiposcuentas 
                                                                where Id = @Id and UsuarioId = @UsuarioId"
                                                                , new {id, usuarioId});
            return tipoCuenta;
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"delete tiposcuentas where id = @id", new {id});
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados)
        {
            var query = "update tiposcuentas set orden = @Orden where Id = @Id";
            var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tipoCuentasOrdenados);

        }
    }
}
