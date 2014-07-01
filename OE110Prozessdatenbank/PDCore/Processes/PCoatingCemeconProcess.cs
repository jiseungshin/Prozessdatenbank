using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PCoatingCemeconProcess : BaseProcess
    {
        public PCoatingCemeconProcess()
        {
            ProgramNumber = -1;
            Date = DateTime.Now;
            AdherentLayerID = -1;
            ProtectiveLayerID = -1;

        }
        public int ProgramNumber { get; set; }
        public Layer? AdherentLayer { get; set; }
        public Layer? ProtectiveLayer { get; set; }

        public int AdherentLayerID { get; set; }
        public int ProtectiveLayerID { get; set; }
        public int? Thickness { get; set; }
        public bool isDecoating { get; set; }

        public string Description { get { return "ID: " + ProgramNumber + " , HS: " + AdherentLayer.GetValueOrDefault().Structure + " , SS: " + ProtectiveLayer.GetValueOrDefault().Structure; } }

        public struct Layer
        {
            public int ID { get; set; }
            public string Structure { get; set; }
        }
    }
}
