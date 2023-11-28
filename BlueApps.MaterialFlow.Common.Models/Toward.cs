using BlueApps.MaterialFlow.Common.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models
{
    public class Toward
    {
        public Direction DriveDirection { get; set; }
        public RoutePosition RoutePosition { get; set; } = new();
        public bool FaultDirection { get; set; }
    }
}
