using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class BaseProcess
    {
        public long ID { get; set; }
        public int UserID { get; set; }
        public int? ProjectID { get; set; }
        public int? IssueID { get; set; }
        public DateTime Date { get; set; }
        public string Remark { get; set; }


    }

    public static class ObjectExtensions
    {
        public static string ToDBObject(this object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            else
                return "'" + obj + "'";

        }
    }   
}
