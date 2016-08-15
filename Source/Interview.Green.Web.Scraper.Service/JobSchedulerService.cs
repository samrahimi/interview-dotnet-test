using System;
using System.Collections.Generic;
using Interview.Green.Web.Scraper.Interfaces;
using Interview.Green.Web.Scraper.Models;
using System.Linq;

namespace Interview.Green.Web.Scraper.Service
{
    /// <summary>
    /// Handles job scheduling and execution. This version only supports WebScrapeJobRequest - in the future, 
    /// should be refactored to handle any job request that implements IJobQueuedRequest    
    /// </summary>
    public class JobSchedulerService
    {
        private static int nextId=0;                                    //Assign sequential IDs to jobs as they are scheduled (not needed if using DB with autonumber)
        private const int maxConcurrentJobs = 0;                        //# of jobs that can run simultaneously. 
        private static int jobCount = 0;                                //# of jobs that are currently executing
        private static List<WebScrapeJobRequest> jobs;                  //All job requests (queued + executing + completed)        

        static JobSchedulerService()
        {
            jobs = new List<WebScrapeJobRequest>();
        }

        public static List<WebScrapeJobRequest> GetAllJobs()
        {
            return jobs;
        }

        public static WebScrapeJobRequest GetJob(int id)
        {
            try
            {
                return jobs.Where(x => x.Id == id).Single();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Schedules an WebScrapeJobRequest for execution. If a thread is available, the job will be executed immediately, 
        /// otherwise, it will be queued.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The Job ID</returns>
        public static int ScheduleJob(WebScrapeJobRequest request)
        {
            request.Id = ++nextId;
            jobs.Add(request);

            if (jobCount < maxConcurrentJobs)
            {
                jobCount++;
                request.Status = JobRequestStatus.InProgress;
                WebScrapeService.ScrapeWebsiteAsync(request);
            }
            else
                request.Status = JobRequestStatus.Queued;

            return request.Id;
      
        }
    }
}