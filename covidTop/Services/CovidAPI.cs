using covidTop.Models;
using covidTop.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace covidTop.Services
{
    public class CovidAPI
    { 
        public static async Task<List<Region>> getJsonServiceRegionAsync()
        {
            var lstData = await getServiceCovidReportAsync("R", "");

            List<Region> lst = lstData == null? new List<Region>(): JsonConvert.DeserializeObject<List<Region>>(lstData);

            return lst;
        }

        private static async Task<List<CovidReport>> getJsonServiceDetailAsync(String iso)
        {
            var lstData = await getServiceCovidReportAsync("P", iso);

            List<CovidReport> lst = lstData == null ? new List<CovidReport>() : JsonConvert.DeserializeObject<List<CovidReport>>(lstData);
             
            return lst;
        } 

        private static async Task<dynamic> getServiceCovidReportAsync(string type, string iso)
        {
            try
            {
                var json = "";
                var client = new HttpClient();
                var uri = Properties.Settings.Default.uri_rapidapi + (type == "R" ? "regions" : (type == "P" ? (iso == "" ? "reports" + iso : "reports?iso=" + iso) : ""));

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri),
                    Headers =
                {
                    { "x-rapidapi-key", Properties.Settings.Default.x_rapidapi_key },
                    { "x-rapidapi-host", Properties.Settings.Default.x_rapidapi_host },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    json = await response.Content.ReadAsStringAsync();
                }

                dynamic lstObj = JsonConvert.DeserializeObject(json);
                var lstData = lstObj["data"].ToString();

                return lstData;
            }
            catch(Exception exp)
            {
                Console.WriteLine("Exception in getServiceCovidReportAsync: " + exp.Message);
                return null;
            } 

        }


        public static async Task<IEnumerable<Report>> getReportAsync(string type, string iso)
        {
             
            if (type == "R")
            {
                var lst = (
                             from rep in await getJsonServiceDetailAsync(iso)
                             join reg in await getJsonServiceRegionAsync()
                             on rep.Region.Iso equals reg.Iso
                             group rep by new { rep.Region.Iso, rep.Region.Name } into regionesX
                             select new
                             Report
                             (
                                  regionesX.Key.Iso, regionesX.Key.Name, regionesX.Sum(x => x.Confirmed), regionesX.Sum(x => x.Deaths)
                             )
                           ).OrderByDescending(x => x.Cases).Take(10);

                Global.lstReport = lst;
            }
            else  
            {
                var lst = (
                          from rep in await getJsonServiceDetailAsync(iso)
                          join reg in await getJsonServiceRegionAsync()
                          on rep.Region.Iso equals reg.Iso
                          group rep by new { rep.Region.Iso, rep.Region.Province } into regionesX
                          select new
                          Report
                          (
                               regionesX.Key.Iso, regionesX.Key.Province, regionesX.Sum(x => x.Confirmed), regionesX.Sum(x => x.Deaths)
                          )
                        ).OrderByDescending(x => x.Cases).Take(10);

                Global.lstReport = lst;

            }
             
            return Global.lstReport ;
        }
    }
}