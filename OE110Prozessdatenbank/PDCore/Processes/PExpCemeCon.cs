using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PExpCemeCon : BaseProcess
    {
        public PExpCemeCon()
        {
            UserID = -1;
            Date = DateTime.Now;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.QualityObject();
        }
        public int? GlassID { get; set; }
        public int? ResultID { get; set; }
        public double? Temperature { get; set; }
        public double? Pressure { get; set; }
        public string Atmosphere { get; set; }
        public double? Duration { get; set; }
        public int? ProcessID { get; set; }
    }
}
