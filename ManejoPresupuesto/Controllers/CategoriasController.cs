using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias _repositorioCategorias;
        private readonly IServicioUsuarios _servicioUsuarios;

        public CategoriasController(IRepositorioCategorias repositorioCategorias,
            IServicioUsuarios servicioUsuarios)
        {
            _repositorioCategorias = repositorioCategorias;
            _servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(PaginacionViewModel paginacion)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categorias = await _repositorioCategorias.Obtener(usuarioId, paginacion);
            var totalCategorias = await _repositorioCategorias.Contar(usuarioId);

            var respuestaVM = new PaginacionRespuesta<Categoria>
            {
                Elementos = categorias,
                Pagina = paginacion.Pagina,
                RecordsPorPagina = paginacion.RecordsPorPagina,
                CantidadTotalRecords = totalCategorias,
                BaseUrl = Url.Action()
            };

            return View(respuestaVM);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if(!ModelState.IsValid) return View(categoria);

            var id = _servicioUsuarios.ObtenerUsuarioId();
            categoria.UsuarioId = id;

            await _repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoria = await _repositorioCategorias.ObtenerPorId(id, usuarioId);

            if (categoria is null) return RedirectToAction("NoEncontrado", "Index");

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoriaExiste = await _repositorioCategorias.ObtenerPorId(categoria.Id, usuarioId);

            if (categoriaExiste is null) return RedirectToAction("NoEncontrado", "Index");

            var tipoOperacionExiste = Enum.GetValues<TipoOperacion>().Where(x => x == categoria.TipoOperacionId).Any();

            if (!tipoOperacionExiste) return RedirectToAction("NoEncontrado", "Index");

            categoria.UsuarioId = usuarioId;
            await _repositorioCategorias.Editar(categoria);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoria = await _repositorioCategorias.ObtenerPorId(id, usuarioId);

            if (categoria is null) return RedirectToAction("NoEncontrado", "Index");

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var categoriaExiste = await _repositorioCategorias.ObtenerPorId(id, usuarioId);

            if (categoriaExiste is null) return RedirectToAction("NoEncontrado", "Index");

            await _repositorioCategorias.Borrar(id);

            return RedirectToAction("Index");
        }
    }
}
