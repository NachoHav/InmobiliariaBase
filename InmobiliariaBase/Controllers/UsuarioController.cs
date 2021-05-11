using InmobiliariaBase.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaBase.Controllers
{
    [Authorize]
    public class UsuarioController : Controller {

        private readonly RepositorioUsuario repositorioUsuario;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            repositorioUsuario = new RepositorioUsuario(configuration);
            this.environment = environment;
            this.configuration = configuration;
        }

        // GET: UsuarioController
        public ActionResult Index()
        {
            try
            {
                ViewBag.Error = TempData["Error"];
                var lista = repositorioUsuario.ObtenerTodos();
                return View(lista);
            }
            catch (SqlException ex)
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
            try
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
            catch (SqlException ex)
            {

                TempData["Error"] = "Error, no se pudo crear el usuario.";
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Crear(Usuario u)
        {

            if (ModelState.IsValid)
            {
                Usuario usuario = repositorioUsuario.ObtenerPorEmail(u.Email);
                if(usuario.Email != u.Email)
                {
                    try
                    {
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                        u.Clave = hashed;
                        u.Rol = User.IsInRole("Admin") ? u.Rol : (int)Roles.Employee;
                        int res = repositorioUsuario.Alta(u);
                        if (u.AvatarFile != null && u.Id > 0)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "Uploads");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                            string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.Avatar = Path.Combine("/Uploads", fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }
                            repositorioUsuario.Modificacion(u);
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Roles = Usuario.ObtenerRoles();
                        TempData["Error"] = "Error, no se pudo crear el usuario";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    TempData["Error"] = "Error, no se pudo crear el usuario el Email ya esta en uso";
                    return View();
                }
               
            }
            else
            {
                TempData["Error"] = "Error, no se pudo crear el usuario.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UsuarioController/Edit/5
        [Authorize(Policy = "Admin")]
        public ActionResult Editar(int id)
        {
            try
            {
                var usuario = repositorioUsuario.ObtenerPorId(id);
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(usuario);
            }
            catch (Exception)
            {

                TempData["Error"] = "Error, no se pudo editar el usuario.";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Editar(int id, Usuario usuario)
        {
            try
            {
                
                usuario.Id = id;
                ViewBag.Roles = Usuario.ObtenerRoles();
                repositorioUsuario.Modificacion(usuario);
                
                var lista = repositorioUsuario.ObtenerTodos();
                return RedirectToAction(nameof(Index));

            }
            catch (SqlException ex)
            {

                TempData["Error"] = "Error, no se pudo editar el usuario.";
                return RedirectToAction(nameof(Index));

            }
        }

        // GET: UsuarioController/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                repositorioUsuario.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {


                TempData["Error"] = "Error, no se pudo eliminar el usuario.";
                return RedirectToAction(nameof(Index));
            }

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


                if (usuario == null || usuario.Clave != hashed)
                    {

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
                TempData["Error"] = "Error de inicio de sesion";
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
