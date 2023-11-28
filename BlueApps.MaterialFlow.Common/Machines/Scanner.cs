﻿using BlueApps.MaterialFlow.Common.Models.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Machines
{
    public class Scanner : IMachine
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string BasePosition { get; set; }
        public string SubPosition { get; set; }
        public string Barcode { get; set; } = string.Empty;

        public Scanner(string baseposition, string subposition)
        {
            BasePosition = baseposition;
            SubPosition = subposition;
        }

        public override string ToString() => $"{BasePosition} {Id}";
    }
}
