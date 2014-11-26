using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Database;
using PDCore.Processes;
using PDCore.BusinessObjects;
using PDCore.IO;
using System.Data;

namespace Fixes
{
    public class ProcessFixes
    {
        private MySQLCommunicator _myCommunicator;

        public ProcessFixes()
        {
            //new instance of MySQL communication class 
            _myCommunicator = new MySQLCommunicator();
            _myCommunicator.Password = SimpleIO.getClearText(@"connection.txt")[3]; //PDCore.Properties.Settings.Default.Password;
            _myCommunicator.Server = SimpleIO.getClearText(@"connection.txt")[0]; //PDCore.Properties.Settings.Default.Server;
            _myCommunicator.User = SimpleIO.getClearText(@"connection.txt")[2]; //PDCore.Properties.Settings.Default.User;
            _myCommunicator.Database = SimpleIO.getClearText(@"connection.txt")[1]; //PDCore.Properties.Settings.Default.Database;

            //Recieve database-errors
            _myCommunicator.MessageThrown += _myCommunicator_MessageThrown;

        }

        void _myCommunicator_MessageThrown(Communicator.MessageType mType, Exception Message)
        {
            //1062 duplicate number
            if ((Message as MySql.Data.MySqlClient.MySqlException).Number == 1062)
            {
                System.Windows.MessageBox.Show("Der Datensatz kann nicht gespeicher werden, da der eingegebene Wert bereits existiert",
                                                    "Hinweis", System.Windows.MessageBoxButton.OK,
                                                    System.Windows.MessageBoxImage.Error);
            }
            else
            {
                System.Windows.MessageBox.Show(Message.Message, "Hinweis", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            }

        }


        //Nur zu initialen Gebrauch!!! Keine Produktiv-Funktion
        public void ImportToshibaMData()
        {

            List<string> _queries = new List<string>();
            int PID = 0;
            foreach (string path in System.IO.Directory.GetFiles(@"Data\Toshiba\"))
            {
                int idx = path.LastIndexOf('\\');
                string PII = path.Remove(0, idx + 1);
                PID = Convert.ToInt32(PII.Replace(".mon", ""));
                //System.Windows.MessageBox.Show(PID.ToString());
                var file = PDCore.ToshibaImport.IO.getMonFileData(path);
                PToshiba Process = PDCore.ToshibaImport.Analytics.AnalyseProcess(file);





                _queries.Add("INSERT INTO " + DBExpToshibaMachineData.Table + " (" + DBExpToshibaMachineData.PID + "," +
                                                                                   DBExpToshibaMachineData.G1_lower + "," +
                                                                                   DBExpToshibaMachineData.G1_lower_quali + "," +
                                                                                   DBExpToshibaMachineData.G1_upper + "," +
                                                                                   DBExpToshibaMachineData.G1_upper_quali + "," +
                                                                                   DBExpToshibaMachineData.G3_lower + "," +
                                                                                   DBExpToshibaMachineData.G3_lower_quali + "," +
                                                                                   DBExpToshibaMachineData.G3_upper + "," +
                                                                                   DBExpToshibaMachineData.G3_upper_quali + "," +
                                                                                   DBExpToshibaMachineData.G4_lower + "," +
                                                                                   DBExpToshibaMachineData.G4_lower_quali + "," +
                                                                                   DBExpToshibaMachineData.G4_upper + "," +
                                                                                   DBExpToshibaMachineData.G4_upper_quali + "," +
                                                                                   DBExpToshibaMachineData.Gv_lower + "," +
                                                                                   DBExpToshibaMachineData.Gv_lower_quali + "," +
                                                                                   DBExpToshibaMachineData.Gv_upper + "," +
                                                                                   DBExpToshibaMachineData.Gv_upper_quali + "," +
                                                                                   DBExpToshibaMachineData.P1 + "," +
                                                                                   DBExpToshibaMachineData.P2 + "," +
                                                                                   DBExpToshibaMachineData.P3 + "," +
                                                                                   DBExpToshibaMachineData.PT1 + "," +
                                                                                   DBExpToshibaMachineData.ST1 + "," +
                                                                                   DBExpToshibaMachineData.ST2 + "," +
                                                                                   DBExpToshibaMachineData.T_press_av + "," +
                                                                                   DBExpToshibaMachineData.T_press_G + "," +
                                                                                   DBExpToshibaMachineData.T_press_G_quali + "," +
                                                                                   DBExpToshibaMachineData.T_start_lower + "," +
                                                                                   DBExpToshibaMachineData.T_start_upper + "," +
                                                                                   DBExpToshibaMachineData.T1_lower + "," +
                                                                                   DBExpToshibaMachineData.T1_upper + "," +
                                                                                   DBExpToshibaMachineData.T1u_lower + "," +
                                                                                   DBExpToshibaMachineData.T1u_upper + "," +
                                                                                   DBExpToshibaMachineData.T2_lower + "," +
                                                                                   DBExpToshibaMachineData.T2_upper + "," +
                                                                                   DBExpToshibaMachineData.T3_lower + "," +
                                                                                   DBExpToshibaMachineData.T3_upper + "," +
                                                                                   DBExpToshibaMachineData.T4_lower + "," +
                                                                                   DBExpToshibaMachineData.T4_upper + "," +
                                                                                   DBExpToshibaMachineData.T5_lower + "," +
                                                                                   DBExpToshibaMachineData.T5_upper + "," +
                                                                                   DBExpToshibaMachineData.Tv_lower + "," +
                                                                                   DBExpToshibaMachineData.Tv_upper + "," +
                                                                                   DBExpToshibaMachineData.Tvu_lower + "," +
                                                                                   DBExpToshibaMachineData.Tvu_upper + ") Values (" +

                                                                               PID + "," +
                                                                               Process.MachinaData.G1_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.G1_lower_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.G1_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.G1_upper_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.G3_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.G3_lower_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.G3_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.G3_upper_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.G4_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.G4_lower_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.G4_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.G4_upper_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.Gv_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.Gv_lower_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.Gv_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.Gv_upper_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.P1.ToDBObject() + "," +
                                                                               Process.MachinaData.P2.ToDBObject() + "," +
                                                                               Process.MachinaData.P3.ToDBObject() + "," +
                                                                               Process.MachinaData.PT1.ToDBObject() + "," +
                                                                               Process.MachinaData.ST1.ToDBObject() + "," +
                                                                               Process.MachinaData.ST2.ToDBObject() + "," +
                                                                               Process.MachinaData.T_press_av.ToDBObject() + "," +
                                                                               Process.MachinaData.T_press_G.ToDBObject() + "," +
                                                                               Process.MachinaData.T_press_G_quali.ToDBObject() + "," +
                                                                               Process.MachinaData.T_start_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T_start_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.T1_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T1_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.T1u_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T1u_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.T2_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T2_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.T3_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T3_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.T4_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T4_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.T5_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.T5_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.Tv_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.Tv_upper.ToDBObject() + "," +
                                                                               Process.MachinaData.Tvu_lower.ToDBObject() + "," +
                                                                               Process.MachinaData.Tvu_upper.ToDBObject() + ")");

                _queries.Add("update " + DBExpToshiba.Table + " set " + DBExpToshiba.T4 + "=" + Process.MachinaData.T4_lower.ToDBObject() + " where " + DBExpToshiba.ID + "=" + PID);
            }







            bool success = _myCommunicator.executeTransactedQueries(_queries);

        }

        public void insertReferences()
        {
            //-------------------------------------------------------------------------------
            //Toshiba
            //-------------------------------------------------------------------------------
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBExpToshiba.Table);

            int LowerRef;
            int UpperRef;

            int finalUpper;
            int finallower;

            List<string> _queries = new List<string>();

            int i = 0;

            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                int PID = dr.Field<int>(DBExpToshiba.ID);
                int UpperWP = dr.Field<int>(DBExpToshiba.UpperWPID);
                int LowerWP = dr.Field<int>(DBExpToshiba.LowerWPID);

                //get references by PID
                DataSet _ds2 = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferenceRelation.Table + " where " + DBProcessReferenceRelation.PID + " = " + PID);

                LowerRef = _ds2.Tables[0].Rows[0].Field<int>(DBProcessReferenceRelation.RefNumber);
                UpperRef = _ds2.Tables[0].Rows[1].Field<int>(DBProcessReferenceRelation.RefNumber);

                DataSet _ds3 = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + " = " + UpperRef);
                DataSet _ds4 = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + " = " + LowerRef);

                int finalUpperWP = _ds3.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);
                int finallowerWP = _ds4.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

                if (finallowerWP == LowerWP)
                {
                    finallower = LowerRef;
                    finalUpper = UpperRef;
                }

                else
                {
                    finalUpper = LowerRef;
                    finallower = UpperRef;
                }

                _queries.Add("update " + DBExpToshiba.Table + " set " + DBExpToshiba.UpperRef + "=" + finalUpper + "," + DBExpToshiba.LowerRef + "=" + finallower + " where " + DBExpToshiba.ID + "=" + PID);

            }

            //-------------------------------------------------------------------------------

            //EXP_Moore
            DataSet _dsMoore = _myCommunicator.getDataSet("SELECT * FROM " + DBExpMoore.Table);

            foreach (DataRow dr in _dsMoore.Tables[0].Rows)
            {
                int PID = dr.Field<int>(DBExpMoore.ID);
                int? UpperWP = dr.Field<int?>(DBExpMoore.UpperWorkpieceID);
                int? LowerWP = dr.Field<int?>(DBExpMoore.LowerWorkpieceID);

                //get references by PID
                DataSet _ds2 = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferenceRelation.Table + " where " + DBProcessReferenceRelation.PID + " = " + PID);

                if (LowerWP != null)
                {

                    DataSet _dsLowerReferences = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.WorkpiceID + " = " + LowerWP);

                    int? LowerReferenceNumber = null;

                    foreach (DataRow cand in _ds2.Tables[0].Rows)
                    {
                        int reference = cand.Field<int>(DBProcessReferenceRelation.RefNumber);
                        foreach (DataRow candReference in _dsLowerReferences.Tables[0].Rows)
                        {
                            if (reference == candReference.Field<int>(DBProcessReferences.RefNumber))
                            {
                                LowerReferenceNumber = reference;
                                break;
                            }
                        }
                        if (LowerReferenceNumber != null)
                            break;
                    }

                    _queries.Add("update " + DBExpMoore.Table + " set " + DBExpMoore.LowerReference + "=" + LowerReferenceNumber.ToDBObject() + " where " + DBExpMoore.ID + "=" + PID);
                }


                if (UpperWP != null)
                {

                    DataSet _dsUpperReferences = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.WorkpiceID + " = " + UpperWP);

                    int? UpperReferenceNumber = null;

                    foreach (DataRow cand in _ds2.Tables[0].Rows)
                    {
                        int reference = cand.Field<int>(DBProcessReferenceRelation.RefNumber);
                        foreach (DataRow candReference in _dsUpperReferences.Tables[0].Rows)
                        {
                            if (reference == candReference.Field<int>(DBProcessReferences.RefNumber))
                            {
                                UpperReferenceNumber = reference;
                                break;
                            }
                        }
                        if (UpperReferenceNumber != null)
                            break;
                    }

                    _queries.Add("update " + DBExpMoore.Table + " set " + DBExpMoore.UpperReference + "=" + UpperReferenceNumber.ToDBObject() + " where " + DBExpMoore.ID + "=" + PID);
                }
            }


            //-------------------------------------------------------------------------------
            //Cemecon
            //-------------------------------------------------------------------------------
            _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBCoatingCemecon.Table);

            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                int PID = dr.Field<int>(DBCoatingCemecon.ID);

                DataSet _ds2 = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferenceRelation.Table + " where " + DBProcessReferenceRelation.PID + " = " + PID);

                int _ref = _ds2.Tables[0].Rows[0].Field<int>(DBProcessReferenceRelation.RefNumber);
                _queries.Add("update " + DBCoatingCemecon.Table + " set " + DBCoatingCemecon.Reference + "=" + _ref + " where " + DBCoatingCemecon.ID + "=" + PID);
            }

            //-------------------------------------------------------------------------------

            _myCommunicator.executeTransactedQueries(_queries);
        }
    }
}
