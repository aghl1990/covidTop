using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace covidTop.Models
{
    public class Region
    {
        public string Iso { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string Lat { get; set; }
        public string @Long { get; set; }

        private List<City> cities;

        public List<City> GetCities()
        {
            return cities;
        }

        public void SetCities(List<City> value)
        {
            if(value != null)   
                cities = value;
        }
    }
}