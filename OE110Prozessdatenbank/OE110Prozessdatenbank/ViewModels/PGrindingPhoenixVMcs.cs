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
    public class PGrindingPhoenixVMcs : BaseViewModel
    {
        private PGrindingPhoenix m_process;
        private bool m_update = false;
        private Workpiece _wp;

        public PGrindingPhoenixVMcs(int RefID, bool update)
        {
            ObjectManager.Instance.update();
            SaveProcess = new RelayCommand(Save, CanSave);
            m_update = update;

            if (!update)
            {
                m_process = new PGrindingPhoenix();
                _wp = ObjectManager.Instance.getWorkpiece(RefID);
                m_process.Date = DateTime.Now;
            }
            else
            {
                m_process = ProcessManager.Instance.getProcessByReference(RefID, 3) as PGrindingPhoenix;             
            }           
            
            
        }

        #region get/set

        public RelayCommand SaveProcess { get; set; }
        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }
        public ObservableCollection<PGrindingPhoenixProcess> Processes { get { return new ObservableCollection<PGrindingPhoenixProcess>(ObjectManager.Instance.PhoenixProcesses); } }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } } 

        public string Remark { get { return m_process.Remark; } set { m_process.Remark = value; } }

        public string WorkpieceLabel
        {
            get
            {
                try
                {
                    return _wp.Label;
                }
                catch { return ""; }
            }
        }


        public DataTable AvailableProcesses
        {
            get { return ProcessManager.Instance.getData(Queries.QueryGrindingPhoenix).Tables[0]; }
        }

        public DataRowView SelectedProcess
        {
            set 
            {
                int ID = value.Row.Field<int>(DBGrindingPhoenix.ID);
                int _refID = ProcessManager.Instance.getReference(ID)[0];
                PGrindingPhoenix _p = ProcessManager.Instance.getProcessByProcessID(ID,_refID, 3) as PGrindingPhoenix;

                m_process.Remark = _p.Remark;
                m_process.UserID = _p.UserID;
                m_process.ProjectID = _p.ProjectID;
                m_process.ProcessID = _p.ProcessID;

                NotifyPropertyChanged("UserID");
                NotifyPropertyChanged("User");
                NotifyPropertyChanged("Project");
                NotifyPropertyChanged("Process");

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
                catch {return null; }
            }

            set
            {
                    m_process.ProjectID = value.ID;
            }
        }

        public PGrindingPhoenixProcess Process
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.PhoenixProcesses.Single(item => item.ID == m_process.ProcessID) as PGrindingPhoenixProcess;
                }
                catch { return null; }
            }

            set
            {
                m_process.ProcessID = value.ID;
            }
        }

        #endregion

        #region Command functions
        public void Save()
        {
            if (m_update)
                ProcessManager.Instance.saveProcess(m_process, null, true);
            else
                ProcessManager.Instance.saveProcess(m_process,new List<Workpiece>(){ _wp}, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.ProjectID != -1 && m_process.ProcessID != -1)
                return true;
            else
                return false;
        }

        #endregion
    }
}
