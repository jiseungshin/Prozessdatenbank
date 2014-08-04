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
            m_glass = ObjectManager.Instance.getGlass(GID);
            m_update = true;
            SaveGlass = new RelayCommand(Save, CanSave);
        }

        public string Name
        { get { return m_glass.Name; } set { m_glass.Name = value; } }

        public string Company
        { get { return m_glass.Comapany; } set { m_glass.Comapany = value; } }

        public double? Abrasion
        { get { return m_glass.Abrasion; } set { m_glass.Abrasion = value; } }

        public double? Acid_Resistance_RA
        { get { return m_glass.Acid_Resistance_RA; } set { m_glass.Acid_Resistance_RA = value; } }

        public double? Acid_Resistance_SR
        { get { return m_glass.Acid_Resistance_SR; } set { m_glass.Acid_Resistance_SR = value; } }

        public double? Aluminium_fluoride
        { get { return m_glass.Aluminium_fluoride; } set { m_glass.Aluminium_fluoride = value; } }

        public double? Aluminiumoxid
        { get { return m_glass.Aluminiumoxid; } set { m_glass.Aluminiumoxid = value; } }

        public double? Annealing_Point
        { get { return m_glass.Annealing_Point; } set { m_glass.Annealing_Point = value; } }

        public double? Antimonoxid
        { get { return m_glass.Antimonoxid; } set { m_glass.Antimonoxid = value; } }

        public double? Barium_fluoride
        { get { return m_glass.Barium_fluoride; } set { m_glass.Barium_fluoride = value; } }

        public double? Bariumoxid
        { get { return m_glass.Bariumoxid; } set { m_glass.Bariumoxid = value; } }

        public double? Boroxid
        { get { return m_glass.Boroxid; } set { m_glass.Boroxid = value; } }

        public double? Caesiumoxid
        { get { return m_glass.Caesiumoxid; } set { m_glass.Caesiumoxid = value; } }

        public double? Calcium_fluoride
        { get { return m_glass.Calcium_fluoride; } set { m_glass.Calcium_fluoride = value; } }

        public double? Calciumoxid
        { get { return m_glass.Calciumoxid; } set { m_glass.Calciumoxid = value; } }

        public double? Fluor
        { get { return m_glass.Fluor; } set { m_glass.Fluor = value; } }

        public double? Germaniumoxid
        { get { return m_glass.Germaniumoxid; } set { m_glass.Germaniumoxid = value; } }

        public double? Kaliumoxid
        { get { return m_glass.Kaliumoxid; } set { m_glass.Kaliumoxid = value; } }

        public double? Knoop_Hardness
        { get { return m_glass.Knoop_Hardness; } set { m_glass.Knoop_Hardness = value; } }

        public double? Lanthanoxid
        { get { return m_glass.Lanthanoxid; } set { m_glass.Lanthanoxid = value; } }

        public double? Lanthanum_fluoride
        { get { return m_glass.Lanthanum_fluoride; } set { m_glass.Lanthanum_fluoride = value; } }

        public double? Lithiumoxid
        { get { return m_glass.Lithiumoxid; } set { m_glass.Lithiumoxid = value; } }

        public double? Magnecium_fluoride
        { get { return m_glass.Magnecium_fluoride; } set { m_glass.Magnecium_fluoride = value; } }

        public double? Molding_Temperature
        { get { return m_glass.Molding_Temperature; } set { m_glass.Molding_Temperature = value; } }

        public double? Natriumoxid
        { get { return m_glass.Natriumoxid; } set { m_glass.Natriumoxid = value; } }

        public double? Niobpentoxid
        { get { return m_glass.Niobpentoxid; } set { m_glass.Niobpentoxid = value; } }

        public double? Phospahte_Resistance
        { get { return m_glass.Phospahte_Resistance; } set { m_glass.Phospahte_Resistance = value; } }

        public double? Phosphorus_oxide
        { get { return m_glass.Phosphorus_oxide; } set { m_glass.Phosphorus_oxide = value; } }

        public double? Photoelastic_Constant
        { get { return m_glass.Photoelastic_Constant; } set { m_glass.Photoelastic_Constant = value; } }

        public double? Siliziumdioxid
        { get { return m_glass.Siliziumdioxid; } set { m_glass.Siliziumdioxid = value; } }

        public double? Softening_Point
        { get { return m_glass.Softening_Point; } set { m_glass.Softening_Point = value; } }

        public double? Specific_Gravity
        { get { return m_glass.Specific_Gravity; } set { m_glass.Specific_Gravity = value; } }

        public double? Strain_Point
        { get { return m_glass.Strain_Point; } set { m_glass.Strain_Point = value; } }

        public double? Strontium_fluoride
        { get { return m_glass.Strontium_fluoride; } set { m_glass.Strontium_fluoride = value; } }


        public double? Strontiumoxid
        { get { return m_glass.Strontiumoxid; } set { m_glass.Strontiumoxid = value; } }

        public double? Tantaloxid
        { get { return m_glass.Tantaloxid; } set { m_glass.Tantaloxid = value; } }

        public double? Tellurium
        { get { return m_glass.Tellurium; } set { m_glass.Tellurium = value; } }


        public double? Titanoxid
        { get { return m_glass.Titanoxid; } set { m_glass.Titanoxid = value; } }

        public double? Transformation_Temperature
        { get { return m_glass.Transformation_Temperature; } set { m_glass.Transformation_Temperature = value; } }

        public double? Tungsten_Oxid
        { get { return m_glass.Tungsten_Oxid; } set { m_glass.Tungsten_Oxid = value; } }

        public double? Wärmeausdehnungskoeffizient
        { get { return m_glass.Wärmeausdehnungskoeffizient; } set { m_glass.Wärmeausdehnungskoeffizient = value; } }

        public double? Wärmeleitfähigkeit
        { get { return m_glass.Wärmeleitfähigkeit; } set { m_glass.Wärmeleitfähigkeit = value; } }

        public double? Water_Resistance
        { get { return m_glass.Water_Resistance; } set { m_glass.Water_Resistance = value; } }

        public double? Weathering_Resistance
        { get { return m_glass.Weathering_Resistance; } set { m_glass.Weathering_Resistance = value; } }

        public double? Wismutoxid
        { get { return m_glass.Wismutoxid; } set { m_glass.Wismutoxid = value; } }

        public double? Wolframtrioxid
        { get { return m_glass.Wolframtrioxid; } set { m_glass.Wolframtrioxid = value; } }

        public double? Yield_Point
        { get { return m_glass.Yield_Point; } set { m_glass.Yield_Point = value; } }

        public double? Yttrium_fluoride
        { get { return m_glass.Yttrium_fluoride; } set { m_glass.Yttrium_fluoride = value; } }

        public double? Yttriumoxid
        { get { return m_glass.Yttriumoxid; } set { m_glass.Yttriumoxid = value; } }

        public double? Zinkoxid
        { get { return m_glass.Zinkoxid; } set { m_glass.Zinkoxid = value; } }

        public double? Zirkonoxid
        { get { return m_glass.Zirkonoxid; } set { m_glass.Zirkonoxid = value; } }

      




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
