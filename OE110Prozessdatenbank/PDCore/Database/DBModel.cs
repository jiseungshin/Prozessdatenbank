﻿using System;
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
    }
    public static class DBTurningMoore
    {
        public static string Table { get { return "Turning_Moore"; } }
        public static string ID { get { return "Turning_Moore_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Date { get { return "ProcessDate"; } }
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
        public static string Date { get { return "ProcessDate"; } }
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
        public static string Date { get { return "ProcessDate"; } }
    }

    public static class DBGrindingOther
    {
        public static string Table { get { return "Grinding_Other"; } }
        public static string ID { get { return "Grinding_Other_ID"; } }
        public static string UserID { get { return "User_ID"; } }
        public static string Remark { get { return "Remark"; } }
        public static string Date { get { return "ProcessDate"; } }
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

    public static class DBUser
    {
        public static string Table { get { return "User"; } }
        public static string ID { get { return "User_ID"; } }
        public static string Token { get { return "Token"; } }
        public static string FirstName { get { return "FirstName"; } }
        public static string LastName { get { return "LastName"; } }
        public static string isActive { get { return "isActive"; } }
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
}
