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
            AdherentLayer = "";
            ProtectiveLayer = "";
            Date = DateTime.Now;
        }
        public int ProgramNumber { get; set; }
        public string AdherentLayer { get; set; }
        public string ProtectiveLayer { get; set; }
        public int Thickness { get; set; }
        public bool isDecoating { get; set; }

        public string Description { get { return "ID: "+ProgramNumber+" , HS: "+AdherentLayer+" , SS: "+ProtectiveLayer; } }
    }
}
