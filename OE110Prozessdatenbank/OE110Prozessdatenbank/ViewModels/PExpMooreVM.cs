using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;
using PDCore.Processes;
using PDCore.Manager;
using System.Collections.ObjectModel;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PExpMooreVM : BaseViewModel
    {
        private PExpMoore m_process;
        private Workpiece m_upper;
        private Workpiece m_lower;
        private bool m_update = false;


        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PExpMooreVM(int PID)
        {
            ObjectManager.Instance.update();
            m_update = true;
            m_process = ProcessManager.Instance.getProcess(PID, 31) as PExpMoore;
            ProcessQualityControl = new Controls.CProcessQuality(m_process);

            m_process.ProjectID = ObjectManager.Instance.getProjectID(m_process.Workpieces[0].CurrentReferenceNumber);
            m_process.IssueID = ObjectManager.Instance.getIssueID(m_process.Workpieces[0].CurrentReferenceNumber);

            if (m_process.UpperWP != null)
            {
                UpperWP = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.UpperWP));
            }
            if (m_process.LowerWP != null)
            {
                LowerWP = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.LowerWP));
            }
            SaveProcess = new RelayCommand(Save, CanSave);

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");

        }

        public PExpMooreVM()
        {
            ObjectManager.Instance.update();
            m_process = new PExpMoore();
            SaveProcess = new RelayCommand(Save, CanSave);
            ProcessQualityControl = new Controls.CProcessQuality(m_process);

            //set User field if user is logged in
            if (UserManager.CurrentUser != null)
                m_process.UserID = UserManager.CurrentUser.ID;

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");
        }

        public RelayCommand SaveProcess { get; set; }

        #region workpieces

        public Controls.CQuality WP_UpperControl { get; set; }
        
        public Controls.CQuality WP_LowerControl { get; set; }
        public Controls.CProcessQuality ProcessQualityControl { get; set; }

        public List<Workpiece> WorkpiecesUpper
        {
            get
            {
                if (m_update)
                    return /*new ObservableCollection<Workpiece>*/(ObjectManager.Instance.Workpieces);
                else
                {
                    return /*new ObservableCollection<Workpiece>*/(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_lower && item.Quality.PID == null));
                }
            }
        }


        public List<Workpiece> WorkpiecesLower
        {
            get
            {
                if (m_update)
                    return /*new ObservableCollection<Workpiece>*/(ObjectManager.Instance.Workpieces);
                else
                {
                    return /*new ObservableCollection<Workpiece>*/(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_upper && item.Quality.PID == null));
                }
            }
        }

        public Workpiece UpperWP
        {
            get 
            {
                try
                {
                    return WorkpiecesUpper.Single(item => item.ID == m_upper.ID);
                }
                catch { return null; }
            }
            set
            {
                    m_upper = value;
                    WP_UpperControl = new Controls.CQuality(value);

                    //try
                    //{
                    //    m_project = ObjectManager.Instance.Projects.Find(item => item.ID == ObjectManager.Instance.getProjectID(m_upper.CurrentReferenceNumber));
                    //    NotifyPropertyChanged("Project");
                    //    m_issues = new ObservableCollection<PDCore.BusinessObjects.Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_project.ID));
                    //    NotifyPropertyChanged("Issues");
                    //    m_issue = ObjectManager.Instance.Issues.Find(item => item.ID == ObjectManager.Instance.getIssueID(m_upper.CurrentReferenceNumber));

                    //    NotifyPropertyChanged("Issue");
                    //}
                    //catch { }
                    //NotifyPropertyChanged("WorkpiecesUpper");
                    NotifyPropertyChanged("UpperWP");
                    NotifyPropertyChanged("WorkpiecesLower");
            }
        }

        public Workpiece LowerWP
        {
            get
            {
                try
                {
                    return WorkpiecesLower.Single(item => item.ID == m_lower.ID);
                }
                catch { return null; }
            }
            set
            {
                m_lower = value;
                WP_LowerControl = new Controls.CQuality(value);

                //try
                //{
                //    m_project = ObjectManager.Instance.Projects.Find(item => item.ID == ObjectManager.Instance.getProjectID(m_lower.CurrentReferenceNumber));
                //    NotifyPropertyChanged("Project");
                //    m_issues = new ObservableCollection<PDCore.BusinessObjects.Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_project.ID));
                //    NotifyPropertyChanged("Issues");
                //    m_issue = ObjectManager.Instance.Issues.Find(item => item.ID == ObjectManager.Instance.getIssueID(m_lower.CurrentReferenceNumber));
                //}
                //catch { }

                NotifyPropertyChanged("WorkpiecesUpper");
            }
        }

        #endregion

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<Issue> Issues {  get { return m_issues; } } 
        public ObservableCollection<Glass> Glasses { get { return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses); } }

        public DateTime Date
        { get { return m_process.Date; } set { m_process.Date = value; } }

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

        public double? Tmin
        { get { return m_process.Tmin; } set { m_process.Tmin = value; } }

        public double? Tmax
        { get { return m_process.Tmax; } set { m_process.Tmax = value; } }

        public double? TOut
        { get { return m_process.TOut; } set { m_process.TOut = value; } }

        public double? Force
        { get { return m_process.Force; } set { m_process.Force = value; } }

        public double? PressTime
        { get { return m_process.PressTime; } set { m_process.PressTime = value; } }

        public string ROI
        { get { return m_process.ROI; } set { m_process.ROI = value; } }

        public string ProgramTitle
        { get { return m_process.ProgramTitle; } set { m_process.ProgramTitle = value; } }
        public string Atmosphere
        { get { return m_process.Atmosphere; } set { m_process.Atmosphere = value; } }

        public string Remark
        { get { return m_process.Remark; } set { m_process.Remark = value; } }

        public int? Cycles
        { get { return m_process.Cycles; } set { m_process.Cycles = value; } }


        #region Command functions
        public void Save()
        {
            if (m_update)
                ProcessManager.Instance.saveProcess(m_process, true);
            else
            {
                if (LowerWP != null)
                {
                    m_process.Workpieces.Add(LowerWP);
                    m_process.LowerWP = LowerWP.ID;
                }
                if (UpperWP != null)
                {
                    m_process.Workpieces.Add(UpperWP);
                    m_process.UpperWP = UpperWP.ID;
                }
                ProcessManager.Instance.saveProcess(m_process, false);
            }

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.ProjectID != null && m_process.IssueID != null
                && m_process.Atmosphere != null && m_process.Cycles != null && m_process.PressTime != null
                && m_process.ProgramTitle != null && m_process.Tmax != null && m_process.Tmin != null && m_process.TOut != null
                && (m_upper != null || m_lower != null))
                return true;
            else
                return false;
        }

        #endregion

    }
}
