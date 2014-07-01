﻿using System;
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
    public class OWorkpieceVM : BaseViewModel
    {
        private bool m_update = false;
        private Workpiece m_workpiece;
        private ObservableCollection<Workpiece> m_workieces;
        private List<string> m_geometries = new List<string>() { "Diffraktiv", "Konvex", "Konkav", "Plan" };
        public OWorkpieceVM()
        {
            ObjectManager.Instance.update();
            m_workpiece = new Workpiece();
            m_workieces = new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces);
            SaveWorkpiece = new RelayCommand(Save, CanSave);
        }

        public OWorkpieceVM(int WPID)
        {
            m_update = true;
            m_workpiece = ObjectManager.Instance.getWorkpiece(WPID);
            SaveWorkpiece = new RelayCommand(Save, CanSave);

        }

        public RelayCommand SaveWorkpiece { get; set; }

        public Material Material
        {
            get
            {
                try
                {
                    return Materials.Single(item => item.ID == m_workpiece.Material.ID);
                }
                catch { return null; }
            }
            set { m_workpiece.Material = value; }
        }

        public ObservableCollection<Material> Materials
        {
            get { return new ObservableCollection<PDCore.BusinessObjects.Material>(ObjectManager.Instance.Materials); }
        }

        public string Label
        { get { return m_workpiece.Label; } set { m_workpiece.Label = value; } }

        public DateTime? PurchaseDate
        { get { return m_workpiece.PurchaseDate; } set { m_workpiece.PurchaseDate = value; } }

        public string Geometry
        { get { return m_workpiece.Geometry; } set { m_workpiece.Geometry = value; System.Windows.MessageBox.Show(value.ToString()); } }

        public List<string> Geometries
        { get { return m_geometries; } }

        public string Kind
        { get { return m_workpiece.KindOfProbe; } set { m_workpiece.KindOfProbe = value; } }

        public bool isOneWay
        {
            get { return m_workpiece.isOneWay; }
            set { m_workpiece.isOneWay = value; }
        }
        public string BatchNumber
        { get { return m_workpiece.BatchNumber; } set { m_workpiece.BatchNumber = value; } }
        


        #region Command functions
        public void Save()
        {
            if (m_update)
                ObjectManager.Instance.saveWorkpiece(m_workpiece, true);
            else
                ObjectManager.Instance.saveWorkpiece(m_workpiece, false);

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (Label != "" && Material != null)
                return true;
            else
                return false;
        }

        #endregion

    }
}
