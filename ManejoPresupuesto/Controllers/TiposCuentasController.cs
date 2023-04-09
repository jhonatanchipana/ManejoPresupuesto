using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas _repositorioTiposCuentas;
        private readonly IServicioUsuarios _servicioUsuario;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas
            ,IServicioUsuarios servicioUsuario)
        {
            _repositorioTiposCuentas = repositorioTiposCuentas;
            _servicioUsuario = servicioUsuario;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var result = await _repositorioTiposCuentas.Obtener(usuarioId);

            return View(result);
        }

        public IActionResult Crear()
        {
            return View();      
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid) return View(tipoCuenta);

            tipoCuenta.UsuarioId = _servicioUsuario.ObtenerUsuarioId();

            var yaExisteTipoCuenta = await _repositorioTiposCuentas.Exists(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El Nombre {tipoCuenta.Nombre} ya existe");
                return View(tipoCuenta);
            }
            
            await _repositorioTiposCuentas.Crear(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var tipoCuentaExiste = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuentaExiste is null) return RedirectToAction("NoEncontrado", "Home");

            return View(tipoCuentaExiste);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var tipoCuentaExiste = await _repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);
            
            if (tipoCuentaExiste is null) return RedirectToAction("NoEncontrado", "Home");

            await _repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var tipoCuenta= await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null) return RedirectToAction("NoEncontrado", "Home");

            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var tipoCuentaExiste = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuentaExiste is null) return RedirectToAction("NoEncontrado", "Home");

            await _repositorioTiposCuentas.Borrar(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre, int id)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await _repositorioTiposCuentas.Exists(nombre, usuarioId, id);

            if (yaExisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var tiposCuentas = await _repositorioTiposCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id);

            var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if(idsTiposCuentasNoPertenecenAlUsuario.Count > 0)
            {
                return Forbid();
            }

            var tiposCuentasOrdenados = ids.Select((valor, indice) =>
                new TipoCuenta()
                {
                    Id = valor, Orden = indice + 1
                }).AsEnumerable();

            await _repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);

            return Ok();
        }

    }
}
