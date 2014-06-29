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
    public class PTurningMooreVM : BaseViewModel
    {

        private PTurningMoore m_process;
        private bool m_update = false;

        public PTurningMooreVM(int RefID, bool update)
        {
            //Update: RefID = WorkPieceID
            ObjectManager.Instance.update();
            SaveProcess = new RelayCommand(Save, CanSave);
            m_update = update;

            if (!update)
            {
                m_process = new PTurningMoore();
                m_process.Date = DateTime.Now;

                if (UserManager.CurrentUser != null)
                    m_process.UserID = UserManager.CurrentUser.ID;

                m_process.Workpieces.Add(ObjectManager.Instance.getWorkpiece(RefID));
            }
            else
            {
                m_process = ProcessManager.Instance.getProcess(RefID, 1) as PTurningMoore;

            }           
            
            
        }

        #region get/set

        public RelayCommand SaveProcess { get; set; }
        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Project> Projects { get { return new ObservableCollection<PDCore.BusinessObjects.Project>(ObjectManager.Instance.Projects); } }

        public DateTime Date { get { return m_process.Date; } set { m_process.Date = value; } } 
        public double? Radius { get { return m_process.Radius; } set { m_process.Radius = value; } }
        public double? Feed { get { return m_process.Feed; } set { m_process.Feed = value; } }
        public double? CuttingAngle { get { return m_process.CuttingAngle; } set { m_process.CuttingAngle = value; } }
        public double? CutDepth { get { return m_process.CuttingDepth; } set { m_process.CuttingDepth = value; } }
        public double? Speed { get { return m_process.Speed; } set { m_process.Speed = value; } }
        public bool isFinish { get { return m_process.isFinish; } set { m_process.isFinish = value; } }
        public int Processing { get { return m_process.Processing; } set { m_process.Processing = value; } }
        public string ToolID { get { return m_process.ToolID; } set { m_process.ToolID = value; } }
        public double? PV { get { return m_process.PV; } set { m_process.PV = value; } }
        public double? RA { get { return m_process.RA; } set { m_process.RA = value; } } 
        public string Remark { get { return m_process.Remark; } set { m_process.Remark = value; } }

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

        public Workpiece Workpiece
        { get { return m_process.Workpieces[0]; } }

        public bool ProcessingKonv
        {
            get
            {
                if (m_process.Processing == 1)
                    return true;
                else
                    return false;
            }

            set 
            {
                if (value == true)
                    m_process.Processing = 1;
                else
                    m_process.Processing = 2;
            }
        }

        public bool ProcessingUltra
        {
            get
            {
                if (m_process.Processing == 2)
                    return true;
                else
                    return false;
            }

            set
            {
                if (value == true)
                    m_process.Processing = 2;
                else
                    m_process.Processing = 1;
            }
        }

        public DataTable AvailableProcesses
        {
            get { return ProcessManager.Instance.getData(Queries.QueryTurningMoore).Tables[0]; }
        }

        public DataRowView SelectedProcess
        {
            set 
            {
                int ID = value.Row.Field<int>(DBTurningMoore.ID);
               // int _refID = ProcessManager.Instance.getReference(ID)[0];
                PTurningMoore _p = ProcessManager.Instance.getProcess(ID, 1) as PTurningMoore;

                m_process.CuttingAngle = _p.CuttingAngle;
                m_process.CuttingDepth = _p.CuttingDepth;
                m_process.Feed = _p.Feed;
                m_process.isFinish = _p.isFinish;
                m_process.Processing = _p.Processing;
                m_process.Radius = _p.Radius;
                m_process.Speed = _p.Speed;
                m_process.ToolID = _p.ToolID;
                m_process.UserID = _p.UserID;
                m_process.ProjectID = _p.ProjectID;

                NotifyPropertyChanged("CuttingAngle");
                NotifyPropertyChanged("CutDepth");
                NotifyPropertyChanged("Feed");
                NotifyPropertyChanged("isFinish");
                NotifyPropertyChanged("ProcessingKonv");
                NotifyPropertyChanged("ProcessingUltra");
                NotifyPropertyChanged("Radius");
                NotifyPropertyChanged("Speed");
                NotifyPropertyChanged("ToolID");
                NotifyPropertyChanged("User");
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
                ProcessManager.Instance.saveProcess(m_process, true);
            else
                ProcessManager.Instance.saveProcess(m_process, false);

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
