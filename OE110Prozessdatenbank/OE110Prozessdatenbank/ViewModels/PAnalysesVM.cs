using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using PDCore.Manager;
using System.Collections.ObjectModel;
using PDCore.BusinessObjects;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PAnalysesVM : BaseViewModel
    {
        List<Analysis> m_analyses;

        int ref_ID = -1;
        public PAnalysesVM(int RefID)
        {
            ref_ID = RefID;
            m_analyses = ProcessManager.Instance.getAnalysis(RefID);
            ProcessManager.Instance.newProcesses += Instance_newProcesses;
        }

        void Instance_newProcesses()
        {
            m_analyses = ProcessManager.Instance.getAnalysis(ref_ID);
            NotifyPropertyChanged("Analyses");
        }

        public ObservableCollection<Analysis> Analyses
        { get { return new ObservableCollection<Analysis>(m_analyses); }
            set { m_analyses = value.ToList(); }
        }

        public string Title
        { get { return "Vorgang " + ref_ID + " - Analysen"; } }

    }

    public class PAnalysisVM : BaseViewModel
    {
        Analysis m_analysis;
        List<AnalyseTypes> m_availNalyses;
        
        public PAnalysisVM(int RefID)
        {

            m_analysis = new Analysis() { ReferenceNumber = RefID };

            m_availNalyses = new List<AnalyseTypes>() 
            {
                new AnalyseTypes(){Description= "REM",DatabaseColumn="REM"},
                new AnalyseTypes(){Description= "XPS",DatabaseColumn="XPS"},
                new AnalyseTypes(){Description= "Weißlicht",DatabaseColumn="WI"},
                new AnalyseTypes(){Description= "Lichtmikroskop",DatabaseColumn="LIMI"},
                new AnalyseTypes(){Description= "XRD",DatabaseColumn="XRD"},
                new AnalyseTypes(){Description= "EBSD",DatabaseColumn="EBSD"},
                new AnalyseTypes(){Description= "Profilometer",DatabaseColumn="PROF"},
                new AnalyseTypes(){Description= "Fotodoku",DatabaseColumn="PHOTO"}

            };

            SaveAnalysis = new RelayCommand(Save, CanSave);

            //set User field if user is logged in
            if (UserManager.CurrentUser != null)
                m_analysis.User = UserManager.CurrentUser;
        }

        public PAnalysisVM(Analysis Analysis)
        {

            m_analysis = Analysis;

            m_availNalyses = new List<AnalyseTypes>() 
            {
                new AnalyseTypes(){Description= "REM",DatabaseColumn="REM"},
                new AnalyseTypes(){Description= "XPS",DatabaseColumn="XPS"},
                new AnalyseTypes(){Description= "Weißlicht",DatabaseColumn="WI"},
                new AnalyseTypes(){Description= "Lichtmikroskop",DatabaseColumn="LIMI"},
                new AnalyseTypes(){Description= "XRD",DatabaseColumn="XRD"},
                new AnalyseTypes(){Description= "EBSD",DatabaseColumn="EBSD"},
                new AnalyseTypes(){Description= "Profilometer",DatabaseColumn="PROF"},
                new AnalyseTypes(){Description= "Fotodoku",DatabaseColumn="PHOTO"}

            };

            SaveAnalysis = new RelayCommand(Save, CanSave);
        }

        public Analysis Analysis
        { get { return m_analysis; } }

        public DateTime? Started
        { get { return m_analysis.Started; } set { m_analysis.Started = value; } }

        public DateTime? Finished
        { get { return m_analysis.Finished; } set { m_analysis.Finished = value; } }

        public AnalyseTypes Description
        { get { return m_availNalyses.Find(item => item.DatabaseColumn == m_analysis.Description); } set { m_analysis.Description = m_availNalyses.Find(item => item.Description == value.Description).DatabaseColumn; } }

        public string Path
        { get { return m_analysis.Path; } }

        public bool Terminated
        { get { return m_analysis.terminated; } set { m_analysis.terminated = value; } }

        public ObservableCollection<AnalyseTypes> AvailableAnalyses
        {

            get { return new ObservableCollection<AnalyseTypes>(m_availNalyses); }
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }

        public User User
        {
            set { m_analysis.User = value; }
            get { try { return Users.Single(item => item.ID == m_analysis.User.ID); } catch { return null; } }
        }

        public struct AnalyseTypes
        {
            public string Description { get; set; }
            public string DatabaseColumn { get; set; }
        }

        public RelayCommand SaveAnalysis { get; set; }

        #region Command functions
        public void Save()
        {
            ProcessManager.Instance.SaveAnalyses(new List<Analysis>() { m_analysis });
            //Updater.Instance.forceUpdate();

        }

        public bool CanSave()
        {
            if (m_analysis.User!=null && m_analysis.Started != null)
                return true;
            else
                return false;
        }

        #endregion
    }
}
