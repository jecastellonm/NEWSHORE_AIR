﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Models
{
    public class Transport
    {
        public string flightCarrier { get; set; }
        public string flightNumber { get; set; }
        public Flight Flight { get; set; }
    }
}
