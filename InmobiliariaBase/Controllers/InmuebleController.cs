using InmobiliariaBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly IConfiguration configuration;
        private readonly RepositorioPropietario repositorioPropietario;

        public InmuebleController(IConfiguration configuration)
        {
            repositorioPropietario = new RepositorioPropietario(configuration);
            repositorioInmueble = new RepositorioInmueble(configuration);
            this.configuration = configuration;
        }
        // GET: InmuebleController
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Error"];
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            var lista = repositorioInmueble.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InmuebleController/Create
        public ActionResult Crear()
        {
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            ViewBag.Propietarios = repositorioPropietario.Obtener();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Inmueble inmueble)
        {
            try
            {
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                repositorioInmueble.Alta(inmueble);       
                
                return RedirectToAction(nameof(Index));                
            }
            catch
            {
                TempData["Error"] = "Error, no se pudo crear el Inmueble.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Editar(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.Obtener();
            ViewBag.Tipos = Inmueble.ObtenerTipos();
            var inmueble = repositorioInmueble.ObtenerInmueble(id);
            return View(inmueble);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Inmueble inm)
        {
            try
            {
                inm.Id = id;


                repositorioInmueble.Modificar(inm);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {

                TempData["Error"] = "Error, no se pudo editar el Inmueble.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InmuebleController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id)
        {
            repositorioInmueble.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult InmueblePropietario(int id)
        {
            try
            {
                var lista = repositorioInmueble.ObtenerPorPropietario(id);
                return View("Index", lista);
            }
            catch (SqlException ex)
            {

                TempData["Error"] = "Error, no se pudo obtener el Inmueble.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult ObtenerPorFiltro(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var list = repositorioInmueble.ObtenerXFiltro(fechaDesde, fechaHasta);
                return View("Index", list);
            }
            catch (SqlException ex)
            {

                TempData["Error"] = "Error, no se pudo obtener el Inmueble." +
                    " Error en las fechas " + ex.Message.ToString();
                return RedirectToAction(nameof(Index));
            }

        }

    }
}
