using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Processes
{
    public class PExpToshiba : BaseProcess
    {
        public PExpToshiba()
        {
            UserID = -1;
            Workpieces = new List<BusinessObjects.Workpiece>();
            InputData = new ToshibaInputData();
            MachinaData = new ToshibaMachineData();
            Quality = new BusinessObjects.ProcessQuality();

        }

        public ToshibaInputData InputData { get; set; }
        public ToshibaMachineData MachinaData { get; set; }
        public ToshibaImport.MonFile File { get; set; }
        public int? GlassID { get; set; }
        public string GlassName { get; set; }
        public int? UpperWorkpiece { get; set; }
        public int? LowerWorkpiece { get; set; }

        public class ToshibaInputData
        {
            public double? Tv { get; set; }
            public double? Tvu { get; set; }
            public double? T1 { get; set; }
            public double? T1u { get; set; }
            public double? T2 { get; set; }
            public double? T3 { get; set; }
            public double? T4 { get; set; }
            public double? T5 { get; set; }
            public double? Gv { get; set; }
            public double? G1 { get; set; }
            public double? G2 { get; set; }
            public double? ST1 { get; set; }
            public double? ST2 { get; set; }
            public double? P1 { get; set; }
            public double? P2 { get; set; }
            public double? P3 { get; set; }
            public double? PT1 { get; set; }
            public double? V1 { get; set; }
            public double? V2 { get; set; }
            public double? V3 { get; set; }
            public double? Z1 { get; set; }
            public double? Z2 { get; set; }
            public string CoolingUpper { get; set; }
            public string CoolingLower { get; set; }

            public double? N2U_A { get; set; }
            public double? N2U_B { get; set; }
            public double? N2U_C { get; set; }
            public double? N2U_AA { get; set; }
            public double? N2U_BB { get; set; }
            public double? N2U_CC { get; set; }
            public double? N2L_A { get; set; }
            public double? N2L_B { get; set; }
            public double? N2L_C { get; set; }
            public double? N2L_AA { get; set; }
            public double? N2L_BB { get; set; }
            public double? N2L_CC { get; set; }
            public double? OutpU { get; set; }
            public double? OutpL { get; set; }

        }

        public class ToshibaMachineData
        {
            public double? Tv_lower { get; set; }
            public double? Tvu_lower { get; set; }
            public double? T1_lower { get; set; }
            public double? T1u_lower { get; set; }
            public double? T2_lower { get; set; }
            public double? T3_lower { get; set; }
            public double? T4_lower { get; set; }
            public double? T5_lower { get; set; }
            public double? Tv_upper { get; set; }
            public double? Tvu_upper { get; set; }
            public double? T1_upper { get; set; }
            public double? T1u_upper { get; set; }
            public double? T2_upper { get; set; }
            public double? T3_upper { get; set; }
            public double? T4_upper { get; set; }
            public double? T5_upper { get; set; }
            public double? Gv_lower { get; set; }
            public double? G1_lower { get; set; }
            public double? G3_lower { get; set; }
            public double? G4_lower { get; set; }
            public double? Gv_lower_quali { get; set; }
            public double? G1_lower_quali { get; set; }
            public double? G3_lower_quali { get; set; }
            public double? G4_lower_quali { get; set; }
            public double? Gv_upper { get; set; }
            public double? G1_upper { get; set; }
            public double? G3_upper { get; set; }
            public double? G4_upper { get; set; }
            public double? Gv_upper_quali { get; set; }
            public double? G1_upper_quali { get; set; }
            public double? G3_upper_quali { get; set; }
            public double? G4_upper_quali { get; set; }
            public double? ST1 { get; set; }
            public double? ST2 { get; set; }
            public double? T_press_av { get; set; }
            public double? T_press_G { get; set; }
            public double? T_press_G_quali { get; set; }
            public double? P1 { get; set; }
            public double? P2 { get; set; }
            public double? P3 { get; set; }
            public double? PT1 { get; set; }
            public double? T_start_upper { get; set; }
            public double? T_start_lower { get; set; }
        }
    }
}
