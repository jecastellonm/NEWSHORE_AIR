using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        public string departureStation { get; set; }
        public string arrivalStation { get; set; }
        public string flightCarrier { get; set; }
        public string flightNumber { get; set; }
        public double price { get; set; }
    }
}
