using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
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

    [StringLength(2, MinimumLength = 2)]
    public string flightCarrier { get; set; }

    [StringLength(4, MinimumLength = 3)]
    public string flightNumber { get; set; }
  
    public int flightID { get; set; }
    public Flight Flight { get; set; }
  }
}
