using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interview.Green.Web.Scraper.Controllers;
using Interview.Green.Web.Scraper.Models;
using Interview.Green.Web.Scraper.Service;
using System.Collections.Generic;

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
        public void ScrapeLeaflyDotCom()
        {
            JobController jc = new JobController();
            //Hit Leafly.com and scrape the description of the Blue Dream strain
            //This retrieves a plaintext description and should work any of the Leafly strains (at the present time)
            //Note that if they update the structure of their HTML or if the description of Blue Dream changes
            //this test will no longer work
            var newid = jc.Post("https://www.leafly.com/hybrid/blue-dream", "div[itemprop='description']");
            Assert.AreNotEqual(newid, 0);
            System.Threading.Thread.Sleep((int)WebScrapeService.httpRequestTimeout + 1000);
            var job = jc.Get(newid);
            Assert.AreEqual(job.Status, JobRequestStatus.Completed);
            Assert.IsFalse(String.IsNullOrEmpty(job.Result.rawHTML));

            //Check that the query worked - the Leafly description of "blue dream" should contain the word "sativa" 
            Assert.IsTrue(job.Result.queryResults[0].Contains("sativa")); 
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


        /// <summary>
        /// Verifies that the queuing system is 
        /// functioning - if there are fewer concurrent jobs than 
        /// JobSchedulerService.maxConcurrentJobs, new jobs should be 
        /// executed immediately - otherwise they should be 
        /// should be queued and executed in the order they were received.
        /// </summary>
        [TestMethod]
        public void TestSchedulingService()
        {
            JobController jc = new JobController();
            List<int> jobIds = new List<int>();

            //Make 6 requests and confirm execution status: 
            //Assumes: 3 < JobSchedulerService.maxConcurrentJobs < 6
            for (var i = 1; i <= 6; i++)
            {
                var jobId = jc.Post("http://google.com", null);
                jobIds.Add(jobId);
                if (i <= JobSchedulerService.maxConcurrentJobs)
                    Assert.AreEqual(jc.Get(jobId).Status, JobRequestStatus.InProgress);
                else
                    Assert.AreEqual(jc.Get(jobId).Status, JobRequestStatus.Queued);
            }

            //Wait until the first batch of jobs have either completed or timed out
            System.Threading.Thread.Sleep((int)WebScrapeService.httpRequestTimeout + 1000);

            //Verify that the first batch is completed, and the remaining jobs are either in progress
            //or have completed.
            for (var i = 1; i <= 6; i++)
            {
                if (i <= JobSchedulerService.maxConcurrentJobs)
                    Assert.AreEqual(jc.Get(jobIds[i - 1]).Status, JobRequestStatus.Completed);
                else
                {
                    var status = jc.Get(jobIds[i - 1]).Status;
                    Assert.IsTrue(status == JobRequestStatus.Completed || status == JobRequestStatus.InProgress);
                }
            }
            
            //TODO: improve test coverage...
        }


    }
}
