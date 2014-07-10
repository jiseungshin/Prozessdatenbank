using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Reference
    {
        public int ReferenceNumber { get; set; }
        public Project Project { get; set; }
        public Issue Issue { get; set; }
    }
}
