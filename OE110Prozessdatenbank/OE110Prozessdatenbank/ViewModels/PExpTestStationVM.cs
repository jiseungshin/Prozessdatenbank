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
    public class PExpTestStationVM : BaseViewModel
    {

        private PExpTestStation m_process;
        private bool m_update = false;

        private Workpiece m_leftWP;
        private Workpiece m_centerWP;
        private Workpiece m_rightWP;

        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PExpTestStation Process
        { get { return m_process; } }
        public PExpTestStationVM(int PID)
        {
            ObjectManager.Instance.update(DBUser.Table);
            ObjectManager.Instance.update(DBProjects.Table);
            ObjectManager.Instance.update(DBIssues.Table);
            ObjectManager.Instance.update(DBGlasses.Table);
            m_update = true;

            m_process = ProcessManager.Instance.getProcess(PID, 32) as PExpTestStation;

            m_process.ProjectID = ObjectManager.Instance.getProjectID(m_process.Workpieces[0].CurrentReferenceNumber);
            m_process.IssueID = ObjectManager.Instance.getIssueID(m_process.Workpieces[0].CurrentReferenceNumber);

            ProcessQualityControl = new Controls.CProcessQuality(m_process);
            if (m_process.LeftWorkpieceID != null)
            {
                //LeftWorkpiece = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.LeftWorkpieceID));
                LeftWorkpiece = m_process.Workpieces.Find(item => item.ID == m_process.LeftWorkpieceID);
            }
            if (m_process.CenterWorkpieceID != null)
            {
                //CenterWorkpiece = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.CenterWorkpieceID));
                CenterWorkpiece = m_process.Workpieces.Find(item => item.ID == m_process.CenterWorkpieceID);
            }
            if (m_process.RightWorkpieceID != null)
            {
                //RightWorkpiece = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.RightWorkpieceID));
                RightWorkpiece = m_process.Workpieces.Find(item => item.ID == m_process.RightWorkpieceID);
            }

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");

            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public PExpTestStationVM()
        {
            m_update = false;
            ObjectManager.Instance.update(DBUser.Table);
            ObjectManager.Instance.update(DBProjects.Table);
            ObjectManager.Instance.update(DBIssues.Table);
            ObjectManager.Instance.update(DBGlasses.Table);

            m_process = new PExpTestStation();
            ProcessQualityControl = new Controls.CProcessQuality(m_process);

            SaveProcess = new RelayCommand(Save, CanSave);

            //set User field if user is logged in
            if (UserManager.CurrentUser != null)
                m_process.UserID = UserManager.CurrentUser.ID;

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");
        }

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
        public ObservableCollection<Glass> Glasses { get { return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses); } }

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

        public Controls.CQuality WP_LeftControl { get; set; }
        public Controls.CQuality WP_CenterControl { get; set; }
        public Controls.CQuality WP_RightControl { get; set; }
        public Controls.CProcessQuality ProcessQualityControl { get; set; }

        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<Issue> Issues { get { return m_issues; } } 

        public ObservableCollection<Workpiece> WorkpiecesLeft
        {
            get
            {
                if (m_update)
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces);
                else
                {
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_centerWP && item != m_rightWP));
                }
            }
        }

        public ObservableCollection<Workpiece> WorkpiecesCenter
        {
            get
            {
                if (m_update)
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces);
                else
                {
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_leftWP && item != m_rightWP));
                }
            }
        }

        public ObservableCollection<Workpiece> WorkpiecesRight
        {
            get
            {
                if (m_update)
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces);
                else
                {
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_centerWP && item != m_leftWP));
                }
            }
        }

        public Workpiece LeftWorkpiece
        {
            get
            {
                try
                {
                    return WorkpiecesLeft.Single(item => item.ID == m_leftWP.ID);
                }
                catch { return null; }
            }
            set
            {
                m_leftWP = value;
                WP_LeftControl = new Controls.CQuality(value);
                NotifyPropertyChanged("WorkpiecesRight");
                NotifyPropertyChanged("WorkpiecesCenter");
            }
        }

        public Workpiece CenterWorkpiece
        {
            get
            {
                try
                {
                    return WorkpiecesCenter.Single(item => item.ID == m_centerWP.ID);
                }
                catch { return null; }
            }
            set
            {
                m_centerWP = value;
                WP_CenterControl = new Controls.CQuality(value);
                NotifyPropertyChanged("WorkpiecesRight");
                NotifyPropertyChanged("WorkpiecesLeft");
            }
        }

        public Workpiece RightWorkpiece
        {
            get
            {
                try
                {
                    return WorkpiecesRight.Single(item => item.ID == m_rightWP.ID);
                }
                catch { return null; }
            }
            set
            {
                m_rightWP = value;
                WP_RightControl = new Controls.CQuality(value);
                NotifyPropertyChanged("WorkpiecesCenter");
                NotifyPropertyChanged("WorkpiecesLeft");
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
                catch { m_process.IssueID = null; }
            }
        }

        public string Atmosphere
        { get { return m_process.Atmosphere; } set { m_process.Atmosphere = value; } }

        public double? CellTemperature
        { get { return m_process.Celltemperature; } set { m_process.Celltemperature = value; } }

        public double? CoolingTemperature
        { get { return m_process.CoolingTempretaure; } set { m_process.CoolingTempretaure = value; } }

        public double? Duration
        { get { return m_process.Duration; } set { m_process.Duration = value; } }

        public int? Pressure
        { get { return m_process.Cycles; } set { m_process.Cycles = value; } }

        public double? MaxForce
        { get { return m_process.MaxForce; } set { m_process.MaxForce = value; } }

        public double? PenDepth
        { get { return m_process.PenDepth; } set { m_process.PenDepth = value; } }
        public double? PressFeed
        { get { return m_process.PressFedd; } set { m_process.PressFedd = value; } }

        public double? PressTemperature
        { get { return m_process.PressTemperature; } set { m_process.PressTemperature = value; } }

        public double? SecondForce
        { get { return m_process.SecondForce; } set { m_process.SecondForce = value; } }

        public int? Cycles
        { get { return m_process.Cycles; } set { m_process.Cycles = value; } }

        public double? NitrogenRate
        { get { return m_process.NitrogenRate; } set { m_process.NitrogenRate = value; } }


        public RelayCommand SaveProcess { get; set; }

        #region Command functions
        public void Save()
        {
            if (m_update)
                ProcessManager.Instance.saveProcess(m_process, true);
            else
            {
                if (m_leftWP != null)
                {
                    m_process.Workpieces.Add(m_leftWP);
                    m_process.LeftWorkpieceID = m_leftWP.ID;
                }
                if (m_centerWP != null)
                {
                    m_process.Workpieces.Add(m_centerWP);
                    m_process.CenterWorkpieceID = m_centerWP.ID;
                }
                if (m_rightWP != null)
                {
                    m_process.Workpieces.Add(m_rightWP);
                    m_process.RightWorkpieceID = m_rightWP.ID;
                }
                ProcessManager.Instance.saveProcess(m_process, false);
            }

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.ProjectID != -1 && m_process.IssueID != -1
                && m_process.Celltemperature!= null && m_process.PressFedd != null && m_process.PressTemperature !=null
                && m_process.Duration != null && m_process.CoolingTempretaure!=null && m_process.Atmosphere!= null
                && m_process.Cycles != null && m_process.SecondForce != null && (m_leftWP != null || m_rightWP != null || m_centerWP != null))
                return true;
            else
                return false;
        }

        #endregion
    }
}
