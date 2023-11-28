using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Connection.PacketHelper
{
    public interface IPacketSettings
    {
        int[] AreaLengths { get; set; }
        int MaxStringLength { get; set; }
        int MaxAreaLength { get; set; }
        char Delimeter { get; set; }
        /// <summary>
        /// A character that fill the rest of the length.
        /// </summary>
        /// <value></value>
        char FillChar { get; set; }
    }
}
