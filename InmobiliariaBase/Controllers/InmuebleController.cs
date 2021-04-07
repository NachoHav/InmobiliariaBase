using InmobiliariaBase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly IConfiguration configuration;

        public InmuebleController(IConfiguration configuration)
        {
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
                TempData["Id"] = inmueble.Id;
                return RedirectToAction(nameof(Index));
                
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
