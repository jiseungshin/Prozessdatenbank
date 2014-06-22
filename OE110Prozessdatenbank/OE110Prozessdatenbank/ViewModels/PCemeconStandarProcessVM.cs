using System;
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
    public class PCemeconStandarProcessVM : BaseViewModel
    {
        private PCoatingCemeconProcess m_process;
        private bool m_update = false;
        public PCemeconStandarProcessVM()
        {
            ProcessManager.Instance.update();
            m_process = new PCoatingCemeconProcess();
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public PCemeconStandarProcessVM(PCoatingCemeconProcess process)
        {
            ProcessManager.Instance.update();
            m_process = process;
            m_update = true;
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public DateTime Date
        { get { return m_process.Date; } set { m_process.Date = value; } }

        public int ProgramNumber
        { get { return m_process.ProgramNumber; } set { m_process.ProgramNumber = value; } }

        public string Adherent
        { get { return m_process.AdherentLayer; } set { m_process.AdherentLayer = value; } }

        public string Protective
        { get { return m_process.ProtectiveLayer; } set { m_process.ProtectiveLayer = value; } }

        public int Thickness
        { get { return m_process.Thickness; } set { m_process.Thickness = value; } }

        public string Remark
        { get { return m_process.Remark; } set { m_process.Remark = value; } }

        public bool isDecoating
        { get { return m_process.isDecoating; } set { m_process.isDecoating = value; } }

         public RelayCommand SaveProcess { get; set; }


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
            if (m_process.ProtectiveLayer !="" && m_process.AdherentLayer != "" && m_process.ProgramNumber !=-1)
                return true;
            else
                return false;
        }

        #endregion


    }
}
