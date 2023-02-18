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
using Flight = NEWSHORE_AIR.DataAccess.Flight;
using Microsoft.EntityFrameworkCore;

namespace NEWSHORE_UI.Controllers
{
  public class HomeController : Controller
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;
    private readonly IAPI_Get _api_Get;
    private readonly JourneyysContext _context;
    private DbSet<NEWSHORE_UI.Models.Journeys> _dbSet;

    public HomeController(IConfiguration configuration, IAPI_Get api_Get, ILogger<HomeController> logger
                              , JourneyysContext context)
    {
      _configuration = configuration;
      _api_Get = api_Get;
      _logger = logger;
      _context = context;
      _dbSet = _context.Set<NEWSHORE_UI.Models.Journeys>();
    }
    public string Message { get; set; }
    Journeyy ViajeIDA = new Journeyy();
    Journeyy ViajeVUELTA = new Journeyy();
    List<NEWSHORE_UI.Models.Journeys> Viajes = new List<NEWSHORE_UI.Models.Journeys>();
    List<NEWSHORE_UI.Models.Journeys> travelDBInOut = new List<NEWSHORE_UI.Models.Journeys>();
    List<NEWSHORE_UI.Models.Journeys> travelDBIn = new List<NEWSHORE_UI.Models.Journeys>();
    List<NEWSHORE_UI.Models.Journeys> travelDBOut = new List<NEWSHORE_UI.Models.Journeys>();
    NEWSHORE_UI.Models.Journeys travelIDA = new NEWSHORE_UI.Models.Journeys();
    NEWSHORE_UI.Models.Journeys travelVUELTA = new NEWSHORE_UI.Models.Journeys();

    int contTransport = 0;
    int contTransportReturn = 0;
    private bool savedJourneyFlight = false;

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
          if (travelDB != null)
            //travelDBInOut.Add(new NEWSHORE_UI.Models.Journeys
            travelDBIn.Add(new NEWSHORE_UI.Models.Journeys
            {
              Destination = travelDB.Destination,
              Origin = travelDB.Origin,
              Flights = travelDB.Flights,
              JourneyyFlights = travelDB.JourneyyFlights,
              Price = travelDB.Price
            });
          travelDB = GetbyIdOriginDestination(destination, origin);
          if (travelDB != null)
            travelDBOut.Add(new NEWSHORE_UI.Models.Journeys
            {
              Destination = travelDB.Destination,
              Origin = travelDB.Origin,
              Flights = travelDB.Flights,
              JourneyyFlights = travelDB.JourneyyFlights,
              Price = travelDB.Price
            });

          if (travelDBIn == null || travelDBIn.Count == 0 && travelDBOut == null || travelDBOut.Count == 0)
          {
            if (travelDBIn == null || travelDBIn.Count == 0)
            {
              ViajeIDA = _api_Get.Rutas(origin, destination);

              //NEWSHORE_UI.Models.Journeys travelIDA = new NEWSHORE_UI.Models.Journeys();
              travelIDA.Flights = new List<Models.Flight>();
              travelIDA.JourneyyFlights = new List<Models.JourneyyFlight>();

              List<Models.Transport> transport = new List<Models.Transport>();
              //Models.Flight flight = new Models.Flight();

              foreach (Flight v in ViajeIDA.Flights)
              {
                transport.Add(new Models.Transport
                {
                  flightCarrier = v.Transport.flightCarrier,
                  flightNumber = v.Transport.flightNumber
                });
                travelIDA.Flights.Add(new Models.Flight
                {
                  Origin = v.Origin,
                  Destination = v.Destination,
                  Price = v.Price,
                  Transport = transport[contTransport++] //(Models.Transport)v.Flights.Select(o=>o.Transport)
                });
                //travelIDA.JourneyyFlights.Add(new Models.JourneyyFlight
                //{
                //  Flight = (Models.Flight)travelIDA.Flights,
                //  DateAddEdit = DateTime.Now
                //});
                //travelIDA.JourneyyFlights.Add(new Models.JourneyyFlight
                //{

                //});
              }
              travelIDA.Origin = ViajeIDA.Origin;
              travelIDA.Destination = ViajeIDA.Destination;
              travelIDA.Price = ViajeIDA.Price;

              Viajes.Add(new NEWSHORE_UI.Models.Journeys
              {
                Destination = travelIDA.Destination,
                Origin = travelIDA.Origin,
                Flights = travelIDA.Flights,
                JourneyyFlights = travelIDA.JourneyyFlights,
                Price = travelIDA.Price
              });
              travelDBIn.Add(travelIDA);
            }
            else
            {
              Viajes.Add(new NEWSHORE_UI.Models.Journeys
              {
                Destination = travelDBIn[0].Destination,
                Origin = travelDBIn[0].Origin,
                Flights = travelDBIn[0].Flights,
                JourneyyFlights = travelDBIn[0].JourneyyFlights,
                Price = travelDBIn[0].Price
              });
            }

            if (travelDBOut == null || travelDBOut.Count == 0)
            {
              ViajeVUELTA = _api_Get.RutasRegreso(destination, origin);
              //Models.Flight flight2 = new Models.Flight();
              travelVUELTA.Flights = new List<Models.Flight>();
              travelVUELTA.JourneyyFlights = new List<Models.JourneyyFlight>();

              List<Models.Transport> transport2 = new List<Models.Transport>();

              foreach (Flight v in ViajeVUELTA.Flights)
              {
                transport2.Add(new Models.Transport
                {
                  flightCarrier = v.Transport.flightCarrier,
                  flightNumber = v.Transport.flightNumber
                });
                travelVUELTA.Flights.Add(new Models.Flight
                {
                  Origin = v.Origin,
                  Destination = v.Destination,
                  Price = v.Price,
                  Transport = transport2[contTransportReturn++] //(Models.Transport)v.Flights.Select(o=>o.Transport)
                });
                //travelVUELTA.JourneyyFlights.Add(new Models.JourneyyFlight
                //{
                //  Flight = (Models.Flight)travelVUELTA.Flights,
                //  DateAddEdit = DateTime.Now
                //});
                //travelVUELTA.JourneyyFlights.Add(new Models.JourneyyFlight
                //{

                //});
              }

              travelVUELTA.Origin = ViajeVUELTA.Origin;
              travelVUELTA.Destination = ViajeVUELTA.Destination;
              travelVUELTA.Price = ViajeVUELTA.Price;

              Viajes.Add(new NEWSHORE_UI.Models.Journeys
              {
                Destination = travelVUELTA.Destination,
                Origin = travelVUELTA.Origin,
                Flights = travelVUELTA.Flights,
                JourneyyFlights = travelVUELTA.JourneyyFlights,
                Price = travelVUELTA.Price
              });
              travelDBOut.Add(travelVUELTA);
            }
            else
            {
              Viajes.Add(new NEWSHORE_UI.Models.Journeys
              {
                Destination = travelDBOut[0].Destination,
                Origin = travelDBOut[0].Origin,
                Flights = travelDBOut[0].Flights,
                JourneyyFlights = travelDBOut[0].JourneyyFlights,
                Price = travelDBOut[0].Price
              });
            }


            var resultExist = GetbyIdOriginDestination(origin, destination);
            var resultExistVuelta = GetbyIdOriginDestination(destination, origin);
            if (resultExist == null) { Task<bool> tI = Add(travelIDA); }
            if (resultExistVuelta == null) { Task<bool> tV = Add(travelVUELTA); }
            return View(Viajes);
          }
          else if ((travelDBIn != null || travelDBIn.Count == 0) || (travelDBOut != null || travelDBOut.Count == 0))
          {
            if ((travelDBIn != null || travelDBIn.Count == 0)) Viajes.AddRange(travelDBIn);
            if ((travelDBOut != null || travelDBOut.Count == 0)) Viajes.AddRange(travelDBOut);
            //Viajes.Add(new NEWSHORE_UI.Models.Journeys
            //{
            //  Destination = travelDB.Destination,
            //  Origin = travelDB.Origin,
            //  Flights = travelDB.Flights,
            //  JourneyyFlights = travelDB.JourneyyFlights,
            //  Price = travelDB.Price
            //});

            return View(Viajes);
          }
          else
          {
            //return View(ViajeIDA);
            return View(Viajes);
          }
        }
        return View(Viajes);
      }
      catch (Exception ex)
      {
        Message = $"Index HomeC-ontroller Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        _logger.LogError(Message);
        return View(Viajes);
      }
    }

    public async Task<bool> Add(NEWSHORE_UI.Models.Journeys journeyy)
    {
      try
      {
        if (journeyy.Flights.Count > 0)
        {
          bool journey1 = _dbSet
                                .Include(jf => jf.Flights)
                                  .ThenInclude(jf => jf.Transport)
                                .Include(jf => jf.JourneyyFlights)
                                  .ThenInclude(jf => jf.Flight)
                                      .ThenInclude(jf => jf.Transport)
                                .AsNoTracking()
                                .Any(e => e.Origin == journeyy.Origin && e.Destination == journeyy.Destination);
          if (journey1)
            return false;
          _dbSet.Add(journeyy);
          bool saved = _context.SaveChanges() > 0;
          if (saved)
          {
            Models.Journeys viajeIda = _context.Journeyys
                        .Include(jf => jf.JourneyyFlights)
                          .ThenInclude(jf => jf.Flight)
                            .ThenInclude(jf => jf.Transport)
                        .Include(jf => jf.Flights)
                          .ThenInclude(jf => jf.Transport)
                         .AsNoTracking()
                         .FirstOrDefault(jf => jf.Origin == journeyy.Origin && jf.Destination == journeyy.Destination);
            bool existI = _dbSet.Include(jf => jf.JourneyyFlights).ThenInclude(jf => jf.Flight)
                              .Any(jf => jf.Origin == journeyy.Origin && jf.Destination == journeyy.Destination);
            var journeyI = _dbSet.FirstOrDefault(e => e.Origin == journeyy.Origin && e.Destination == journeyy.Destination);
            Models.Journeys viajeVuelta = _context.Journeyys
                        .Include(jf => jf.JourneyyFlights)
                          .ThenInclude(jf => jf.Flight)
                            .ThenInclude(jf => jf.Transport)
                        .Include(jf => jf.Flights)
                          .ThenInclude(jf => jf.Transport)
                         .AsNoTracking()
                         .FirstOrDefault(jf => jf.Origin == journeyy.Destination && jf.Destination == journeyy.Origin);
            bool existV = _dbSet.Include(jf => jf.JourneyyFlights).ThenInclude(jf => jf.Flight)
                              .Any(jf => jf.Origin == journeyy.Destination && jf.Destination == journeyy.Origin);
            var journeyV = _dbSet.FirstOrDefault(e => e.Origin == journeyy.Destination && e.Destination == journeyy.Origin);

            if (viajeIda != null && existI && journeyI != null && viajeVuelta != null && existV && journeyV != null)
            {
              Models.JourneyyFlight journeyyFlights = new Models.JourneyyFlight();
              travelIDA.JourneyyFlights = new List<Models.JourneyyFlight>();

              foreach (Models.Flight vI in viajeIda.Flights)
              {
                journeyyFlights.JourneyID = viajeIda.ID;
                journeyyFlights.FlightID = vI.ID;
                journeyyFlights.DateAddEdit = DateTime.Now;
                _context.JourneyyFilghts.Add(journeyyFlights);
                bool savedJourneyFlight = _context.SaveChanges() > 0;
              }
              foreach (Models.Flight vV in viajeVuelta.Flights)
              {
                journeyyFlights.JourneyID = viajeVuelta.ID;
                journeyyFlights.FlightID = vV.ID;
                journeyyFlights.DateAddEdit = DateTime.Now;
                _context.JourneyyFilghts.Add(journeyyFlights);
                bool savedJourneyFlight = _context.SaveChanges() > 0;
              }
              //bool savedJourneyFlight = await _context.SaveChangesAsync() > 0;
              return savedJourneyFlight;
            }
          }
          return true;
        }
        return false;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error en {nameof(Add)}: " + ex.Message);
      }
      return false;
    }

    public NEWSHORE_UI.Models.Journeys GetbyIdOriginDestination(string origin, string destination)
    {
      var viaje = _context.Journeyys
                    .Include(jf => jf.JourneyyFlights)
                      .ThenInclude(jf => jf.Flight)
                        .ThenInclude(jf => jf.Transport)
                    .Include(jf => jf.Flights)
                      .ThenInclude(jf => jf.Transport)
                     .AsNoTracking()
                     .FirstOrDefault(jf => jf.Origin == origin && jf.Destination == destination);
      bool exist = _dbSet.Include(jf => jf.JourneyyFlights).ThenInclude(jf => jf.Flight)
                        .Any(jf => jf.Origin == origin && jf.Destination == destination);
      var journey = _dbSet.FirstOrDefault(e => e.Origin == origin && e.Destination == destination);
      if (viaje != null)
        return viaje;
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
