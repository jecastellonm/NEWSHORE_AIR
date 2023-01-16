using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
    public class Journey
    {
        //public Journey()
        //{
        //    Flights = new List<Flight>();
        //}
        public ICollection<Flight> Flights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }

    //public class Flights  
    //{
    //    Transports Transporty { get; set; }
    //    public string Origin { get; set; }
    //    public string Destination { get; set; }
    //    public double price { get; set; }
    //}

    //public class Transports
    //{
    //    public string flightCarrier { get; set; }
    //    public string flightNumber { get; set; }
    //}

}
