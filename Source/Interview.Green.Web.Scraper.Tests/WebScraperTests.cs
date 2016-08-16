using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interview.Green.Web.Scraper.Controllers;
using Interview.Green.Web.Scraper.Models;
using Interview.Green.Web.Scraper.Service;

namespace Interview.Green.Web.Scraper.Tests
{
    /// <summary>
    /// The following tests assume that JobSchedulerService.maxConcurrentJobs > 1 
    /// </summary>
    [TestClass]
    public class WebScraperTests
    {

        [TestMethod]
        public void VerifyAsynchronousScrape()
        {
            JobController jc = new JobController();
            int newid = jc.Post("http://www.google.com", null);
            WebScrapeJobRequest job = jc.Get(newid);
            Assert.AreEqual(job.Status, JobRequestStatus.InProgress);
            System.Threading.Thread.Sleep((int)WebScrapeService.httpRequestTimeout + 1000);
            job = jc.Get(newid);
            Assert.AreEqual(job.Status, JobRequestStatus.Completed);
            Assert.IsFalse(String.IsNullOrEmpty(job.Result.rawHTML));
        }

        [TestMethod]
        public void VerifyAsyncCalling()
        {
            JobController jc = new JobController();
            var newid = jc.Post("http://www.google.com", null);
            Assert.AreNotEqual(newid, 0);
        }

    [TestMethod]
        public void VerifyConcurrentExecution()
        {
            JobController jc = new JobController();
            int j1 = jc.Post("http://www.google.com", null);
            int j2 = jc.Post("http://www.leafly.com", null);
            WebScrapeJobRequest job1 = jc.Get(j1);
            WebScrapeJobRequest job2 = jc.Get(j2);
            Assert.AreEqual(job1.Status, JobRequestStatus.InProgress);
            Assert.AreEqual(job2.Status, JobRequestStatus.InProgress);
            System.Threading.Thread.Sleep((int)WebScrapeService.httpRequestTimeout + 1000);
            Assert.AreEqual(job1.Status, JobRequestStatus.Completed);
            Assert.AreEqual(job2.Status, JobRequestStatus.Completed);

        }
    }
}
