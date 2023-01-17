using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.Interfaces;
using NEWSHORE_UI.Models;
using NEWSHORE_UI.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NEWSHORE_AIR.DataAccess;

namespace NEWSHORE_UI.Controllers
{
  public class HomeController : Controller
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;
    private readonly IAPI_Get _api_Get;
    public HomeController(IConfiguration configuration, IAPI_Get api_Get, ILogger<HomeController> logger)
    {
      _configuration = configuration;
      _api_Get = api_Get;
      _logger = logger;
    }
    public string Message { get; set; }

    /// <summary>
    /// Index Home
    /// </summary>
    /// <param></param>
    /// <param></param>
    /// <returns>Vista NEWSHORE</returns>
    public IActionResult Index()
    {
      ActionResult result = null;
      try
      {
        string url = _configuration["Rutas:MultipyRetorno"];
        var origenes = _api_Get.Origins(url).ToArray();
        //List<SelectListItem> origenes = new List<SelectListItem>();
        var destinos = _api_Get.Destinations().ToArray();
        List<SelectListItem> origenes_sli = new List<SelectListItem>();
        List<SelectListItem> destinos_sli = new List<SelectListItem>();
        origenes_sli = DataAccess.Data.Origins(origenes);
        destinos_sli = Data.Destinations(destinos);
        ViewBag.origenes = origenes_sli;
        ViewBag.destinos = destinos_sli;
        result = View();
      }
      catch (Exception ex)
      {
        Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        _logger.LogInformation(Message);
        result = NotFound();
      }
      return result;
    }

    [HttpPost]
    public IActionResult Index(string? origin, string? destination)
    {
      ActionResult result = null;
      try
      {
        Journeyy lstViaje = new Journeyy();
        lstViaje = (Journeyy)Business.SearchJourney.Journeys(origin, destination);
        result = View();
      }
      catch (Exception ex)
      {
        Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        _logger.LogInformation(Message);
        result = NotFound();
      }
      return result;
    }
    /// <summary>
    /// Busca origen y destino del viaje
    /// </summary>
    /// <param origin="origin">Origen Viaje</param>
    /// <param destination="destination">Destino Viaje</param>
    /// <returns>Retorna price y flightNumber </returns>
    public IActionResult CalcularRuta(string origin, string destination)
    {
      Journeyy lstViaje = new Journeyy();
      lstViaje = (Journeyy)Business.SearchJourney.Journeys(origin, destination);
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
