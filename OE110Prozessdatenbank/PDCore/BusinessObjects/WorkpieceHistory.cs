using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;

namespace PDCore.BusinessObjects
{
    public class WorkpieceHistory
    {
        public WorkpieceHistory()
        {

            Processes = new List<ProcessMetaData>();
        }
        public int ReferenceNumber { get; set; }
        public Workpiece Workpiece { get; set; }
        public Project Project { get; set; }
        public Issue Issue { get; set; }
        public string Conclusion { get; set; }
        public string Status { get; set; }
        public List<ProcessMetaData> Processes { get; set; }

    }

    public class ProcessMetaData
    {
        public DateTime Date { get; set; }
        public Machine Machine { get; set; }
        public User User { get; set; }
        public int PID { get; set; }
        public string Table { get; set; }

    }
}
