using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interview.Green.Web.Scrapper.Interfaces;

namespace Interview.Green.Web.Scrapper.Models
{
    public class JobRequest : IJobQueuedRequest
    {
        public int Id { get; set; }
        public DateTime RequestedAt { get; set; }
        public Guid RequestId { get; set; }
    }
}
