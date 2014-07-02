using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using System.Collections.ObjectModel;
using PDCore.BusinessObjects;
using PDCore.Manager;

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

        public PToshibaImportVM(List<PToshiba> processes)
        {
            m_ProcessVms = new List<PToshibaVM>();
            m_user = UserManager.CurrentUser;

            foreach(var p in processes)
            {
                m_ProcessVms.Add(new PToshibaVM(p));
            }

        }
        public PToshibaImportVM()
        {
            m_ProcessVms = new List<PToshibaVM>();
            if (UserManager.CurrentUser!=null)
                m_user = ObjectManager.Instance.Users.Find(item => item.ID == UserManager.CurrentUser.ID); ;
        }

        public ObservableCollection<PToshibaVM> Processes
        { 
            get { return new ObservableCollection<PToshibaVM>(m_ProcessVms.OrderBy(item=>item.Date)); }
        }

        public void addProcess(PToshiba process, int index)
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

            m_ProcessVms.Add(new PToshibaVM(process));
            NotifyPropertyChanged("Processes");
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }
        public ObservableCollection<Glass> Glasses { get { return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses); } }
        
        public string LensName
        { get { return m_lensName; } set { m_lensName = value; } }
        
        public ObservableCollection<Workpiece> Workpieces
        {
            get
            {
                return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces);
            }
        }

        public Workpiece UpperWP
        {
            get
            {
                try
                {
                    return Workpieces.Single(item => item.ID == m_upper.ID);
                }
                catch { return null; }
            }
            set
            {
                m_upper = value;
                //WP_UpperControl = new Controls.CQuality(value);

                NotifyPropertyChanged("Project"); NotifyPropertyChanged("Issue");
            }
        }

        public Workpiece LowerWP
        {
            get
            {
                try
                {
                    return Workpieces.Single(item => item.ID == m_lower.ID);
                }
                catch { return null; }
            }
            set
            {
                m_lower = value;
                //WP_LowerControl = new Controls.CQuality(value);

                NotifyPropertyChanged("Project"); NotifyPropertyChanged("Issue");
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

    }
}
