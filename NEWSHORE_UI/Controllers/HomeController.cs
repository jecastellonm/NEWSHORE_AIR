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

namespace NEWSHORE_UI.Controllers
{
  public class HomeController : Controller
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;
    private readonly IAPI_Get _api_Get;
    private readonly JourneyysContext _context;
    public HomeController(IConfiguration configuration, IAPI_Get api_Get, ILogger<HomeController> logger
                              , JourneyysContext context)
    {
      _configuration = configuration;
      _api_Get = api_Get;
      _logger = logger;
      _context = context;
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
          var viewModelJourneysDb = new JourneyIndexData();
          viewModelJourneysDb.JourneysDb = _context.Journeyys;
          var journeysDbs = _context.Journeyys;
          var flight = _context.Flights;
          var transport = _context.Transports;
          var travelDB = (from o in journeysDbs
                          where (o.Destination == destination && o.Origin == origin)
                          select o).ToList();
          var travelDB2 = journeysDbs.Where(t => t.Origin == origin && t.Destination == destination).ToList();
          if (travelDB2.Count() == 0)
          {
            return View(Viajes);
          }
          else if (travelDB.Count() < 0)
          {
            List<Flight> flights = new List<Flight>();
            List<Transport> transports = new List<Transport>();

            travelDB2.ForEach(e => transports.Add(
                new Transport()
                {
                  flightCarrier = e.Origin,
                  flightNumber = e.Destination
                }));
            travelDB2.ForEach(e => flights.Add(
                new Flight()
                {
                  Transport = transports[0],
                  Origin = e.Origin,
                  Destination = e.Destination,
                  Price = e.Price
                }));
            travelDB2.ForEach(e => Viajes.Add(
              new Journeyy()
              {
                Flights = (ICollection<Flight>)flights[0],
                Origin = e.Origin,
                Destination = e.Destination,
                Price = e.Price
              }));
              return View(Viajes);
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
