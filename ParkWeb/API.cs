using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkWeb
{
    public static class API
    {
        public static string APIBaseURL = "http://localhost:5000/";
        public static string NationalParkAPIPath = APIBaseURL + "/api/nationals";
        public static string TrailAPIPath = APIBaseURL + "/api/trails";
    }
}
