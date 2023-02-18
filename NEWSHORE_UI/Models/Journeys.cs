using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
  public class Journeys
  {
    public Journeys()
    {
      this.JourneyyFlights = new HashSet<JourneyyFlight>();
    }
    public int ID { get; set; }
    public ICollection<Flight> Flights { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string Origin { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string Destination { get; set; }
    public double Price { get; set; }

    public ICollection<JourneyyFlight> JourneyyFlights{ get; set; }

  }
}
