using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;
using PDCore.Manager;
using OE110Prozessdatenbank.Commands;
using System.Windows;
using System.Collections.ObjectModel;
using PDCore.Database;

namespace OE110Prozessdatenbank.ViewModels
{
    public class OReferenceVM : BaseViewModel
    {
        private WorkpieceHistory m_history;
        public OReferenceVM(int ReferenceNumber)
        {
            m_history = ProcessManager.Instance.getWorkpieceHistory(ReferenceNumber);
            SaveReference = new RelayCommand(Save, CanSave);
            CancelProcess = new RelayCommand(cancelProcess, canCancelProcess);

        }

        public int ReferenceNumber
        { get { return m_history.ReferenceNumber; } }

        public string Status
        { get { return m_history.Status; } }
        public Workpiece Workpiece
        { get { return m_history.Workpiece; } }
        public Project Project
        { get { return m_history.Project; } }
        public Issue Issue
        { get { return m_history.Issue; } }
        public string Conclusion
        { get { return m_history.Conclusion; } set { m_history.Conclusion = value; } }

        public ObservableCollection<ProcessMetaData> History
        { get { return new ObservableCollection<ProcessMetaData>(m_history.Processes); } }



        public RelayCommand SaveReference { get; set; }
        public RelayCommand CancelProcess { get; set; }

        #region Command functions

        public void cancelProcess()
        {
            if (MessageBox.Show(Properties.Messages.q_CancelReference, "Vorgang abbrechen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                PDCore.Manager.ProcessManager.Instance.OverrideReferenceStatus(Workpiece, DBEnum.EnumReference.CANCELLED);
            }
        }

        public bool canCancelProcess()
        {
            if (Status == DBEnum.EnumReference.CANCELLED || Status == DBEnum.EnumReference.TERMINATED)
                return false;
            else
                return true;
        
        }

        public void Save()
        {

            ProcessManager.Instance.saveWorkpieceHistory(m_history);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (Conclusion!="")
                return true;
            else
                return false;
        }

        #endregion
    }
}
