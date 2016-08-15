using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interview.Green.Web.Scraper.Controllers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Interview.Green.Web.Scraper.Tests
{
    [TestClass]
    public class WebScraperTests
    {

        [TestMethod]
        public void TestPostNewJob()
        {
            JobController jc = new JobController();
            int newid = jc.Post("http://www.google.com", "a");

            Assert.AreNotEqual<int>(0, newid);
        }
    }
}
