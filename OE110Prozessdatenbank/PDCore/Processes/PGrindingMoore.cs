using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PGrindingMoore : BaseProcess
    {
        public PGrindingMoore()
        {
            UserID = -1;
            //ProjectID = -1;
            //IssueID = -1;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.ProcessQuality();
        }

        public double? ToolRadius { get; set; }
        public double? Feed { get; set; }
        public double? TippRadius { get; set; }
        public string GrindingDirection { get; set; }
        public int? RA { get; set; }
        public int? PV { get; set; }
        public double? InFeed { get; set; }
        public double? ToolSpeed { get; set; }
        public double? GrindingWheelSpeed { get; set; }
        public bool PostProduction { get; set; }

    }
}
