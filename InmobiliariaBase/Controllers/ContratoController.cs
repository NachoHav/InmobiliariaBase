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
            ViewBag.Error = TempData["Error"];
            var lista = repositorioContrato.ObtenerTodos();
            var list = new List<Contrato>();

            foreach(var item in lista)
            {
                if(item.FechaHasta < DateTime.Now)
                {
                    repositorioContrato.Baja(item.Id);
                }
                else
                {
                    list.Add(item);
                }
            }


            return View(list);
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
                
                if(contrato.FechaHasta > DateTime.Now && contrato.FechaDesde >= DateTime.Now && contrato.FechaDesde < contrato.FechaHasta && contrato.FechaDesde < contrato.FechaHasta)
                {

                    var lista = repositorioContrato.ObtenerTodos();                    
                    var e = 1;

                    foreach (var item in lista)
                    {
                        if (contrato.InmuebleId == item.InmuebleId && contrato.FechaDesde >= item.FechaDesde && contrato.FechaHasta <= item.FechaHasta)
                        {
                            e = 0;
                        }
                    }
                    
                    if(e == 1)
                    {
                        repositorioContrato.Alta(contrato);
                    }
                    else
                    {
                        TempData["Error"] = "Error, no se puede crear un contrato con esas fechas";
                    }                   

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Error, no se puede crear un contrato con esas fechas";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error, no se pudo crear el contrato";
                return RedirectToAction(nameof(Index));
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
        public ActionResult Editar(int id, Contrato c)
        {
            try
            {
                c.Id = id;

                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();

                repositorioContrato.Modificar(c);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error, no se pudo editar el contrato";
                return RedirectToAction(nameof(Index));
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
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult ContratosInmueble(int id)
        {
            try
            {
                var lista = repositorioContrato.ObtenerPorInmueble(id);
                return View("Index", lista);
            }
            catch (Exception ex)
            {

                TempData["Error"] = "Error, no se pudo obtener los contratos del inmueble";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult ContratosVigentes()
        {
            try
            {
                var list = repositorioContrato.ObtenerVigentes();
                return View("Index", list);
            }
            catch (Exception ex)
            {

                TempData["Error"] = "Error, no se pudo obtener los contratos vigentes";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult ContratosBajas()
        {
            try
            {
                var list = repositorioContrato.ObtenerBajas();
                return View("Index", list);
            }
            catch (Exception ex)
            {

                TempData["Error"] = "Error, no se pudo obtener las Bajas";
                return RedirectToAction(nameof(Index));
            }        
        }

        public ActionResult Activar(int id)
        {
            repositorioContrato.Activar(id);           
            return RedirectToAction(nameof(Index));
        }



        public ActionResult ObtenerPorFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if(fechaDesde < fechaHasta)
                {
                    var list = repositorioContrato.ObtenerXFechas(fechaDesde, fechaHasta);
                    return View("Index", list);
                }
                else
                {
                    TempData["Error"] = "Error en las fechas, la fecha de Inicio debe ser menor que la fecha de Fin";
                    return RedirectToAction(nameof(Index));
                }
                    
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction(nameof(Index));
            }
        }


  

        public ActionResult Cancelar(int id)
        {

            var c = repositorioContrato.ObtenerContrato(id);
            
            var dias = c.FechaHasta - c.FechaDesde;
            var mitad = dias / 2;
            var diasRestantes = c.FechaHasta - DateTime.Now;
            var monto = c.Importe;

            if (mitad.Days > diasRestantes.Days)
            {
                ViewData["Multa"] = monto * 2;
            }
            else
            {
                ViewData["Multa"] = monto;
            }

            ViewData["Contrato"] = c;

            return View("Cancelacion");    
        }

        public ActionResult Cancel(int id)
        {
            repositorioContrato.Cancelar(id);
            return RedirectToAction(nameof(Index));
        }



    }
}
