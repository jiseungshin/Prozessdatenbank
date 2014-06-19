using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using PDCore.Manager;
using PDCore.BusinessObjects;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PTurningMooreVM
    {

        private PTurningMoore m_process;
        public PTurningMooreVM()
        {
            m_process = new PTurningMoore();
        }

        public PTurningMooreVM(int RefID)
        {
            m_process = ProcessManager.Instance.getProcessByReference(RefID, 1) as PTurningMoore;
        }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } } 
        public int? Radius { get { return m_process.Radius; } set { m_process.Radius = value; } }
        public int? Feed { get { return m_process.Feed; } set { m_process.Feed = value; } }
        public int? CuttingAngle { get { return m_process.CuttingAngle; } set { m_process.CuttingAngle = value; } }
        public int? CuttingDepth { get { return m_process.CuttingDepth; } set { m_process.CuttingDepth = value; } }
        public int? Speed { get { return m_process.Speed; } set { m_process.Speed = value; } }
        public bool isFinish { get { return m_process.isFinish; } set { m_process.isFinish = value; } }
        public int Processing { get { return m_process.Processing; } set { m_process.Processing = value; } }
        public string ToolID { get { return m_process.ToolID; } set { m_process.ToolID = value; } }
        public int? PV { get { return m_process.PV; } set { m_process.PV = value; } }
        public int? RA { get { return m_process.RA; } set { m_process.RA = value; } } 
        public string Remark { get { return m_process.Remark; } set { m_process.Remark = value; } }

        public ObservableCollection<User> Users { get { return ObjectManager.Instance.getUser(); } }
        public ObservableCollection<Project> Projects { get { return ObjectManager.Instance.getProjects(); } }
        public User User
        {
            get 
            {
                return ObjectManager.Instance.getUser().Single(item => item.ID == m_process.UserID) as User;
            }

            set { m_process.UserID = value.ID; }
        }

        public Project Project
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.getProjects().Single(item => item.ID == m_process.ProjectID) as Project;
                }
                catch {return null; }
            }

            set { m_process.ProjectID = value.ID; }
        }
        

    }
}
