using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Green.Web.Scrapper.Interfaces
{
    /// <summary>
    ///  Common interface used by other objects that share these properties.
    /// </summary>
    public interface IBasic
    {
        int Id { get; set; }
        DateTime RequestedAt { get; set; }
    }
}
