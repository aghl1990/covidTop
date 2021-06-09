using covidTop.Models;
using covidTop.Services;
using covidTop.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace covidTop.Controllers
{
    public class HomeController : Controller
    {
         
        public async Task<ActionResult> Index()
        {
            ViewBag.listRegions = await ListRegionsAsync();
            return View();
        }

        public async Task<PartialViewResult> ViewReport(string iso)
        {
            Global.typeList = (iso == "" ? "Region" : "Province");
            ViewBag.listRegions = Global.typeList;
            return PartialView("ViewReport", await CovidAPI.getReportAsync((iso == "" ? "R" : "P"), iso));
        }

        [HttpPost]
        [AllowMultipleButton(Name = "action", Argument = "ExportToCsv")]
        public async Task<string> ExportToCsv() => Export.ToCsv(Response);

        [HttpPost]
        [AllowMultipleButton(Name = "action", Argument = "ExportToJson")]
        public async Task<string> ExportToJson() => Export.ToJson(Response);

        [HttpPost]
        [AllowMultipleButton(Name = "action", Argument = "ExportToXml")]
        public async Task<string> ExportToXML() => Export.ToXML(Response);

         
        public async Task<List<SelectListItem>> ListRegionsAsync()
        {
            var regions = from reg in await CovidAPI.getJsonServiceRegionAsync()
                          orderby reg.Name ascending
                          select reg;
            
            List<SelectListItem> items = regions.ToList().ConvertAll(
                item =>
                {
                    return new SelectListItem()
                    {
                        Text = item.Name.ToString(),
                        Value = item.Iso.ToString(),
                        Selected = false
                    };
                }

            );
            return items;
        }

    }
}