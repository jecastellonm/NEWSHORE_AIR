using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.DataAccess
{
    public class Flight
    {
        public Transport Transport { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
        public Journeyy Journey { get; set; }
    }
}
