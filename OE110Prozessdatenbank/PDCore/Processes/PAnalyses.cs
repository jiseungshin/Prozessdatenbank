using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;

namespace PDCore.Processes
{
    public class Analysis
    {
        public Analysis()
        {
            ID = -1;
            Started = DateTime.Now;
            Finished = null;
            Path = "D:\\";
        }
        public int ID { get; set; }
        public User User { get; set; }
        public string Path { get; set; }

        public int ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string EnumDatabase { get; set; }


        public DateTime? Started { get; set; }
        public DateTime? Finished { get; set; }
    }

    
}
