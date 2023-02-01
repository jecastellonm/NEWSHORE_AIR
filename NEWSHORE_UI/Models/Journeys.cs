using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
  public class Journeys
  {
    public int ID { get; set; }
    public ICollection<Flight> Flights { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Price { get; set; }

    public ICollection<JourneyyFlight> JourneyyFlights{ get; set; }

  }
}
