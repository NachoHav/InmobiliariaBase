using InmobiliariaBase.Models;
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
    public class UsuarioController : Controller {

        private readonly RepositorioUsuario repositorioUsuario;

        private readonly IConfiguration configuration;

    public UsuarioController(IConfiguration configuration)
    {
        repositorioUsuario = new RepositorioUsuario(configuration);
        this.configuration = configuration;
    }

        // GET: UsuarioController
        public ActionResult Index()
        {
            try
            {
                var lista = repositorioUsuario.ObtenerTodos();
                return View(lista);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Crear()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Usuario usuario)
        {
            try
            {
                repositorioUsuario.Alta(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Ocurrio un error " + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Editar(int id)
        {
            var usuario = repositorioUsuario.ObtenerPorId(id);
            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Usuario usuario)
        {
            try
            {
                usuario.Id = id;
                repositorioUsuario.Modificacion(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Eliminar(int id)
        {
            repositorioUsuario.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
