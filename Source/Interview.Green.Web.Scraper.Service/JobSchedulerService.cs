using System;
using System.Collections.Generic;
using Interview.Green.Web.Scraper.Interfaces;
using Interview.Green.Web.Scraper.Models;

namespace Interview.Green.Web.Scraper.Service
{
    public class JobSchedulerService : IJobSchedulerService
    {
        public static int nextId = 1; //For assigning sequential IDs to jobs as they are scheduled
        public const int maxConcurrentJobs = 4; //Max number of jobs that can run concurrently before they are queued. TODO: make this a config value  
        
        // TODO: IMP JOB SCHEDULE LOGIC HERE.
        public static IEnumerable<WebScrapeJobRequest> GetAllJobs()
        {
            throw new NotImplementedException();
        }

        public static WebScrapeJobRequest GetJob(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Given a request, schedules the job for execution and returns the ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int ScheduleJob(IJobQueuedRequest request)
        {
            request.Id = ++nextId;
            if (request is WebScrapeJobRequest)
                ((WebScrapeJobRequest)request).Result = WebScrapeService.ScrapeWebsite((WebScrapeJobRequest)request);
            else 
                throw new ArgumentException("Unsupported request type");

            return request.Id;
      
        }
    }
}