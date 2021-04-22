﻿using InmobiliariaBase.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaBase.Controllers
{
    [Authorize]
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
        [Authorize(Policy = "Admin")]
        public ActionResult Crear(Usuario usuario)
        {
            try
            {
                usuario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                usuario.Rol = 0;
                repositorioUsuario.Alta(usuario);
                TempData["Info"] = "Cuenta creada correctamente";
                return RedirectToAction(nameof(Iniciar));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Error: " + ex.ToString();
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
        [Authorize(Policy = "Admin")]
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

        [AllowAnonymous]
        public ActionResult Iniciar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Iniciar(string email, string clave)
        {
            try
            {
                Usuario usuario = repositorioUsuario.ObtenerPorEmail(email);
 
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));


                    if (usuario.Email == null || usuario.Clave != hashed)
                    {
                        TempData["Error"] = "Error al iniciar sesión.";
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Email),
                        new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                        new Claim(ClaimTypes.Role, usuario.RolNombre),
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    TempData["Info"] = "Inicio de sesión correcto.";
                    return RedirectToAction("Index", "Home");
                
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        [Route("Salir", Name = "Logout")]
        // GET: Usuarios/Logout/
        public async Task<ActionResult> Salir()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
