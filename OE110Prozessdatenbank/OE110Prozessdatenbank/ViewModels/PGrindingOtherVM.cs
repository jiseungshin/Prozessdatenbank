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
    public  class PGrindingOtherVM : BaseViewModel
    {
        private PGrindingOther m_process;
        private bool m_update = false;
        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PGrindingOtherVM(int RefID, bool update)
        {
            ObjectManager.Instance.update();
            SaveProcess = new RelayCommand(Save, CanSave);
            m_update = update;

            if (!update)
            {
                m_process = new PGrindingOther();
                m_process.Date = DateTime.Now;
                if (UserManager.CurrentUser != null)
                    m_process.UserID = UserManager.CurrentUser.ID;

                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpiece(RefID));

            }
            else
            {
                m_process = ProcessManager.Instance.getProcess(RefID, 14) as PGrindingOther;             
            }

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");
            
            
        }

        #region get/set

        public RelayCommand SaveProcess { get; set; }
        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } } 

        public string Remark { get { return m_process.Remark; } set { m_process.Remark = value; } }

        public string WorkpieceLabel
        {
            get
            {
                try
                {
                    return  m_process.Workpieces[0].Label;
                }
                catch { return ""; }
            }
        }


        public DataTable AvailableProcesses
        {
            get { return ProcessManager.Instance.getData(Queries.QueryGrindingOther).Tables[0]; }
        }

        public DataRowView SelectedProcess
        {
            set 
            {
                int ID = value.Row.Field<int>(DBGrindingOther.ID);
                //int _refID = ProcessManager.Instance.getReference(ID)[0];
                PGrindingOther _p = ProcessManager.Instance.getProcess(ID, 14) as PGrindingOther;

                m_process.Remark = _p.Remark;
                //m_process.UserID = _p.UserID;
                m_process.ProjectID = _p.ProjectID;
                m_process.Date = _p.Date;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == _p.ProjectID));
                NotifyPropertyChanged("Issues");
                m_process.IssueID = _p.IssueID;
                NotifyPropertyChanged("Issue");

                NotifyPropertyChanged("Date");
                NotifyPropertyChanged("User");
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
            if (m_process.UserID != -1 )
                return true;
            else
                return false;
        }

        #endregion
    
    }
}
