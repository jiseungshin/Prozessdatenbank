using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PTurningMoore : BaseProcess
    {
        public PTurningMoore()
        {
            UserID = -1;
            ProjectID = -1;
            IssueID = -1;
            Processing = 1;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.ProcessQuality();
            CuttingAngle = 0;
        }
        public double? Radius { get; set; }
        public double? Feed { get; set; }
        public double? CuttingAngle { get; set; }
        public double? CuttingDepth { get; set; }
        public double? RA { get; set; }
        public double? PV { get; set; }
        public double? Speed { get; set; }
        public bool isFinish { get; set; }
        public int Processing { get; set; }
        public string ToolID { get; set; }
    }
}
