using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;
using PDCore.Manager;
using System.Collections.ObjectModel;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class ProjectHistoryVM : BaseViewModel
    {

        private Project m_project;
        private Issue m_issue;
        private List<WorkpieceHistory> m_history;
        private ObservableCollection<Issue> m_issues;

        public ProjectHistoryVM()
        {
            m_issues = new ObservableCollection<Issue>();
            m_history = new List<WorkpieceHistory>();
            GetHistory = new RelayCommand(getHistory, CanGetHistory);

        }

        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }

        public Project Project
        {
            get
            {
                try
                {
                    return m_project;
                }
                catch { return null; }
            }

            set
            {
                m_project = value;
                m_issues = new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item => item.ProjectID == m_project.ID));
                NotifyPropertyChanged("Issues");
            }
        }

        public Issue Issue
        {
            get
            {
                try
                {
                    return m_issue;
                }
                catch { return null; }
            }

            set
            {
                try
                {
                    m_issue = value;
                }
                catch { }
            }
        }

        public ObservableCollection<Issue> Issues
        { get { return m_issues; } }

        public ObservableCollection<WorkpieceHistory> History
        {
            get 
            {
                if (m_issue != null)
                {
                    return new ObservableCollection<WorkpieceHistory>(m_history.FindAll(item => item.Issue.ID == m_issue.ID));
                }
                else
                    return new ObservableCollection<WorkpieceHistory>(m_history);
            }
        }

        public RelayCommand GetHistory { get; set; }

        public void getHistory()
        {
            m_history.Clear();
            m_history = ProcessManager.Instance.getReferences(m_project.ID);
            NotifyPropertyChanged("History");
        }

        public bool CanGetHistory()
        {
            if (m_project != null)
                return true;
            else
                return false;
        }

    }

    
}
