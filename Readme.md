
**Backend _.Net_ Interview Coding Exercise** 

> Using the 4.5 .Net Solution provided, build a solution to solve the problem.

**Problem:**  
1. We need an API end-point that has the ability to post a new Web site job and have the api process this job which can be retrieved later by id.  
2. Currently they only job type needed is the web site scrapping job.  
3. Web site scrapping job is a simple job that does the following.  
    * Make a request to website and gathers its response.  
	* If items to scrape were requested the next step should be to process the response and find the items.  
	* Store the result of the job so it can be retrieved later by ID.  
4. This end-points will be hit very heavly, so it has been asked that we implement a job scheduler. All new request are logged and an ID is given back as a response.  
The request ID can be used to check the current status of the job running and return back the results of the job.  

**Solution:**  
1. Interview.Green.Web.Scrapper  
	A. JobControler  - `[api/job]` - Use this controller as you're end-point during this exercise.  

**Hints:**  
1. Look at using Quartz for scheduling.  
2. Be sure to write unit tests for diffrent cases...  
3. Async everything...  
4. The solution has been "mockedup" but don't feel this is how it needs to be implemented.  
5. Concurrency with multiple jobs running.  

**Bonus:**  
1. Solve this issue with out using a database.  
2. Dont use any third party web scrapping frameworks.  
3. Think how this API will be consumed and how it might suggest to improve this.  
4. Documentation & Local repo.  

