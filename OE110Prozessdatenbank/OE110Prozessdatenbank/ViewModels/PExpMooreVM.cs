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
        public PExpMooreVM(int PID)
        {
            m_update = true;
            m_process = ProcessManager.Instance.getProcess(PID, 6) as PExpMoore;
            if (m_process.UpperWP !=null)
                UpperWP = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.UpperWP));
            if (m_process.LowerWP != null)
                LowerWP = ObjectManager.Instance.getWorkpiece(Convert.ToInt32(m_process.LowerWP));
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public PExpMooreVM()
        {
            m_process = new PExpMoore();
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public RelayCommand SaveProcess { get; set; }

        #region workpieces

        public Controls.CQuality WP_UpperControl { get; set; }
        
        public Controls.CQuality WP_LowerControl { get; set; }

        public ObservableCollection<Workpiece> Workpieces
        {
            get
            {
                if (m_update)
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces);
                else
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.getCoatedWorkpieces());
            }
        }

        public Workpiece UpperWP
        {
            get { return Workpieces.Single(item => item.ID == m_upper.ID); }
            set
            {
                m_upper = new Workpiece();
                m_upper.CurrentRefereneNumber = value.CurrentRefereneNumber;
                m_upper.Label = value.Label;
                m_upper.ID = value.ID;
                m_process.UpperWP = value.ID;
                WP_UpperControl = new Controls.CQuality(UpperWP);
            }
        }

        public Workpiece LowerWP
        {
            get { return Workpieces.Single(item => item.ID == m_lower.ID); }
            set
            {
                m_lower = new Workpiece();
                m_lower.CurrentRefereneNumber = value.CurrentRefereneNumber;
                m_lower.Label = value.Label;
                m_lower.ID = value.ID;
                m_process.LowerWP = value.ID;
                WP_LowerControl = new Controls.CQuality(LowerWP);
            }
        }

        #endregion

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<Issue> Issues { get { return new ObservableCollection<Issue>(ObjectManager.Instance.Issues); } }
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
                m_process.IssueID = value.ID;
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
                    m_process.Workpieces.Add(LowerWP);
                if (UpperWP != null)
                    m_process.Workpieces.Add(UpperWP);
                ProcessManager.Instance.saveProcess(m_process, false);
            }

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.ProjectID != -1 && m_process.IssueID != -1)
                return true;
            else
                return false;
        }

        #endregion

    }
}
