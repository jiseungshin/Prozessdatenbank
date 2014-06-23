using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PCoatingCemecon : BaseProcess
    {
        public PCoatingCemecon()
        {
            CoatingProcessID = -1;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.ProcessQuality();
        }
        public int CoatingProcessID { get; set; }
        public string Abnormalities { get; set; }
        public int Processnumber { get; set; }
    }
}
