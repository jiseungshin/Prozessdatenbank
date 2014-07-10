using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;
using PDCore.Manager;
using OE110Prozessdatenbank.Commands;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.ViewModels
{
    public class OProjectVM : BaseViewModel
    {
        private Project m_project;
        private bool m_update;
        public OProjectVM()
        {
            m_project = new Project();
            save = new RelayCommand(Save, CanSave);
            //Updater.Instance.newData += Instance_newData;
            ObjectManager.Instance.newObjects += Instance_newObjects;
        }

        

        public OProjectVM(int ID)
        {
            m_project = ObjectManager.Instance.Projects.Find(item => item.ID == ID);
            m_project.OLDDescription = m_project.Description;
            m_update = true;
            save = new RelayCommand(Save, CanSave);
            //Updater.Instance.newData += Instance_newData;
            ObjectManager.Instance.newObjects += Instance_newObjects;

        }

        void Instance_newObjects()
        {
            NotifyPropertyChanged("Issues");
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }

        public RelayCommand save { get; set; }

        public Project Project
        { get { return m_project; } }

        public int ProjectID
        { get { return m_project.ID; } set { m_project.ID = value; } }
        public string ProjectName
        { get { return m_project.Description; } set { m_project.Description = value; } }

        public DateTime ProjectStarted
        { get { return m_project.Started; } set { m_project.Started = value; } }

        public DateTime? ProjectFinished
        { get { return m_project.Finished; } set { m_project.Finished = value; } }

        public string ProjectRemark
        { get { return m_project.Remark; } set { m_project.Remark = value; } }

        public ObservableCollection<Issue> Issues
        { get { return new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_project.ID)); } }

        public User User
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.Users.Single(item => item.ID == m_project.User.ID) as User;
                    //return m_project.User;
                }
                catch { return null; }
            }

            set
            {
                m_project.User = value;

            }
        }


        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.saveProject(m_project, true);
            else
                ObjectManager.Instance.saveProject(m_project, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_project.Description !="" && m_project.User!=null)
                return true;
            else
                return false;
        }

        #endregion
    }

    public class OIssueVM : BaseViewModel
    {
        private Issue m_issue;
        private bool m_update;
        public OIssueVM(int Project_ID)
        {
            m_issue = new Issue();
            m_issue.ProjectID = Project_ID;
            save = new RelayCommand(Save, CanSave);

        }

        public OIssueVM(Issue issue)
        {
            m_issue = issue;
            m_issue.OLDDescription = issue.Description;
            m_update = true;
            save = new RelayCommand(Save, CanSave);
            m_update = true;
        }

        public string Description
        { get { return m_issue.Description; } set { m_issue.Description = value; } }

        public string Conclusion
        { get { return m_issue.Conclusion; } set { m_issue.Conclusion = value; } }

        public RelayCommand save { get; set; }

        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.saveIssue(m_issue, true);
            else
                ObjectManager.Instance.saveIssue(m_issue, false);

            Updater.Instance.forceUpdate();
        }


        public bool CanSave()
        {
            if (m_issue.Description != "")
                return true;
            else
                return false;
        }

        #endregion
    }


}
