using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
    public class Route
    {
        public int RouteID { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string departureStation { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string arrivalStation { get; set; }

    [StringLength(2, MinimumLength = 2)]
    public string flightCarrier { get; set; }

    [StringLength(4, MinimumLength = 3)]
    public string flightNumber { get; set; }
        public double price { get; set; }
    }
}
