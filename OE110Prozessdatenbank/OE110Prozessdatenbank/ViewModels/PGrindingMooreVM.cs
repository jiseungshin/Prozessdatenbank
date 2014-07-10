using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using PDCore.Manager;
using PDCore.Database;
using PDCore.BusinessObjects;
using System.Collections.ObjectModel;
using OE110Prozessdatenbank.Commands;
using System.Data;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PGrindingMooreVM : BaseViewModel
    {
        private PGrindingMoore m_process;
        private bool m_update = false;
        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PGrindingMooreVM(int RefID, bool update)
        {
            ObjectManager.Instance.update();
            SaveProcess = new RelayCommand(Save, CanSave);
            m_update = update;

            if (!update)
            {
                m_process = new PGrindingMoore();
                m_process.Date = DateTime.Now;
                if (UserManager.CurrentUser != null)
                    m_process.UserID = UserManager.CurrentUser.ID;

                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpiece(RefID));
            }
            else
            {
                m_process = ProcessManager.Instance.getProcess(RefID, 12) as PGrindingMoore;             
            }

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");
               
        }

        #region get/set

        public RelayCommand SaveProcess { get; set; }
        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } }
        public double? InFeed { get { return m_process.InFeed; } set { m_process.InFeed = value; } }
        public double? Feed { get { return m_process.Feed; } set { m_process.Feed = value; } }
        public string GrindingDirection { get { return m_process.GrindingDirection; } set { m_process.GrindingDirection = value; } }
        public double? GrindingWheelSpeed { get { return m_process.GrindingWheelSpeed; } set { m_process.GrindingWheelSpeed = value; } }
        public bool Speed { get { return m_process.PostProduction; } set { m_process.PostProduction = value; } }
        public double? TippRadius { get { return m_process.TippRadius; } set { m_process.TippRadius = value; } }
        public double? ToolRadius { get { return m_process.ToolRadius; } set { m_process.ToolRadius = value; } }
        public double? ToolSpeed { get { return m_process.ToolSpeed; } set { m_process.ToolSpeed = value; } }
        public int? PV { get { return m_process.PV; } set { m_process.PV = value; } }
        public int? RA { get { return m_process.RA; } set { m_process.RA = value; } } 
        public string Remark { get { return m_process.Remark; } set { m_process.Remark = value; } }
        public string WorkpieceLabel
        {
            get
            {
                try
                {
                    return m_process.Workpieces[0].Label;
                }
                catch { return ""; }
            }
        }


        public DataTable AvailableProcesses
        {
            get { return ProcessManager.Instance.getData(Queries.QueryGrindingMoore).Tables[0]; }
        }

        public DataRowView SelectedProcess
        {
            set 
            {
                int ID = value.Row.Field<int>(DBGrindingMoore.ID);
                //int _refID = ProcessManager.Instance.getReference(ID)[0];
                PGrindingMoore _p = ProcessManager.Instance.getProcess(ID, 12) as PGrindingMoore;

                m_process.GrindingDirection = _p.GrindingDirection;
                m_process.GrindingWheelSpeed = _p.GrindingWheelSpeed;
                m_process.Feed = _p.Feed;
                m_process.InFeed = _p.InFeed;
                m_process.PostProduction = _p.PostProduction;
                m_process.Remark = _p.Remark;
                m_process.TippRadius = _p.TippRadius;
                m_process.ToolRadius = _p.ToolRadius;
                m_process.UserID = _p.UserID;
                m_process.ToolSpeed = _p.ToolSpeed;

                m_process.ProjectID = _p.ProjectID;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == _p.ProjectID));
                NotifyPropertyChanged("Issues");
                m_process.IssueID = _p.IssueID;
                NotifyPropertyChanged("Issue");

                NotifyPropertyChanged("GrindingDirection");
                NotifyPropertyChanged("GrindingWheelSpeed");
                NotifyPropertyChanged("Feed");
                NotifyPropertyChanged("InFeed");
                NotifyPropertyChanged("PostProduction");
                NotifyPropertyChanged("Remark");
                NotifyPropertyChanged("TippRadius");
                NotifyPropertyChanged("ToolRadius");
                NotifyPropertyChanged("User");
                NotifyPropertyChanged("ToolSpeed");
                NotifyPropertyChanged("Project");

            }
        }

       
        public User User
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.Users.Single(item => item.ID == m_process.UserID) as User;
                }
                catch { return null; }
            }

            set
            {
                 m_process.UserID = value.ID; 

            }
        }

        public Project Project
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.Projects.Single(item => item.ID == m_process.ProjectID) as Project;
                }
                catch { return null; }
            }

            set
            {
                m_process.ProjectID = value.ID;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
                NotifyPropertyChanged("Issues");
            }
        }

        public Issue Issue
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.Issues.Single(item => item.ID == m_process.IssueID) as Issue;
                }
                catch { return null; }
            }

            set
            {
                try
                {
                    m_process.IssueID = value.ID;
                }
                catch { }
            }
        }

        public ObservableCollection<Issue> Issues
        { get { return m_issues; } }

        #endregion

        #region Command functions
        public void Save()
        {
            if (m_update)
                ProcessManager.Instance.saveProcess(m_process, true);
            else
                ProcessManager.Instance.saveProcess(m_process, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.GrindingDirection != null && m_process.GrindingWheelSpeed != null && m_process.InFeed != null &&
                m_process.TippRadius != null && m_process.ToolRadius != null && m_process.ToolSpeed != null)
                return true;
            else
                return false;
        }

        #endregion
    }
}
