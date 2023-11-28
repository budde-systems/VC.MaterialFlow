using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models.Machines
{
    public interface IMachine
    {
        string Id { get; set; }
        string Name { get; set; }
        /// <summary>
        /// The position on drawing or at sector
        /// </summary>
        string BasePosition { get; set; }
        /// <summary>
        /// Different machines on same position
        /// </summary>
        string SubPosition { get; set; }
    }
}
