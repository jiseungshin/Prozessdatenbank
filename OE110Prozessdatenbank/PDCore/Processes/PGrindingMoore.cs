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
            ProjectID = -1;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.ProcessQuality();
        }

        public int? ToolRadius { get; set; }
        public int? Feed { get; set; }
        public int? TippRadius { get; set; }
        public string GrindingDirection { get; set; }
        public int? RA { get; set; }
        public int? PV { get; set; }
        public int? InFeed { get; set; }
        public int? ToolSpeed { get; set; }
        public int? GrindingWheelSpeed { get; set; }
        public bool PostProduction { get; set; }

    }
}
