using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;
using PDCore.Manager;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class OReferenceVM : BaseViewModel
    {
        private WorkpieceHistory m_history;
        public OReferenceVM(int ReferenceNumber)
        {
            m_history = ProcessManager.Instance.getWorkpieceHistory(ReferenceNumber);
            SaveReference = new RelayCommand(Save, CanSave);

        }

        public Workpiece Workpiece
        { get { return m_history.Workpiece; } }
        public Project Project
        { get { return m_history.Project; } }
        public Issue Issue
        { get { return m_history.Issue; } }
        public string Conclusion
        { get { return m_history.Conclusion; } set { m_history.Conclusion = value; } }





        public RelayCommand SaveReference { get; set; }

        #region Command functions
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
