using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.DataAccess
{
  public class Journeyy
  {

    public int ID { get; set; }
    public ICollection<Flight> Flights { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public double Price { get; set; }

  }

}
