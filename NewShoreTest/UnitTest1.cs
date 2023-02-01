using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.API;
using NEWSHORE_UI.Controllers;
using NEWSHORE_UI.DataAccess;
using NUnit.Framework;
using System;
using System.Net.Http;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace NewShoreTest
{
  [TestFixture]
  public class UnitTest1
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<API_Get> _logger;
    private readonly JourneyysContext _context;
    public UnitTest1(JourneyysContext context)
    {
      _context = context;
    }


    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CalcularDirecto_Retorna_MZL_MDE()
    {
      Journey journey = new Journey();
      Transport transport = new Transport();
      journey.Destination = "MDE";
      journey.Origin = "MZL";
      journey.Price = 200;
      transport.flightCarrier = "";
      transport.flightNumber = "";
      journey.Flights.Add(new Flight
      {
        Transport = transport,
        Origin = "",
        Destination = "",
        Price = 200
      });

      var apiGet = new API_Get(_configuration, _logger);
      Assert.AreEqual(journey, apiGet.RutasRegreso("MZL", "MDE"));
    }

    public void CalcularUnaEscala_Retorna_CTG_BOG_MAD()
    {
      Journey journey = new Journey();
      Transport transport = new Transport();
      journey.Destination = "BOG";
      journey.Origin = "MDE";
      journey.Price = 200;
      transport.flightCarrier = "CO";
      transport.flightNumber = "8009";
      journey.Flights.Add(new Flight
      {
        Transport = transport,
        Origin = "MDE",
        Destination = "CTG",
        Price = 200
      });
      transport.flightCarrier = "CO";
      transport.flightNumber = "8009";
      journey.Flights.Add(new Flight
      {
        Transport = transport,
        Origin = "CTG",
        Destination = "BOG",
        Price = 200
      });

      var apiGet = new API_Get(_configuration, _logger);
      Assert.AreEqual(journey, apiGet.Rutas("MED", "BOG"));
    }

    public void Calcular2Escalas_Retorna()
    {
      Journey journey = new Journey();
      Transport transport = new Transport();
      journey.Destination = "BCN";
      journey.Origin = "BOG";
      journey.Price = 700;
      transport.flightCarrier = "CO";
      transport.flightNumber = "8010";
      journey.Flights.Add(new Flight
      {
        Transport = transport,
        Origin = "BOG",
        Destination = "CTG",
        Price = 200
      });
      transport.flightCarrier = "CO";
      transport.flightNumber = "9009";
      journey.Flights.Add(new Flight
      {
        Transport = transport,
        Origin = "CTG",
        Destination = "MDE",
        Price = 200
      });
      transport.flightCarrier = "CO";
      transport.flightNumber = "8004";
      journey.Flights.Add(new Flight
      {
        Transport = transport,
        Origin = "MDE",
        Destination = "BCN",
        Price = 200
      });

      var apiGet = new API_Get(_configuration, _logger);
      Assert.AreEqual(journey, apiGet.Rutas("BOG", "BCN"));
    }
  }
}
