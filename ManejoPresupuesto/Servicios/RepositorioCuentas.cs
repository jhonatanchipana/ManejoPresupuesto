using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(Cuenta cuenta);
        Task Borrar(int id);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }

    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string ConnectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(ConnectionString);
            var id = await connection.QueryFirstAsync<int>(
                            @"insert into cuentas (Nombre, TipoCuentaId, Balance, Descripcion) 
                            values (@Nombre,@TipoCuentaId,@Balance,@Descripcion);
                            select scope_identity();", cuenta);

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(ConnectionString);
            return await connection.QueryAsync<Cuenta>(@"SELECT 
                                            c.Id , c.Nombre , c.Balance, tc.Nombre as TipoCuenta
                                            FROM Cuentas c
                                            inner join TiposCuentas tc ON c.TipoCuentaId = tc.Id 
                                            WHERE tc.UsuarioId = @usuaioId
                                            ORDER BY tc.Orden ; ", new { usuaioId = usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(ConnectionString);

            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"SELECT 
                                            c.Id , c.Nombre , c.Balance, Descripcion, c.tipoCuentaId
                                            FROM Cuentas c
                                            inner join TiposCuentas tc ON c.TipoCuentaId = tc.Id 
                                            WHERE tc.UsuarioId = @UsuarioId and c.Id = @Id; ", new { Id = id, UsuarioId = usuarioId});
        }

        public async Task Actualizar(Cuenta cuenta)
        {
            using var connection = new SqlConnection(ConnectionString);

            await connection.ExecuteAsync(@"update cuentas
                                            set Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion, TipoCuentaId = @TipoCuentaId
                                            where Id = @Id", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.ExecuteAsync("delete cuentas where id = @id", new { id });
        }

    }
}
