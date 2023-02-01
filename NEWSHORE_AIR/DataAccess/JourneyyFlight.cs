using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.DataAccess
{
  public class JourneyyFlight
  {  
    public int JourneyID { get; set; }

    public int FlightID { get; set; }

    public Journeys Journeys { get; set; }

    public Flight Flight { get; set; }
  }
}
