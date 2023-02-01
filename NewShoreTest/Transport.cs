using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewShoreTest
{
  public enum FlightCarrier
  {
    CO,
    ES,
    MX
  }
  public class Transport
  {
    public int TransportID { get; set; }
    public string flightCarrier { get; set; }
    public string flightNumber { get; set; }
    public Flight Flight { get; set; }
  }
}
