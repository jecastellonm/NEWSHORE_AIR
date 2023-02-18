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

namespace NEWSHORE_AIR.API
{
  public class API_Get : IAPI_Get
  {
    private const string FLIGHT_CARRIER_COLOMBIA = "CO";
    private const string FLIGHT_CARRIER_ESPAÑA = "ES";
    private const string FLIGHT_CARRIER_MEXICO = "MX";

    private readonly IConfiguration _configuration;
    private readonly ILogger<API_Get> _logger;

    public API_Get(IConfiguration configuration, ILogger<API_Get> logger)
    {
      _configuration = configuration;
      _logger = logger;
    }
    public string Message { get; set; }

    List<ResponseNewShoreAPI> lstviajes = new List<ResponseNewShoreAPI>();
    Journeyy journey = new Journeyy();
    Journeyy journey2 = new Journeyy();
    List<Flight> flights = new List<Flight>();
    List<Flight> flights2 = new List<Flight>();
    Transport transport = new Transport();
    Transport transport2 = new Transport();
    List<Transport> transports = new List<Transport>();
    List<Transport> transports2 = new List<Transport>();
    double totalPrice0 = 0; double totalPrice1 = 0; double totalPrice2 = 0; double totalPrice3 = 0;
    double totalPrice4 = 0; double totalPrice5 = 0; double totalPrice6 = 0;

    List<string> flightCarrierOrigen = new List<string>();
    List<string> flightCarrierDestino = new List<string>();
    List<string> newTarget = new List<string>();
    List<string> pri_LOCAL = new List<string>();
    List<string> pri_EXT = new List<string>();
    List<string> pri_LOCAL_O = new List<string>();
    List<string> pri_EXT_O = new List<string>();
    List<string> pri_LOCAL_VUELTA = new List<string>();
    List<string> pri_EXT_VUELTA = new List<string>();
    List<string> pri_LOCAL_VUELTA_D = new List<string>();
    List<string> pri_EXT_VUELTA_D = new List<string>();

    int contTransport = 0;
    int contTransportReturn = 0;
    public void flightsFill(List<ResponseNewShoreAPI> stopovers)
    {
      stopovers.ForEach(e => transports.Add(
        new Transport()
        {
          flightCarrier = e.flightCarrier,
          flightNumber = e.flightNumber
        }));

      stopovers.ForEach(e => flights.Add(
          new Flight()
          {
            Transport = transports[contTransport++],
            Origin = e.departureStation,
            Destination = e.arrivalStation,
            Price = e.price
          }));
    }
    public Journeyy Rutas(string? origen = null, string? destino = null, string? url = null)
    {
      if (url == null)
        url = _configuration[Constants.RUTA_NIVEL_2];

      List<ResponseNewShoreAPI> response = Get(url);
      lstviajes.Clear();
      lstviajes.AddRange(response);
      for (int x = 0; x < pri_LOCAL.Count();) pri_LOCAL.RemoveAt(x);
      for (int x = 0; x < pri_EXT.Count();) pri_EXT.RemoveAt(x);
      for (int x = 0; x < pri_LOCAL_VUELTA.Count();) pri_LOCAL_VUELTA.RemoveAt(x);
      for (int x = 0; x < pri_EXT_VUELTA.Count();) pri_EXT_VUELTA.RemoveAt(x);
      string[] pri0 = new string[10]; string[] pri1 = new string[10];
      string[] pri3 = new string[10]; string[] pri4 = new string[10];
      string[] pri00 = new string[10]; string[] pri11 = new string[10];
      string[] pri33 = new string[10]; string[] pri44 = new string[10];
      var viajessList = (from o in lstviajes
                         where o.departureStation == origen && o.arrivalStation == destino
                         select o.departureStation + " " + o.arrivalStation).ToList();
      flightCarrierOrigen = lstviajes.Where(x => x.departureStation == origen).Select(x => x.flightCarrier).ToList();
      flightCarrierDestino = lstviajes.Where(x => x.departureStation == destino).Select(x => x.flightCarrier).ToList();


      newTarget = flightCarrierDestino[0] == FLIGHT_CARRIER_COLOMBIA ?
                            lstviajes.Where(x => x.arrivalStation == destino && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                                .Select(x => x.departureStation).ToList()
                            : lstviajes.Where(x => x.arrivalStation == destino) //&& x.flightCarrier == flightCarrierDestino[0]
                                .Select(x => x.departureStation).ToList();
      pri_LOCAL = lstviajes.Where(x => x.arrivalStation == destino && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                           .Select(x => x.departureStation).ToList();
      pri_EXT = lstviajes.Where(x => x.arrivalStation == destino)
                         .Select(x => x.departureStation).ToList();
      pri_LOCAL_VUELTA = lstviajes.Where(x => x.departureStation == origen && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                                  .Select(x => x.arrivalStation).ToList();
      pri_EXT_VUELTA = lstviajes.Where(x => x.departureStation == origen && x.flightCarrier != FLIGHT_CARRIER_COLOMBIA)
                                 .Select(x => x.arrivalStation).ToList();


      int a = 0;
      if (pri_LOCAL.Count() > 0)
      {
        for (int i = 0; i < pri_LOCAL.Count(); i++)
        {
          pri00 = pri_LOCAL.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_LOCAL[i]
                                && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.arrivalStation).ToArray() : pri00 = Array.Empty<string>();
          if (pri00.Length > 0) _ = pri0[a++] = pri00[0];
        }
        pri0 = pri0.Where(e => e != null).ToArray();
      }
      else { pri0 = Array.Empty<string>(); }


      if (pri_EXT.Count() > 0)
      {
        a = 0;
        for (int i = 0; i < pri_EXT.Count(); i++)
        {
          pri11 = pri_EXT.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_EXT[i]
                                && x.flightCarrier != FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.arrivalStation).ToArray() : pri11 = Array.Empty<string>();
          if (pri11.Length > 0) pri1[a++] = pri11[0];
        }
        pri1 = pri1.Where(e => e != null).ToArray();
      }
      else { pri1 = Array.Empty<string>(); }


      if (pri_LOCAL_VUELTA.Count() > 0)
      {
        a = 0;
        for (int i = 0; i < pri_LOCAL_VUELTA.Count(); i++)
        {
          pri33 = pri_LOCAL_VUELTA.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_LOCAL_VUELTA[i]
                                    && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.arrivalStation).ToArray() : pri33 = Array.Empty<string>();
          if (pri33.Length > 0) pri3[a++] = pri33[0];
        }
        pri3 = pri3.Where(e => e != null).ToArray();
      }
      else { pri3 = Array.Empty<string>(); }

      if (pri_EXT_VUELTA.Count() > 0)
      {
        a = 0;
        for (int i = 0; i < pri_EXT_VUELTA.Count(); i++)
        {
          pri44 = pri_EXT_VUELTA.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == origen && x.arrivalStation == pri_EXT_VUELTA[i]
                                      && x.flightCarrier != FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.arrivalStation).ToArray() : pri44 = Array.Empty<string>();
          if (pri44.Length > 0) pri4[a++] = pri44[0];
        }
        pri4 = pri4.Where(e => e != null).ToArray();
      }
      else { pri4 = Array.Empty<string>(); }

      totalPrice0 = flightCarrierOrigen[0] == FlightCarrier.CO.ToString() && flightCarrierDestino[0] == FlightCarrier.CO.ToString() ?
      (double)(from o in lstviajes
               where (o.departureStation == pri_LOCAL[0] && o.arrivalStation == destino && o.flightCarrier == FlightCarrier.CO.ToString())
               select o.price).FirstOrDefault()
      : (double)(from o in lstviajes
                 where (o.departureStation == newTarget[0] && o.arrivalStation == destino)
                 select o.price).FirstOrDefault();


      if (viajessList.Count() == 0)
      {
        if (pri0.Length > 0 || pri1.Length > 0 || pri4.Length > 0)
        {
          //if (flightCarrierOrigen[0] == FlightCarrier.CO.ToString() && flightCarrierDestino[0] == FLIGHT_CARRIER_COLOMBIA)
          //{
          if (pri0.Count() > 0)
          {

            var pri_1 = (from o in lstviajes
                         where (o.departureStation == origen && o.arrivalStation == pri0[0])
                         select o).ToList();
            flightsFill(pri_1);
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == pri0[0] && o.arrivalStation == destino)
                          select o).ToList();
            flightsFill(pri_U1);
            if (pri0.Length < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri0[0])
                                     select o.price).Sum();
            if (flights[0].Origin == pri0[0] && flights[1].Destination == pri0[0])
              flights.Reverse();
            if (pri0.Length > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri0[1])
                           select o).ToList();
              flightsFill(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri0[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri0[1])
                                     select o.price).Sum();
              if ((flights[0].Origin == pri0[0] && flights[1].Destination == pri0[0])
               && (flights[2].Origin == pri0[1] && flights[3].Destination == pri0[1]))
                flights.Reverse();
            }
          }
          else if (pri3.Count() > 0)
          {
            var pri_1 = (from o in lstviajes
                         where (o.departureStation == origen && o.arrivalStation == pri3[0])
                         select o).ToList();
            flightsFill(pri_1);
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == pri3[0] && o.arrivalStation == destino)
                          select o).ToList();
            flightsFill(pri_U1);
            if (pri3.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri3[0])
                                     select o.price).Sum();
            if (flights[0].Origin == pri3[0] && flights[1].Destination == pri3[0] && pri3.Count() < 2)
              flights.Reverse();
            if (pri3.Count() > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri3[1])
                           select o).ToList();
              flightsFill(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri3[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri3[1])
                                     select o.price).Sum();
              if ((flights[0].Origin == pri3[0] && flights[1].Destination == pri3[0])
                    && (flights[2].Origin == pri3[1] && flights[3].Destination == pri3[1]))
                flights.Reverse();
            }
          }
          //}
          //else
          //{
          else if (pri1.Count() > 0)
          {
            var pri_1 = (from o in lstviajes
                         where (o.departureStation == origen && o.arrivalStation == pri1[0])
                         select o).ToList();
            flightsFill(pri_1);
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == pri1[0] && o.arrivalStation == destino)
                          select o).ToList();
            flightsFill(pri_U1);
            if (pri1.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri1[0])
                                     select o.price).Sum();
            if (flights[0].Origin == pri1[0] && flights[1].Destination == pri1[0])
              flights.Reverse();
            if (pri1.Count() > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri1[1])
                           select o).ToList();
              flightsFill(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri1[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri1[1])
                                     select o.price).Sum();
              if ((flights[0].Origin == pri1[0] && flights[1].Destination == pri1[0])
                    && (flights[2].Origin == pri1[1] && flights[3].Destination == pri1[1]))
                flights.Reverse();
            }
          }
          else if (pri4.Count() > 0)
          {

            var pri_1 = (from o in lstviajes
                         where (o.departureStation == origen && o.arrivalStation == pri4[0])
                         select o).ToList();
            flightsFill(pri_1);
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == pri4[0] && o.arrivalStation == destino)
                          select o).ToList();
            flightsFill(pri_U1);
            if (pri4.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri4[0])
                                     select o.price).Sum();
            if (flights[0].Origin == pri4[0] && flights[1].Destination == pri4[0])
              flights.Reverse();
            if (pri4.Count() > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri4[1])
                           select o).ToList();
              flightsFill(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri4[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == origen && o.arrivalStation == pri4[1])
                                     select o.price).Sum();
              if ((flights[0].Origin == pri4[0] && flights[1].Destination == pri4[0])
                    && (flights[2].Origin == pri4[1] && flights[3].Destination == pri4[1]))
                flights.Reverse();
            }
          }
          //}
        }
        else
        {
          if (flightCarrierOrigen[0] == FLIGHT_CARRIER_COLOMBIA && flightCarrierDestino[0] == FlightCarrier.CO.ToString())
          {
            var sdo_i_0 = (from o in lstviajes
                           where o.departureStation == origen || o.arrivalStation == pri_LOCAL[0]
                           select o.arrivalStation)
                             .Intersect((from o in lstviajes
                                         where o.departureStation == origen || o.arrivalStation == pri_LOCAL[0]
                                         select o.departureStation)).ToList();
            if (sdo_i_0.Count() == 0 && pri_LOCAL_VUELTA.Count() > 0)
            {
              for (int x = 0; x < pri_LOCAL.Count();) pri_LOCAL.RemoveAt(x);
              pri_LOCAL.AddRange(pri_LOCAL_VUELTA);
              sdo_i_0 = sdo_i_0.Count() == 0 && pri_LOCAL_VUELTA.Count() > 0 ?
                (from o in lstviajes
                 where o.departureStation == destino || o.arrivalStation == pri_LOCAL_VUELTA[0]
                 select o.arrivalStation)
                                 .Intersect((from o in lstviajes
                                             where o.departureStation == destino || o.arrivalStation == pri_LOCAL_VUELTA[0]
                                             select o.departureStation)).ToList()
                : new List<string>() { };
            }
            if (sdo_i_0.Count() > 0)
            {
              var sdo_0 = (from o in lstviajes
                           where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                   || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_LOCAL[0]))
                           select o).ToList();
              if ((sdo_0[0].departureStation == sdo_0[1].arrivalStation))
                //|| (sdo_0[0].departureStation != origen && sdo_0[1].arrivalStation != pri_LOCAL[0]))
                sdo_0.Reverse();
              flightsFill(sdo_0);
              if (sdo_i_0.Count() < 2)
                totalPrice3 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                               || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_LOCAL[0])
                                       select o.price).Sum();
              if (sdo_i_0.Count() > 1)
              {
                var sdo_2 = (from o in lstviajes
                             where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                     || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_LOCAL[1]))
                             select o).ToList();
                if ((sdo_2[0].departureStation == sdo_2[1].arrivalStation)
                     || (sdo_2[2].departureStation == sdo_2[3].arrivalStation))
                  sdo_2.Reverse();
                flightsFill(sdo_2);
                totalPrice5 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                               || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_LOCAL[1])
                                       select o.price).Sum();
                if (pri_LOCAL.Count() > 1)
                {
                  var sdo_4 = (from o in lstviajes
                               where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                       || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_LOCAL[1]))
                               select o).ToList();
                  if ((sdo_4[0].departureStation == sdo_4[1].arrivalStation)
                        || (sdo_4[2].departureStation == sdo_4[3].arrivalStation))
                    sdo_4.Reverse();
                  flightsFill(sdo_4);
                  totalPrice6 = (double)(from o in lstviajes
                                         where (o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                                 || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_LOCAL[1])
                                         select o.price).Sum();
                }
              }
              if ((flights[0].Origin == flights[1].Destination)
                    || (flights[2].Origin == flights[3].Destination))
                flights.Reverse();
              var priU1 = (from o in lstviajes
                           where (o.departureStation == pri_LOCAL[0] && o.arrivalStation == destino)
                           select o).ToList();
              flightsFill(priU1);
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
            if (sdo_i_0.Count == 0 && pri_EXT_VUELTA.Count > 0)
            {
              for (int x = 0; x < pri_EXT.Count();) pri_EXT.RemoveAt(x);
              pri_EXT.AddRange(pri_EXT_VUELTA);
              sdo_i_0 = sdo_i_0.Count == 0 && pri_EXT_VUELTA.Count > 0 ?
                (from o in lstviajes
                 where o.departureStation == destino || o.arrivalStation == pri_EXT_VUELTA[0]
                 select o.arrivalStation)
                                 .Intersect((from o in lstviajes
                                             where o.departureStation == destino || o.arrivalStation == pri_EXT_VUELTA[0]
                                             select o.departureStation)).ToList()
                : new List<string>() { };
            }
            if (sdo_i_0.Count > 0)
            {
              var sdo_0 = (from o in lstviajes
                           where (((o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                   || (o.departureStation == destino && o.arrivalStation == sdo_i_0[0]))
                                   || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_EXT[0]))
                           select o).ToList();
              if ((sdo_0[0].departureStation == sdo_0[1].arrivalStation))
                sdo_0.Reverse();
              flightsFill(sdo_0);
              if (sdo_i_0.Count() < 2)
                totalPrice3 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == sdo_i_0[0])
                                               || (o.departureStation == sdo_i_0[0] && o.arrivalStation == pri_EXT[0])
                                       select o.price).Sum();
              if (sdo_i_0.Count() > 1)
              {
                var sdo_1 = (from o in lstviajes
                             where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                     || (o.departureStation == destino && o.arrivalStation == sdo_i_0[1])
                                     || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[0]))
                             select o).ToList();
                if ((sdo_1[0].departureStation == sdo_1[1].arrivalStation)
                   || (sdo_1[2].departureStation == sdo_1[3].arrivalStation))
                  sdo_1.Reverse();

                flightsFill(sdo_1);
                totalPrice5 = (double)(from o in lstviajes
                                       where (o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                               || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[0])
                                       select o.price).Sum();
                if (pri_EXT.Count() > 1)
                {
                  var sdo_00 = (from o in lstviajes
                                where ((o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                    || (o.departureStation == destino && o.arrivalStation == sdo_i_0[1])
                                    || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[1]))
                                select o).ToList();
                  flightsFill(sdo_00);
                  totalPrice6 = (double)(from o in lstviajes
                                         where (o.departureStation == origen && o.arrivalStation == sdo_i_0[1])
                                                 || (o.departureStation == sdo_i_0[1] && o.arrivalStation == pri_EXT[1])
                                         select o.price).Sum();
                }
              }
              if ((flights[0].Origin == flights[1].Destination))
                flights.Reverse();
            }
          }
          var pri_0 = (from o in lstviajes
                       where o.departureStation == pri_EXT[0] && o.arrivalStation == destino
                       select o).ToList();
          transport.flightCarrier = pri_0[0].flightCarrier;
          transport.flightNumber = pri_0[0].flightNumber;
          pri_0.ForEach(e => flights.Add(
              new Flight()
              {
                Transport = transport,
                Origin = e.departureStation,
                Destination = e.arrivalStation,
                Price = e.price
              }));
        }

        double totalPrice = totalPrice0 + totalPrice1 + totalPrice2 + totalPrice3 + totalPrice4 + totalPrice5 + totalPrice6;
        journey.Flights = flights;
        journey.Destination = destino;
        journey.Origin = origen;
        journey.Price = totalPrice;
      }
      else
      {
        var pri_0 = (from o in lstviajes
                     where (o.departureStation == origen && o.arrivalStation == destino)
                     select o).ToList();
        transport.flightCarrier = pri_0[0].flightCarrier;
        transport.flightNumber = pri_0[0].flightNumber;
        pri_0.ForEach(e => flights.Add(
            new Flight()
            {
              Transport = transport,
              Origin = e.departureStation,
              Destination = e.arrivalStation,
              Price = e.price
            }));
        totalPrice0 = (double)(from o in lstviajes
                               where (o.departureStation == origen && o.arrivalStation == destino)
                               select o.price).FirstOrDefault();
        double totalPrice = totalPrice0;
        journey.Flights = flights;
        journey.Destination = destino;
        journey.Origin = origen;
        journey.Price = totalPrice;
      }
      return journey;
    }

    public void flightsFill_Return(List<ResponseNewShoreAPI> stopovers)
    {
      stopovers.ForEach(e => transports2.Add(
        new Transport()
        {
          flightCarrier = e.flightCarrier,
          flightNumber = e.flightNumber
        }));

      stopovers.ForEach(e => flights2.Add(
          new Flight()
          {
            Transport = transports2[contTransportReturn++],
            Origin = e.departureStation,
            Destination = e.arrivalStation,
            Price = e.price
          }));
    }


    public Journeyy RutasRegreso(string? origen = null, string? destino = null, string? url = null)
    {
      if (url == null)
        url = _configuration[Constants.RUTA_NIVEL_2];

      List<ResponseNewShoreAPI> response = Get(url);
      lstviajes.Clear();
      lstviajes.AddRange(response);
      for (int x = 0; x < pri_LOCAL.Count();) pri_LOCAL.RemoveAt(x);
      for (int x = 0; x < pri_EXT.Count();) pri_EXT.RemoveAt(x);
      for (int x = 0; x < pri_LOCAL_VUELTA.Count();) pri_LOCAL_VUELTA.RemoveAt(x);
      for (int x = 0; x < pri_EXT_VUELTA.Count();) pri_EXT_VUELTA.RemoveAt(x);
      string[] pri0 = new string[10]; string[] pri1 = new string[10];
      string[] pri3 = new string[10]; string[] pri4 = new string[10];
      string[] pri00 = new string[10]; string[] pri11 = new string[10];
      string[] pri33 = new string[10]; string[] pri44 = new string[10];
      var viajessList = (from o in lstviajes
                         where o.departureStation == origen && o.arrivalStation == destino
                         select o.departureStation + " " + o.arrivalStation).ToList();
      flightCarrierOrigen = lstviajes.Where(x => x.departureStation == origen).Select(x => x.flightCarrier).ToList();
      flightCarrierDestino = lstviajes.Where(x => x.departureStation == destino).Select(x => x.flightCarrier).ToList();


      newTarget = flightCarrierDestino[0] == FLIGHT_CARRIER_COLOMBIA ?
                            lstviajes.Where(x => x.arrivalStation == destino && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                                .Select(x => x.departureStation).ToList()
                            : lstviajes.Where(x => x.arrivalStation == destino) // && x.flightCarrier == flightCarrierDestino[0])
                                .Select(x => x.departureStation).ToList();
      pri_LOCAL = lstviajes.Where(x => x.arrivalStation == origen && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                           .Select(x => x.departureStation).ToList();
      pri_EXT = lstviajes.Where(x => x.arrivalStation == origen)
                         .Select(x => x.departureStation).ToList();
      pri_LOCAL_VUELTA = lstviajes.Where(x => x.departureStation == destino && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                                  .Select(x => x.arrivalStation).ToList();
      pri_EXT_VUELTA = lstviajes.Where(x => x.departureStation == destino && x.flightCarrier != FLIGHT_CARRIER_COLOMBIA)
                                 .Select(x => x.arrivalStation).ToList();


      int a = 0;
      if (pri_LOCAL.Count() > 0)
      {
        for (int i = 0; i < pri_LOCAL.Count(); i++)
        {
          pri00 = pri_LOCAL.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == pri_LOCAL[i] && x.arrivalStation == destino
                                && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.departureStation).ToArray() : pri00 = Array.Empty<string>();
          if (pri00.Length > 0) _ = pri0[a++] = pri00[0];
        }
        pri0 = pri0.Where(e => e != null).ToArray();
      }
      else { pri0 = Array.Empty<string>(); }


      if (pri_EXT.Count() > 0)
      {
        a = 0;
        for (int i = 0; i < pri_EXT.Count(); i++)
        {
          pri11 = pri_EXT.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == pri_EXT[i] && x.arrivalStation == destino
                                && x.flightCarrier != FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.departureStation).ToArray() : pri11 = Array.Empty<string>();
          if (pri11.Length > 0) pri1[a++] = pri11[0];
        }
        pri1 = pri1.Where(e => e != null).ToArray();
      }
      else { pri1 = Array.Empty<string>(); }


      if (pri_LOCAL_VUELTA.Count() > 0)
      {
        a = 0;
        for (int i = 0; i < pri_LOCAL_VUELTA.Count(); i++)
        {
          pri33 = pri_LOCAL_VUELTA.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == pri_LOCAL_VUELTA[i] && x.arrivalStation == destino
                                    && x.flightCarrier == FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.departureStation).ToArray() : pri33 = Array.Empty<string>();
          if (pri33.Length > 0) pri3[a++] = pri33[0];
        }
        pri3 = pri3.Where(e => e != null).ToArray();
      }
      else { pri3 = Array.Empty<string>(); }

      if (pri_EXT_VUELTA.Count() > 0)
      {
        a = 0;
        for (int i = 0; i < pri_EXT_VUELTA.Count(); i++)
        {
          pri44 = pri_EXT_VUELTA.Count() > 0 ?
                lstviajes.Where(x => x.departureStation == pri_EXT_VUELTA[i] && x.arrivalStation == destino
                                      && x.flightCarrier != FLIGHT_CARRIER_COLOMBIA)
                         .Select(x => x.departureStation).ToArray() : pri44 = Array.Empty<string>();
          if (pri44.Length > 0) pri4[a++] = pri44[0];
        }
        pri4 = pri4.Where(e => e != null).ToArray();
      }
      else { pri4 = Array.Empty<string>(); }

      totalPrice0 = flightCarrierOrigen[0] == FlightCarrier.CO.ToString() && flightCarrierDestino[0] == FlightCarrier.CO.ToString() ?
      (double)(from o in lstviajes
               where (o.departureStation == origen && o.arrivalStation == pri_LOCAL[0] && o.flightCarrier == FlightCarrier.CO.ToString())
               select o.price).FirstOrDefault()
      : (double)(from o in lstviajes
                 where (o.departureStation == origen && o.arrivalStation == pri_EXT[0])
                 select o.price).FirstOrDefault();


      if (viajessList.Count() == 0)
      {
        if (pri0.Length > 0 || pri1.Length > 0)
        {
          //if (flightCarrierOrigen[0] == FlightCarrier.CO.ToString() && flightCarrierDestino[0] == FLIGHT_CARRIER_COLOMBIA)
          //{
          if (pri0.Count() > 0)
          {
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == origen && o.arrivalStation == pri0[0])
                          select o).ToList();
            flightsFill_Return(pri_U1);
            var pri_1 = (from o in lstviajes
                         where (o.departureStation == pri0[0] && o.arrivalStation == destino)
                         select o).ToList();
            flightsFill_Return(pri_1);

            if (pri0.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == pri0[0] && o.arrivalStation == destino)
                                     select o.price).Sum();
            //if (flights2[0].Origin == pri0[0] && flights2[1].Destination == pri0[0] && pri0.Count() < 2)
            if (flights2[0].Origin == flights2[1].Destination ) //&& pri0.Count() < 2)
              flights2.Reverse();
            if (pri0.Count() > 1)
            {
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == origen && o.arrivalStation == pri0[1])
                            select o).ToList();
              flightsFill_Return(pri_U2);
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == pri0[1] && o.arrivalStation == destino)
                           select o).ToList();
              flightsFill_Return(pri_2);

              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == pri0[1] && o.arrivalStation == destino)
                                     select o.price).Sum();
              if ((flights2[0].Origin == flights2[1].Destination)
                    && (flights2[2].Origin == flights2[3].Destination))
                flights2.Reverse();
            }
          }
          else if (pri1.Count() > 0)
          {
            var pri_1 = (from o in lstviajes
                         where (o.departureStation == origen && o.arrivalStation == pri1[0])
                         select o).ToList();
            flightsFill_Return(pri_1);
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == pri1[0] && o.arrivalStation == destino)
                          select o).ToList();
            flightsFill_Return(pri_U1);
            if (pri1.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == pri1[0] && o.arrivalStation == destino)
                                     select o.price).Sum();
            if (flights2[0].Origin == flights2[1].Destination)
              flights2.Reverse();
            if (pri1.Count() > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri1[1])
                           select o).ToList();
              flightsFill_Return(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri1[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill_Return(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == pri1[1] && o.arrivalStation == destino)
                                     select o.price).Sum();
              if ((flights2[0].Origin == flights2[1].Destination)
                    && (flights2[2].Origin == pri1[1] && flights2[3].Destination == pri1[1]))
                flights2.Reverse();
            }
          }
          else if (pri3.Count() > 0)
          {
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == origen && o.arrivalStation == pri3[0])
                          select o).ToList();
            flightsFill_Return(pri_U1);
            var pri_1 = (from o in lstviajes
                         where (o.departureStation == pri3[0] && o.arrivalStation == destino)
                         select o).ToList();
            flightsFill_Return(pri_1);

            if (pri3.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == pri3[0] && o.arrivalStation == destino)
                                     select o.price).Sum();
            if (flights2[0].Origin == flights2[1].Destination)
              flights2.Reverse();
            if (pri3.Count() > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri3[1])
                           select o).ToList();
              flightsFill_Return(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri3[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill_Return(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == pri3[1] && o.arrivalStation == destino)
                                     select o.price).Sum();
              if ((flights[0].Origin == flights[1].Destination)
                    && (flights[2].Origin == flights[3].Destination))
                flights.Reverse();
            }
          }
          //}
          //else
          //{
          else if (pri4.Count() > 0)
          {

            var pri_1 = (from o in lstviajes
                         where (o.departureStation == origen && o.arrivalStation == pri4[0])
                         select o).ToList();
            flightsFill_Return(pri_1);
            var pri_U1 = (from o in lstviajes
                          where (o.departureStation == pri4[0] && o.arrivalStation == destino)
                          select o).ToList();
            flightsFill_Return(pri_U1);
            if (pri4.Count() < 2)
              totalPrice1 = (double)(from o in lstviajes
                                     where (o.departureStation == pri4[0] && o.arrivalStation == destino)
                                     select o.price).Sum();
            if (flights2[0].Origin == flights2[1].Destination)
              flights2.Reverse();
            if (pri4.Count() > 1)
            {
              var pri_2 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri4[1])
                           select o).ToList();
              flightsFill_Return(pri_2);
              var pri_U2 = (from o in lstviajes
                            where (o.departureStation == pri4[1] && o.arrivalStation == destino)
                            select o).ToList();
              flightsFill_Return(pri_U2);
              totalPrice2 = (double)(from o in lstviajes
                                     where (o.departureStation == pri4[1] && o.arrivalStation == destino)
                                     select o.price).Sum();
              if ((flights2[0].Origin == flights2[1].Destination)
                    && (flights2[2].Origin == flights2[3].Destination))
                flights2.Reverse();
            }
          }
          //}
        }
        else
        {
          var pri_0 = (from o in lstviajes
                       where (o.departureStation == origen && o.arrivalStation == pri_EXT[0])
                       select o).ToList();
          transport2.flightCarrier = pri_0[0].flightCarrier;
          transport2.flightNumber = pri_0[0].flightNumber;
          pri_0.ForEach(e => flights2.Add(
              new Flight()
              {
                Transport = transport2,
                Origin = e.departureStation,
                Destination = e.arrivalStation,
                Price = e.price
              }));
          if (flightCarrierOrigen[0] == FLIGHT_CARRIER_COLOMBIA && flightCarrierDestino[0] == FlightCarrier.CO.ToString())
          {
            var sdo_i_0 = (from o in lstviajes
                           where o.departureStation == pri_LOCAL[0] || o.arrivalStation == destino
                           select o.arrivalStation)
                             .Intersect((from o in lstviajes
                                         where o.departureStation == pri_LOCAL[0] || o.arrivalStation == destino
                                         select o.departureStation)).ToList();
            if (sdo_i_0.Count() == 0 && pri_LOCAL_VUELTA.Count() > 0)
            {
              for (int x = 0; x < pri_LOCAL.Count();) pri_LOCAL.RemoveAt(x);
              pri_LOCAL.AddRange(pri_LOCAL_VUELTA);
              sdo_i_0 = sdo_i_0.Count() == 0 && pri_LOCAL_VUELTA.Count() > 0 ?
                (from o in lstviajes
                 where o.departureStation == pri_LOCAL_VUELTA[0] || o.arrivalStation == origen
                 select o.arrivalStation)
                                 .Intersect((from o in lstviajes
                                             where o.departureStation == pri_LOCAL_VUELTA[0] || o.arrivalStation == origen
                                             select o.departureStation)).ToList()
                : new List<string>() { };
            }
            if (sdo_i_0.Count() > 0)
            {
              var sdo_0 = (from o in lstviajes
                           where ((o.departureStation == sdo_i_0[0] && o.arrivalStation == destino)
                                   || (o.departureStation == pri_LOCAL[0] && o.arrivalStation == sdo_i_0[0]))
                           select o).ToList();
              if ((sdo_0[0].departureStation != pri_LOCAL[0] && sdo_0[0].arrivalStation != destino)
                    || (sdo_0[0].departureStation != pri_LOCAL[0] && sdo_0[0].arrivalStation != destino))
                sdo_0.Reverse();
              flightsFill_Return(sdo_0);
              if (sdo_i_0.Count() < 2)
                totalPrice3 = (double)(from o in lstviajes
                                       where (o.departureStation == sdo_i_0[0] && o.arrivalStation == destino)
                                               || (o.departureStation == pri_LOCAL[0] && o.arrivalStation == sdo_i_0[0])
                                       select o.price).Sum();
              if (sdo_i_0.Count() > 1)
              {
                var sdo_2 = (from o in lstviajes
                             where ((o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                     || (o.departureStation == pri_LOCAL[1] && o.arrivalStation == sdo_i_0[1]))
                             select o).ToList();
                if ((sdo_2[0].departureStation != pri_LOCAL[0] && sdo_2[1].arrivalStation != destino))
                  sdo_2.Reverse();
                flightsFill_Return(sdo_2);
                totalPrice5 = (double)(from o in lstviajes
                                       where (o.departureStation == sdo_i_0[0] && o.arrivalStation == destino)
                                               || (o.departureStation == pri_LOCAL[1] && o.arrivalStation == sdo_i_0[0])
                                       select o.price).Sum();
                if (pri_LOCAL.Count() > 1)
                {
                  var sdo_4 = (from o in lstviajes
                               where ((o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                       || (o.departureStation == pri_LOCAL[1] && o.arrivalStation == sdo_i_0[1]))
                               select o).ToList();
                  if ((sdo_4[0].departureStation != pri_LOCAL[0] && sdo_4[1].arrivalStation != destino)
                        || (sdo_4[0].departureStation != pri_LOCAL[0] && sdo_4[1].arrivalStation != destino))
                    sdo_4.Reverse();
                  flightsFill_Return(sdo_4);
                  totalPrice6 = (double)(from o in lstviajes
                                         where (o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                                 || (o.departureStation == pri_LOCAL[1] && o.arrivalStation == sdo_i_0[1])
                                         select o.price).Sum();
                }
              }
              if ((flights2[0].Origin != pri_LOCAL[0] && flights2[1].Destination != destino)
                    || (flights2[0].Origin != pri_LOCAL[0] && flights2[1].Destination != destino))
                flights2.Reverse();
              var priU1 = (from o in lstviajes
                           where (o.departureStation == origen && o.arrivalStation == pri_LOCAL[0])
                           select o).ToList();
              flightsFill_Return(priU1);
            }
          }
          else
          {
            var sdo_i_0 = (from o in lstviajes
                           where o.departureStation == pri_EXT[0] || o.arrivalStation == destino
                           select o.arrivalStation)
                             .Intersect((from o in lstviajes
                                         where o.departureStation == pri_EXT[0] || o.arrivalStation == destino
                                         select o.departureStation)).ToList();
            if (sdo_i_0.Count() == 0 && pri_EXT_VUELTA.Count() > 0)
            {
              for (int x = 0; x < pri_EXT.Count();) pri_EXT.RemoveAt(x);
              pri_EXT.AddRange(pri_EXT_VUELTA);
              sdo_i_0 = sdo_i_0.Count() == 0 && pri_EXT_VUELTA.Count() > 0 ?
                (from o in lstviajes
                 where o.departureStation == pri_EXT_VUELTA[0] || o.arrivalStation == origen
                 select o.arrivalStation)
                                 .Intersect((from o in lstviajes
                                             where o.departureStation == pri_EXT_VUELTA[0] || o.arrivalStation == origen
                                             select o.departureStation)).ToList()
                : new List<string>() { };
            }
            if (sdo_i_0.Count() > 0)
            {
              var sdo_0 = (from o in lstviajes
                           where (((o.departureStation == sdo_i_0[0] && o.arrivalStation == destino)
                                   || (o.departureStation == sdo_i_0[0] && o.arrivalStation == origen))
                                   || (o.departureStation == pri_EXT[0] && o.arrivalStation == sdo_i_0[0]))
                           select o).ToList();
              if ((sdo_0[0].departureStation != pri_EXT[0] && sdo_0[1].arrivalStation != destino))
                sdo_0.Reverse();
              flightsFill_Return(sdo_0);
              if (sdo_i_0.Count() < 2)
                totalPrice3 = (double)(from o in lstviajes
                                       where (o.departureStation == sdo_i_0[0] && o.arrivalStation == destino)
                                               || (o.departureStation == pri_EXT[0] && o.arrivalStation == sdo_i_0[0])
                                       select o.price).Sum();
              if (sdo_i_0.Count() > 1)
              {
                var sdo_1 = (from o in lstviajes
                             where ((o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                       || (o.departureStation == sdo_i_0[1] && o.arrivalStation == origen)
                                     || (o.departureStation == pri_EXT[0] && o.arrivalStation == sdo_i_0[1]))
                             select o).ToList();
                if ((sdo_1[0].departureStation != pri_EXT[0] && sdo_1[1].arrivalStation != destino)
                  || (sdo_1[2].departureStation != pri_EXT[0] && sdo_1[3].arrivalStation != destino))
                  sdo_1.Reverse();

                flightsFill_Return(sdo_1);
                totalPrice5 = (double)(from o in lstviajes
                                       where (o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                               || (o.departureStation == pri_EXT[0] && o.arrivalStation == sdo_i_0[1])
                                       select o.price).Sum();
                if (pri_EXT.Count() > 1)
                {
                  var sdo_00 = (from o in lstviajes
                                where ((o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                    || (o.departureStation == sdo_i_0[1] && o.arrivalStation == origen)
                                    || (o.departureStation == pri_EXT[1] && o.arrivalStation == sdo_i_0[1]))
                                select o).ToList();
                  flightsFill_Return(sdo_00);
                  totalPrice6 = (double)(from o in lstviajes
                                         where (o.departureStation == sdo_i_0[1] && o.arrivalStation == destino)
                                                 || (o.departureStation == pri_EXT[1] && o.arrivalStation == sdo_i_0[1])
                                         select o.price).Sum();
                }
              }
              if ((flights2[0].Origin != pri_EXT[0] && flights2[1].Destination != destino))
                flights2.Reverse();

            }
          }
        }

        double totalPrice = totalPrice0 + totalPrice1 + totalPrice2 + totalPrice3 + totalPrice4 + totalPrice5 + totalPrice6;
        journey2.Flights = flights2;
        journey2.Destination = destino;
        journey2.Origin = origen;
        journey2.Price = totalPrice;
      }
      else
      {
        var pri_0 = (from o in lstviajes
                     where (o.departureStation == origen && o.arrivalStation == destino)
                     select o).ToList();
        transport2.flightCarrier = pri_0[0].flightCarrier;
        transport2.flightNumber = pri_0[0].flightNumber;
        pri_0.ForEach(e => flights2.Add(
            new Flight()
            {
              Transport = transport2,
              Origin = e.departureStation,
              Destination = e.arrivalStation,
              Price = e.price
            }));
        totalPrice0 = (double)(from o in lstviajes
                               where (o.departureStation == origen && o.arrivalStation == destino)
                               select o.price).FirstOrDefault();
        double totalPrice = totalPrice0;
        journey2.Flights = flights2;
        journey2.Destination = destino;
        journey2.Origin = origen;
        journey2.Price = totalPrice;
      }
      return journey2;
    }


    public List<ResponseNewShoreAPI> Get(string url)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.Credentials = CredentialCache.DefaultCredentials;
      request.Proxy = null;
      try
      {
        HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
        Stream stream = httpWebResponse.GetResponseStream();
        if (httpWebResponse.StatusCode == System.Net.HttpStatusCode.OK)
        {
          StreamReader streamReader = new StreamReader(stream);
          string datos = HttpUtility.HtmlDecode(streamReader.ReadToEnd());
          List<ResponseNewShoreAPI> data = JsonConvert.DeserializeObject<List<ResponseNewShoreAPI>>(datos);
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
        url = _configuration[Constants.RUTA_NIVEL_2];
      List<ResponseNewShoreAPI> response = Get(url);
      if (response != null)
      {
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
        return originssList;
      }
      return null;
    }

    public List<string> Destinations(string? url = null)
    {
      if (url == null)
        url = _configuration[Constants.RUTA_NIVEL_2];
      List<ResponseNewShoreAPI> response = Get(url);

      List<ResponseNewShoreAPI> lstviajes = new List<ResponseNewShoreAPI>();
      lstviajes.AddRange(response);
      if (response != null)
      {
        List<ResponseNewShoreAPI> responseNewShoreList = new List<ResponseNewShoreAPI>();
        List<Destinations> destinationsList = new List<Destinations>();
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
        return destinationssList;
      }
      return null;
    }
  }
}
