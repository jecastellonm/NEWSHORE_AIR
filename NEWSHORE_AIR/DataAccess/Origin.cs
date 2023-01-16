using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.DataAccess
{
    public class OriginsList
    {
        public List<Origin> OriginList { get; set; }
    }

    public class Origin
    {
        public string Station { get; set; }
        public string flightCarrier { get; set; }
    }
}
