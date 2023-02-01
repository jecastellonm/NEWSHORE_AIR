using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewShoreTest
{
  public class Flight
  {
    public int FlightID { get; set; }
    public Transport Transport { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Price { get; set; }
  }
}
