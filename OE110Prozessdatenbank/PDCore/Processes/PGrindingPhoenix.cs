using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PGrindingPhoenix : BaseProcess
    {

        public PGrindingPhoenix()
        {
            UserID = -1;
            ProjectID = -1;
            ProcessID = -1;
            Quality = new BusinessObjects.ProcessQuality();
            Workpieces = new List<BusinessObjects.Workpiece>();
        }

        public int? ProcessID { get; set; }
    }
}
