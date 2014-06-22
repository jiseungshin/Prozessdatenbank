using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PExpMoore : BaseProcess
    {
        public PExpMoore()
        {
            UserID = -1;
            Date = DateTime.Now;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.ProcessQuality();
        }

        public int? GlassID { get; set; }
        public int? ResultID { get; set; }
        public int? UpperWP { get; set; }
        public int? LowerWP { get; set; }
        public double? Tmin { get; set; }
        public double? Tmax { get; set; }
        public double? TOut { get; set; }
        public string Atmosphere { get; set; }
        public double? Force { get; set; }
        public double? PressTime { get; set; }
        public int? Cycles { get; set; }
        public string ProgramTitle { get; set; }
        public string ROI { get; set; }

    }
}
