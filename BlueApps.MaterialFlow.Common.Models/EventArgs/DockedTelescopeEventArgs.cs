using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models.EventArgs
{
    public class DockedTelescopeEventArgs : System.EventArgs
    {
        public IEnumerable<string>? Gates { get; set; }
        /// <summary>
        /// Event time
        /// </summary>
        public DateTime AtTime { get; set; }
    }
}
