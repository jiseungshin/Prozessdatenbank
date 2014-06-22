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
            Processing = 1;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.QualityObject();
        }
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
