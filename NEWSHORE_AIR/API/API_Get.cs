using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.DataAccess;
using NEWSHORE_AIR.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
//using static NEWSHORE_AIR.DataAccess.Destination;

namespace NEWSHORE_AIR.API
{
  public class API_Get : IAPI_Get
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<API_Get> _logger;

    //API_Get newshore_api = new API_Get();
    public API_Get(IConfiguration configuration, ILogger<API_Get> logger)
    {
      _configuration = configuration;
      _logger = logger;
    }
    public string Message { get; set; }

    public dynamic Get(string url)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.Credentials = CredentialCache.DefaultCredentials;
      request.Proxy = null;
      try
      {
        HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
        Stream stream = httpWebResponse.GetResponseStream();
        if(httpWebResponse.StatusCode == System.Net.HttpStatusCode.OK)
        {
          StreamReader streamReader = new StreamReader(stream);
          string datos = HttpUtility.HtmlDecode(streamReader.ReadToEnd());
          dynamic data = JsonConvert.DeserializeObject<List<ResponseNewShoreAPI>>(datos);
          return data;
        }
        return null;
      }
      catch
      {
        Message = $"Get NewSHoreAirAPI Fallido {DateTime.UtcNow.ToLongTimeString()}";
        _logger.LogInformation(Message);
        return null;
      }

    }

    public List<string> Origins(string? url = null)
    {
      if (url == null)
        url = _configuration["Rutas:MultipyRetorno"];
      dynamic response = Get(url);
      //if (response != null)
      //{
      List<ResponseNewShoreAPI> responseNewShoreList = new List<ResponseNewShoreAPI>();
      List<Origin> originsList = new List<Origin>();
      //List<Origin> originssList = new List<Origin>();
      responseNewShoreList.AddRange(response);
      responseNewShoreList.ForEach(d => originsList.Add(
          new Origin()
          {
            Station = d.departureStation,
            flightCarrier = d.flightCarrier
          }));
      var originssList = (from o in originsList select o.Station).Distinct().ToList();
      Message = $"Get Origenes Exitoso {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);
      return originssList; //(IEnumerable<ResponseNewShoreAPI>)
                           //}
                           //return null;

    }

    public List<string> Destinations(string? url = null)
    {
      if (url == null)
        url = _configuration["Rutas:MultipyRetorno"];
      dynamic response = Get(url);

      List<ResponseNewShoreAPI> lstviajes = new List<ResponseNewShoreAPI>();
      lstviajes.AddRange(response);
      //if (response != null)
      //{
      List<ResponseNewShoreAPI> responseNewShoreList = new List<ResponseNewShoreAPI>();
      List<Destinations> destinationsList = new List<Destinations>();
      //List<Destinations> destinationssList = new List<Destinations>();
      responseNewShoreList.AddRange(response);
      responseNewShoreList.ForEach(d => destinationsList.Add(
          new Destinations()
          {
            Station = d.departureStation,
            flightCarrier = d.flightCarrier
          }));
      var destinationssList = (from o in destinationsList select o.Station).Distinct().ToList();
      Message = $"Get Origenes Exitoso {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}";
      _logger.LogInformation(Message);
      return destinationssList; //(IEnumerable<ResponseNewShoreAPI>)
                                //}
                                //return null;
    }


    public Journeyy Rutas(string? origen = null, string? destino = null, string? url = null)
    {
      if (url == null)
        url = _configuration["Rutas:MultipyRetorno"];
      dynamic response = Get(url);

      List<ResponseNewShoreAPI> lstviajes = new List<ResponseNewShoreAPI>();
      lstviajes.AddRange(response);
      Journeyy journey = new Journeyy();
      List<Flight> flights = new List<Flight>();
      Transport transport = new Transport();
      double totalPrice0 = 0; double totalPrice1 = 0; double totalPrice2 = 0; double totalPrice3 = 0; double totalPrice4 = 0;

      origen = "MDE"; destino = "BOG";

      var viajessList = (from o in lstviajes
                         where o.departureStation == origen && o.arrivalStation == destino
                         select o.departureStation + " " + o.arrivalStation).ToList();
      var flightCarrierOrigen = lstviajes.Where(x => x.departureStation == origen).Select(x => x.flightCarrier).ToList();
      var flightCarrierDestino = lstviajes.Where(x => x.departureStation == destino).Select(x => x.flightCarrier).ToList();
      var pri_CO = lstviajes.Where(x => x.arrivalStation == destino && x.flightCarrier == "CO").Select(x => x.departureStation).ToList();
      var pri_EXT = lstviajes.Where(x => x.arrivalStation == destino).Select(x => x.departureStation).ToList();
      var pri0 = pri_CO.Count() > 0 ?
          (lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_CO[0] && x.flightCarrier == "CO")
                              .Select(x => x.arrivalStation).ToList()) : null;
      var pri1 = pri_CO.Count() > 1 ?
              lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_CO[1] && x.flightCarrier == "CO")
                                  .Select(x => x.arrivalStation).ToList() : null;
      var pri3 = pri_EXT.Count() > 0 ?
          (lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_EXT[0])
                              .Select(x => x.arrivalStation).ToList()) : null;
      var pri4 = pri_EXT.Count() > 1 ?
              lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_EXT[1])
                                  .Select(x => x.arrivalStation).ToList() : null;

      totalPrice0 = flightCarrierOrigen[0] == "CO" && flightCarrierDestino[0] == "CO" ?
        (double)(from o in lstviajes
                 where (o.arrivalStation == destino && o.flightCarrier == "CO")
                 select o.price).FirstOrDefault()
        : (double)(from o in lstviajes
                   where (o.arrivalStation == destino)
                   select o.price).FirstOrDefault();


      if (viajessList.Count() == 0)
      {
        if (pri0.Count() > 0 && pri0 is not null || pri1.Count() > 0 && pri1 is not null || pri0.Count() > 0 && pri0 is not null || pri1.Count() > 0 && pri1 is not null)
        {
          if (flightCarrierOrigen[0] == "CO" && flightCarrierDestino[0] == "CO")
          {
            if (pri0.Count() > 0)
            {
              var pri_0 = (from o in lstviajes
                           where (o.departureStation == pri0[0] && o.arrivalStation == destino)
                           select o).ToList();
              pri_0.ForEach(e => transport.flightCarrier = e.flightCarrier);
              pri_0.ForEach(e => transport.flightNumber = e.flightNumber);

              pri_0.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));
              var pri_1 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri0[0])
                           select o).ToList();
              pri_1.ForEach(d => transport.flightCarrier = d.flightCarrier);
              pri_1.ForEach(d => transport.flightNumber = d.flightNumber);

              pri_1.ForEach(d => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = d.departureStation,
                    Destination = d.arrivalStation,
                    Price = d.price
                  }));
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri0[0])
                                     select o.price).Sum();
              if (pri0.Count() > 1)
              {
                var pri_00 = (from o in lstviajes
                              where (o.departureStation == pri0[1] && o.arrivalStation == destino)
                              select o).ToList();
                pri_00.ForEach(e => transport.flightCarrier = e.flightCarrier);
                pri_00.ForEach(e => transport.flightNumber = e.flightNumber);

                pri_00.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));
                var pri_2 = (from o in lstviajes
                             where (o.departureStation == origen && o.arrivalStation == pri0[1])
                             select o).ToList();
                pri_2.ForEach(d => transport.flightCarrier = d.flightCarrier);
                pri_2.ForEach(d => transport.flightNumber = d.flightNumber);

                pri_2.ForEach(d => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = d.departureStation,
                      Destination = d.arrivalStation,
                      Price = d.price
                    }));
                totalPrice2 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == pri0[1])
                                       select o.price).Sum();

              }
            }
            if (pri1.Count() > 0)
            {
              var pri_0 = (from o in lstviajes
                           where (o.departureStation == pri1[0] && o.arrivalStation == destino)
                           select o).ToList();
              pri_0.ForEach(e => transport.flightCarrier = e.flightCarrier);
              pri_0.ForEach(e => transport.flightNumber = e.flightNumber);

              pri_0.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));
              var pri_1 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri1[0])
                           select o).ToList();
              //select o.departureStation + " " + o.arrivalStation).ToList();
              pri_1.ForEach(d => transport.flightCarrier = d.flightCarrier);
              pri_1.ForEach(d => transport.flightNumber = d.flightNumber);

              pri_1.ForEach(d => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = d.departureStation,
                    Destination = d.arrivalStation,
                    Price = d.price
                  }));
              //journey.Flights.Add(flights);
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri1[0])
                                     select o.price).Sum();

              if (pri1.Count() > 1)
              {
                var pri_00 = (from o in lstviajes
                              where (o.departureStation == pri1[1] && o.arrivalStation == destino)
                              select o).ToList();
                pri_00.ForEach(e => transport.flightCarrier = e.flightCarrier);
                pri_00.ForEach(e => transport.flightNumber = e.flightNumber);

                pri_00.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));

                var pri_2 = (from o in lstviajes
                             where (o.departureStation == origen && o.arrivalStation == pri1[1])
                             select o).ToList();
                pri_2.ForEach(d => transport.flightCarrier = d.flightCarrier);
                pri_2.ForEach(d => transport.flightNumber = d.flightNumber);

                pri_2.ForEach(d => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = d.departureStation,
                      Destination = d.arrivalStation,
                      Price = d.price
                    }));
                totalPrice2 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == pri1[1])
                                       select o.price).Sum();
              }
            }
          }
          else
          {
            if (pri3.Count() > 0)
            {
              var pri_0 = (from o in lstviajes
                           where (o.departureStation == pri3[0] && o.arrivalStation == destino)
                           select o).ToList();
              pri_0.ForEach(e => transport.flightCarrier = e.flightCarrier);
              pri_0.ForEach(e => transport.flightNumber = e.flightNumber);

              pri_0.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));

              var pri_1 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri3[0])
                           select o).ToList();
              pri_1.ForEach(d => transport.flightCarrier = d.flightCarrier);
              pri_1.ForEach(d => transport.flightNumber = d.flightNumber);

              pri_1.ForEach(d => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = d.departureStation,
                    Destination = d.arrivalStation,
                    Price = d.price
                  }));
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri3[0])
                                     select o.price).Sum();
              if (pri3.Count() > 1)
              {
                var pri_00 = (from o in lstviajes
                              where (o.departureStation == pri3[1] && o.arrivalStation == destino)
                              select o).ToList();
                pri_00.ForEach(e => transport.flightCarrier = e.flightCarrier);
                pri_00.ForEach(e => transport.flightNumber = e.flightNumber);

                pri_00.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));

                var pri_2 = (from o in lstviajes
                             where (o.departureStation == origen && o.arrivalStation == pri3[1])
                             select o).ToList();
                pri_2.ForEach(d => transport.flightCarrier = d.flightCarrier);
                pri_2.ForEach(d => transport.flightNumber = d.flightNumber);

                pri_2.ForEach(d => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = d.departureStation,
                      Destination = d.arrivalStation,
                      Price = d.price
                    }));
                totalPrice2 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == pri3[1])
                                       select o.price).Sum();

              }
            }

            if (pri4.Count() > 0)
            {
              var pri_0 = (from o in lstviajes
                           where (o.departureStation == pri4[0] && o.arrivalStation == destino)
                           select o).ToList();
              pri_0.ForEach(e => transport.flightCarrier = e.flightCarrier);
              pri_0.ForEach(e => transport.flightNumber = e.flightNumber);

              pri_0.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));

              var pri_1 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri4[0])
                           select o).ToList();
              //select o.departureStation + " " + o.arrivalStation).ToList();
              pri_1.ForEach(d => transport.flightCarrier = d.flightCarrier);
              pri_1.ForEach(d => transport.flightNumber = d.flightNumber);

              pri_1.ForEach(d => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = d.departureStation,
                    Destination = d.arrivalStation,
                    Price = d.price
                  }));
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri4[0])
                                     select o.price).Sum();
              if (pri4.Count() > 1)
              {
                var pri_00 = (from o in lstviajes
                              where (o.departureStation == pri4[1] && o.arrivalStation == destino)
                              select o).ToList();
                pri_00.ForEach(e => transport.flightCarrier = e.flightCarrier);
                pri_00.ForEach(e => transport.flightNumber = e.flightNumber);

                pri_00.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));

                var pri_2 = (from o in lstviajes
                             where (o.departureStation == origen && o.arrivalStation == pri4[1])
                             select o).ToList();
                pri_2.ForEach(d => transport.flightCarrier = d.flightCarrier);
                pri_2.ForEach(d => transport.flightNumber = d.flightNumber);

                pri_2.ForEach(d => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = d.departureStation,
                      Destination = d.arrivalStation,
                      Price = d.price
                    }));
                totalPrice2 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == pri4[1])
                                       select o.price).Sum();
              }
            }
          }
        }
        else
        {
          if (flightCarrierOrigen[0] == "CO" && flightCarrierDestino[0] == "CO")
          {
            var sdo_i_0 = (from o in lstviajes
                           where o.departureStation == origen || o.arrivalStation == pri_CO[0]
                           select o.arrivalStation)
                             .Intersect((from o in lstviajes
                                         where o.departureStation == origen || o.arrivalStation == pri_CO[0]
                                         select o.departureStation)).ToList();

            if (sdo_i_0.Count() > 0)
            {
              var sdo_0 = (from o in lstviajes
                            where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                    || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_CO[0]))
                            select o).ToList();
              sdo_0.ForEach(e => transport.flightCarrier = e.flightCarrier);
              sdo_0.ForEach(e => transport.flightNumber = e.flightNumber);
              sdo_0.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));
              var sdo_1 = (from o in lstviajes
                                where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                        || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_CO[0])
                                select o).ToList();
              sdo_1.ForEach(e => transport.flightCarrier = e.flightCarrier);
              sdo_1.ForEach(e => transport.flightNumber = e.flightNumber);
              sdo_1.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));
              totalPrice3 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                             || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_CO[0])
                                     select o.price).Sum();
              if (sdo_i_0.Count() > 1)
              {
                var sdo_00 = (from o in lstviajes
                              where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                      || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_CO[1]))
                              select o).ToList();
                sdo_00.ForEach(e => transport.flightCarrier = e.flightCarrier);
                sdo_00.ForEach(e => transport.flightNumber = e.flightNumber);
                sdo_00.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));

                var sdo_2 = (from o in lstviajes
                                  where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                          || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_CO[1]))
                                  select o).ToList();
                sdo_2.ForEach(e => transport.flightCarrier = e.flightCarrier);
                sdo_2.ForEach(e => transport.flightNumber = e.flightNumber);
                sdo_2.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));
                totalPrice4 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                               || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_CO[1])
                                       select o.price).Sum();
              }
            }
          }
          else
          {
            var sdo_i_0 = (from o in lstviajes
                           where o.departureStation == origen || o.arrivalStation == pri_EXT[0]
                           select o.arrivalStation)
                             .Intersect((from o in lstviajes
                                         where o.departureStation == origen || o.arrivalStation == pri_EXT[0]
                                         select o.departureStation)).ToList();
            if (sdo_i_0.Count() > 0)
            {
              var sdo_0 = (from o in lstviajes
                           where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                   || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_EXT[0]))
                           select o).ToList();
              sdo_0.ForEach(e => transport.flightCarrier = e.flightCarrier);
              sdo_0.ForEach(e => transport.flightNumber = e.flightNumber);
              sdo_0.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));
              var sdo_1 = (from o in lstviajes
                                where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                        || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_EXT[0])
                                select o).ToList();
              sdo_1.ForEach(e => transport.flightCarrier = e.flightCarrier);
              sdo_1.ForEach(e => transport.flightNumber = e.flightNumber);
              sdo_1.ForEach(e => flights.Add(
                  new Flight()
                  {
                    Transport = transport,
                    Origin = e.departureStation,
                    Destination = e.arrivalStation,
                    Price = e.price
                  }));
              totalPrice3 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                             || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_EXT[0])
                                     select o.price).Sum();

              if (sdo_i_0.Count() > 1)
              {
                var sdo_00 = (from o in lstviajes
                              where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                      || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[1]))
                              select o).ToList();
                sdo_00.ForEach(e => transport.flightCarrier = e.flightCarrier);
                sdo_00.ForEach(e => transport.flightNumber = e.flightNumber);
                sdo_00.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));
                var sdo_2 = (from o in lstviajes
                                  where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                          || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[1]))
                                  select o).ToList();
                sdo_2.ForEach(e => transport.flightCarrier = e.flightCarrier);
                sdo_2.ForEach(e => transport.flightNumber = e.flightNumber);
                sdo_2.ForEach(e => flights.Add(
                    new Flight()
                    {
                      Transport = transport,
                      Origin = e.departureStation,
                      Destination = e.arrivalStation,
                      Price = e.price
                    }));
                totalPrice4 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                               || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[1])
                                       select o.price).Sum();
              }
            }

          }
        }

        double totalPrice = totalPrice0 + totalPrice1 + totalPrice2 + totalPrice3 + totalPrice4;
        journey.Flights = flights;
        journey.Destination = destino;
        journey.Origin = origen;
        journey.Price = totalPrice;
      }
      else
      {
        totalPrice0 = (double)(from o in lstviajes
                               where (o.departureStation == origen && o.arrivalStation == destino)
                               select o.price).FirstOrDefault();
      }
      return journey;
    }
  }
}
