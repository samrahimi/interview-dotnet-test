using Interview.Green.Web.Scraper.Service;
using Interview.Green.Web.Scraper.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Interview.Green.Web.Scraper.Controllers
{
    public class JobController : ApiController
    {
        // GET: api/job
        // Returns all jobs sorted by request date
        public IEnumerable<WebScrapeJobRequest> Get()
        {
            return JobSchedulerService.GetAllJobs();
        }

        // GET: api/job/5
        public WebScrapeJobRequest Get(int id)
        {
            return JobSchedulerService.GetJob(id);
        }

        // POST: api/job
        // Creates a new job and schedules it for execution
        // Returns: a WebScrapeJobRequest with status 
        // TODO: refactor so that this endpoint can be used for requesting multiple types of jobs 
        //(currently specific to web scraping jobs only)
        public int Post([FromUri] string url, [FromUri] string selector)
        {
            return JobSchedulerService.ScheduleJob(new WebScrapeJobRequest(url, selector));
        }

        // PUT: api/job/5
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/job/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}