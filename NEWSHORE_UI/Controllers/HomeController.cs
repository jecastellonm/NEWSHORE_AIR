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
using Transport = NEWSHORE_AIR.DataAccess.Transport;
using Flight = NEWSHORE_AIR.DataAccess.Flight;
using Microsoft.EntityFrameworkCore;
using Journeys = NEWSHORE_AIR.DataAccess.Journeys;

namespace NEWSHORE_UI.Controllers
{
  public class HomeController : Controller
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;
    private readonly IAPI_Get _api_Get;
    private readonly JourneyysContext _context;
    private DbSet<Journeyy> _dbSet;

    public HomeController(IConfiguration configuration, IAPI_Get api_Get, ILogger<HomeController> logger
                              , JourneyysContext context)
    {
      _configuration = configuration;
      _api_Get = api_Get;
      _logger = logger;
      _context = context;
      _dbSet = _context.Set<Journeyy>();
    }
    public string Message { get; set; }
    Journeyy ViajeIDA = new Journeyy();
    Journeyy ViajeVUELTA = new Journeyy();
    List<Journeyy> Viajes = new List<Journeyy>();

    /// <summary>
    /// Index Home
    /// </summary>
    /// <param></param>
    /// <param></param>
    /// <returns>Vista NEWSHORE</returns>
    public IActionResult Index(string? origin, string? destination)
    {
      try
      {

        string url = _configuration[Constants.RUTA_NIVEL_2];
        var origenes = _api_Get.Origins(url).ToArray();
        var destinos = _api_Get.Destinations().ToArray();
        List<SelectListItem> origenes_sli = new List<SelectListItem>();
        List<SelectListItem> destinos_sli = new List<SelectListItem>();
        origenes_sli = DataAccess.Data.Origins(origenes);
        destinos_sli = DataAccess.Data.Destinations(destinos);
        ViewBag.origenes = origenes_sli;
        ViewBag.destinos = destinos_sli;
        if (origin is not null && destination is not null && origin != destination)
        {
          var travelDB = GetbyIdOriginDestination(origin, destination);

          if (travelDB == null)
          {
            return View(Viajes);
          }
          else if (travelDB != null)
          {
            return View(travelDB);
          }
          else
          {
            ViajeIDA = _api_Get.Rutas(origin, destination);
            Viajes.Add(new Journeyy
            {
              Destination = ViajeIDA.Destination,
              Origin = ViajeIDA.Origin,
              Flights = ViajeIDA.Flights,
              Price = ViajeIDA.Price
            });
            ViajeVUELTA = _api_Get.RutasRegreso(destination, origin);
            Viajes.Add(new Journeyy
            {
              Destination = ViajeVUELTA.Destination,
              Origin = ViajeVUELTA.Origin,
              Flights = ViajeVUELTA.Flights,
              Price = ViajeVUELTA.Price
            });
            var resultExist = GetbyIdOriginDestination(origin, destination);
            if (resultExist == null)
            {
              Add(ViajeIDA);
              Add(ViajeVUELTA);
            }
          }
          //return View(ViajeIDA);
          return View(Viajes);
        }
        return View(Viajes);
      }
      catch (Exception ex)
      {
        Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        _logger.LogError(Message);
        return View(Viajes);
      }
    }

    public async Task<bool> Add(Journeyy journeyy)
    {
      try
      {
        bool journey1 = _dbSet.Any(e => e.Origin == journeyy.Origin && e.Destination == journeyy.Destination);
        if (journey1)
          return false;
        _dbSet.Add(journeyy);
        return await _context.SaveChangesAsync() > 0;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error en {nameof(Add)}: " + ex.Message);
      }
      return false;
    }

    public Journeyy GetbyIdOriginDestination(string origin, string destination)
    {
      bool exist = _dbSet.Any(e => e.Origin == origin && e.Destination == destination);
      var journey = _dbSet.FirstOrDefault(e => e.Origin == origin && e.Destination == destination);
      if (journey != null)
        return journey;
      return null;
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
      lstViaje = _api_Get.RutasRegreso(origin, destination);
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
