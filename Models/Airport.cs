using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportDistanceAPI.Models
{
    public class Airport
    {
        public string Country { get; set; }
        public string CityIata { get; set; }
        public string Iata { get; set; }
        public string City { get; set; }
        public string CountryIata { get; set; }
        public int Rating { get; set; }
        public string Name { get; set; }
        public AirportCoordinates Location { get; set; }
        public string Type { get; set; }
        public string Nearby { get; set; }
    }
    public class AirportCoordinates
    {
        public float Lon { get; set; }
        public float Lat { get; set; }
    }
}
