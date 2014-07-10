using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Project : BusinessObject
    {
        private List<Issue> m_issues;
        public Project()
        {
            m_issues = new List<Issue>();
            Started = DateTime.Now;
            Finished = null;
            User = null;
            Description = "";
            
        }
        public string Description { get; set; }
        public string OLDDescription { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }
        public List<Issue> Issues { get { return m_issues; } set { m_issues = value;} }
        public string Remark { get; set; }
        public User User { get; set; }
        
    }

    public class Issue : BusinessObject
    {
        public string Description { get; set; }
        public string OLDDescription { get; set; }
        public int ProjectID { get; set; }
        public string Remark { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string Conclusion { get; set; }
    }
}
