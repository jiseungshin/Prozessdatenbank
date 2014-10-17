using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PGrindingPhoenixProcess : BaseProcess
    {
        public PGrindingPhoenixProcess()
        {
            Description = "";
        }

        public string Description { get; set; }
        public double Ra { get; set; }
    }
}
