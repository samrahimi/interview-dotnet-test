***Scalable Web Scraping Service***  

> API for scraping web pages, and querying them to retrieve specific content. Supports concurrent scraping and job scheduling (queuing) to ensure high availability under load.
 
**Building and Testing:**      
1. Clone this repo, open the solution in Visual Studio 2015 and build. 
2. Run the unit tests in Interview.Green.Web.Scraper.Test 
3. Run the solution and test manually - the endpoint is http://localhost:10820/api/job if running in the VS debugger

**Usage**

1. To request a scraping job, POST to /api/job/?url=[url to scrape]&selector=[CSS selector to query]
2. To check the status and/or view the results of a specific job, GET /api/job/[id] using the id returned by the earlier POST
   *If the status is pending, in progress, queued, or error, no results will be present
   *If the job has completed, the response JSON will contain a Result object - Result.rawHtml contains the entire html page that was retrieved, Result.queryResults contains an array of strings
   *If querying fails or a valid selector is not provided, Result.queryResults will be an empty array.
3. To delete a job, call DELETE /api/job/[id] - jobs cannot be deleted if they are in progress.
4. To get all jobs (with status and results of applicable), GET /api/job

**Configuration**

* To change the maximum number of concurrent jobs, edit the value of maxConcurrentJobs in JobSchedulerService.cs (default is 4)
* The Http request timeout for scraping a page is set by default to 15000ms - to modify this, edit the value of httpRequestTimeout in WebScrapeService.cs

**Known Issues and Possible Improvements**

* In a production version, it would be useful to refactor the above config values into a config file (e.g. App.config) rather than hard-coding them. This would allow them to be modified without redeploying 
* For expediency, the solution uses a 3rd party library for parsing HTML, CsQuery (https://github.com/jamietre/CsQuery) - in my testing, certain advanced queries did not work correctly. A more robust scraping library should be used, as CsQuery is no longer being actively maintained
* It would be useful to add support for batch jobs - given the real world use of such libraries, it would be a common use case to scrape multiple web pages (with the same query) and aggregate the results into a single list. 
* Along the same lines, "spidering" would be a useful feature: given a URL, follow all links matching a selector, then scrape the resulting pages...
* PUT /api/job is not implemented; therefore jobs cannot be edited once submitted. This would be simple to implement if desired. 
* JobSchedulerService and/or WebScrapeService should be made into actual services - this way they can run on their own servers, leaving the web server free to fulfill HTTP requests and not overloading it.
* This solution is specific to web scraping. With a little refactoring and proper use of Interfaces, the job scheduler would be able to handle multiple types of jobs (without having to understand the details of its execution).

**Architecture Summary**

* A job request is POSTed to the API
* The API controller passes the request to the JobSchedulerService 
* The service either queues the job, or spawns a new thread to execute it asynchronously. The id of the job is returned immediately (so that the API is not blocked waiting for scrape jobs to complete)
* When a job completes, the results are attached to the request and the status is updated. If there are requests in the queue, the oldest queued job is then executed. If there are no queued request, the thread is terminated, freeing up a slot for the next request that comes in.
