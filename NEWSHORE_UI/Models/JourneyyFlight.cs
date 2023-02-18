using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
  public class JourneyyFlight
  {
    public DateTime DateAddEdit { get; set; }
 
    public int JourneyID { get; set; }

    public Journeys Journeys { get; set; }

    public int FlightID { get; set; }

    public Flight Flight { get; set; }
  }
}
