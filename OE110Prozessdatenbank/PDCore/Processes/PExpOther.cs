using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PExpOther : BaseProcess
    {
        public PExpOther()
        {
            Date = DateTime.Now;
            Workpieces = new List<BusinessObjects.Workpiece>();
            UserID = -1;
            Quality = new BusinessObjects.ProcessQuality();
        }
    }
}
