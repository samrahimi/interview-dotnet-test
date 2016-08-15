using System;
using Interview.Green.Web.Scraper.Interfaces;

namespace Interview.Green.Web.Scraper.Models
{
    public class WebScrapeJobRequest : IJobQueuedRequest
    {

        public int Id { get; set; }
        public DateTime RequestedAt { get; set; }
        public Guid RequestId { get; set; }
        public string Url { get; set; } 
        public string Selector { get; set; }
        public JobRequestStatus Status { get; set; }
        public WebScrapeJobResult Result { get; set; }
     

        /// <summary>
        /// Creates a new web scrape job request
        /// </summary>
        /// <param name="url">The url to scrape</param>
        /// <param name="selector">Jquery-style selector for filtering the results. If null, the result will contain the scraped page as HTML</param>
        public WebScrapeJobRequest(string url, string selector)
        {
            this.RequestedAt = DateTime.UtcNow;
            this.RequestId = new Guid();
            this.Url = url;
            this.Selector = selector;
            this.Status = JobRequestStatus.Pending;
        }
    }

    public enum JobRequestStatus {Pending, Queued, InProgress, Completed, Error}
}