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
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly IConfiguration configuration;

        public InquilinoController(IConfiguration configuration)
        {
            repositorioInquilino = new RepositorioInquilino(configuration);
            this.configuration = configuration;
        }
        // GET: InquilinoController
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Error"];
            var lista = repositorioInquilino.ObtenerTodos();

            return View(lista);
        }



        // GET: InquilinoController/Create
        public ActionResult Crear()
        {
            
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Inquilino inquilino)
        {
            try
            {
                repositorioInquilino.Alta(inquilino);
                TempData["Id"] = inquilino.Id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Error, no se pudo crear el Inquilino.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Editar(int id)
        {
            var inquilino = repositorioInquilino.ObtenerInquilino(id);  
            return View(inquilino);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                Inquilino inquilino = repositorioInquilino.ObtenerInquilino(id);
                inquilino.Nombre = collection["Nombre"];
                inquilino.Apellido = collection["Apellido"];
                inquilino.Dni = collection["Dni"];
                inquilino.Telefono = collection["Telefono"];
                inquilino.Email = collection["Email"];
                
                repositorioInquilino.Modificar(inquilino);
                TempData["Mensaje"] = "Datos guardados correctamente";

                return RedirectToAction(nameof(Index));                
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Error, no se pudo editar el Inquilino.";
                return RedirectToAction(nameof(Index));
                ViewBag.StackTrate = ex.StackTrace;
                   
            }
        }

        // GET: InquilinoController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id)
        {

            try
            {
                repositorioInquilino.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Ocurrio un error " + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: InquilinoController/Delete/5
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
