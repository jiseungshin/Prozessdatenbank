using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using PDCore.Manager;
using System.Collections.ObjectModel;
using PDCore.BusinessObjects;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PAnalysesVM : BaseViewModel
    {
        List<Analysis> m_analyses;
        public PAnalysesVM()
        {
            ObjectManager.Instance.update();
            m_analyses = new List<Analysis>();
            Updater.Instance.newData += Instance_newData;
        }

        

        public PAnalysesVM(int RefID)
        {
            ObjectManager.Instance.update();
            m_analyses = ProcessManager.Instance.getAnalysis(RefID);
            Updater.Instance.newData += Instance_newData;
        }

        void Instance_newData()
        {
            NotifyPropertyChanged("Analyses");
        }

        public ObservableCollection<Analysis> Analyses
        { get { return new ObservableCollection<Analysis>(m_analyses); } }
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
        }

        public DateTime Started
        { get { return m_analysis.Started; } set { m_analysis.Started = value; } }

        public DateTime Finished
        { get { return m_analysis.Finished; } set { m_analysis.Finished = value; } }

        public string Description
        { get { return m_analysis.Description; } set { m_analysis.Description = value; } }

        public string Path
        { get { return m_analysis.Path; } }

        public ObservableCollection<AnalyseTypes> AvailableAnalyses
        {

            get { return new ObservableCollection<AnalyseTypes>(m_availNalyses); }
        }

        public ObservableCollection<User> Users { get { return new ObservableCollection<PDCore.BusinessObjects.User>(ObjectManager.Instance.Users); } }

        public struct AnalyseTypes
        {
            public string Description { get; set; }
            public string DatabaseColumn { get; set; }
        }
    }
}
