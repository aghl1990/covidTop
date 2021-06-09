using covidTop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace covidTop.Utils
{
    public static class Global
    {
        public static IEnumerable<Report> lstReport
        {
            get { return  HttpContext.Current.Session["lstReport"] as IEnumerable<Report>; }

            set { HttpContext.Current.Session["lstReport"] = value; }
        } 
        public static string typeList { get; set; } 
    }
}