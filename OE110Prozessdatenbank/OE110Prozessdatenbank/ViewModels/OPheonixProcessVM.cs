using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using PDCore.BusinessObjects;
using OE110Prozessdatenbank.Commands;
using PDCore.Manager;

namespace OE110Prozessdatenbank.ViewModels
{
    public class OPheonixProcessVM : BaseViewModel
    {
        private PGrindingPhoenixProcess m_GindingPhoenix;
        private bool m_update = false;
        public OPheonixProcessVM(PGrindingPhoenixProcess process)
        {
            m_GindingPhoenix = process;
            m_update = true;
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public OPheonixProcessVM()
        {
            m_GindingPhoenix = new PGrindingPhoenixProcess();
            SaveProcess = new RelayCommand(Save, CanSave);
        }

        public string Description
        {
            get { return m_GindingPhoenix.Description; }
            set { m_GindingPhoenix.Description = value; }
        }

        public double Ra
        {
            get { return m_GindingPhoenix.Ra; }
            set { m_GindingPhoenix.Ra = value; }
        }

        public string Remark
        {
            get { return m_GindingPhoenix.Remark; }
            set { m_GindingPhoenix.Remark = value; }
        }


        public RelayCommand SaveProcess { get; set; }


        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.savePheonixProcess(m_GindingPhoenix, true);
            else
                ObjectManager.Instance.savePheonixProcess(m_GindingPhoenix, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_GindingPhoenix.Description != "" && m_GindingPhoenix.Ra != null)
                return true;
            else
                return false;
        }

        #endregion
    }
}
