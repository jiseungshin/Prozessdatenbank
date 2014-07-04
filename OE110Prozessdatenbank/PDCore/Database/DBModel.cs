using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Database
{

    public static class DBProcessReferenceRelation
    {
        public static string Table { get { return "ProcessReferenceRelation"; } }
        public static string ID { get { return "ProcessReferenceRelation_ID"; } }
        public static string RefNumber { get { return "ReferenceNumber"; } }
        public static string PID { get { return "Process_ID"; } }
        public static string MachineID { get { return "Machine_ID"; } }
    }
    public static class DBProcessReferences
    {
        public static string Table { get { return "ProcessReferences"; } }
        public static string RefNumber { get { return "ReferenceNumber"; } }
        public static string ProjectID { get { return "Project_ID"; } }
        public static string IssueID { get { return "Issue_ID"; } }
        public static string WorkpiceID { get { return "Workpiece_ID"; } }
        public static string Status { get { return "Status"; } }
        public static string Conclusion { get { return "Conclusion"; } }
    }
    public static class DBTurningMoore
    {
        public static string Table { get { return "Turning_Moore"; } }
        public static string ID { get { return "Turning_Moore_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Radius { get { return "Radius"; } }
        public static string Feed { get { return "Feed"; } }
        public static string CuttingAngle { get { return "CuttingAngle"; } }
        public static string CutDepth { get { return "CutDepth"; } }
        public static string Speed { get { return "Speed"; } }
        public static string Processing { get { return "Processing"; } }
        public static string IsFinish { get { return "IsFinish"; } }
        public static string Remark { get { return "Remark"; } }
        public static string RA { get { return "Ra"; } }
        public static string PV { get { return "PV"; } }
        public static string ToolID { get { return "ToolID"; } }
    }
    public static class DBGrindingMoore
    {
        public static string Table { get { return "Grinding_Moore"; } }
        public static string ID { get { return "Grinding_Moore_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Remark { get { return "Remark"; } }
        public static string Date { get { return "Date"; } }
        public static string ToolRadius { get { return "ToolRadius"; } }
        public static string TipRadius { get { return "TippRadius"; } }
        public static string GrindingDirection { get { return "GrindingDirection"; } }
        public static string InFeed { get { return "InFeed"; } }
        public static string RA { get { return "RA"; } }
        public static string PV { get { return "PV"; } }
        public static string ToolSpeed { get { return "ToolSpeed"; } }
        public static string Feed { get { return "Feed"; } }
        public static string GrindingWheelSpeed { get { return "GrindingWheelSpeed"; } }
        public static string PostProduction { get { return "PostProduction"; } }
    }
    public static class DBGrindingPhoenix
    {
        public static string Table { get { return "Grinding_Phoenix"; } }
        public static string ID { get { return "Grinding_Phoenix_ID"; } }
        public static string ProcessID { get { return "Grinding_PhoenixProcesses_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Remark { get { return "Remark"; } }
        public static string Date { get { return "Date"; } }
    }
    public static class DBGrindingOther
    {
        public static string Table { get { return "Grinding_Other"; } }
        public static string ID { get { return "Grinding_Other_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Remark { get { return "Remark"; } }
        public static string Date { get { return "Date"; } }
    }
    public static class DBWorkpieces
    {
        public static string Table { get { return "Workpieces"; } }
        public static string ID { get { return "Workpiece_ID"; } }
        public static string Status { get { return "Status"; } }
        public static string Label { get { return "Label"; } }
        public static string MaterialID { get { return "Material_ID"; } }
        public static string PurchaseDate { get { return "PurchaseDate"; } }
        public static string Geometry { get { return "Geometry"; } }
        public static string KindOfProbe { get { return "KindOfProbe"; } }
        public static string BatchNumber { get { return "BatchNumber"; } }
        public static string isOneWay { get { return "isOneWay"; } }
        public static string isActive { get { return "isActive"; } }

    }
    public static class DBWorkpieceQuality
    {
        public static string Table { get { return "WorkpieceQualityParameters"; } }
        public static string ID { get { return "WorkpieceQualityParameter_ID"; } }
        public static string ReferenceNumber { get { return "ReferenceNumber"; } }
        public static string Corrosion { get { return "Corrosion"; } }
        public static string MoldScratches { get { return "MoldScratches"; } }
        public static string GlassAdherence { get { return "GlassAdherence"; } }
        public static string OverallResult { get { return "OverallResult"; } }
        public static string PID { get { return "Process_ID"; } }
    }
    public static class DBProcessQuality
    {
        public static string Table { get { return "ProcessQualityParameters"; } }
        public static string ID { get { return "ProcessQualityParameter_ID"; } }
        public static string PID { get { return "Process_ID"; } }
        public static string GlassScratches { get { return "GlassScratches"; } }
        public static string GlassPeeling { get { return "GlassPeeling"; } }
        public static string GlassBreakage { get { return "GlassBreakage"; } }
        public static string OverallResult { get { return "OverallResult"; } }
    }
    public static class DBProjects
    {
        public static string Table { get { return "Projects"; } }
        public static string ID { get { return "Project_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Name { get { return "ProjectName"; } }
        public static string Started { get { return "Started"; } }
        public static string Finished { get { return "Finished"; } }
        public static string Remark { get { return "Remark"; } }
        public static string DirPath { get { return "ProjectDirPath"; } }
    }
    public static class DBIssues
    {
        public static string Table { get { return "Issues"; } }
        public static string ID { get { return "Issue_ID"; } }
        public static string Description { get { return "Description"; } }
        public static string ProjectID { get { return "Project_ID"; } }
        public static string Remark { get { return "Remark"; } }
    }
    public static class DBGlasses
    {
        public static string Table { get { return "Glasses"; } }
        public static string ID { get { return "Glass_ID"; } }
        public static string Description { get { return "GlassName"; } }
        public static string Company { get { return "Company"; } }
    }
    public static class DBUser
    {
        public static string Table { get { return "User"; } }
        public static string ID { get { return "User_ID"; } }
        public static string Token { get { return "Token"; } }
        public static string FirstName { get { return "FirstName"; } }
        public static string LastName { get { return "LastName"; } }
        public static string isActive { get { return "isActive"; } }
        public static string MachineID { get { return "Machine_ID"; } }
    }

    public static class DBMAterial
    {
        public static string Table { get { return "Materials"; } }
        public static string ID { get { return "Material_ID"; } }
        public static string Name { get { return "Name"; } }
    }

    public static class DBMachine
    {
        public static string Table { get { return "Machines"; } }
        public static string ID { get { return "Machine_ID"; } }
        public static string Name { get { return "Name"; } }
        public static string Process { get { return "Process"; } }
    }

    public static class DBPhoenixProcesses
    {
        public static string Table { get { return "Grinding_PhoenixProcesses"; } }
        public static string ID { get { return "GrindingPhoenixProcesses_ID"; } }
        public static string Description { get { return "Name"; } }
    }

    public static class DBCoatingCemecon
    {
        public static string Table { get { return "Coating_CemeCon"; } }
        public static string ID { get { return "Coating_CemeCon_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string CoatingProcessID { get { return "CoatingProcess_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Abnormalities { get { return "Abnormalities"; } }
        public static string Remark { get { return "Remark"; } }
        public static string ProcessNumber { get { return "ProcessNumber"; } }
    }

    public static class DBDeCoatingCemecon
    {
        public static string Table { get { return "Decoating_CemeCon"; } }
        public static string ID { get { return "Decoating_CemeCon_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string CoatingProcessID { get { return "CoatingProcess_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Remark { get { return "Remark"; } }
        public static string ProcessNumber { get { return "ProcessNumber"; } }
    }

    public static class DBCoatingCemeconProcess
    {
        public static string Table { get { return "Coating_CemeConProcesses"; } }
        public static string ID { get { return "Coating_CemeConProcesses_ID"; } }
        public static string ProgramNumber { get { return "MachineProgramNumber"; } }
        public static string AdherentLayer { get { return "AdherentLayer"; } }
        public static string ProtectiveLayer { get { return "ProtectiveLayer"; } }
        public static string Thickness { get { return "Thickness"; } }
        public static string Remark { get { return "Remark"; } }
        public static string Date { get { return "Date"; } }
        public static string IsDecoating { get { return "IsDecoating"; } }

    }

    public static class DBCoatingLayers
    {
        public static string Table { get { return "Coating_Layers"; } }
        public static string ID { get { return "Coating_Layer_ID"; } }
        public static string Layer { get { return "Layer"; } }

    }

    public static class DBExpOther
    {
        public static string Table { get { return "Exp_Other"; } }
        public static string ID { get { return "Exp_Other_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Remark { get { return "Remark"; } }
    }

    public static class DBExpCemeCon
    {
        public static string Table { get { return "Exp_CemeCon"; } }
        public static string ID { get { return "Exp_CemeCon_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Remark { get { return "Remark"; } }
        public static string GlassID { get { return "Glass_ID"; } }
        public static string ResultID { get { return "Exp_Result_ID"; } }
        public static string Temperature { get { return "Temperature"; } }
        public static string Pressure { get { return "Pressure"; } }
        public static string Atmosphere { get { return "Atmosphere"; } }
        public static string Duration { get { return "Duration"; } }
        public static string ProcessID { get { return "Process_ID"; } }
    }

    public static class DBExpTestStation
    {
        public static string Table { get { return "Exp_TestStation"; } }
        public static string ID { get { return "Exp_TestStation_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Remark { get { return "Remark"; } }
        public static string GlassID { get { return "Glass_ID"; } }
        public static string CellTemperature { get { return "CellTemperature"; } }
        public static string Atmosphere { get { return "Atmosphere"; } }
        public static string PressTemperature { get { return "PressTemperature"; } }
        public static string CoolingTemperature { get { return "CoolingTemperature"; } }
        public static string MaxForce { get { return "MaxForce"; } }
        public static string SecForce { get { return "SecForce"; } }
        public static string PressFeed { get { return "PressFeed"; } }
        public static string PenDepth { get { return "PenDepth"; } }
        public static string Duration { get { return "Duration"; } }
        public static string Cycles { get { return "Cycles"; } }
        public static string LeftWPID { get { return "LeftWP_ID"; } }
        public static string CenterWPID { get { return "CenterWP_ID"; } }
        public static string RightWPID { get { return "RightWP_ID"; } }
    }

    public static class DBExpMoore
    {
        public static string Table { get { return "Exp_Moore"; } }
        public static string ID { get { return "Exp_Moore_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Remark { get { return "Remark"; } }
        public static string GlassID { get { return "Glass_ID"; } }
        public static string ResultID { get { return "Exp_Result_ID"; } }
        public static string UpperWorkpieceID { get { return "UpperWorkpiece_ID"; } }
        public static string LowerWorkpieceID { get { return "LowerWorkpiece_ID"; } }
        public static string ProgramTitle { get { return "ProgramTitle"; } }
        public static string RegionOfInterest { get { return "ROI"; } }
        public static string Tmin { get { return "Tmin"; } }
        public static string Tmax { get { return "Tmax"; } }
        public static string TOut { get { return "Tout"; } }
        public static string Atmosphere { get { return "Atmosphere"; } }
        public static string Force { get { return "PressForce"; } }
        public static string PressTime { get { return "PressTime"; } }
        public static string Cycles { get { return "Cycles"; } }
    }

    public static class DBAnalyses
    {
        public static string Table { get { return "Analyses"; } }
        public static string ID { get { return "Analyse_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string RefNumber { get { return "ReferenceNumber"; } }
        public static string Started { get { return "Started"; } }
        public static string Finished { get { return "Finished"; } }
        public static string Type { get { return "Type"; } }
    }

    public static class DBExpToshiba
    {
        public static string Table { get { return "Exp_Toshiba"; } }
        public static string ID { get { return "Exp_Toshiba_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string GlassID { get { return "Glass_ID"; } }
        public static string UpperWPID { get { return "UpperWP_ID"; } }
        public static string LowerWPID { get { return "LowerWP_ID"; } }
        public static string Date { get { return "Date"; } }
        public static string Remark { get { return "Remark"; } }
        public static string Tv { get { return "Tv"; } }
        public static string Tvu { get { return "Tvu"; } }
        public static string T1u { get { return "T1u"; } }
        public static string T1 { get { return "T1"; } }
        public static string T2 { get { return "T2"; } }
        public static string T3 { get { return "T3"; } }
        public static string T4 { get { return "T4"; } }
        public static string T5 { get { return "T5"; } }
        public static string P1 { get { return "P1"; } }
        public static string P2 { get { return "P2"; } }
        public static string P3 { get { return "P3"; } }
        public static string PT1 { get { return "PT1"; } }
        public static string V1 { get { return "V1"; } }
        public static string V2 { get { return "V2"; } }
        public static string Z2 { get { return "Z2"; } }
        public static string Z1 { get { return "Z1"; } }
        public static string V3 { get { return "V3"; } }
        public static string Gv { get { return "Gv"; } }
        public static string G1 { get { return "G1"; } }
        public static string G2 { get { return "G2"; } }
        public static string ST1 { get { return "ST1"; } }
        public static string ST2 { get { return "ST2"; } }
        public static string CoolingUpper { get { return "CoolingUpper"; } }
        public static string CoolingLower { get { return "CoolingLower"; } }
        public static string GlassName { get { return "GlassName"; } }
    }

    public static class DBExpToshibaMachineData
    {
        public static string Table { get { return "Exp_Toshiba_MData"; } }
        public static string ID { get { return "Exp_Toshiba_MData_ID"; } }
        public static string PID { get { return "Process_ID"; } }
        public static string Tv_lower { get { return "Tv_lower"; } }
        public static string Tvu_lower { get { return "Tvu_lower"; } }
        public static string T1_lower { get { return "T1_lower"; } }
        public static string T1u_lower { get { return "T1u_lower"; } }
        public static string T2_lower { get { return "T2_lower"; } }
        public static string T3_lower { get { return "T3_lower"; } }
        public static string T4_lower { get { return "T4_lower"; } }
        public static string T5_lower { get { return "T5_lower"; } }
        public static string Tv_upper { get { return "Tv_upper"; } }
        public static string Tvu_upper { get { return "Tvu_upper"; } }
        public static string T1_upper { get { return "T1_upper"; } }
        public static string T1u_upper { get { return "T1u_upper"; } }
        public static string T2_upper { get { return "T2_upper"; } }
        public static string T3_upper { get { return "T3_upper"; } }
        public static string T4_upper { get { return "T4_upper"; } }
        public static string T5_upper { get { return "T5_upper"; } }
        public static string Gv_lower { get { return "Gv_lower"; } }
        public static string G1_lower { get { return "G1_lower"; } }
        public static string G3_lower { get { return "G3_lower"; } }
        public static string G4_lower { get { return "G4_lower"; } }
        public static string Gv_lower_quali { get { return "Gv_lower_quali"; } }
        public static string G1_lower_quali { get { return "G1_lower_quali"; } }
        public static string G3_lower_quali { get { return "G3_lower_quali"; } }
        public static string G4_lower_quali { get { return "G4_lower_quali"; } }
        public static string Gv_upper { get { return "Gv_upper"; } }
        public static string G1_upper { get { return "G1_upper"; } }
        public static string G3_upper { get { return "G3_upper"; } }
        public static string G4_upper { get { return "G4_upper"; } }
        public static string Gv_upper_quali { get { return "Gv_upper_quali"; } }
        public static string G1_upper_quali { get { return "G1_upper_quali"; } }
        public static string G3_upper_quali { get { return "G3_upper_quali"; } }
        public static string G4_upper_quali { get { return "G4_upper_quali"; } }
        public static string ST1 { get { return "ST1"; } }
        public static string ST2 { get { return "ST2"; } }
        public static string T_press_av { get { return "T_press_av"; } }
        public static string T_press_G { get { return "T_press_G"; } }
        public static string T_press_G_quali { get { return "T_press_G_quali"; } }
        public static string P1 { get { return "P1"; } }
        public static string P2 { get { return "P2"; } }
        public static string P3 { get { return "P3"; } }
        public static string PT1 { get { return "PT1"; } }
        public static string T_start_upper { get { return "T_start_upper"; } }
        public static string T_start_lower { get { return "T_start_lower"; } }

    }

    public static class DBEnum
    {
        public static class EnumWorkpiece
        {
            public static string RAW { get { return "raw"; } }
            public static string IN_PROCESS { get { return "INPROCESS"; } }

        }

        public static class EnumReference
        {
            public static string RAW { get { return "raw"; } }
            public static string POLISHED { get { return "polished"; } }
            public static string COATED { get { return "coated"; } }
            public static string PROCESSED { get { return "processed"; } }
            public static string ANALYSED { get { return "analysed"; } }
            public static string DECOATED { get { return "decoated"; } }
            public static string TERMINATED { get { return "terminated"; } }
            public static string CANCELLED { get { return "cancelled"; } }

        }




    }
}
