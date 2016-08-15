using Interview.Green.Web.Scraper.Interfaces;
using System.Collections.Generic;
namespace Interview.Green.Web.Scraper.Models
{
    public class WebScrapeJobResult
    {

        public string rawHTML { get; set; }
        public List<string> queryResults { get; set; }
    }
}