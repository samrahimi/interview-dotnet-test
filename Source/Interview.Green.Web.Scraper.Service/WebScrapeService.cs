using Interview.Green.Web.Scraper.Interfaces;
using Interview.Green.Web.Scraper.Models;
using System.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Interview.Green.Web.Scraper.Service
{
    public class WebScrapeService 
    {
        public const double httpRequestTimeout = 15000.0; //in milliseconds
        /// <summary>
        /// Scrapes a webpage asynchronously, adds the result to 
        /// the WebScrapeJobRequest, and calls JobSchedulerService.OnJobCompleted  
        /// when done. If there is an error, request.Result.rawHTML will be set to null
        /// Note that this method blocks and should not be called on the main thread 
        /// </summary>
        public static async Task ScrapeWebsiteAsync(WebScrapeJobRequest request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(httpRequestTimeout);

                try
                {
                    Task<string> scrapeTask = client.GetStringAsync(request.Url);
                    Debug.WriteLine("Begin Job ID " + request.Id+", Url:"+ request.Url);
                    string html = await scrapeTask;
                    Debug.WriteLine("Job ID " + request.Id + " completed successfully");
                    request.Result = new WebScrapeJobResult { rawHTML = html };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Job ID " + request.Id + " completed with error: " + ex.Message);
                    request.Result = null;
                }
                finally
                {
                    JobSchedulerService.OnJobCompleted(request);
                }
            }
        }


        /// <summary>
        /// TODO: Implement parsing and filtering, JQuery style. 
        /// </summary>
        /// <param name="rawHTML"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        private static List<string> parseAndQuery(string rawHTML, string selector)
        {
            return new List<string>();
        }
    }
}