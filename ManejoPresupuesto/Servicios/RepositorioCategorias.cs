using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Borrar(int id);
        Task<int> Contar(int usuarioId);
        Task Crear(Categoria categoria);
        Task Editar(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, PaginacionViewModel paginacion);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectioString;

        public RepositorioCategorias(IConfiguration configuration) 
        {
            connectioString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Contar(int usuarioId)
        {
            using var connection = new SqlConnection(connectioString);
            return await connection.ExecuteScalarAsync<int>(@"select count(*) from Categorias
                                                                where UsuarioId = @usuarioId", new { usuarioId });

        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectioString);
            return await connection.QueryAsync<Categoria>
                (@$" SELECT Id, Nombre, TipoOperacionId, UsuarioId
                    FROM Categorias
                    WHERE UsuarioId = @usuarioId
                    ORDER BY Nombre
                    OFFSET {paginacion.RecordsAsaltar} Rows fetch next {paginacion.RecordsPorPagina}
                    ROWS ONLY   "
,                   new { usuarioId });            
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectioString);
            return await connection.QueryAsync<Categoria>(@"SELECT Id, Nombre, TipoOperacionId, UsuarioId
                                                                FROM Categorias
                                                                WHERE UsuarioId = @usuarioId AND TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId });
        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectioString);
            var id = await connection.QueryFirstOrDefaultAsync<int>(@"INSERT INTO Categorias
                                                        (Nombre, TipoOperacionId, UsuarioId)
                                                        VALUES(@Nombre, @TipoOperacionId, @UsuarioId);
                                                        select scope_identity();", categoria);
            categoria.Id= id;
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectioString);
            return await connection.QueryFirstAsync<Categoria>(@"select * from Categorias
                                                                where Id = @id and usuarioId = @usuarioid", new { id, usuarioId });
        }

        public async Task Editar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectioString);
            await connection.ExecuteAsync(@"update Categorias 
                                            set Nombre = @Nombre, TipoOperacionId = @TipoOperacionId, UsuarioId = @UsuarioId
                                            where id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectioString);
            await connection.ExecuteAsync(@"delete from Categorias where id = @id", new { id });
        }

    }
}
