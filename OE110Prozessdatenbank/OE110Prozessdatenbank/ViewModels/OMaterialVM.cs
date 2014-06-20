using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Manager;
using PDCore.BusinessObjects;
using System.Collections.ObjectModel;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class OMaterialVM : BaseViewModel
    {
        private Material m_material;
        private ObservableCollection<Material> m_materials;
        private bool m_update = false;
        
        public OMaterialVM()
        {
            m_material = new Material();
            m_materials = new ObservableCollection<Material>(ObjectManager.Instance.Materials);
            SaveMaterial = new RelayCommand(Save, CanSave);

        }
 
        public OMaterialVM(int MatID)
        {
            m_material = ObjectManager.Instance.getMaterial(MatID);
            m_update = true;
            SaveMaterial = new RelayCommand(Save, CanSave);
        }

        public string Name
        {
            get { return m_material.Name; }
            set { m_material.Name = value; }
        }

        public ObservableCollection<Material> Materials
        {
            get { return m_materials; }
        }

        public RelayCommand SaveMaterial { get; set; }


        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.saveMaterial(m_material, true);
            else
                ObjectManager.Instance.saveMaterial(m_material, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (Name!= "")
                return true;
            else
                return false;
        }

        #endregion

    }
}
