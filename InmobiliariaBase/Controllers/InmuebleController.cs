using InmobiliariaBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
                repositorioInmueble.Alta(inmueble);                
                return RedirectToAction(nameof(Index));                
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Editar(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.Obtener();
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

                return View(inm);
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
    }
}
