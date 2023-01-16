using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.DataAccess
{
    public class DestinationsList
    {
        public List<Destinations> DestinationList { get; set; }
    }

    public class Destinations
    {
        public string Station { get; set; }
        public string flightCarrier { get; set; }
    }

}
