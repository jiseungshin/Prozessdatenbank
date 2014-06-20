using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public abstract class BaseProcess
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int? ProjectID { get; set; }
        public int? IssueID { get; set; }
        public DateTime Date { get; set; }
        public string Remark { get; set; }

    }

    
}
