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
        public const int maxConcurrentJobs = 4;                         //# of jobs that can run simultaneously. 
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

        //Deletes a job, unless it is in the middle of (asynchronous) execution. In that case, don't mess with it.
        public static void DeleteJob(int id)
        {
            var job = GetJob(id);
            if (job == null)
                throw new ArgumentException("Job not found");
            if (job.Status == JobRequestStatus.InProgress)
                throw new InvalidOperationException("Cannot delete a job that's in progress"); 

            jobs.Remove(job);
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
                var t= WebScrapeService.ScrapeWebsiteAsync(request);
            }
            else
                request.Status = JobRequestStatus.Queued;

            return request.Id;
        }

        //Called by WebScrapeService after the job is complete
        public static void OnJobCompleted(WebScrapeJobRequest request)
        {
            if (!String.IsNullOrEmpty(request.Result.rawHTML))
                request.Status = JobRequestStatus.Completed;
            else
                request.Status = JobRequestStatus.Error; //A null value for rawHTML indicates an exception was encountered during scraping

            if (jobs.Where(x => x.Status == JobRequestStatus.Queued).Count() > 0)
            {
                var nextJob = jobs.Where(x => x.Status == JobRequestStatus.Queued).
                    OrderBy(y => y.Id)
                    .First();

                nextJob.Status = JobRequestStatus.InProgress;
                var t= WebScrapeService.ScrapeWebsiteAsync(nextJob);
            }
            else
            {
                //Queue is empty, free up a thread
                jobCount--;
            }
        }
    }
}