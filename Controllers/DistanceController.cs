using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using AirportDistanceAPI.Models;
using Microsoft.Extensions.Options;

namespace AirportDistanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistanceController : Controller
    {
        private readonly APIConfig apiConfig;
        public DistanceController(IOptions<APIConfig> apiConfigAccessor)
        {
            apiConfig = apiConfigAccessor.Value;
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string iata1, string iata2)
        {
            Error err = new Error();

            try
            {
                string URL = string.Empty;
                string URL_Param = string.Empty;
                bool Isvalid_Iata1 = false;
                bool Isvalid_Iata2 = false;
                string I1 = string.Empty;
                string I1Name = string.Empty;
                float I1Lon = 0;
                float I1Lat = 0;
                string I2 = string.Empty;
                string I2Name = string.Empty;
                float I2Lon = 0;
                float I2Lat = 0;

                // Get API URL from appsettings configuration
                URL = apiConfig.URL;
                if (iata1 != null && iata2 != null)
                {
                    HttpClient client = new HttpClient();
                    // Add - Accept header for JSON format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Get response for Iata1
                    URL_Param = URL + "/" + iata1.ToUpper();
                    HttpResponseMessage response = client.GetAsync(URL_Param).Result;  // Wait for response
                    if (response.IsSuccessStatusCode)
                    {
                        var airport_data1 = JsonConvert.DeserializeObject<Airport>(response.Content.ReadAsStringAsync().Result.Replace("[", "").Replace("]", ""));
                        // Get data
                        I1Lon = airport_data1.Location.Lon;
                        I1Lat = airport_data1.Location.Lat;
                        I1 = airport_data1.Iata;
                        I1Name = airport_data1.Name;
                        Isvalid_Iata1 = true;
                    }
                    else
                    {
                        err.ErrorNumber = (int)response.StatusCode;
                        err.ErrorDescription = response.StatusCode.ToString();
                    }

                    // Get response for Iata2
                    URL_Param = URL + "/" + iata2.ToUpper();
                    response = client.GetAsync(URL_Param).Result;  // Wait for response
                    if (response.IsSuccessStatusCode)
                    {
                        var airport_data2 = JsonConvert.DeserializeObject<Airport>(response.Content.ReadAsStringAsync().Result.Replace("[", "").Replace("]", ""));
                        // Get data
                        I2Lon = airport_data2.Location.Lon;
                        I2Lat = airport_data2.Location.Lat;
                        I2 = airport_data2.Iata;
                        I2Name = airport_data2.Name;
                        Isvalid_Iata2 = true;
                    }
                    else
                    {
                        err.ErrorNumber = (int)response.StatusCode;
                        err.ErrorDescription = response.StatusCode.ToString();
                    }

                    // Dispose HttpClient call
                    client.Dispose();

                    double DistanceMetres = 0;

                    if (Isvalid_Iata1 && Isvalid_Iata2)
                    {
                        // Get distance in meters based on geo coordinates
                        var sCoord = new GeoCoordinate(I1Lat, I1Lon);
                        var eCoord = new GeoCoordinate(I2Lat, I2Lon);
                        DistanceMetres = sCoord.GetDistanceTo(eCoord);

                        // Add results
                        Distance dist = new Distance()
                        {
                            IATA1 = I1,
                            IATA1Name = I1Name,
                            IATA1Longitude = I1Lon,
                            IATA1Latitude = I1Lat,
                            IATA2 = I2,
                            IATA2Name = I2Name,
                            IATA2Longitude = I2Lon,
                            IATA2Latitude = I2Lat,
                            TotalDistanceMiles = (DistanceMetres * 0.000621371),
                            TotalDistanceKm = (DistanceMetres / 1000)
                        };

                        // Return results
                        string json = JsonConvert.SerializeObject(dist, Formatting.None);
                        return new string[] { json };
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(err, Formatting.None);
                        return new string[] { json };
                    }
                }
                else
                {
                    err.ErrorNumber = 1;
                    err.ErrorDescription = "Please ensure that you supply valid values for parameters 'iata1' and 'iata2', e.g. ~/api/distance?iata1=JNB&iata2=NYC";
                    {
                        string json = JsonConvert.SerializeObject(err, Formatting.None);
                        return new string[] { json };
                    }
                }
            }
            catch (Exception ex)
            {
                err.ErrorNumber = ex.HResult;
                err.ErrorDescription = ex.Message;

                string json = JsonConvert.SerializeObject(err, Formatting.None);
                return new string[] { json };
            }
        }
    }
}
