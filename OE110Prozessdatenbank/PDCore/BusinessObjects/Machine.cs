using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Machine : BusinessObject
    {
        public string Name { get; set; }
        public int Process { get; set; }
    }
}
