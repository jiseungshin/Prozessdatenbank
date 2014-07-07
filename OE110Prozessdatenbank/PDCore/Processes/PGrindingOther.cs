using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PGrindingOther : BaseProcess
    {
        public PGrindingOther()
        {
            UserID = -1;
            //ProjectID = -1; IssueID = -1;

            Workpieces = new List<BusinessObjects.Workpiece>();
        }
    }
}
