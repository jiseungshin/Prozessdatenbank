using System;
using PDCore.Database;

namespace PDCore.Database
{
    public static class Queries
    {
        public static string QueryRaw
        {
            get { return "SELECT Workpieces.Label, Materials.Name, Workpieces.WorkPiece_ID FROM Workpieces "+
                            "INNER JOIN Materials ON Workpieces.Material_ID = Materials.Material_ID "+
                            "WHERE Workpieces.Status='raw'";
}
        }

        public static string QueryTurningMoore
        {
            get
            {
                return "SELECT * FROM " + DBTurningMoore.Table +

                                          //join ReferenceRelations
                                          " LEFT JOIN " + DBProcessReferenceRelation.Table +
                                          " On " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID +
                                          "=" + DBTurningMoore.Table + "." + DBTurningMoore.ID +

                                          //join References
                                          " LEFT JOIN " + DBProcessReferences.Table +
                                          " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                          "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber +

                                          //join Workpiece
                                          " LEFT JOIN " + DBWorkpieces.Table +
                                          " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                          //join User
                                          " LEFT JOIN " + DBUser.Table +
                                          " On " + DBUser.Table + "." + DBUser.ID +
                                          "=" + DBTurningMoore.Table + "." + DBTurningMoore.UserID +

                                          //join Material
                                          " LEFT JOIN " + DBMAterial.Table +
                                          " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                          "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +

                                          //join project
                                          " LEFT JOIN " + DBProjects.Table +
                                          " On " + DBProjects.Table + "." + DBProjects.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                          //join issues
                                          " LEFT JOIN " + DBIssues.Table +
                                          " On " + DBIssues.Table + "." + DBIssues.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID;

            }
        }

        public static string QueryGrindingMoore
        {
            get 
            {
                return "SELECT * FROM " + DBGrindingMoore.Table +

                                         //join ReferenceRelations
                                         " LEFT JOIN " + DBProcessReferenceRelation.Table +
                                         " On " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID +
                                         "=" + DBGrindingMoore.Table + "." + DBGrindingMoore.ID +

                                         //join References
                                         " LEFT JOIN " + DBProcessReferences.Table +
                                         " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                         "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber +

                                         //join Workpiece
                                         " LEFT JOIN " + DBWorkpieces.Table +
                                         " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                         "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                         //join User
                                         " LEFT JOIN " + DBUser.Table +
                                         " On " + DBUser.Table + "." + DBUser.ID +
                                         "=" + DBGrindingMoore.Table + "." + DBGrindingMoore.UserID +

                                         //join Material
                                         " LEFT JOIN " + DBMAterial.Table +
                                         " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                         "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +

                                         //join project
                                         " LEFT JOIN " + DBProjects.Table +
                                         " On " + DBProjects.Table + "." + DBProjects.ID +
                                         "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                         //join issues
                                         " LEFT JOIN " + DBIssues.Table +
                                         " On " + DBIssues.Table + "." + DBIssues.ID +
                                         "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID;

            }
        }

        public static string QueryGrindingPhoenix
        {
            get
            {
                return "SELECT * FROM " + DBGrindingPhoenix.Table +

                                        //join ReferenceRelations
                                        " LEFT JOIN " + DBProcessReferenceRelation.Table +
                                        " On " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID +
                                        "=" + DBGrindingPhoenix.Table + "." + DBGrindingPhoenix.ID +

                                        //join References
                                        " LEFT JOIN " + DBProcessReferences.Table +
                                        " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                        "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber +

                                        //join Workpiece
                                        " LEFT JOIN " + DBWorkpieces.Table +
                                        " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                        "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                        //join User
                                        " LEFT JOIN " + DBUser.Table +
                                        " On " + DBUser.Table + "." + DBUser.ID +
                                        "=" + DBGrindingPhoenix.Table + "." + DBGrindingPhoenix.UserID +

                                        //join Material
                                        " LEFT JOIN " + DBMAterial.Table +
                                        " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +

                                        //join project
                                        " LEFT JOIN " + DBProjects.Table +
                                        " On " + DBProjects.Table + "." + DBProjects.ID +
                                        "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                        //join issues
                                        " LEFT JOIN " + DBIssues.Table +
                                        " On " + DBIssues.Table + "." + DBIssues.ID +
                                        "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID;

            }
        }

        public static string QueryGrindingOther
        {
            get
            {
                return "SELECT * FROM " + DBGrindingOther.Table +

                                        //join ReferenceRelations
                                        " LEFT JOIN " + DBProcessReferenceRelation.Table +
                                        " On " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID +
                                        "=" + DBGrindingOther.Table + "." + DBGrindingOther.ID +

                                        //join References
                                        " LEFT JOIN " + DBProcessReferences.Table +
                                        " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                        "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber +

                                        //join Workpiece
                                        " LEFT JOIN " + DBWorkpieces.Table +
                                        " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                        "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                        //join User
                                        " LEFT JOIN " + DBUser.Table +
                                        " On " + DBUser.Table + "." + DBUser.ID +
                                        "=" + DBGrindingOther.Table + "." + DBGrindingOther.UserID +

                                        //join Material
                                        " LEFT JOIN " + DBMAterial.Table +
                                        " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +

                                        //join project
                                        " LEFT JOIN " + DBProjects.Table +
                                        " On " + DBProjects.Table + "." + DBProjects.ID +
                                        "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                        //join issues
                                        " LEFT JOIN " + DBIssues.Table +
                                        " On " + DBIssues.Table + "." + DBIssues.ID +
                                        "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID;

            }
        }

        public static string QueryGrinded
        {
            get
            {

                return "SELECT * FROM " + DBProcessReferences.Table +

                                          //join Workpiece
                                          " LEFT JOIN " + DBWorkpieces.Table +
                                          " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                          //join Material
                                          " LEFT JOIN " + DBMAterial.Table +
                                          " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                          "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +


                                          //join project
                                          " LEFT JOIN " + DBProjects.Table +
                                          " On " + DBProjects.Table + "." + DBProjects.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                          //join issues
                                          " LEFT JOIN " + DBIssues.Table +
                                          " On " + DBIssues.Table + "." + DBIssues.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID +
                                          " WHERE " + DBWorkpieces.Table + "." + DBWorkpieces.Status + "='polished'";
            }
        }

        public static string QueryCoated
        {
            get
            {
                return "SELECT * FROM " + DBCoatingCemecon.Table +

                                          //join ReferenceRelations
                                          " LEFT JOIN " + DBProcessReferenceRelation.Table +
                                          " On " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID +
                                          "=" + DBCoatingCemecon.Table + "." + DBCoatingCemecon.ID +

                                          //join References
                                          " LEFT JOIN " + DBProcessReferences.Table +
                                          " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                          "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber +

                                          //join Standardprocesses
                                          " LEFT JOIN " + DBCoatingCemeconProcess.Table +
                                          " On " + DBCoatingCemeconProcess.Table + "." + DBCoatingCemeconProcess.ID +
                                          "=" + DBCoatingCemecon.Table + "." + DBCoatingCemecon.CoatingProcessID +

                                          //join Workpiece
                                          " LEFT JOIN " + DBWorkpieces.Table +
                                          " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                           //join Material
                                          " LEFT JOIN " + DBMAterial.Table +
                                          " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                          "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +

                                           //join User
                                          " LEFT JOIN " + DBUser.Table +
                                          " On " + DBUser.Table + "." + DBUser.ID +
                                          "=" + DBCoatingCemecon.Table + "." + DBCoatingCemecon.UserID +

                                          //join project
                                          " LEFT JOIN " + DBProjects.Table +
                                          " On " + DBProjects.Table + "." + DBProjects.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                          //join issues
                                          " LEFT JOIN " + DBIssues.Table +
                                          " On " + DBIssues.Table + "." + DBIssues.ID +
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID;// +
                                          //" WHERE " + DBWorkpieces.Table + "." + DBWorkpieces.Status + "='coated'";

            }
        }

        public static string QueryCoatedReferences
        {
            get
            {
                return "SELECT * FROM " + DBProcessReferences.Table +

                                                 //join Workpiece
                                                 " LEFT JOIN " + DBWorkpieces.Table +
                                                 " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                                 "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +
                                                 " WHERE " + DBProcessReferences.Table + "." + DBProcessReferences.Status + "='coated'";


            }
        }

        public static string QueryProcessedMoore
        {
            get
            {
                return "SELECT * FROM " + DBExpMoore.Table +

                                              //join ReferenceRelations
                                              " LEFT JOIN " + DBProcessReferenceRelation.Table +
                                              " On " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID +
                                              "=" + DBExpMoore.Table + "." + DBExpMoore.ID +

                                              //join References
                                              " LEFT JOIN " + DBProcessReferences.Table +
                                              " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                              "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber +

                                              //join Workpiece
                                              " LEFT JOIN " + DBWorkpieces.Table +
                                              " On " + DBWorkpieces.Table + "." + DBWorkpieces.ID +
                                              "=" + DBProcessReferences.Table + "." + DBProcessReferences.WorkpiceID +

                                               //join Material
                                              " LEFT JOIN " + DBMAterial.Table +
                                              " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                              "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID +

                                               //join Glasses
                                              " LEFT JOIN " + DBGlasses.Table +
                                              " On " + DBGlasses.Table + "." + DBGlasses.ID +
                                              "=" + DBExpMoore.Table + "." + DBExpMoore.GlassID +

                                               //join User
                                              " LEFT JOIN " + DBUser.Table +
                                              " On " + DBUser.Table + "." + DBUser.ID +
                                              "=" + DBExpMoore.Table + "." + DBExpMoore.UserID +

                                              //join project
                                              " LEFT JOIN " + DBProjects.Table +
                                              " On " + DBProjects.Table + "." + DBProjects.ID +
                                              "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                              //join issues
                                              " LEFT JOIN " + DBIssues.Table +
                                              " On " + DBIssues.Table + "." + DBIssues.ID +
                                              "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID +
                                              " WHERE " + DBWorkpieces.Table + "." + DBWorkpieces.Status + "='processed'";
            }
        }
    }

    public class FilterCriteria
    {
        public string DatabaseField { get; set; }
        public string Name { get; set; }
    }


}
