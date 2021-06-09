using covidTop.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace covidTop.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod_getJsonServiceRegionAsync()
        {

            dynamic serv = await Services.CovidAPI.getJsonServiceRegionAsync();

            Assert.IsNotNull(serv);

        }
        [TestMethod]
        public async Task TestMethod_getReportAsync()
        {

            dynamic serv = await Services.CovidAPI.getReportAsync("R", "");

            Assert.IsNotNull(serv);

        }
    }
}
