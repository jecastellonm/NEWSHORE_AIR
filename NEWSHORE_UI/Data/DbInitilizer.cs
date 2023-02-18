using NEWSHORE_UI.DataAccess;
using NEWSHORE_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Data
{
  public class DbInitilizer
  {

    public static void Initialize(JourneyysContext context)
    {
      context.Database.EnsureCreated();

      if (context.Journeyys.Any())
      {
        return;   // DB has been seeded
      }

      var journeys = new Journeys[]
      {
        new Journeys { Origin = "MDE", Destination = "BOG", Price = 400}
        , new Journeys { Origin = "BOG", Destination = "MDE", Price = 400}
      };
      foreach (Journeys j in journeys)
      {
        context.Journeyys.Add(j);
      }
      context.SaveChanges();


      var flights = new Flight[]
      {
        new Flight {Origin = "MDE", Destination = "CTG", Price = 200 }
                //, JourneysID = journeys.Single( p => p.Origin== "MDE" && p.Destination == "BOG").ID }
        ,new Flight {Origin = "CTG", Destination = "BOG", Price = 200 }
        ,new Flight {Origin = "BOG", Destination = "CTG", Price = 200 }
        ,new Flight {Origin = "CTG", Destination = "MDE", Price = 200 }
      };
      foreach (Flight f in flights)
      {
        context.Flights.Add(f);
      }
      context.SaveChanges();



      var journeyFlight = new JourneyyFlight[]
      {
        new JourneyyFlight { JourneyID = 1, FlightID = 1, DateAddEdit = DateTime.Now },
        new JourneyyFlight { JourneyID = 1, FlightID = 2, DateAddEdit = DateTime.Now },
        new JourneyyFlight { JourneyID = 2, FlightID = 3, DateAddEdit = DateTime.Now },
        new JourneyyFlight { JourneyID = 2, FlightID = 4, DateAddEdit = DateTime.Now }
      };
      foreach (JourneyyFlight j in journeyFlight)
      {
        context.JourneyyFilghts.Add(j);
      }
      context.SaveChanges();


      var transports = new Transport[]
      {
        new Transport {flightCarrier = "CO", flightNumber ="8009"
                    ,flightID = flights.Single( p => p.Origin== "MDE" && p.Destination == "CTG").ID
        },
        new Transport {flightCarrier = "CO", flightNumber ="9010",
                    flightID = flights.Single( p => p.Origin== "CTG" && p.Destination == "BOG").ID
        },
        new Transport {flightCarrier = "CO", flightNumber ="8010"
                    ,flightID = flights.Single( p => p.Origin== "BOG" && p.Destination == "CTG").ID
        },
        new Transport {flightCarrier = "CO", flightNumber ="9009",
                    flightID = flights.Single( p => p.Origin== "CTG" && p.Destination == "MDE").ID
        }

      };
      foreach (Transport t in transports)
      {
        context.Transports.Add(t);
      }
      context.SaveChanges();


      var route = new Route[]
{
        new Route { departureStation = "MZL", arrivalStation = "MDE", flightCarrier = "CO",flightNumber = "8001", price = 200},
        new Route { departureStation = "MZL", arrivalStation = "CTG", flightCarrier = "CO",flightNumber = "8002", price = 200},
        new Route { departureStation = "PEI", arrivalStation = "BOG", flightCarrier = "CO",flightNumber = "8003", price = 200},
        new Route { departureStation = "MDE", arrivalStation = "BCN", flightCarrier = "CO",flightNumber = "8004", price = 500},
        new Route { departureStation = "CTG", arrivalStation = "CAN", flightCarrier = "CO",flightNumber = "8005", price = 300},
        new Route { departureStation = "BOG", arrivalStation = "MAD", flightCarrier = "CO",flightNumber = "8006", price = 500},
        new Route { departureStation = "BOG", arrivalStation = "MEX", flightCarrier = "CO",flightNumber = "8007", price = 300},
        new Route { departureStation = "MZL", arrivalStation = "PEI", flightCarrier = "CO",flightNumber = "8008", price = 200},
        new Route { departureStation = "MDE", arrivalStation = "CTG", flightCarrier = "CO",flightNumber = "8009", price = 200},
        new Route { departureStation = "BOG", arrivalStation = "CTG", flightCarrier = "CO",flightNumber = "8010", price = 200},
        new Route { departureStation = "MDE", arrivalStation = "MZL", flightCarrier = "CO",flightNumber = "9001", price = 200},
        new Route { departureStation = "CTG", arrivalStation = "MZL", flightCarrier = "CO",flightNumber = "9002", price = 200},
        new Route { departureStation = "BOG", arrivalStation = "PEI", flightCarrier = "CO",flightNumber = "9003", price = 200},
        new Route { departureStation = "BCN", arrivalStation = "MDE", flightCarrier = "ES",flightNumber = "9004", price = 500},
        new Route { departureStation = "CAN", arrivalStation = "CTG", flightCarrier = "MX",flightNumber = "9005", price = 300},
        new Route { departureStation = "MAD", arrivalStation = "BOG", flightCarrier = "ES",flightNumber = "9006", price = 500},
        new Route { departureStation = "MEX", arrivalStation = "BOG", flightCarrier = "MX",flightNumber = "9007", price = 300},
        new Route { departureStation = "PEI", arrivalStation = "MZL", flightCarrier = "CO",flightNumber = "9008", price = 200},
        new Route { departureStation = "CTG", arrivalStation = "MDE", flightCarrier = "CO",flightNumber = "9009", price = 200},
        new Route { departureStation = "CTG", arrivalStation = "BOG", flightCarrier = "CO",flightNumber = "9010", price = 200},
      };
      
      foreach (Route v in route)
      {
        context.Routes.Add(v);
      }
      context.SaveChanges();
    }

  }
}
