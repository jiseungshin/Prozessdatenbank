using System;
using System.Collections.Generic;
using PDCore.BusinessObjects;

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
        public List<Workpiece> Workpieces { get; set; }
        public ProcessQuality Quality { get; set; }

    }

    
}
