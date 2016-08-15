using Interview.Green.Web.Scraper.Interfaces;
using Interview.Green.Web.Scraper.Models;
using System.Net;
using System;
using System.Collections.Generic;

namespace Interview.Green.Web.Scraper.Service
{
    public class WebScrapService : IWebScrapService
    {
        /// <summary>
        /// Scrapes a webpage.
        /// Note that this method blocks and should not be called on the main thread 
        /// It is intended to be used with a job queuing / scheduling service such as 
        /// JobSchedulerService. Alternatively, we could make this method 
        /// asynchronous (using WebClient.DownloadStringAsync) and simplify the 
        /// JobSchedulerService.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static WebScrapJobResult ScrapeWebsite(string url, string selector)
        {
            WebClient wc = new WebClient();
            WebScrapJobResult result = new WebScrapJobResult();
            try
            {
                result.rawHTML = wc.DownloadString(url);
                if (!string.IsNullOrEmpty(selector))
                    result.queryResults = parseAndQuery(result.rawHTML, selector);
            }
            catch (WebException ex)
            {
                return null;
            }
            return result;
        }


        /// <summary>
        /// TODO: Implement a JQuery-style parser / filter for scraped pages.
        /// A selector of "p" would give the contents of all p tags on the page, and so on...
        /// </summary>
        /// <param name="rawHTML"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        private static List<string> parseAndQuery(string rawHTML, string selector)
        {
            throw new NotImplementedException();
        }
    }
}