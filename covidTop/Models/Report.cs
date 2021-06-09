using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace covidTop.Models 
{
    public class Report  
    {
        public string Iso { get; set; }
        public string Name { get; set; }
        public int Cases { get; set; }
        public int Deaths { get; set; }

        public Report(string iso, string name, int cases, int deaths)
        {
            this.Iso = iso;
            this.Name = name;
            this.Cases = cases;
            this.Deaths = deaths;
        } 
         
    }
}