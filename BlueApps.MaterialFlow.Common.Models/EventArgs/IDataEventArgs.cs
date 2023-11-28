using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models.EventArgs
{
    public interface IDataEventArgs
    {
         DateTime AtTime { get; set; }
    }
}
