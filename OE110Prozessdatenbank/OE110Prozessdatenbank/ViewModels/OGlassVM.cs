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
    public class OGlassVM : BaseViewModel
    {
        private Glass m_glass;
        private bool m_update;
        public OGlassVM()
        {
            m_glass = new Glass();
            m_update = false;
            SaveGlass = new RelayCommand(Save, CanSave);
        }

        public OGlassVM(int GID)
        {
            m_glass = ObjectManager.Instance.Glasses.Find(item => item.ID == GID);
            m_update = true;
            SaveGlass = new RelayCommand(Save, CanSave);
        }

        public string Name
        { get { return m_glass.Name; } set { m_glass.Name = value; } }

        public string Company
        { get { return m_glass.Comapany; } set { m_glass.Comapany = value; } }


        public RelayCommand SaveGlass { get; set; }


        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.saveGlass(m_glass, true);
            else
                ObjectManager.Instance.saveGlass(m_glass, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_glass.Name != "" && m_glass.Comapany != "")
                return true;
            else
                return false;
        }

        #endregion

    }
}
