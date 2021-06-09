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
        public async Task TestMethod1Async()
        {

            IEnumerable<Report> serv = await Services.CovidAPI.getReportAsync("P", "USA");
              
            Assert.IsNull(serv);
             
        }
    }
}
