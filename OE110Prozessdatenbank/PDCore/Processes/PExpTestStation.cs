using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PExpTestStation : BaseProcess
    {
        public PExpTestStation()
        {
            UserID = -1;
            Date = DateTime.Now;
            Workpieces = new List<BusinessObjects.Workpiece>();
            Quality = new BusinessObjects.ProcessQuality();
            NitrogenRate = 2.0;
        }

        public int? GlassID { get; set; }
        public double? Celltemperature { get; set; }
        public string Atmosphere { get; set; }
        public double? PressTemperature { get; set; }
        public double? CoolingTempretaure { get; set; }
        public double? MaxForce { get; set; }
        public double? SecondForce { get; set; }
        public double? PressFedd { get; set; }
        public double? PenDepth { get; set; }
        public double? Duration { get; set; }
        public int? Cycles { get; set; }
        public int? LeftWorkpieceID { get; set; }
        public int? CenterWorkpieceID { get; set; }
        public int? RightWorkpieceID { get; set; }
        public double? NitrogenRate { get; set; }


    }
}
