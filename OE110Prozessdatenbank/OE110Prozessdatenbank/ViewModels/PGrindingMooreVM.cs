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
    public class PGrindingMooreVM : BaseViewModel
    {
        private PGrindingMoore m_process;
        private bool m_update = false;
        private Workpiece _wp;

        public PGrindingMooreVM(int RefID, bool update)
        {
            ObjectManager.Instance.update();
            SaveProcess = new RelayCommand(Save, CanSave);
            m_update = update;

            if (!update)
            {
                m_process = new PGrindingMoore();
                _wp = ObjectManager.Instance.getWorkpiece(RefID);
                m_process.Date = DateTime.Now;
            }
            else
            {
                m_process = ProcessManager.Instance.getProcessByReference(RefID, 2) as PGrindingMoore;             
            }           
               
        }

        #region get/set

        public RelayCommand SaveProcess { get; set; }
        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } } 
        public int? InFeed { get { return m_process.InFeed; } set { m_process.InFeed = value; } }
        public int? Feed { get { return m_process.Feed; } set { m_process.Feed = value; } }
        public string GrindingDirection { get { return m_process.GrindingDirection; } set { m_process.GrindingDirection = value; } }
        public int? GrindingWheelSpeed { get { return m_process.GrindingWheelSpeed; } set { m_process.GrindingWheelSpeed = value; } }
        public bool Speed { get { return m_process.PostProduction; } set { m_process.PostProduction = value; } }
        public int? TippRadius { get { return m_process.TippRadius; } set { m_process.TippRadius = value; } }
        public int? ToolRadius { get { return m_process.ToolRadius; } set { m_process.ToolRadius = value; } }
        public int? ToolSpeed { get { return m_process.ToolSpeed; } set { m_process.ToolSpeed = value; } }
        public int? PV { get { return m_process.PV; } set { m_process.PV = value; } }
        public int? RA { get { return m_process.RA; } set { m_process.RA = value; } } 
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
            get { return ProcessManager.Instance.getData(Queries.QueryGrindingMoore).Tables[0]; }
        }

        public DataRowView SelectedProcess
        {
            set 
            {
                int ID = value.Row.Field<int>(DBGrindingMoore.ID);
                int _refID = ProcessManager.Instance.getReference(ID)[0];
                PGrindingMoore _p = ProcessManager.Instance.getProcessByProcessID(ID,_refID, 2) as PGrindingMoore;

                m_process.GrindingDirection = _p.GrindingDirection;
                m_process.GrindingWheelSpeed = _p.GrindingWheelSpeed;
                m_process.Feed = _p.Feed;
                m_process.InFeed = _p.InFeed;
                m_process.PostProduction = _p.PostProduction;
                m_process.Remark = _p.Remark;
                m_process.TippRadius = _p.TippRadius;
                m_process.ToolRadius = _p.ToolRadius;
                m_process.UserID = _p.UserID;
                m_process.ToolSpeed = _p.ToolSpeed;
                m_process.ProjectID = _p.ProjectID;

                NotifyPropertyChanged("GrindingDirection");
                NotifyPropertyChanged("GrindingWheelSpeed");
                NotifyPropertyChanged("Feed");
                NotifyPropertyChanged("InFeed");
                NotifyPropertyChanged("PostProduction");
                NotifyPropertyChanged("Remark");
                NotifyPropertyChanged("TippRadius");
                NotifyPropertyChanged("ToolRadius");
                NotifyPropertyChanged("User");
                NotifyPropertyChanged("ToolSpeed");
                NotifyPropertyChanged("Project");

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
            if (m_process.UserID !=-1 && m_process.ProjectID != -1)
                return true;
            else
                return false;
        }

        #endregion
    }
}
