using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class ProcessQuality
    {
        public int ID { get; set; }
        public int ProcessID { get; set; }
        public bool GlassSratches { get; set; }
        public bool GlassPeeling { get; set; }
        public bool GlassBreakage { get; set; }
        public int OverallResult { get; set; }
        public int? PV { get; set; }

    }

    public class WorkpieceQuality
    {
        public WorkpieceQuality()
        {
            MoldScratches = 0;
            Corrosion = 0;
            GlassAdherence = 0;
            OverallResult = 0;
        }

        public int ID { get; set; }
        public int ReferenceNumber { get; set; }
        public int? MoldScratches { get; set; }
        public int? Corrosion { get; set; }
        public int? GlassAdherence { get; set; }
        public int? OverallResult { get; set; }
    }
}
