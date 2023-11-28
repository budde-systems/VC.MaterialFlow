using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models.EventArgs
{
    public class LoadFactorEventArgs : System.EventArgs
    {
        public List<LoadFactor> LoadFactors { get; set; } = new();
        /// <summary>
        /// Event time
        /// </summary>
        public DateTime AtTime { get; set; }
    }

    public struct LoadFactor //TODO: In class?
    {
        public double Factor { get; set; }
        public string? Gate { get; set; }
    }
}
