using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Glass : BusinessObject
    {
        public string Name { get; set; }
        public string Comapany { get; set; }
        public string VisualName { get { return Name + " (" + Comapany + ")"; } }

        public double? Softening_Point { get; set; }
        public double? Annealing_Point { get; set; }
        public double? Strain_Point { get; set; }
        public double? Transformation_Temperature { get; set; }
        public double? Yield_Point { get; set; }
        public double? Wärmeausdehnungskoeffizient { get; set; }
        public double? Wärmeleitfähigkeit { get; set; }
        public double? Weathering_Resistance { get; set; }
        public double? Acid_Resistance_SR { get; set; }
        public double? Specific_Gravity { get; set; }
        public double? Abrasion { get; set; }
        public double? Water_Resistance { get; set; }
        public double? Acid_Resistance_RA { get; set; }
        public double? Phospahte_Resistance { get; set; }
        public double? Knoop_Hardness { get; set; }
        public double? Photoelastic_Constant { get; set; }
        public double? Boroxid { get; set; }
        public double? Fluor { get; set; }
        public double? Kaliumoxid { get; set; }
        public double? Antimonoxid { get; set; }
        public double? Siliziumdioxid { get; set; }
        public double? Lanthanoxid { get; set; }
        public double? Zinkoxid { get; set; }
        public double? Tantaloxid { get; set; }
        public double? Niobpentoxid { get; set; }
        public double? Zirkonoxid { get; set; }
        public double? Yttriumoxid { get; set; }
        public double? Lithiumoxid { get; set; }
        public double? Aluminiumoxid { get; set; }
        public double? Bariumoxid { get; set; }
        public double? Natriumoxid { get; set; }
        public double? Strontiumoxid { get; set; }
        public double? Titanoxid { get; set; }
        public double? Tellurium { get; set; }
        public double? Calciumoxid { get; set; }
        public double? Tungsten_Oxid { get; set; }
        public double? Lanthanum_fluoride { get; set; }
        public double? Phosphorus_oxide { get; set; }
        public double? Strontium_fluoride { get; set; }
        public double? Calcium_fluoride { get; set; }
        public double? Barium_fluoride { get; set; }
        public double? Magnecium_fluoride { get; set; }
        public double? Aluminium_fluoride { get; set; }
        public double? Yttrium_fluoride { get; set; }
        public double? Wismutoxid { get; set; }
        public double? Caesiumoxid { get; set; }
        public double? Germaniumoxid { get; set; }
        public double? Wolframtrioxid { get; set; }
        public double? Molding_Temperature { get; set; }
    }
}
