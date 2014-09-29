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
    public class PGrindingPhoenixVMcs : BaseViewModel
    {
        private PGrindingPhoenix m_process;
        private bool m_update = false;
        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PGrindingPhoenixVMcs(int RefID, bool update)
        {
            ObjectManager.Instance.update(DBUser.Table);
            ObjectManager.Instance.update(DBProjects.Table);
            ObjectManager.Instance.update(DBIssues.Table);
            ObjectManager.Instance.update(DBWorkpieces.Table);

            SaveProcess = new RelayCommand(Save, CanSave);
            m_update = update;

            if (!update)
            {
                m_process = new PGrindingPhoenix();
                m_process.Date = DateTime.Now;
                if (UserManager.CurrentUser != null)
                    m_process.UserID = UserManager.CurrentUser.ID;

                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpiece(RefID));
            }
            else
            {
                m_process = ProcessManager.Instance.getProcess(RefID, 13) as PGrindingPhoenix;             
            }

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");
            
            
        }

        #region get/set

        public RelayCommand SaveProcess { get; set; }
        public ObservableCollection<User> Users
        {
            get
            {
                if (m_update)
                    return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users);
                else
                    return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users.FindAll(item => item.isActive));
            }
        }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<PGrindingPhoenixProcess> Processes { get { return new ObservableCollection<PGrindingPhoenixProcess>(ObjectManager.Instance.PhoenixProcesses); } }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } } 

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


        public DataView AvailableProcesses
        {
            get
            {
                DataView dv = ProcessManager.Instance.getData(Queries.QueryGrindingPhoenix).Tables[0].DefaultView;
                dv.Sort = "Date DESC";
                return dv;
            }
        }

        public DataRowView SelectedProcess
        {
            set 
            {
                int ID = value.Row.Field<int>(DBGrindingPhoenix.ID);
                //int _refID = ProcessManager.Instance.getReference(ID)[0];
                PGrindingPhoenix _p = ProcessManager.Instance.getProcess(ID, 13) as PGrindingPhoenix;

                m_process.Remark = _p.Remark;
                m_process.UserID = _p.UserID;
               
                m_process.ProjectID = _p.ProjectID;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == _p.ProjectID));
                NotifyPropertyChanged("Issues");
                m_process.IssueID = _p.IssueID;
                NotifyPropertyChanged("Issue");
                
                m_process.ProcessID = _p.ProcessID;

                NotifyPropertyChanged("UserID");
                NotifyPropertyChanged("User");
                NotifyPropertyChanged("Project");
                NotifyPropertyChanged("Process");

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

        public PGrindingPhoenixProcess Process
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.PhoenixProcesses.Single(item => item.ID == m_process.ProcessID) as PGrindingPhoenixProcess;
                }
                catch { return null; }
            }

            set
            {
                m_process.ProcessID = value.ID;
            }
        }

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
            if (m_process.UserID != -1 && m_process.ProcessID != -1)
                return true;
            else
                return false;
        }

        #endregion
    }
}
