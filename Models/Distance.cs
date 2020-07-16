using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportDistanceAPI.Models
{
    public class Distance
    {
        public string IATA1 { get; set; }
        public string IATA1Name { get; set; }
        public float IATA1Longitude { get; set; }
        public float IATA1Latitude { get; set; }
        public string IATA2 { get; set; }
        public string IATA2Name { get; set; }
        public float IATA2Longitude { get; set; }
        public float IATA2Latitude { get; set; }
        public double TotalDistanceMiles { get; set; }
        public double TotalDistanceKm { get; set; }
    }
}
