using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.DataAccess
{
    public class ResponseNewShoreAPIList
    {
        public List<ResponseNewShoreAPI> ResponseNewShoreList { get; set; }
    }
    public class ResponseNewShoreAPI
    {
        [JsonProperty("departureStation")] 
        public string departureStation { get; set; }
        [JsonProperty("arrivalStation")]
        public string arrivalStation { get; set; }
        [JsonProperty("flightCarrier")]
        public string flightCarrier { get; set; }
        [JsonProperty("flightNumber")]
        public string flightNumber { get; set; }
        [JsonProperty("price")]
        public double price { get; set; }
    }
}
