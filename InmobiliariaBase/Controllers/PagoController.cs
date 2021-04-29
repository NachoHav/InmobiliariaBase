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
    public class PagoController : Controller
    {
        private readonly RepositorioPago repositorioPago;
        private readonly RepositorioContrato repositorioContrato;
        private readonly IConfiguration configuration;

        public PagoController(IConfiguration configuration)
        {
            repositorioPago = new RepositorioPago(configuration);
            repositorioContrato = new RepositorioContrato(configuration);
            this.configuration = configuration;
        }
        // GET: PagoController
        public ActionResult Index()
        {

            ViewBag.Error = TempData["Error"];
            return View();
        }

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagoController/Create
        public ActionResult Crear(int id)
        {

          
            /*            ViewBag.ContratoId = id;*/
          
            //ViewData["Contrato"] = repositorioContrato.ObtenerContrato(id);
            ViewBag.Contrato = repositorioContrato.ObtenerContrato(id);
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(int id, Pago pago)
        {
            try
            {
                pago.Id = id;

                repositorioPago.Alta(pago);
                var list = repositorioPago.ObtenerTodos(pago.IdContrato);
                ViewBag.Contrato = repositorioContrato.ObtenerContrato(id);
                ViewBag.ContratoId = id;


                return View("Index", list);
            }
            catch
            {
                TempData["Error"] = "Error, no se creo el pago.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PagoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PagoController/Edit/5
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

        // GET: PagoController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id, int idC)
        {
            try
            {                 
                repositorioPago.Baja(id);
                var list = repositorioPago.ObtenerTodos(idC);
                ViewBag.ContratoId = idC;
                return View("Index", list);
            }
            catch (Exception ex)
            {

                TempData["Error"] = "Error, no se eliminó el pago.";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: PagoController/Delete/5
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

        public ActionResult ObtenerPorContrato(int id)
        {
            try
            {
                var list = repositorioPago.ObtenerTodos(id);
                ViewBag.Contrato = repositorioContrato.ObtenerContrato(id);
                ViewBag.ContratoId = id;
                //ViewBag.ContratoId = id;*/

                //ViewData["Contrato"] = repositorioContrato.ObtenerContrato(id);


                return View("Index", list);
            }
            catch (Exception)
            {

                TempData["Error"] = "Error, no se obtuvo el pago.";
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
