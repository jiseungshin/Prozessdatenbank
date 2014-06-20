using System;
using PDCore.Database;

namespace PDCore.Manager
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


        public static String QueryTurningMoore
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
                                          

                //return "SELECT ProcessReferenceRelation.ReferenceNumber, Workpieces.Label, Materials.Name, Projects.ProjectName, Turning_Moore.*, User.Token " +
                //        "FROM (Turning_Moore INNER JOIN (((Projects INNER JOIN ProcessReferences ON Projects.Project_ID = ProcessReferences.Project_ID) INNER JOIN (Workpieces INNER JOIN Materials ON Workpieces.Material_ID = Materials.Material_ID) ON ProcessReferences.Workpiece_ID = Workpieces.WorkPiece_ID) INNER JOIN ProcessReferenceRelation ON ProcessReferences.ReferenceNumber = ProcessReferenceRelation.ReferenceNumber) ON Turning_Moore.Turning_Moore_ID = ProcessReferenceRelation.Process_ID) INNER JOIN User ON Turning_Moore.User_ID = User.User_ID " +
                //        "WHERE (((Workpieces.Status)='polished') AND ((ProcessReferenceRelation.Machine_ID)=1))";

            }
        }

        public static string QueryGrindingMoore
        {
            get 
            {
                return "SELECT ProcessReferenceRelation.ReferenceNumber, Workpieces.Label, Materials.Name, Projects.ProjectName, Workpieces.Status, Grinding_Moore.*, User.Token "+
                        "FROM (Grinding_Moore INNER JOIN (Materials INNER JOIN (((ProcessReferenceRelation INNER JOIN ProcessReferences ON ProcessReferenceRelation.ReferenceNumber = ProcessReferences.ReferenceNumber) INNER JOIN Projects ON ProcessReferences.Project_ID = Projects.Project_ID) INNER JOIN Workpieces ON ProcessReferences.Workpiece_ID = Workpieces.WorkPiece_ID) ON Materials.Material_ID = Workpieces.Material_ID) ON Grinding_Moore.Grinding_Moore_ID = ProcessReferenceRelation.Process_ID) INNER JOIN User ON Grinding_Moore.User_ID = User.User_ID "+
                        "WHERE (((Workpieces.Status)='polished'))";

            }
        }

        public static string QueryGrindingPhoenix
        {
            get
            {
                return "SELECT ProcessReferenceRelation.ReferenceNumber, Workpieces.Label, Materials.Name, Projects.ProjectName, Workpieces.Status, Grinding_Phoenix.*, User.Token "+
                        "FROM ((Grinding_Phoenix INNER JOIN ((Materials INNER JOIN Workpieces ON Materials.Material_ID = Workpieces.Material_ID) INNER JOIN (ProcessReferenceRelation INNER JOIN ProcessReferences ON ProcessReferenceRelation.ReferenceNumber = ProcessReferences.ReferenceNumber) ON Workpieces.WorkPiece_ID = ProcessReferences.Workpiece_ID) ON Grinding_Phoenix.Grinding_Phoenix_ID = ProcessReferenceRelation.Process_ID) INNER JOIN Projects ON ProcessReferences.Project_ID = Projects.Project_ID) INNER JOIN User ON Grinding_Phoenix.User_ID = User.User_ID "+
                        "WHERE (((Workpieces.Status)='polished'))";

            }
        }

        public static string QueryGrindingOther
        {
            get
            {
                return "SELECT ProcessReferenceRelation.ReferenceNumber, Workpieces.Label, Materials.Name, Projects.ProjectName, Grinding_Other.*, User.Token "+
                        "FROM (Grinding_Other INNER JOIN ((((ProcessReferenceRelation INNER JOIN ProcessReferences ON ProcessReferenceRelation.ReferenceNumber = ProcessReferences.ReferenceNumber) INNER JOIN Projects ON ProcessReferences.Project_ID = Projects.Project_ID) INNER JOIN Workpieces ON ProcessReferences.Workpiece_ID = Workpieces.WorkPiece_ID) INNER JOIN Materials ON Workpieces.Material_ID = Materials.Material_ID) ON Grinding_Other.Grinding_Other_ID = ProcessReferenceRelation.Process_ID) INNER JOIN User ON Grinding_Other.User_ID = User.User_ID "+
                        "WHERE (((Workpieces.Status)='polished'))";

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
                                          "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID +
                                          " WHERE " + DBWorkpieces.Table + "." + DBWorkpieces.Status + "='coated'";

            }
        }
    }


}
