﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
  public class Flight
  {
    public int FlightID { get; set; }
    public Transport Transport { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Price { get; set; }

    //public int JourneyID { get; set; }
    public ICollection<JourneyyFlight> JourneyyFlights { get; set; }
  }
}
