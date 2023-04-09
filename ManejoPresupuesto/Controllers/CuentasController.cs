using AutoMapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas _repositorioTiposCuentas;
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly IRepositorioCuentas _repositorioCuentas;
        private readonly IRepositorioTransacciones _repositorioTransacciones;
        private readonly IServicioReporte _servicioReporte;
        private readonly IMapper _mapper;

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
            IServicioUsuarios servicioUsuarios,
            IRepositorioCuentas repositorioCuentas,
            IRepositorioTransacciones repositorioTransacciones,
            IServicioReporte servicioReporte,
            IMapper mapper)
        {
            _repositorioTiposCuentas = repositorioTiposCuentas;
            _servicioUsuarios = servicioUsuarios;
            _repositorioCuentas = repositorioCuentas;
            _repositorioTransacciones = repositorioTransacciones;
            _servicioReporte = servicioReporte;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await _repositorioCuentas.Buscar(usuarioId);

            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(x => new IndiceCuentasViewModel
                {
                    TipoCuenta = x.Key,
                    Cuentas = x.AsEnumerable()
                }).ToList();

            return View(modelo);
        }

        public async Task<IActionResult> Detalle(int id, int mes, int año)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id , usuarioId);

            if(cuenta is null) return View("NoEncontrado", "Home");          

            ViewBag.Cuenta = cuenta.Nombre;

            var modelo = await _servicioReporte
                .ObtenerReporteTransaccionesDetallasPorCuenta(usuarioId, id, mes, año, ViewBag);

            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _repositorioTiposCuentas.Obtener(usuarioId);
            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = await ObtenerTipoCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = _repositorioTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return View("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTipoCuentas(usuarioId);
                return View(cuenta);
            }

            await _repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return View("NoEncontrado", "Home");
            }

            var modelo = _mapper.Map<CuentaCreacionViewModel>(cuenta);

            modelo.TiposCuentas = await ObtenerTipoCuentas(usuarioId);

            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if (cuenta is null)
            {
                return View("NoEncontrado", "Home");
            }

            var tipoCuenta = await _repositorioTiposCuentas.ObtenerPorId(cuentaEditar.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return View("NoEncontrado", "Home");
            }

            await _repositorioCuentas.Actualizar(cuentaEditar);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null) return RedirectToAction("NoEncontrado", "Home");

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null) return RedirectToAction("NoEncontrado", "Home");

            await _repositorioCuentas.Borrar(id);
            return RedirectToAction("Index");
        }


        private async Task<IEnumerable<SelectListItem>> ObtenerTipoCuentas(int usuarioId)
        {
            var tiposCuentas = await _repositorioTiposCuentas.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

       
    }
}
