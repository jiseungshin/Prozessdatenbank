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
    public class PCoatingCemeconVM : BaseViewModel
    {

        private bool m_update = false;
        private PCoatingCemecon m_process;
        private ObservableCollection<Issue> m_issues = new ObservableCollection<Issue>();

        public PCoatingCemeconVM(int refID, bool update)
        {
            ObjectManager.Instance.update(DBUser.Table);
            ObjectManager.Instance.update(DBProjects.Table);
            ObjectManager.Instance.update(DBIssues.Table);
            
            ProcessManager.Instance.update();
            m_update = update;
            //newProcess
            if (!update)
            {
                m_process = new PCoatingCemecon();
                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(refID));
                m_process.ProjectID = ObjectManager.Instance.getProjectID(refID);
                m_process.IssueID = ObjectManager.Instance.getIssueID(refID);
                
                m_process.Date = DateTime.Now;

                //set User field if user is logged in
                if (UserManager.CurrentUser != null)
                    m_process.UserID = UserManager.CurrentUser.ID;
            }
            //update
            else
            {
                m_process = ProcessManager.Instance.getProcess(refID, 21) as PCoatingCemecon;

                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
                NotifyPropertyChanged("Issue");
            }

            SaveProcess = new RelayCommand(Save, CanSave);

            m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_process.ProjectID));
            NotifyPropertyChanged("Issues");

           
        }

        public RelayCommand SaveProcess { get; set; }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<PCoatingCemeconProcess> Processes { get { return new ObservableCollection<PCoatingCemeconProcess>(ProcessManager.Instance.CemeConStandardProcesses); } }

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

        public ObservableCollection<Issue> Issues
        { get { return m_issues; } }

        public DataView AvailableProcesses
        {
            get
            {
                DataView dv = ProcessManager.Instance.getData(Queries.QueryCoated).Tables[0].DefaultView;
                dv.Sort = "Date DESC";
                return dv;
            }
        }

        public DataRowView SelectedProcess
        {
            set
            {
                int ID = value.Row.Field<int>(DBCoatingCemecon.ID);
                //int _refID = ProcessManager.Instance.getReference(ID)[0];
                PCoatingCemecon _p = ProcessManager.Instance.getProcess(ID, 21) as PCoatingCemecon;

                m_process.Date = _p.Date;
                m_process.Processnumber = _p.Processnumber;
                //Project = new PDCore.BusinessObjects.Project() { ID = Convert.ToInt32(_p.ProjectID) };
                //NotifyPropertyChanged("Project");
                //Issue = new PDCore.BusinessObjects.Issue() { ID = Convert.ToInt32(_p.IssueID) };
                //NotifyPropertyChanged("Issue");
                //m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == _p.ProjectID));

                m_process.CoatingProcessID = _p.CoatingProcessID;
                m_process.UserID = _p.UserID;

                

                NotifyPropertyChanged("Process");
                NotifyPropertyChanged("Date");
                NotifyPropertyChanged("ProcessNumber");
                NotifyPropertyChanged("User");
                NotifyPropertyChanged("Project");
                NotifyPropertyChanged("Issue");

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

        public PCoatingCemeconProcess Process
        {
            get
            {
                try
                {
                    return ProcessManager.Instance.CemeConStandardProcesses.Single(item => item.ID == m_process.CoatingProcessID) as PCoatingCemeconProcess;
                }
                catch { return null; }
            }

            set
            {
                m_process.CoatingProcessID = value.ID;
               
            }
        }

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

        public int ProcessNumber
        { get { return m_process.Processnumber; } set { m_process.Processnumber = value; } }

        public string Abnormalities
        { get { return m_process.Abnormalities; } set { m_process.Abnormalities = value; } }

        public string Remark
        { get { return m_process.Remark; } set { m_process.Remark = value; } }


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
            if (m_process.CoatingProcessID != -1 && m_process.UserID != -1 && m_process.Processnumber != -1 && m_process.ProjectID!=-1 && m_process.IssueID!= -1)
                return true;
            else
                return false;
        }

        #endregion

    }
}
