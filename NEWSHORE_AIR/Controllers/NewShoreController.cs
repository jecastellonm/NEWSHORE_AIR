using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.API;
using NEWSHORE_AIR.DataAccess;
using NEWSHORE_AIR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.Controllers
{
  [ApiController]
  [Route("[controller]")]

  public class NewShoreController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<NewShoreController> _logger;
    private readonly IAPI_Get _api_Get;
    //API_Get newshore_api = new API_Get();
    public NewShoreController(IAPI_Get api_Get, IConfiguration configuration, ILogger<NewShoreController> logger)
    {
      _api_Get = api_Get;
      _configuration = configuration;
      _logger = logger;
    }
    public string Message { get; set; }

    [HttpGet]
    [Route("Get_NewShoreAir")]
    public IEnumerable<ResponseNewShoreAPI> Get_NewShoreAir()
    {
      string url = _configuration["Rutas:MultipyRetorno"];
      dynamic response = _api_Get.Get(url);
      //if (response != null)
      //{
      List<ResponseNewShoreAPI> responseNewShoreList = new List<ResponseNewShoreAPI>();
      responseNewShoreList.AddRange(response);
      Message = $"Get NewSHoreAirAPI Successfull {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);
      return responseNewShoreList; //(IEnumerable<ResponseNewShoreAPI>)
                                   //}
                                   //return null;
    }

    [HttpGet]
    [Route("Origin")]
    public IEnumerable<string> Origin()
    {
      List<string> origins = _api_Get.Origins().ToList();
      Message = $"Get Origin Successfull {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);

      return origins.ToList();
    }

    [HttpGet]
    [Route("Destination")]
    public IEnumerable<string> Destination()
    {
      List<string> destinations = _api_Get.Destinations().ToList();
      Message = $"Get Destination Successfull {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);
      return destinations.ToList();
    }

    [HttpGet]
    [Route("Rutas")]
    public Journeyy Rutas()
    {
      Journeyy Rutass = _api_Get.Rutas();
      Message = $"Get Rutas Successfull {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);
      return Rutass;
    }

    [HttpGet]
    [Route("RutasRegreso")]
    public Journeyy RutasRegreso()
    {
      Journeyy RutasRegreso = _api_Get.RutasRegreso();
      Message = $"Get RutasRegreso Successfull {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);
      return RutasRegreso;
    }

  }
}
