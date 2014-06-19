﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PTurningMoore : BaseProcess
    {
        public int? Radius { get; set; }
        public int? Feed { get; set; }
        public int? CuttingAngle { get; set; }
        public int? CuttingDepth { get; set; }
        public int? RA { get; set; }
        public int? PV { get; set; }
        public int? Speed { get; set; }
        public bool isFinish { get; set; }
        public int Processing { get; set; }
        public string ToolID { get; set; }
    }
}