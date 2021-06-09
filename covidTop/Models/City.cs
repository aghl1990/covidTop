using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace covidTop.Models
{
    public class City
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public int Fips { get; set; }
        public string Lat { get; set; }
        public string @Long { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Confirmed_diff { get; set; }
        public int Deaths_diff { get; set; }
        public string Last_update { get; set; }
    }
}