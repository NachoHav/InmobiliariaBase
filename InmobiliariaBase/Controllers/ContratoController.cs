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
    public class ContratoController : Controller
    {

        private readonly RepositorioContrato repositorioContrato;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly IConfiguration configuration;


        public ContratoController(IConfiguration configuration)
        {
            repositorioContrato = new RepositorioContrato(configuration);
            repositorioInmueble = new RepositorioInmueble(configuration);
            repositorioInquilino = new RepositorioInquilino(configuration);
            this.configuration = configuration;
        }

        // GET: ContratoController
        public ActionResult Index()
        {
            var lista = repositorioContrato.ObtenerTodos();
            return View(lista);
        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContratoController/Create
        public ActionResult Crear()
        {
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Contrato contrato)
        {
            try
            {
                repositorioContrato.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        public ActionResult Editar(int id)
        {
            var contrato = repositorioContrato.ObtenerContrato(id);
            ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
            return View(contrato);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                var contrato = repositorioContrato.ObtenerContrato(id);
                repositorioContrato.Modificar(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id)
        {
            repositorioContrato.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: ContratoController/Delete/5
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
