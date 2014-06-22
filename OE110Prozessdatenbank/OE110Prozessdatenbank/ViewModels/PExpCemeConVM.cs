﻿using System;
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
        public PExpCemeConVM(int ID, bool update)
        {
            m_update = update;
            //update
            if (update)
            {
                m_process = ProcessManager.Instance.getProcess(ID, 8) as PExpCemeCon;
            }

            //new Process
            else 
            {
                m_process = new PExpCemeCon();
                m_process.ProjectID = ObjectManager.Instance.getProjectID(ID);
                m_process.IssueID = ObjectManager.Instance.getIssueID(ID);
                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(ID));
            }

            //NotifyPropertyChanged("Project");
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<Issue> Issues { get { return new ObservableCollection<Issue>(ObjectManager.Instance.Issues); } }
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
                m_process.ProjectID = value.ID;
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
            if (m_process.UserID != -1 && m_process.ProjectID != -1 && m_process.IssueID != -1)
                return true;
            else
                return false;
        }

        #endregion
    }
}
