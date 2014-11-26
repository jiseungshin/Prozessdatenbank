using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using System.Collections.ObjectModel;
using PDCore.BusinessObjects;
using PDCore.Manager;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PToshibaImportVM : BaseViewModel
    {
        private List<PToshibaVM> m_ProcessVms;
        private Workpiece m_upper;
        private Workpiece m_lower;
        private User m_user;
        private string m_lensName = "";
        private Glass m_glass;
        private int m_LensIndex = 1;

        private Project m_project;
        private Issue m_issue;

        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PToshibaImportVM(List<PExpToshiba> processes)
        {
            m_ProcessVms = new List<PToshibaVM>();
            //if (UserManager.CurrentUser != null)
                m_user = UserManager.CurrentUser;

            foreach(var p in processes)
            {
                m_ProcessVms.Add(new PToshibaVM(p));
            }

        }
        public PToshibaImportVM()
        {

            ObjectManager.Instance.update(PDCore.Database.DBGlasses.Table);

            m_ProcessVms = new List<PToshibaVM>();
            if (UserManager.CurrentUser!=null)
                m_user = ObjectManager.Instance.Users.Find(item => item.ID == UserManager.CurrentUser.ID);

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues);
            NotifyPropertyChanged("Issues");

            Import = new RelayCommand(import, CanImport);
        }

        public ObservableCollection<PToshibaVM> Processes
        {
            get
            {
                foreach (var p in m_ProcessVms)
                { p.plotModelP.InvalidatePlot(false); p.plotModelT.InvalidatePlot(false); }
                return new ObservableCollection<PToshibaVM>(m_ProcessVms);
                
            }

        }

        public RelayCommand Import { get; set; }

        public void addProcess(PExpToshiba process, int index)
        {
            if (UpperWP != null)
            {
                process.Workpieces.Add(UpperWP.clone());
                process.UpperWorkpiece = UpperWP.ID;
            }
            if (LowerWP != null)
            {
                process.Workpieces.Add(LowerWP.clone());
                process.LowerWorkpiece = LowerWP.ID;
            }

            if (Glass != null)
                process.GlassID = Glass.ID;

            process.GlassName = LensName + "_"+index;

            process.UserID = User.ID;
            process.ProjectID = m_project.ID;
            process.IssueID = m_issue.ID;

            m_ProcessVms.Add(new PToshibaVM(process));
            NotifyPropertyChanged("Processes");
        }

        public void RemoveProcess(int index)
        {
            //m_ProcessVms.RemoveAt(index);
            //NotifyPropertyChanged("Processes");
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users.FindAll(item => item.isActive)); } }
        public ObservableCollection<Glass> Glasses { get { return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses); } }
        
        public string LensName
        { get { return m_lensName; } set { m_lensName = value; } }

        public int LensStartNumber
        { get { return m_LensIndex; } set { m_LensIndex = value; } }

        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<Issue> Issues { get { return m_issues; } }

        public Issue Issue
        {
            get
            {
                try
                {
                    return m_issue;// ObjectManager.Instance.Issues.Find(item => item.ID == Processes[0].IssueID);
                }
                catch { return null; }
            }
            set
            {
                m_issue = value;
                //try
                //{
                //    foreach (var p in Processes)
                //    {
                //        p.m_process.IssueID = value.ID;
                //    }
                //}
                //catch
                //{
                //    foreach (var p in Processes)
                //    {
                //        p.m_process.IssueID = null;
                //    }
                //}
            }
        }

        public Project Project
        {
            get 
            {
                try
                {
                    return m_project;// ObjectManager.Instance.Projects.Find(item => item.ID == Processes[0].ProjectID);
                }
                catch { return null; }
            }
            set
            {
                //foreach (var p in Processes)
                //{
                //    p.m_process.ProjectID = value.ID;
                //}
                m_project = value;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == value.ID));
                NotifyPropertyChanged("Issues");
            }
        }

        public ObservableCollection<Workpiece> WorkpiecesUpper
        {
            get
            {

                return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_lower));
                
            }
        }

        public ObservableCollection<Workpiece> WorkpiecesLower
        {
            get
            {

                return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.FindAll(item => item != m_upper));
                
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

                try
                {
                    m_project = ObjectManager.Instance.Projects.Find(item => item.ID == ObjectManager.Instance.getProjectID(m_upper.CurrentReferenceNumber));
                    NotifyPropertyChanged("Project");
                    m_issues = new ObservableCollection<PDCore.BusinessObjects.Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_project.ID));
                    NotifyPropertyChanged("Issues");
                    m_issue = ObjectManager.Instance.Issues.Find(item => item.ID == ObjectManager.Instance.getIssueID(m_upper.CurrentReferenceNumber));

                    NotifyPropertyChanged("Issue");
                }
                catch { }

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

                try
                {
                    m_project = ObjectManager.Instance.Projects.Find(item => item.ID == ObjectManager.Instance.getProjectID(m_lower.CurrentReferenceNumber));
                    NotifyPropertyChanged("Project");
                    m_issues = new ObservableCollection<PDCore.BusinessObjects.Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_project.ID));
                    NotifyPropertyChanged("Issues");
                    m_issue = ObjectManager.Instance.Issues.Find(item => item.ID == ObjectManager.Instance.getIssueID(m_lower.CurrentReferenceNumber));
                }
                catch { }
                
                NotifyPropertyChanged("Issue");

                NotifyPropertyChanged("WorkpiecesUpper");
            }
        }

        public Glass Glass
        {
            get
            {
                try
                {
                    return m_glass;
                }
                catch { return null; }
            }

            set
            {
                m_glass = value;
            }
        }


        public User User
        { get { return m_user; } set { m_user = ObjectManager.Instance.Users.Find(item => item.ID == value.ID); } }

        #region Command functions
        public void import()
        {

        }

        public bool CanImport()
        {
            if (User!=null && Project!=null && Issue !=null && (LowerWP!=null || UpperWP !=null))
                return true;
            else
                return false;
        }

        #endregion

    }
}
