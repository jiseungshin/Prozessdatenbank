using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Glass : BusinessObject
    {
        public string Name { get; set; }
        public string Comapany { get; set; }
        public string VisualName { get { return Name + " (" + Comapany + ")"; } }
    }
}
