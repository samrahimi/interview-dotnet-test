using Interview.Green.Web.Scraper.Service;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Interview.Green.Web.Scraper.Controllers
{
    public class JobController : ApiController
    {
        // GET: api/job
        // Returns all jobs sorted by request date
        public IEnumerable<string> Get()
        {
            return JobSchedulerService.GetAllJobs();
        }

        // GET: api/job/5
        public string Get(int id)
        {
            return JobSchedulerService.GetJob(id);
        }

        // POST: api/job
        // Creates a new job
        public void Post([FromBody] string value)
        {
            throw new NotImplementedException();
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