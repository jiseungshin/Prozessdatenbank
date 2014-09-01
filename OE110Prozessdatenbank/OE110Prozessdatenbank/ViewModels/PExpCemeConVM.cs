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
    public class PExpCemeConVM : BaseViewModel
    {

        private PExpCemeCon m_process;
        private bool m_update = false;

        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();
        public PExpCemeConVM(int ID, bool update)
        {
            ObjectManager.Instance.update(DBUser.Table);
            ObjectManager.Instance.update(DBProjects.Table);
            ObjectManager.Instance.update(DBIssues.Table);
            ObjectManager.Instance.update(DBGlasses.Table);
            m_update = update;
            //update
            if (update)
            {
                m_process = ProcessManager.Instance.getProcess(ID, 33) as PExpCemeCon;
                WorkpieceQualityControl = new Controls.CQuality(m_process.Workpieces[0]);
                
            }

            //new Process
            else 
            {
                m_process = new PExpCemeCon();
                m_process.ProjectID = ObjectManager.Instance.getProjectID(ID);
                m_process.IssueID = ObjectManager.Instance.getIssueID(ID);
                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(ID, null));

                WorkpieceQualityControl = new Controls.CQuality(m_process.Workpieces[0]);

                //set User field if user is logged in
                if (UserManager.CurrentUser != null)
                    m_process.UserID = UserManager.CurrentUser.ID;
            }
            ProcessQualityControl = new Controls.CProcessQuality(m_process);
            
            SaveProcess = new RelayCommand(Save, CanSave);

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users.FindAll(item => item.isActive)); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<Issue> Issues { get { return m_issues; } } 
        public ObservableCollection<Glass> Glasses { get { return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses); } }


        public DataTable AvailableProcesses
        {
            get { return ProcessManager.Instance.getData(Queries.QueryProcessedCemeCon).Tables[0]; }
        }

        public DataRowView SelectedProcess
        {
            set
            {
                int PID = value.Row.Field<int>(DBExpCemeCon.ID);

                PExpCemeCon _p = ProcessManager.Instance.getProcess(PID, 33) as PExpCemeCon;

                m_process.Atmosphere = _p.Atmosphere;
                m_process.Duration = _p.Duration;
                m_process.GlassID = _p.GlassID;
                m_process.Pressure = _p.Pressure;
                m_process.ProcessID = _p.ProcessID;
                m_process.Temperature = _p.Temperature;


                m_process.ProjectID = _p.ProjectID;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == _p.ProjectID));
                NotifyPropertyChanged("Project");
                m_process.IssueID = _p.IssueID;
                NotifyPropertyChanged("Issues");
                NotifyPropertyChanged("Issue");

                NotifyPropertyChanged("Glass");
                NotifyPropertyChanged("Duration");
                NotifyPropertyChanged("Atmosphere");
                NotifyPropertyChanged("Pressure");
                NotifyPropertyChanged("ProcessID");
                NotifyPropertyChanged("Temperature");
            }
        }

        public DateTime Date
        { get { return m_process.Date; } set { m_process.Date = value; } }
        public string Remark
        {
            get { return m_process.Remark; }
            set { m_process.Remark = value; }
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

        public Glass Glass
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.Glasses.Single(item => item.ID == m_process.GlassID) as Glass;
                }
                catch { return null; }
            }

            set
            {
                m_process.GlassID = value.ID;
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
                catch { m_process.IssueID = null; }
            }
        }

        public string WorkpieceLabel
        { get { return m_process.Workpieces[0].Label; } }

        public int? ProcessID
        { get { return m_process.ProcessID; } set { m_process.ProcessID = value; } }

        public double? Temperature
        { get { return m_process.Temperature; } set { m_process.Temperature = value; } }

        public double? Duration
        { get { return m_process.Duration; } set { m_process.Duration = value; } }

        public string Atmosphere
        { get { return m_process.Atmosphere; } set { m_process.Atmosphere = value; } }

        public double? Pressure
        { get { return m_process.Pressure; } set { m_process.Pressure = value; } }


        public Controls.CQuality WorkpieceQualityControl { get; set; }
        public Controls.CProcessQuality ProcessQualityControl { get; set; }


        public RelayCommand SaveProcess { get; set; }

        #region Command functions
        public void Save()
        {
            if (m_update)
                ProcessManager.Instance.saveProcess(m_process,  true);
            else
                ProcessManager.Instance.saveProcess(m_process,  false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.ProjectID != -1 && m_process.IssueID != -1 /*&& m_process.ProcessID!=null*/)
                return true;
            else
                return false;
        }

        #endregion
    }
}
