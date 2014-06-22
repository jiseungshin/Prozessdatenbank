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
        //[Obsolete ("Use new Base Process Workpiece Relation")]
        public void saveProcess(BaseProcess process, bool update)
        {
            string test = process.GetType().ToString();
            switch (process.GetType().ToString())
            {
                case "PDCore.Processes.PTurningMoore":
                    saveTurningMooreProcess(process, update);
                    break;
                case "PDCore.Processes.PGrindingMoore":
                    saveGrindingMooreProcess(process, update);
                    break;
                case "PDCore.Processes.PGrindingPhoenix":
                    saveGrindingPhoenixProcess(process, update);
                    break;
                case "PDCore.Processes.PGrindingOther":
                    saveGrindingOtherProcess(process, update);
                    break;
                case "PDCore.Processes.PCoatingCemecon":
                    saveCoatingCemeconProcess(process, update);
                    break;
                case "PDCore.Processes.PCoatingCemeconProcess":
                    SaveCoatingStandardProcess(process, update);
                    break;
                case "PDCore.Processes.PExpOther":
                    saveExpOtherProcess(process, update);
                    break;
                case "PDCore.Processes.PExpCemeCon":
                    saveExpCemeConProcess(process, update);
                    break;
                case "PDCore.Processes.PExpTestStation":
                    saveExpTestStationProcess(process, update);
                    break;
            }
        }

        public BaseProcess getProcess(int PID, int MachineID)
        {

            switch (MachineID)
            {
                case 1:
                    return getTurningMooreProcess(PID);
                case 2:
                    return getGrindingMooreProcess(PID);
                case 3:
                    return getGrindingPhoenixProcess(PID);
                case 4:
                    return getGrindingOtherProcess(PID);
                case 5:
                    return getCoatingCemeConProcess(PID);
                case 7:
                    return getExpTestStationProcess(PID);
                case 8:
                    return getExpCemeConProcess(PID);
                case 11:
                    return getExpOtherProcess(PID);
                      
            }

            return null;
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

        private BaseProcess getTurningMooreProcess(int PID)
        {
            PTurningMoore _p = new PTurningMoore();
            List<int> _references = getReference(PID);


            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBTurningMoore.Table  +
                                                               " where " +DBTurningMoore.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];
           
            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }

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

        private BaseProcess getGrindingMooreProcess(int PID)
        {
            PGrindingMoore _p = new PGrindingMoore();
            List<int> _references = getReference(PID);

            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBGrindingMoore.Table +
                                                               " where " + DBGrindingMoore.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }

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

        private BaseProcess getGrindingPhoenixProcess(int PID)
        {
            PGrindingPhoenix _p = new PGrindingPhoenix();
            List<int> _references = getReference(PID);
            
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBGrindingPhoenix.Table +
                                                               " where " + DBGrindingPhoenix.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBGrindingPhoenix.Date);
            _p.UserID = _dr.Field<int>(DBGrindingPhoenix.UserID);
            _p.ProcessID = _dr.Field<int>(DBGrindingPhoenix.ProcessID);
            _p.Remark = _dr.Field<string>(DBGrindingPhoenix.Remark);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getGrindingOtherProcess(int PID)
        {
            PGrindingOther _p = new PGrindingOther();
            List<int> _references = getReference(PID);

            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBGrindingOther.Table +
                                                               " where " + DBGrindingOther.ID + "=" + PID)).Tables[0].Rows[0];

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];


            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBGrindingOther.Date);
            _p.UserID = _dr.Field<int>(DBGrindingOther.UserID);
            _p.Remark = _dr.Field<string>(DBGrindingOther.Remark);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getCoatingCemeConProcess(int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBCoatingCemecon.Table +
                                                               " where " + DBCoatingCemecon.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            PCoatingCemecon _p = new PCoatingCemecon();

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


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

        private BaseProcess getExpCemeConProcess(int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBExpCemeCon.Table +
                                                               " where " + DBExpCemeCon.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            PExpCemeCon _p = new PExpCemeCon();

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBExpCemeCon.Date);
            _p.UserID = _dr.Field<int>(DBExpCemeCon.UserID);
            _p.Remark = _dr.Field<string>(DBExpCemeCon.Remark);
            _p.Atmosphere = _dr.Field<string>(DBExpCemeCon.Atmosphere);
            _p.Duration = _dr.Field<double?>(DBExpCemeCon.Duration);
            _p.GlassID = _dr.Field<int?>(DBExpCemeCon.GlassID);
            _p.Pressure = _dr.Field<double?>(DBExpCemeCon.Pressure);
            _p.ProcessID = _dr.Field<int?>(DBExpCemeCon.ProcessID);
            _p.ResultID = _dr.Field<int?>(DBExpCemeCon.ResultID);
            _p.Temperature = _dr.Field<double?>(DBExpCemeCon.Temperature);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getExpTestStationProcess(int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBExpTestStation.Table +
                                                               " where " + DBExpTestStation.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            PExpTestStation _p = new PExpTestStation();

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBExpTestStation.Date);
            _p.UserID = _dr.Field<int>(DBExpTestStation.UserID);
            _p.Remark = _dr.Field<string>(DBExpTestStation.Remark);
            _p.Atmosphere = _dr.Field<string>(DBExpTestStation.Atmosphere);
            _p.Duration = _dr.Field<double?>(DBExpTestStation.Duration);
            _p.GlassID = _dr.Field<int?>(DBExpTestStation.GlassID);
            _p.Celltemperature = _dr.Field<double?>(DBExpTestStation.CellTemperature);
            _p.CoolingTempretaure = _dr.Field<double?>(DBExpTestStation.CoolingTemperature);
            _p.Cycles = _dr.Field<int?>(DBExpTestStation.Cycles);
            _p.MaxForce = _dr.Field<double?>(DBExpTestStation.MaxForce);
            _p.PenDepth = _dr.Field<double?>(DBExpTestStation.PenDepth);
            _p.PressFedd = _dr.Field<double?>(DBExpTestStation.PressFeed);
            _p.PressTemperature = _dr.Field<double?>(DBExpTestStation.PressTemperature);
            _p.ResultID = _dr.Field<int?>(DBExpTestStation.ResultID);
            _p.SecondForce = _dr.Field<double?>(DBExpTestStation.SecForce);
            _p.WPPosition = _dr.Field<int>(DBExpTestStation.WPPosition);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getExpOtherProcess(int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBExpOther.Table +
                                                               " where " + DBExpOther.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            PExpOther _p = new PExpOther();

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBExpOther.Date);
            _p.UserID = _dr.Field<int>(DBExpOther.UserID);
            _p.Remark = _dr.Field<string>(DBExpOther.Remark);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }


        #endregion

        #region get standard processes

        private void getCemeconStandardProcesses()
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

        private List<string> createReferenceSatements(BaseProcess Process, int ProcessID, int MachineID, string status)
        {
            List<string> _queries = new List<string>();
            int _ref;

            foreach (Workpiece wp in Process.Workpieces)
            {
                _ref = getNextRefNumber();
                _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID + "," + DBProcessReferences.IssueID +
                                ") VALUES (" + _ref + ", " + wp.ID + ", " + Process.ProjectID.ToDBObject() + ", " + Process.IssueID.ToDBObject() + ")");

                _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                               ") VALUES (" + ProcessID + ", " + MachineID + "," + _ref + ")");

                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = '"+status+"' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
            
            }
            return _queries;
        }

        private void saveTurningMooreProcess(BaseProcess process, bool update)
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
            
                int _pro = getNextProcessIndex();

                _queries.AddRange(createReferenceSatements(Process, _pro,1, "polished"));


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

        private void saveGrindingMooreProcess(BaseProcess process, bool update)
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

                int _pro = getNextProcessIndex();

                _queries.AddRange(createReferenceSatements(Process, _pro,2, "polished"));


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

        private void saveGrindingPhoenixProcess(BaseProcess process, bool update)
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

                int _pro = getNextProcessIndex();

                _queries.AddRange(createReferenceSatements(Process, _pro, 3, "polished"));


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

        private void saveGrindingOtherProcess(BaseProcess process, bool update)
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
                int _pro = getNextProcessIndex();

                _queries.AddRange(createReferenceSatements(Process, _pro,4, "polished"));

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

        private void saveCoatingCemeconProcess(BaseProcess process, bool update)
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

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + wp.CurrentRefereneNumber + ")");


                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'coated' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
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

        private void saveExpCemeConProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();

            PExpCemeCon Process = process as PExpCemeCon;

            if (update)
            {

                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.Atmosphere + " = " + Process.Atmosphere.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.Duration + " = " + Process.Duration.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.GlassID + " = " + Process.GlassID.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.Pressure + " = " + Process.Pressure.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.ProcessID + " = " + Process.ProcessID.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.ResultID + " = " + Process.ResultID.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpCemeCon.Table + " Set " + DBExpCemeCon.Temperature + " = " + Process.Temperature.ToDBObject() + " WHERE " + DBExpCemeCon.ID + "=" + Process.ID);
                

            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + wp.CurrentRefereneNumber + ")");


                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'processed' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                }


                _queries.Add("INSERT INTO " + DBExpCemeCon.Table + " (" + DBExpCemeCon.ID + "," +
                                                                        DBExpCemeCon.UserID + "," +
                                                                         DBExpCemeCon.Date + "," +
                                                                          DBExpCemeCon.Atmosphere + "," +
                                                                          DBExpCemeCon.Duration + "," +
                                                                          DBExpCemeCon.GlassID + "," +
                                                                          DBExpCemeCon.Pressure + "," +
                                                                          DBExpCemeCon.ProcessID + "," +
                                                                          DBExpCemeCon.ResultID + "," +
                                                                          DBExpCemeCon.Temperature + "," +
                                                                                   DBExpCemeCon.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.Atmosphere.ToDBObject() + "," +
                                                                             Process.Duration.ToDBObject() + "," +
                                                                             Process.GlassID.ToDBObject() + "," +
                                                                             Process.Pressure.ToDBObject() + "," +
                                                                             Process.ProcessID.ToDBObject() + "," +
                                                                             Process.ResultID.ToDBObject() + "," +
                                                                             Process.Temperature.ToDBObject() + "," +
                                                                               Process.Remark.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveExpTestStationProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();

            PExpTestStation Process = process as PExpTestStation;

            if (update)
            {
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Atmosphere + " = " + Process.Atmosphere.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Duration + " = " + Process.Duration.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.GlassID + " = " + Process.GlassID.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.ResultID + " = " + Process.ResultID.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.CellTemperature + " = " + Process.Celltemperature.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.CoolingTemperature + " = " + Process.CoolingTempretaure.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Cycles + " = " + Process.Cycles.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.MaxForce + " = " + Process.MaxForce.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.PenDepth + " = " + Process.PenDepth.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.PressFeed + " = " + Process.PressFedd.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.PressTemperature + " = " + Process.PressTemperature.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.SecForce + " = " + Process.SecondForce.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.WPPosition + " = " + Process.WPPosition.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + wp.CurrentRefereneNumber + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'processed' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                }

                _queries.Add("INSERT INTO " + DBExpTestStation.Table + " (" + DBExpTestStation.ID + "," +
                                                                          DBExpTestStation.UserID + "," +
                                                                          DBExpTestStation.Date + "," +
                                                                          DBExpTestStation.Atmosphere + "," +
                                                                          DBExpTestStation.Duration + "," +
                                                                          DBExpTestStation.GlassID + "," +
                                                                          DBExpTestStation.CellTemperature + "," +
                                                                          DBExpTestStation.CoolingTemperature + "," +
                                                                          DBExpTestStation.Cycles + "," +
                                                                          DBExpTestStation.MaxForce + "," +
                                                                          DBExpTestStation.PenDepth + "," +
                                                                          DBExpTestStation.PressFeed + "," +
                                                                          DBExpTestStation.PressTemperature + "," +
                                                                          DBExpTestStation.ResultID + "," +
                                                                          DBExpTestStation.SecForce + "," +
                                                                          DBExpTestStation.WPPosition + "," +
                                                                          DBExpTestStation.Remark + ") Values (" +
                                                                             _pro + "," +
                                                                             Process.UserID.ToDBObject() + "," +
                                                                             Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.Atmosphere.ToDBObject() + "," +
                                                                             Process.Duration.ToDBObject() + "," +
                                                                             Process.GlassID.ToDBObject() + "," +
                                                                             Process.Celltemperature.ToDBObject() + "," +
                                                                             Process.CoolingTempretaure.ToDBObject() + "," +
                                                                             Process.Cycles.ToDBObject() + "," +
                                                                             Process.MaxForce.ToDBObject() + "," +
                                                                             Process.PenDepth.ToDBObject() + "," +
                                                                             Process.PressFedd.ToDBObject() + "," +
                                                                             Process.PressTemperature.ToDBObject() + "," +
                                                                             Process.ResultID.ToDBObject() + "," +
                                                                             Process.SecondForce.ToDBObject() + "," +
                                                                             Process.WPPosition.ToDBObject() + "," +
                                                                             Process.Remark.ToDBObject() + ")");
            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveExpOtherProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();

            PExpOther Process = process as PExpOther;

            if (update)
            {
                _queries.Add("Update " + DBExpOther.Table + " Set " + DBExpOther.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBExpOther.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpOther.Table + " Set " + DBExpOther.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBExpOther.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpOther.Table + " Set " + DBExpOther.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBExpOther.ID + "=" + Process.ID);
            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 1 + "," + wp.CurrentRefereneNumber + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'processed' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
                }

                _queries.Add("INSERT INTO " + DBExpOther.Table + " (" + DBExpOther.ID + "," +
                                                                          DBExpOther.UserID + "," +
                                                                          DBExpOther.Date + "," +
                                                                          DBExpOther.Remark + ") Values (" +
                                                                             _pro + "," +
                                                                             Process.UserID.ToDBObject() + "," +
                                                                             Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
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

            this.update();
        }

        #endregion

    }
}
