using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Green.Web.Scrapper.Interfaces
{
    /// <summary>
    /// Used for Queued Jobs.
    /// </summary>
    public interface IJobQueuedRequest : IBasic
    {
        Guid RequestId { get; set; }
    }
}
