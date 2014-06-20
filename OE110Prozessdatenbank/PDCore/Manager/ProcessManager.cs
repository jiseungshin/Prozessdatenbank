using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PDCore.Processes;
using PDCore.Database;
using PDCore.BusinessObjects;

namespace PDCore.Manager
{
    public class ProcessManager
    {
          #region instancing

        static ProcessManager instance = null;
        static readonly object padlock = new object();

        ProcessManager()
        {
            //new instance of MySQL communication class 
            _myCommunicator = new MySQLCommunicator();
            _myCommunicator.Password = PDCore.Properties.Settings.Default.Password;
            _myCommunicator.Server = PDCore.Properties.Settings.Default.Server;
            _myCommunicator.User = PDCore.Properties.Settings.Default.User;
            _myCommunicator.Database =PDCore.Properties.Settings.Default.Database;

            //Recieve database-errors
            _myCommunicator.MessageThrown += _myCommunicator_MessageThrown;

            if (_myCommunicator.checkConnection())
            { 
            }

        }

         void _myCommunicator_MessageThrown(Communicator.MessageType mType, Exception Message)
        {
            System.Windows.MessageBox.Show(Message.Message);
        }

        public static ProcessManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ProcessManager();
                    }
                    return instance;
                }
            }
        }

        #endregion

        private MySQLCommunicator _myCommunicator;


        private List<PCoatingCemeconProcess> m_CoatingProcesses = new List<PCoatingCemeconProcess>();

        public void update()
        {
            getCemeconStandardProcesses();
        }

        public void saveProcess(BaseProcess process, List<Workpiece> Workpieces, bool update)
        {
            string test = process.GetType().ToString();
            switch (process.GetType().ToString())
            {
                case "PDCore.Processes.PTurningMoore":
                    saveTurningMooreProcess(process, Workpieces, update);
                    break;
                case "PDCore.Processes.PGrindingMoore":
                    saveGrindingMooreProcess(process, Workpieces, update);
                    break;
                case "PDCore.Processes.PGrindingPhoenix":
                    saveGrindingPhoenixProcess(process, Workpieces, update);
                    break;
                case "PDCore.Processes.PGrindingOther":
                    saveGrindingOtherProcess(process, Workpieces, update);
                    break;
                case "PDCore.Processes.PCoatingCemecon":
                    saveCoatingCemeconProcess(process, Workpieces, update);
                    break;
                case "PDCore.Processes.PCoatingCemeconProcess":
                    SaveCoatingStandardProcess(process, update);
                    break;
            }
        }


        public List<int> getReference(int PID)
        {

            DataSet _ds = _myCommunicator.getDataSet("SELECT * from " + DBProcessReferenceRelation.Table +
                                                            " where " + DBProcessReferenceRelation.PID + "=" + PID);

            List<int> m_list = new List<int>();

            for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
            {
                m_list.Add(_ds.Tables[0].Rows[i].Field<int>(DBProcessReferenceRelation.RefNumber));
            }

            return m_list;
        }

        public BaseProcess getProcessByReference(int ReferenceNumber, int MachineID)
        {
            int _PID = getPIDbyReference(ReferenceNumber);

            switch (MachineID)
            { 
                case 1:
                    return getTurningMooreProcess(_PID, ReferenceNumber);
                case 2:
                    return  getGrindingMooreProcess(_PID, ReferenceNumber);
                case 3:
                    return getGrindingPhoenixProcess(_PID, ReferenceNumber);
                case 4:
                    return getGrindingOtherProcess(_PID, ReferenceNumber);
                case 5:
                    return getCoatingCemeConProcess(_PID, ReferenceNumber);

            }
            return null;
        }

        public BaseProcess getProcessByProcessID(int ProcessID, int ReferenceNumber, int MachineID)
        {
            switch (MachineID)
            {
                case 1:
                    return getTurningMooreProcess(ProcessID, ReferenceNumber);
                case 2:
                    return getGrindingMooreProcess(ProcessID, ReferenceNumber);
                case 3:
                    return getGrindingPhoenixProcess(ProcessID, ReferenceNumber);
                case 4:
                    return getGrindingOtherProcess(ProcessID, ReferenceNumber);
                case 5:
                    return getCoatingCemeConProcess(ProcessID, ReferenceNumber);
                    
            }
            return null;
        }


        public DataSet getData(string query)
        {
            return _myCommunicator.getDataSet(query);
        }




        private int getPIDbyReference(int ReferenceNumber)
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * from " + DBProcessReferenceRelation.Table +
                                                            " where " + DBProcessReferences.RefNumber + "=" + ReferenceNumber);

            return _ds.Tables[0].Rows[0].Field<Int32>(DBProcessReferenceRelation.PID);
        }

        private int getNextRefNumber()
        {
            return _myCommunicator.getNextIndex(DBProcessReferences.Table, DBProcessReferences.RefNumber);
        }

        private int getNextProcessIndex()
        {
            return _myCommunicator.getNextIndex(DBProcessReferenceRelation.Table,DBProcessReferenceRelation.PID );
        }

        #region getProcesses

        private BaseProcess getTurningMooreProcess(int PID, int ReferenceNumber)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBTurningMoore.Table  +
                                                               " where " +DBTurningMoore.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + ReferenceNumber)).Tables[0].Rows[0];
           
            PTurningMoore _p = new PTurningMoore();

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBTurningMoore.Date);
            _p.UserID = _dr.Field<int>(DBTurningMoore.UserID);
            _p.ToolID = _dr.Field<string>(DBTurningMoore.ToolID);
            _p.Speed = _dr.Field<int?>(DBTurningMoore.Speed);
            _p.CuttingAngle = _dr.Field<int?>(DBTurningMoore.CuttingAngle);
            _p.CuttingDepth = _dr.Field<int?>(DBTurningMoore.CutDepth);
            _p.Remark = _dr.Field<string>(DBTurningMoore.Remark);
            _p.Radius = _dr.Field<int?>(DBTurningMoore.Radius);
            _p.RA = _dr.Field<int?>(DBTurningMoore.RA);
            _p.PV = _dr.Field<int?>(DBTurningMoore.PV);
            _p.Processing = _dr.Field<int>(DBTurningMoore.Processing);
            _p.isFinish = _dr.Field<bool>(DBTurningMoore.IsFinish);
            _p.Feed = _dr.Field<int?>(DBTurningMoore.Feed);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getGrindingMooreProcess(int PID, int ReferenceNumber)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBGrindingMoore.Table +
                                                               " where " + DBGrindingMoore.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + ReferenceNumber)).Tables[0].Rows[0];

            PGrindingMoore _p = new PGrindingMoore();

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBGrindingMoore.Date);
            _p.UserID = _dr.Field<int>(DBGrindingMoore.UserID);
            _p.GrindingDirection = _dr.Field<string>(DBGrindingMoore.GrindingDirection);
            _p.GrindingWheelSpeed = _dr.Field<int?>(DBGrindingMoore.GrindingWheelSpeed);
            _p.InFeed = _dr.Field<int?>(DBGrindingMoore.InFeed);
            _p.PostProduction = _dr.Field<bool>(DBGrindingMoore.PostProduction);
            _p.Remark = _dr.Field<string>(DBGrindingMoore.Remark);
            _p.PV = _dr.Field<int?>(DBGrindingMoore.PV);
            _p.RA = _dr.Field<int?>(DBGrindingMoore.RA);
            _p.TippRadius = _dr.Field<int?>(DBGrindingMoore.TipRadius);
            _p.ToolRadius = _dr.Field<int?>(DBGrindingMoore.ToolRadius);
            _p.ToolSpeed = _dr.Field<int?>(DBGrindingMoore.ToolSpeed);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getGrindingPhoenixProcess(int PID, int ReferenceNumber)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBGrindingPhoenix.Table +
                                                               " where " + DBGrindingPhoenix.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + ReferenceNumber)).Tables[0].Rows[0];

            PGrindingPhoenix _p = new PGrindingPhoenix();

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBGrindingPhoenix.Date);
            _p.UserID = _dr.Field<int>(DBGrindingPhoenix.UserID);
            _p.ProcessID = _dr.Field<int>(DBGrindingPhoenix.ProcessID);
            _p.Remark = _dr.Field<string>(DBGrindingPhoenix.Remark);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getGrindingOtherProcess(int PID, int ReferenceNumber)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBGrindingOther.Table +
                                                               " where " + DBGrindingOther.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + ReferenceNumber)).Tables[0].Rows[0];

           PGrindingOther _p = new PGrindingOther();

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBGrindingOther.Date);
            _p.UserID = _dr.Field<int>(DBGrindingOther.UserID);
            _p.Remark = _dr.Field<string>(DBGrindingOther.Remark);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getCoatingCemeConProcess(int PID, int ReferenceNumber)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBCoatingCemecon.Table +
                                                               " where " + DBCoatingCemecon.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + ReferenceNumber)).Tables[0].Rows[0];

            PCoatingCemecon _p = new PCoatingCemecon();

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBCoatingCemecon.Date);
            _p.UserID = _dr.Field<int>(DBCoatingCemecon.UserID);
            _p.Remark = _dr.Field<string>(DBCoatingCemecon.Remark);
            _p.Abnormalities = _dr.Field<string>(DBCoatingCemecon.Abnormalities);
            _p.CoatingProcessID = _dr.Field<int>(DBCoatingCemecon.CoatingProcessID);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }


        #endregion

        #region get standard processes

        public void getCemeconStandardProcesses()
        {
            m_CoatingProcesses.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBCoatingCemeconProcess.Table + " ORDER BY " + DBCoatingCemeconProcess.ID);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_CoatingProcesses.Add(new PCoatingCemeconProcess()
                {
                    ID = row.Field<int>(DBCoatingCemeconProcess.ID),
                    AdherentLayer = row.Field<string>(DBCoatingCemeconProcess.AdherentLayer),
                    ProtectiveLayer = row.Field<string>(DBCoatingCemeconProcess.ProtectiveLayer),
                    ProgramNumber = row.Field<int>(DBCoatingCemeconProcess.ProgramNumber),
                    Thickness = row.Field<int>(DBCoatingCemeconProcess.Thickness),
                    Date = row.Field<DateTime>(DBCoatingCemeconProcess.Date),
                    Remark = row.Field<string>(DBCoatingCemeconProcess.Remark),
                    isDecoating = row.Field<bool>(DBCoatingCemeconProcess.IsDecoating),
                    
                });
            }
        }

        public List<PCoatingCemeconProcess> CemeConStandardProcesses
        {
            get { return m_CoatingProcesses; }
        }
       
        #endregion

        #region SaveProcesses

        private void saveTurningMooreProcess(BaseProcess process, List<Workpiece> workpieces, bool update)
        {
            List<string> _queries = new List<string>();

            PTurningMoore Process = process as PTurningMoore;

            if (update)
            {

                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.CutDepth + " = " + Process.CuttingDepth.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.CuttingAngle + " = " + Process.CuttingAngle.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.Feed + " = " + Process.Feed.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.IsFinish + " = " + Process.isFinish.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.Processing + " = " + Process.Processing.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.PV + " = " + Process.PV.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.RA + " = " + Process.RA.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.Radius + " = " + Process.Radius.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.Speed + " = " + Process.Speed.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.ToolID + " = " + Process.ToolID.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBTurningMoore.Table + " Set " + DBTurningMoore.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBTurningMoore.ID + "=" + Process.ID);


            
            }
            else
            { 
            
                int _ref;
                int _pro = getNextProcessIndex();

                foreach(Workpiece wp in workpieces)
                {
                    _ref = getNextRefNumber();
                    _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID +
                                    ") VALUES (" + _ref + ", " + wp.ID + ", " + process.ProjectID.ToDBObject() + ")");

                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + _ref + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'polished' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                }


                _queries.Add("INSERT INTO " + DBTurningMoore.Table + " (" + DBTurningMoore.ID + "," +
                                                                        DBTurningMoore.UserID + "," +
                                                                         DBTurningMoore.Date + "," +
                                                                          DBTurningMoore.ToolID + "," +
                                                                           DBTurningMoore.Radius + "," +
                                                                            DBTurningMoore.CuttingAngle + "," +
                                                                             DBTurningMoore.Feed + "," +
                                                                             DBTurningMoore.Speed + "," +
                                                                              DBTurningMoore.CutDepth + "," +
                                                                               DBTurningMoore.Remark + "," +
                                                                                DBTurningMoore.Processing + "," +
                                                                                 DBTurningMoore.IsFinish + "," +
                                                                                  DBTurningMoore.RA + "," +
                                                                                   DBTurningMoore.PV + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.ToolID.ToDBObject() + "," +
                                                                              Process.Radius.ToDBObject() + "," +
                                                                               Process.CuttingAngle.ToDBObject() + "," +
                                                                                Process.Feed.ToDBObject() + "," +
                                                                                Process.Speed.ToDBObject() + "," +
                                                                                 Process.CuttingDepth.ToDBObject() + "," +
                                                                                  Process.Remark.ToDBObject() + "," +
                                                                                   Process.Processing.ToDBObject() + "," +
                                                                                    Process.isFinish.ToDBObject() + "," +
                                                                                     Process.RA.ToDBObject() + "," +
                                                                                      Process.PV.ToDBObject() + ")");

                
                
            
            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveGrindingMooreProcess(BaseProcess process, List<Workpiece> workpieces, bool update)
        {
            List<string> _queries = new List<string>();

            PGrindingMoore Process = process as PGrindingMoore;

            if (update)
            {

                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.GrindingDirection + " = " + Process.GrindingDirection.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.GrindingWheelSpeed + " = " + Process.GrindingWheelSpeed.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.Feed + " = " + Process.Feed.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.InFeed + " = " + Process.InFeed.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.PostProduction + " = " + Process.PostProduction.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.PV + " = " + Process.PV.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.RA + " = " + Process.RA.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.TipRadius + " = " + Process.TippRadius.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.ToolRadius + " = " + Process.ToolRadius.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.ToolSpeed + " = " + Process.ToolSpeed.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingMoore.Table + " Set " + DBGrindingMoore.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBGrindingMoore.ID + "=" + Process.ID);

            }
            else
            {

                int _ref;
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in workpieces)
                {
                    _ref = getNextRefNumber();
                    _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID +
                                    ") VALUES (" + _ref + ", " + wp.ID + ", " + process.ProjectID.ToDBObject() + ")");

                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + _ref + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'polished' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                }


                _queries.Add("INSERT INTO " + DBGrindingMoore.Table + " (" + DBGrindingMoore.ID + "," +
                                                                        DBGrindingMoore.UserID + "," +
                                                                         DBGrindingMoore.Date + "," +
                                                                          DBGrindingMoore.Feed + "," +
                                                                           DBGrindingMoore.GrindingDirection + "," +
                                                                            DBGrindingMoore.GrindingWheelSpeed + "," +
                                                                             DBGrindingMoore.InFeed + "," +
                                                                              DBGrindingMoore.PostProduction + "," +
                                                                               DBGrindingMoore.Remark + "," +
                                                                                DBGrindingMoore.TipRadius + "," +
                                                                                 DBGrindingMoore.ToolRadius + "," +
                                                                                 DBGrindingMoore.ToolSpeed + "," +
                                                                                  DBGrindingMoore.RA + "," +
                                                                                   DBGrindingMoore.PV + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.Feed.ToDBObject() + "," +
                                                                              Process.GrindingDirection.ToDBObject() + "," +
                                                                               Process.GrindingWheelSpeed.ToDBObject() + "," +
                                                                                Process.InFeed.ToDBObject() + "," +
                                                                                 Process.PostProduction.ToDBObject() + "," +
                                                                                  Process.Remark.ToDBObject() + "," +
                                                                                   Process.TippRadius.ToDBObject() + "," +
                                                                                    Process.ToolRadius.ToDBObject() + "," +
                                                                                     Process.ToolSpeed.ToDBObject() + "," +
                                                                                     Process.RA.ToDBObject() + "," +
                                                                                      Process.PV.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveGrindingPhoenixProcess(BaseProcess process, List<Workpiece> workpieces, bool update)
        {
            List<string> _queries = new List<string>();

            PGrindingPhoenix Process = process as PGrindingPhoenix;

            if (update)
            {

                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.ProcessID + " = " + Process.ProcessID.ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                

            }
            else
            {

                int _ref;
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in workpieces)
                {
                    _ref = getNextRefNumber();
                    _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID +
                                    ") VALUES (" + _ref + ", " + wp.ID + ", " + process.ProjectID.ToDBObject() + ")");

                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + _ref + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'polished' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                }


                _queries.Add("INSERT INTO " + DBGrindingPhoenix.Table + " (" + DBGrindingPhoenix.ID + "," +
                                                                        DBGrindingPhoenix.UserID + "," +
                                                                         DBGrindingPhoenix.Date + "," +
                                                                          DBGrindingPhoenix.ProcessID + "," +
                                                                                   DBGrindingPhoenix.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.ProcessID.ToDBObject() + "," +
                                                                               Process.Remark.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveGrindingOtherProcess(BaseProcess process, List<Workpiece> workpieces, bool update)
        {
            List<string> _queries = new List<string>();

            PGrindingOther Process = process as PGrindingOther;

            if (update)
            {

                _queries.Add("Update " + DBGrindingOther.Table + " Set " + DBGrindingOther.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBGrindingOther.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingOther.Table + " Set " + DBGrindingOther.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBGrindingOther.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingOther.Table + " Set " + DBGrindingOther.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBGrindingOther.ID + "=" + Process.ID);
               
            }
            else
            {

                int _ref;
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in workpieces)
                {
                    _ref = getNextRefNumber();
                    _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID +
                                    ") VALUES (" + _ref + ", " + wp.ID + ", " + process.ProjectID.ToDBObject() + ")");

                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + _ref + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'polished' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                }


                _queries.Add("INSERT INTO " + DBGrindingOther.Table + " (" + DBGrindingOther.ID + "," +
                                                                        DBGrindingOther.UserID + "," +
                                                                         DBGrindingOther.Date + "," +
                                                                                   DBGrindingOther.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                                      Process.Remark.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveCoatingCemeconProcess(BaseProcess process, List<Workpiece> workpieces, bool update)
        {
            List<string> _queries = new List<string>();

            PCoatingCemecon Process = process as PCoatingCemecon;

            if (update)
            {

                _queries.Add("Update " + DBCoatingCemecon.Table + " Set " + DBCoatingCemecon.Abnormalities + " = " + Process.Abnormalities.ToDBObject() + " WHERE " + DBCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemecon.Table + " Set " + DBCoatingCemecon.CoatingProcessID + " = " + Process.CoatingProcessID.ToDBObject() + " WHERE " + DBCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemecon.Table + " Set " + DBCoatingCemecon.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemecon.Table + " Set " + DBCoatingCemecon.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemecon.Table + " Set " + DBCoatingCemecon.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBCoatingCemecon.ID + "=" + Process.ID);

            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + wp.RefereneNumber +")");


                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'coated' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.RefereneNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.RefereneNumber);
                }


                _queries.Add("INSERT INTO " + DBCoatingCemecon.Table + " (" + DBCoatingCemecon.ID + "," +
                                                                        DBCoatingCemecon.UserID + "," +
                                                                         DBCoatingCemecon.Date + "," +
                                                                          DBCoatingCemecon.CoatingProcessID + "," +
                                                                          DBCoatingCemecon.Abnormalities + "," +
                                                                                   DBCoatingCemecon.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.CoatingProcessID.ToDBObject() + "," +
                                                                             Process.Abnormalities.ToDBObject() + "," +
                                                                               Process.Remark.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        #endregion

        #region save standard processes

        private void SaveCoatingStandardProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();

            PCoatingCemeconProcess Process = process as PCoatingCemeconProcess;

            if (update)
            {

                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.AdherentLayer + " = " + Process.AdherentLayer.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.ProtectiveLayer + " = " + Process.ProtectiveLayer.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.Thickness + " = " + Process.Thickness.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.ProgramNumber + " = " + Process.ProgramNumber.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.IsDecoating + " = " + Process.isDecoating.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);

            }
            else
            {
                _queries.Add("INSERT INTO " + DBCoatingCemeconProcess.Table + " (" + DBCoatingCemeconProcess.Date + "," +
                                                                        DBCoatingCemeconProcess.AdherentLayer + "," +
                                                                        DBCoatingCemeconProcess.ProtectiveLayer + "," +
                                                                        DBCoatingCemeconProcess.Thickness + "," +
                                                                        DBCoatingCemeconProcess.ProgramNumber + "," +
                                                                        DBCoatingCemeconProcess.Remark + "," +
                                                                        DBCoatingCemeconProcess.IsDecoating + ") Values (" +
                                                                          Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                           Process.AdherentLayer.ToDBObject() + "," +
                                                                            Process.ProtectiveLayer.ToDBObject() + "," +
                                                                             Process.Thickness.ToDBObject() + "," +
                                                                             Process.ProgramNumber.ToDBObject() + "," +
                                                                             Process.Remark.ToDBObject() + "," +
                                                                               Process.isDecoating.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        #endregion

    }
}
