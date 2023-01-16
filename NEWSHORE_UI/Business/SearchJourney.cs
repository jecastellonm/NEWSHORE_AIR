using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.DataAccess;
using NEWSHORE_AIR.Interfaces;
using NEWSHORE_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Business
{
  public class SearchJourney
  {
    private static IConfiguration _configuration;
    private static ILogger<SearchJourney> _logger;
    private static IAPI_Get _api_Get;
    public SearchJourney(IConfiguration configuration, IAPI_Get api_Get, ILogger<SearchJourney> logger)
    {
      _configuration = configuration;
      _api_Get = api_Get;
      _logger = logger;
    }
    public static string Message { get; set; }

    public static Journeyy Journeys(string? origen = null, string? destino = null)
    {
      try
      {
        Journeyy journey = new Journeyy();
        journey = (Journeyy)_api_Get.Rutas(origen, destino);
        return journey;
      }
      catch (Exception ex)
      {
        Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        _logger.LogInformation(Message);
      }
      return null;
    }

  }
}
