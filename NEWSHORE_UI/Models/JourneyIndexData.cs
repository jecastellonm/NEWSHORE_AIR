using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
  public class JourneyIndexData
  {
    public IEnumerable<Journeys> JourneysDb { get; set; }
    public IEnumerable<Flight> FlightsDb { get; set; }
    public IEnumerable<Transport> TransportsDb { get; set; }

  }
}
