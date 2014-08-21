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
            _myCommunicator.Password = IO.SimpleIO.getClearText(@"connection.txt")[3]; //PDCore.Properties.Settings.Default.Password;
            _myCommunicator.Server = IO.SimpleIO.getClearText(@"connection.txt")[0]; //PDCore.Properties.Settings.Default.Server;
            _myCommunicator.User = IO.SimpleIO.getClearText(@"connection.txt")[2]; //PDCore.Properties.Settings.Default.User;
            _myCommunicator.Database = IO.SimpleIO.getClearText(@"connection.txt")[1]; //PDCore.Properties.Settings.Default.Database;

            //Recieve database-errors
            _myCommunicator.MessageThrown += _myCommunicator_MessageThrown;

            if (_myCommunicator.checkConnection())
            { 
            }

            m_criteria.Add(new FilterCriteria() { DatabaseField = DBProjects.Table + "." + DBProjects.Name, Name = "Projekt" });
            m_criteria.Add(new FilterCriteria() { DatabaseField = DBWorkpieces.Table + "." + DBWorkpieces.Label, Name = "Werkstück" });
            m_criteria.Add(new FilterCriteria() { DatabaseField = DBProcessReferences.Table + "." + DBProcessReferences.RefNumber, Name = "Vorgangsnummer" });

            m_criteriaCoating.Add(new FilterCriteria() { DatabaseField = DBProjects.Table + "." + DBProjects.Name, Name = "Projekt" });
            m_criteriaCoating.Add(new FilterCriteria() { DatabaseField = DBWorkpieces.Table + "." + DBWorkpieces.Label, Name = "Werkstück" });
            m_criteriaCoating.Add(new FilterCriteria() { DatabaseField = DBCoatingCemecon.Table + "." + DBCoatingCemecon.ProcessNumber, Name = "Prozessnummer" });
            m_criteriaCoating.Add(new FilterCriteria() { DatabaseField = "pl." + DBCoatingLayers.Layer, Name = "Schutzschicht" });
            m_criteriaCoating.Add(new FilterCriteria() { DatabaseField = "al."+DBCoatingLayers.Layer, Name = "Haftschicht" });
            m_criteriaCoating.Add(new FilterCriteria() { DatabaseField = DBProcessReferences.Table + "." + DBProcessReferences.RefNumber, Name = "Vorgangsnummer" });

            Updater.Instance.newData += Instance_newData;

        
        }

        void Instance_newData(params string[] values)
        {
            OnUpdateTrigger();
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
        private List<FilterCriteria> m_criteria = new List<FilterCriteria>();
        private List<FilterCriteria> m_criteriaCoating = new List<FilterCriteria>();
        private List<FilterCriteria> m_criteriaProcessingToshiba = new List<FilterCriteria>();
        private List<PCoatingCemeconProcess.Layer> m_layers = new List<PCoatingCemeconProcess.Layer>();

        public void update()
        {
            getCemeconStandardProcesses();
            getLayers();
            OnUpdateTrigger();
        }

        public void TriggerUpdate()
        { OnUpdateTrigger(); }

        public delegate void UpdateHandler();
        public event UpdateHandler newProcesses;

        private void OnUpdateTrigger()
        {
            if (newProcesses != null)
                newProcesses();
        }

        public void saveProcess(BaseProcess process, bool update, bool nextTry = false)
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
                case "PDCore.Processes.PExpMoore":
                    saveExpMooreProcess(process, update);
                    break;
                case "PDCore.Processes.PDECoatingCemecon":
                    saveDeCoatingCemeconProcess(process, update);
                    break;
                case "PDCore.Processes.PToshiba":
                    saveExpToshibaProcess(process, update);
                    break;
            }
        }

        public BaseProcess getProcess(int PID, int MachineID)
        {

            switch (MachineID)
            {
                case 11:
                    return getTurningMooreProcess(PID);
                case 12:
                    return getGrindingMooreProcess(PID);
                case 13:
                    return getGrindingPhoenixProcess(PID);
                case 14:
                    return getGrindingOtherProcess(PID);
                case 21:
                    return getCoatingCemeConProcess(PID);
                case 31:
                    return getExpMooreProcess(PID);
                case 32:
                    return getExpTestStationProcess(PID);
                case 33:
                    return getExpCemeConProcess(PID);
                case 34:
                    return getExpToshibaProcess(PID);
                case 36:
                    return getExpOtherProcess(PID);
                case 51:
                    return getDeCoatingCemeConProcess(PID);
                     
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

        public List<WorkpieceHistory> getReferences(int ProjectID)
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                            " where " + DBProcessReferences.ProjectID + "=" + ProjectID);


            List<WorkpieceHistory> m_list = new List<WorkpieceHistory>();
            foreach(DataRow dr in _ds.Tables[0].Rows)
            {
                m_list.Add(getWorkpieceHistory(dr.Field<int>(DBProcessReferences.RefNumber)));
            }
            
            return m_list;
        }

        public List<WorkpieceHistory> getReferences(int ProjectID, int IssueID)
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                            " where " + DBProcessReferences.ProjectID + "=" + ProjectID +
                                                            " AND " + DBProcessReferences.IssueID + "=" + IssueID);


            List<WorkpieceHistory> m_list = new List<WorkpieceHistory>();
            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                m_list.Add(getWorkpieceHistory(dr.Field<int>(DBProcessReferences.RefNumber)));
            }

            return m_list;
        }

        public DataSet getData(string query)
        {
            return _myCommunicator.getDataSet(query);
        }

        public List<int> getPIDbyReference(int ReferenceNumber, int MachineID)
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferenceRelation.Table +
                                                            " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber);
            List<int> PID_list = new List<int>();
            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                if (dr.Field<Int32>(DBProcessReferenceRelation.MachineID) == MachineID)
                    PID_list.Add(dr.Field<Int32>(DBProcessReferenceRelation.PID));
            }

            return PID_list;
        }

        internal int getNextRefNumber()
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

            DataRow _drQuality = (_myCommunicator.getDataSet("SELECT * from " + DBWorkpieceQuality.Table +
                                                              " where " + DBWorkpieceQuality.ReferenceNumber + "=" + _references[0])).Tables[0].Rows[0];
           
            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBTurningMoore.Date);
            _p.UserID = _dr.Field<int>(DBTurningMoore.UserID);
            _p.ToolID = _dr.Field<string>(DBTurningMoore.ToolID);
            _p.Speed = _dr.Field<double?>(DBTurningMoore.Speed);
            _p.CuttingAngle = _dr.Field<double?>(DBTurningMoore.CuttingAngle);
            _p.CuttingDepth = _dr.Field<double?>(DBTurningMoore.CutDepth);
            _p.Remark = _dr.Field<string>(DBTurningMoore.Remark);
            _p.Radius = _dr.Field<double?>(DBTurningMoore.Radius);
            
            _p.Processing = _dr.Field<int>(DBTurningMoore.Processing);
            _p.isFinish = _dr.Field<bool>(DBTurningMoore.IsFinish);
            _p.Feed = _dr.Field<double?>(DBTurningMoore.Feed);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            _p.RA = _drQuality.Field<int?>(DBWorkpieceQuality.Grinding_RA);
            _p.PV = _drQuality.Field<int?>(DBWorkpieceQuality.Grinding_PV);

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

            DataRow _drQuality = (_myCommunicator.getDataSet("SELECT * from " + DBWorkpieceQuality.Table +
                                                   " where " + DBWorkpieceQuality.ReferenceNumber + "=" + _references[0])).Tables[0].Rows[0];
            
            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }

            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBGrindingMoore.Date);
            _p.UserID = _dr.Field<int>(DBGrindingMoore.UserID);
            _p.GrindingDirection = _dr.Field<string>(DBGrindingMoore.GrindingDirection);
            _p.GrindingWheelSpeed = _dr.Field<double?>(DBGrindingMoore.GrindingWheelSpeed);
            _p.InFeed = _dr.Field<double?>(DBGrindingMoore.InFeed);
            _p.Feed = _dr.Field<double?>(DBGrindingMoore.Feed);
            _p.PostProduction = _dr.Field<bool>(DBGrindingMoore.PostProduction);
            _p.Remark = _dr.Field<string>(DBGrindingMoore.Remark);
            _p.TippRadius = _dr.Field<double?>(DBGrindingMoore.TipRadius);
            _p.ToolRadius = _dr.Field<double?>(DBGrindingMoore.ToolRadius);
            _p.ToolSpeed = _dr.Field<double?>(DBGrindingMoore.ToolSpeed);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            _p.PV = _drQuality.Field<int?>(DBWorkpieceQuality.Grinding_PV);
            _p.RA = _drQuality.Field<int?>(DBWorkpieceQuality.Grinding_RA);

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
            _p.Processnumber = _dr.Field<int>(DBCoatingCemecon.ProcessNumber);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            return _p;
        }

        private BaseProcess getDeCoatingCemeConProcess(int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBDeCoatingCemecon.Table +
                                                               " where " + DBDeCoatingCemecon.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

            PDECoatingCemecon _p = new PDECoatingCemecon();

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i]));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBCoatingCemecon.Date);
            _p.UserID = _dr.Field<int>(DBCoatingCemecon.UserID);
            _p.Remark = _dr.Field<string>(DBCoatingCemecon.Remark);
            _p.CoatingProcessID = _dr.Field<int>(DBCoatingCemecon.CoatingProcessID);
            _p.Processnumber = _dr.Field<int>(DBCoatingCemecon.ProcessNumber);

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
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i], PID));
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

            getProcessQuality(_p.Quality, PID);

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
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i], PID));
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
            _p.SecondForce = _dr.Field<double?>(DBExpTestStation.SecForce);
            _p.LeftWorkpieceID = _dr.Field<int?>(DBExpTestStation.LeftWPID);
            _p.CenterWorkpieceID = _dr.Field<int?>(DBExpTestStation.CenterWPID);
            _p.RightWorkpieceID = _dr.Field<int?>(DBExpTestStation.RightWPID);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            getProcessQuality(_p.Quality, _p.ID);


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

        private BaseProcess getExpMooreProcess(int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBExpMoore.Table +
                                                               " where " + DBExpMoore.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _dr2 = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];

           

            PExpMoore _p = new PExpMoore();
            
            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i], PID));
            }


            _p.ID = PID;
            _p.Date = _dr.Field<DateTime>(DBExpMoore.Date);
            _p.UserID = _dr.Field<int>(DBExpMoore.UserID);
            _p.Remark = _dr.Field<string>(DBExpMoore.Remark);
            _p.Atmosphere = _dr.Field<string>(DBExpMoore.Atmosphere);
            _p.Cycles = _dr.Field<int?>(DBExpMoore.Cycles);
            _p.GlassID = _dr.Field<int?>(DBExpMoore.GlassID);
            _p.LowerWP = _dr.Field<int?>(DBExpMoore.LowerWorkpieceID);
            _p.Force = _dr.Field<double?>(DBExpMoore.Force);
            _p.PressTime = _dr.Field<double?>(DBExpMoore.PressTime);
            _p.ProgramTitle = _dr.Field<string>(DBExpMoore.ProgramTitle);
            _p.ResultID = _dr.Field<int?>(DBExpMoore.ResultID);
            _p.ROI = _dr.Field<string>(DBExpMoore.RegionOfInterest);
            _p.Tmax = _dr.Field<double?>(DBExpMoore.Tmax);
            _p.Tmin = _dr.Field<double?>(DBExpMoore.Tmin);
            _p.TOut = _dr.Field<double?>(DBExpMoore.TOut);
            _p.UpperWP = _dr.Field<int?>(DBExpMoore.UpperWorkpieceID);

            _p.ProjectID = _dr2.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _dr2.Field<int?>(DBProcessReferences.IssueID);

            getProcessQuality(_p.Quality,PID);

            return _p;
        }

        private BaseProcess getExpToshibaProcess(int PID)
        {
            DataRow _drProcess = (_myCommunicator.getDataSet("SELECT * from " + DBExpToshiba.Table +
                                                               " where " + DBExpToshiba.ID + "=" + PID)).Tables[0].Rows[0];

            List<int> _references = getReference(PID);

            DataRow _drProject = (_myCommunicator.getDataSet("SELECT * from " + DBProcessReferences.Table +
                                                               " where " + DBProcessReferences.RefNumber + "=" + _references[0])).Tables[0].Rows[0];



            PToshiba _p = new PToshiba();

            //get Workpieces 
            for (int i = 0; i < _references.Count; i++)
            {
                _p.Workpieces.Add(ObjectManager.Instance.getWorkpieceByReference(_references[i], PID));
            }

            _p.ID = PID;
            _p.Date = _drProcess.Field<DateTime>(DBExpToshiba.Date);
            _p.UserID = _drProcess.Field<int>(DBExpToshiba.UserID);
            _p.Remark = _drProcess.Field<string>(DBExpToshiba.Remark);
            _p.GlassID = _drProcess.Field<int?>(DBExpToshiba.GlassID);
            _p.GlassName = _drProcess.Field<string>(DBExpToshiba.GlassName);
            _p.LowerWorkpiece = _drProcess.Field<int?>(DBExpToshiba.LowerWPID);
            _p.UpperWorkpiece = _drProcess.Field<int?>(DBExpToshiba.UpperWPID);
            _p.InputData.CoolingLower = _drProcess.Field<string>(DBExpToshiba.CoolingLower);
            _p.InputData.CoolingUpper = _drProcess.Field<string>(DBExpToshiba.CoolingUpper);

            _p.InputData.G1 = _drProcess.Field<double?>(DBExpToshiba.G1);
            _p.InputData.G2 = _drProcess.Field<double?>(DBExpToshiba.G2);
            _p.InputData.Gv = _drProcess.Field<double?>(DBExpToshiba.Gv);
            _p.InputData.P1 = _drProcess.Field<double?>(DBExpToshiba.P1);
            _p.InputData.P2 = _drProcess.Field<double?>(DBExpToshiba.P2);
            _p.InputData.P3 = _drProcess.Field<double?>(DBExpToshiba.P3);
            _p.InputData.PT1 = _drProcess.Field<double?>(DBExpToshiba.PT1);
            _p.InputData.ST1 = _drProcess.Field<double?>(DBExpToshiba.ST1);
            _p.InputData.ST2 = _drProcess.Field<double?>(DBExpToshiba.ST2);
            _p.InputData.T1 = _drProcess.Field<double?>(DBExpToshiba.T1);
            _p.InputData.T1u = _drProcess.Field<double?>(DBExpToshiba.T1u);
            _p.InputData.T2 = _drProcess.Field<double?>(DBExpToshiba.T2);
            _p.InputData.T3 = _drProcess.Field<double?>(DBExpToshiba.T3);
            _p.InputData.T4 = _drProcess.Field<double?>(DBExpToshiba.T4);
            _p.InputData.T5 = _drProcess.Field<double?>(DBExpToshiba.T5);
            _p.InputData.Tv = _drProcess.Field<double?>(DBExpToshiba.Tv);
            _p.InputData.Tvu = _drProcess.Field<double?>(DBExpToshiba.Tvu);
            _p.InputData.V1 = _drProcess.Field<double?>(DBExpToshiba.V1);
            _p.InputData.V2 = _drProcess.Field<double?>(DBExpToshiba.V2);
            _p.InputData.V3 = _drProcess.Field<double?>(DBExpToshiba.V3);
            _p.InputData.Z1 = _drProcess.Field<double?>(DBExpToshiba.Z1);
            _p.InputData.Z2 = _drProcess.Field<double?>(DBExpToshiba.Z2);

            _p.InputData.N2L_A = _drProcess.Field<double?>(DBExpToshiba.N2L_A);
            _p.InputData.N2L_AA = _drProcess.Field<double?>(DBExpToshiba.N2L_AA);
            _p.InputData.N2L_B = _drProcess.Field<double?>(DBExpToshiba.N2L_B);
            _p.InputData.N2L_BB = _drProcess.Field<double?>(DBExpToshiba.N2L_BB);
            _p.InputData.N2L_C = _drProcess.Field<double?>(DBExpToshiba.N2L_C);
            _p.InputData.N2L_CC = _drProcess.Field<double?>(DBExpToshiba.N2L_CC);
            _p.InputData.N2U_A = _drProcess.Field<double?>(DBExpToshiba.N2U_A);
            _p.InputData.N2U_AA = _drProcess.Field<double?>(DBExpToshiba.N2U_AA);
            _p.InputData.N2U_B = _drProcess.Field<double?>(DBExpToshiba.N2U_B);
            _p.InputData.N2U_BB = _drProcess.Field<double?>(DBExpToshiba.N2U_BB);
            _p.InputData.N2U_C = _drProcess.Field<double?>(DBExpToshiba.N2U_C);
            _p.InputData.N2U_CC = _drProcess.Field<double?>(DBExpToshiba.N2U_CC);
            _p.InputData.OutpL = _drProcess.Field<double?>(DBExpToshiba.OutpL);
            _p.InputData.OutpU = _drProcess.Field<double?>(DBExpToshiba.OutpU);

            _p.ProjectID = _drProject.Field<int?>(DBProcessReferences.ProjectID);
            _p.IssueID = _drProject.Field<int?>(DBProcessReferences.IssueID);

            getProcessQuality(_p.Quality, PID);

            return _p;
        }

        private void getProcessQuality(ProcessQuality q, int PID)
        {
            DataRow _dr = (_myCommunicator.getDataSet("SELECT * from " + DBProcessQuality.Table +
                                                              " where " + DBProcessQuality.PID + "=" + PID)).Tables[0].Rows[0];

            //q = new ProcessQuality()
            {
                q.GlassBreakage = _dr.Field<bool>(DBProcessQuality.GlassBreakage);
                q.GlassSratches = _dr.Field<bool>(DBProcessQuality.GlassScratches);
                q.GlassPeeling = _dr.Field<bool>(DBProcessQuality.GlassPeeling);
                q.OverallResult = _dr.Field<int>(DBProcessQuality.OverallResult);
                q.PV = _dr.Field<int?>(DBProcessQuality.GlassPV);
            };

        }

        #endregion

        #region get standard processes

        public List<PCoatingCemeconProcess.Layer> Layer
        { get { return m_layers; } }

        private void getLayers()
        {

            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBCoatingLayers.Table);
            m_layers.Clear();
            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_layers.Add(new PCoatingCemeconProcess.Layer() { ID = row.Field<int>(DBCoatingLayers.ID), Structure = row.Field<string>(DBCoatingLayers.Layer) });
            }
        }


        private void getCemeconStandardProcesses()
        {
            getLayers();
            m_CoatingProcesses.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBCoatingCemeconProcess.Table + " ORDER BY " + DBCoatingCemeconProcess.ID);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_CoatingProcesses.Add(new PCoatingCemeconProcess()
                {
                    ID = row.Field<int>(DBCoatingCemeconProcess.ID),
                    AdherentLayer = Layer.Find(item => item.ID == row.Field<int>(DBCoatingCemeconProcess.AdherentLayer)),
                    ProtectiveLayer = Layer.Find(item => item.ID == row.Field<int>(DBCoatingCemeconProcess.ProtectiveLayer)),
                    ProgramNumber = row.Field<int>(DBCoatingCemeconProcess.ProgramNumber),
                    Thickness = row.Field<double?>(DBCoatingCemeconProcess.Thickness),
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

        private List<string> createReferenceSatements(BaseProcess Process, int ProcessID, int MachineID, string status, ref List<int> RefNumbers)
        {
            List<string> _queries = new List<string>();
            int _ref;

            foreach (Workpiece wp in Process.Workpieces)
            {

                _ref = getNextRefNumber();
                RefNumbers.Add(_ref);

                _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID + "," + DBProcessReferences.IssueID + "," + DBProcessReferences.Status +
                            ") VALUES (" + _ref + ", " + wp.ID + ", " + Process.ProjectID.ToDBObject() + ", " + Process.IssueID.ToDBObject() + ", " + status.ToDBObject() + ")");

                _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                              ") VALUES (" + ProcessID + ", " + MachineID + "," + _ref + ")");

                _queries.Add("INSERT INTO " + DBWorkpieceQuality.Table + " (" + DBProcessReferences.RefNumber + ") VALUES (" + _ref + ")");

                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'INPROCESS' WHERE " + DBWorkpieces.ID + "=" + wp.ID);

                //check if workpice is oneway and append ReferenceNumber
                if (wp.isOneWay)
                {
                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Label + " = " + (wp.Label + _ref).ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                          
                }

            }
            return _queries;
        }

        private void saveTurningMooreProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            List<int> _refs = new List<int>();
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

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_RA + " = " + Process.RA.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_PV + " = " + Process.PV.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber);
                }
            }
            else
            { 
            
                int _pro = getNextProcessIndex();
                
                _queries.AddRange(createReferenceSatements(Process, _pro,11, "polished",ref _refs));

                _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_RA + " = " + Process.RA.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + _refs[0]);
                _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_PV + " = " + Process.PV.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + _refs[0]);

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

            bool success = _myCommunicator.executeTransactedQueries(_queries);

            if (success&& !update)
            {
                foreach (var refnr in _refs)
                    FileManager.Instance.createReferenceDirectory(refnr);

                foreach (var wp in process.Workpieces)
                {
                    if (wp.isOneWay)
                    {
                        //Copy Workpiece
                        Workpiece ClonedWorpiece = wp.clone();
                        ClonedWorpiece.isActive = true;
                        ObjectManager.Instance.saveWorkpiece(ClonedWorpiece, false);
                    }
                }
            }
        }

        private void saveGrindingMooreProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            List<int> _refs = new List<int>();

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

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_RA + " = " + Process.RA.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_PV + " = " + Process.PV.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber);
                }
            }
            else
            {

                int _pro = getNextProcessIndex();

                _queries.AddRange(createReferenceSatements(Process, _pro,12, "polished", ref _refs));

                _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_RA + " = " + Process.RA.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + _refs[0]);
                _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.Grinding_PV + " = " + Process.PV.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + _refs[0]);


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

            bool success = _myCommunicator.executeTransactedQueries(_queries);

            if (success && !update)
            {
                foreach (var refnr in _refs)
                    FileManager.Instance.createReferenceDirectory(refnr);

                foreach (var wp in process.Workpieces)
                {
                    if (wp.isOneWay)
                    {
                        //Copy Workpiece
                        Workpiece ClonedWorpiece = wp.clone();
                        ClonedWorpiece.isActive = true;
                        ObjectManager.Instance.saveWorkpiece(ClonedWorpiece, false);
                    }
                }
            }
        }

        private void saveGrindingPhoenixProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            List<int> _refs = new List<int>();

            PGrindingPhoenix Process = process as PGrindingPhoenix;

            if (update)
            {

                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.ProcessID + " = " + Process.ProcessID.ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);
                _queries.Add("Update " + DBGrindingPhoenix.Table + " Set " + DBGrindingPhoenix.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBGrindingPhoenix.ID + "=" + Process.ID);

                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + Process.Workpieces[0].CurrentReferenceNumber);
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + Process.Workpieces[0].CurrentReferenceNumber);

            }
            else
            {

                int _pro = getNextProcessIndex();

                _queries.AddRange(createReferenceSatements(Process, _pro, 13, "polished", ref _refs));


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

            bool success = _myCommunicator.executeTransactedQueries(_queries);

            if (success && !update)
            {
                foreach (var refnr in _refs)
                    FileManager.Instance.createReferenceDirectory(refnr);

                foreach (var wp in process.Workpieces)
                {
                    if (wp.isOneWay)
                    {
                        //Copy Workpiece
                        Workpiece ClonedWorpiece = wp.clone();
                        ClonedWorpiece.isActive = true;
                        ObjectManager.Instance.saveWorkpiece(ClonedWorpiece, false);
                    }
                }
            }
        }

        private void saveGrindingOtherProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            List<int> _refs = new List<int>();

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

                _queries.AddRange(createReferenceSatements(Process, _pro,14, "polished", ref _refs));

                _queries.Add("INSERT INTO " + DBGrindingOther.Table + " (" + DBGrindingOther.ID + "," +
                                                                        DBGrindingOther.UserID + "," +
                                                                         DBGrindingOther.Date + "," +
                                                                                   DBGrindingOther.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                                      Process.Remark.ToDBObject() + ")");




            }

            bool success = _myCommunicator.executeTransactedQueries(_queries);

            if (success && !update)
            {
                foreach (var refnr in _refs)
                    FileManager.Instance.createReferenceDirectory(refnr);

                foreach (var wp in process.Workpieces)
                {
                    if (wp.isOneWay)
                    {
                        //Copy Workpiece
                        Workpiece ClonedWorpiece = wp.clone();
                        ClonedWorpiece.isActive = true;
                        ObjectManager.Instance.saveWorkpiece(ClonedWorpiece, false);
                    }
                }
            }
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
                _queries.Add("Update " + DBCoatingCemecon.Table + " Set " + DBCoatingCemecon.ProcessNumber + " = " + Process.Processnumber.ToDBObject() + " WHERE " + DBCoatingCemecon.ID + "=" + Process.ID);
                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }
            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 21 + "," + wp.CurrentReferenceNumber + ")");

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'coated' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }


                _queries.Add("INSERT INTO " + DBCoatingCemecon.Table + " (" + DBCoatingCemecon.ID + "," +
                                                                        DBCoatingCemecon.UserID + "," +
                                                                         DBCoatingCemecon.Date + "," +
                                                                          DBCoatingCemecon.CoatingProcessID + "," +
                                                                          DBCoatingCemecon.ProcessNumber + "," +
                                                                          DBCoatingCemecon.Abnormalities + "," +
                                                                                   DBCoatingCemecon.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.CoatingProcessID.ToDBObject() + "," +
                                                                             Process.Processnumber.ToDBObject() + "," +
                                                                             Process.Abnormalities.ToDBObject() + "," +
                                                                               Process.Remark.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private void saveExpCemeConProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();

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

                _queries.AddRange(updateProcessQuality(process));

                foreach (Workpiece wp in Process.Workpieces)
                {
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + Process.ID }));
                    values.Clear();
                }

            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 33 + "," + wp.CurrentReferenceNumber + ")");


                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'processed' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);

                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.PID + " = " + _pro.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + " is NULL");
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + _pro }));
                    values.Clear();
                }

                Process.ID = _pro;

                _queries.Add(saveProcessQuality(process));

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
            List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();

            PExpTestStation Process = process as PExpTestStation;

            if (update)
            {
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Atmosphere + " = " + Process.Atmosphere.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Duration + " = " + Process.Duration.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.GlassID + " = " + Process.GlassID.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.CellTemperature + " = " + Process.Celltemperature.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.CoolingTemperature + " = " + Process.CoolingTempretaure.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.Cycles + " = " + Process.Cycles.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.MaxForce + " = " + Process.MaxForce.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.PenDepth + " = " + Process.PenDepth.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.PressFeed + " = " + Process.PressFedd.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.PressTemperature + " = " + Process.PressTemperature.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpTestStation.Table + " Set " + DBExpTestStation.SecForce + " = " + Process.SecondForce.ToDBObject() + " WHERE " + DBExpTestStation.ID + "=" + Process.ID);




                foreach (Workpiece wp in Process.Workpieces)
                {
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + Process.ID }));
                    values.Clear();

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }
            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 32 + "," + wp.CurrentReferenceNumber + ")");

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'processed' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);

                    //Insert PID into WP-Quality Table
                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.PID + " = " + _pro.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + " is NULL");
                    
                    //Insert all WP-Quality parameters
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + _pro }));
                    values.Clear();

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }
                
                Process.ID = _pro;
                _queries.Add(saveProcessQuality(process));

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
                                                                          DBExpTestStation.SecForce + "," +
                                                                          DBExpTestStation.LeftWPID + "," +
                                                                          DBExpTestStation.CenterWPID + "," +
                                                                          DBExpTestStation.RightWPID + "," +
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
                                                                             Process.SecondForce.ToDBObject() + "," +
                                                                             Process.LeftWorkpieceID.ToDBObject() + "," +
                                                                             Process.CenterWorkpieceID.ToDBObject() + "," +
                                                                             Process.RightWorkpieceID.ToDBObject() + "," +
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
                                   ") VALUES (" + _pro + ", " + 36 + "," + wp.CurrentReferenceNumber + ")");

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'processed' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }

                _queries.Add(saveProcessQuality(process));

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

        public bool importToshibaProcesses(List<PToshiba> processes)
        {


            foreach (var process in processes)
            {
                bool success = saveExpToshibaProcess(process, false);
            }
            

            return true;
        }

        private bool saveExpToshibaProcess(BaseProcess process,bool update)
        {
            List<string> _queries = new List<string>();
            List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();

            PToshiba Process = process as PToshiba;

            if (update)
            {

                #region UpdateProcessParameters
                
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.G1, Value = Process.InputData.G1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.G2, Value = Process.InputData.G2 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.Gv, Value = Process.InputData.Gv });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.P1, Value = Process.InputData.P1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.P2, Value = Process.InputData.P2 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.P3, Value = Process.InputData.P3 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.PT1, Value = Process.InputData.PT1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.ST1, Value = Process.InputData.ST1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.ST2, Value = Process.InputData.ST2 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.T1, Value = Process.InputData.T1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.T1u, Value = Process.InputData.T1u });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.T2, Value = Process.InputData.T2 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.T3, Value = Process.InputData.T3 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.T4, Value = Process.InputData.T4 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.T5, Value = Process.InputData.T5 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.Tv, Value = Process.InputData.Tv });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.Tvu, Value = Process.InputData.Tvu });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.V1, Value = Process.InputData.V1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.V2, Value = Process.InputData.V2 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.V3, Value = Process.InputData.V3 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.Z1, Value = Process.InputData.Z1 });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.Z2, Value = Process.InputData.Z2 });

                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2L_A, Value = Process.InputData.N2L_A });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2L_B, Value = Process.InputData.N2L_B });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2L_C, Value = Process.InputData.N2L_C });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2L_AA, Value = Process.InputData.N2L_AA });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2L_BB, Value = Process.InputData.N2L_BB });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2L_CC, Value = Process.InputData.N2L_CC });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2U_A, Value = Process.InputData.N2U_A });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2U_B, Value = Process.InputData.N2U_B });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2U_C, Value = Process.InputData.N2U_C });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2U_AA, Value = Process.InputData.N2U_AA });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2U_BB, Value = Process.InputData.N2U_BB });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.N2U_CC, Value = Process.InputData.N2U_CC });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.OutpL, Value = Process.InputData.OutpL });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.OutpU, Value = Process.InputData.OutpU });

                #endregion


                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBExpToshiba.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBExpToshiba.ID, Value = Process.ID }));
                values.Clear();
                foreach (Workpiece wp in Process.Workpieces)
                {
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + Process.ID }));
                    values.Clear();

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }

                _queries.AddRange(updateProcessQuality(Process));

                _myCommunicator.executeTransactedQueries(_queries);
            
            }

            else
            {
                 int _pro = getNextProcessIndex();

                 foreach (Workpiece wp in Process.Workpieces)
                 {
                     _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                    ") VALUES (" + _pro + ", " + 34 + "," + wp.CurrentReferenceNumber + ")");

                     _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'processed' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                     _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                     _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);

                     _queries.Add("DELETE FROM " + DBWorkpieceQuality.Table + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + " is null");
                     
                     _queries.Add("INSERT INTO " + DBWorkpieceQuality.Table + " (" + DBProcessReferences.RefNumber + "," +
                                                                                    DBWorkpieceQuality.GlassAdherence + "," +
                                                                                    DBWorkpieceQuality.MoldScratches + "," +
                                                                                    DBWorkpieceQuality.OverallResult + "," +
                                                                                    DBWorkpieceQuality.PID + "," +
                                                                                    DBWorkpieceQuality.Corrosion+") VALUES ("
                                                                                    + wp.CurrentReferenceNumber.ToDBObject() + "," +
                                                                                    wp.Quality.GlassAdherence.ToDBObject() + "," +
                                                                                    wp.Quality.MoldScratches.ToDBObject() + "," +
                                                                                    wp.Quality.OverallResult.ToDBObject() + "," +
                                                                                    _pro + "," +
                                                                                    wp.Quality.Corrosion.ToDBObject()+")");
                 }

                 Process.ID = _pro;

                 _queries.Add(saveProcessQuality(process));

                 _queries.Add("INSERT INTO " + DBExpToshiba.Table + " (" + DBExpToshiba.ID + "," +
                                                                           DBExpToshiba.CoolingLower + "," +
                                                                           DBExpToshiba.CoolingUpper + "," +
                                                                           DBExpToshiba.Date + "," +
                                                                           DBExpToshiba.G1 + "," +
                                                                           DBExpToshiba.G2 + "," +
                                                                           DBExpToshiba.GlassID + "," +
                                                                           DBExpToshiba.GlassName + "," +
                                                                           DBExpToshiba.Gv + "," +
                                                                           DBExpToshiba.LowerWPID + "," +
                                                                           DBExpToshiba.P1 + "," +
                                                                           DBExpToshiba.P2 + "," +
                                                                           DBExpToshiba.P3 + "," +
                                                                           DBExpToshiba.PT1 + "," +
                                                                           DBExpToshiba.Remark + "," +
                                                                           DBExpToshiba.ST1 + "," +
                                                                           DBExpToshiba.ST2 + "," +
                                                                           DBExpToshiba.T1 + "," +
                                                                           DBExpToshiba.T1u + "," +
                                                                           DBExpToshiba.T2 + "," +
                                                                           DBExpToshiba.T3 + "," +
                                                                           DBExpToshiba.T4 + "," +
                                                                           DBExpToshiba.T5 + "," +
                                                                           DBExpToshiba.Tv + "," +
                                                                           DBExpToshiba.Tvu + "," +
                                                                           DBExpToshiba.UpperWPID + "," +
                                                                           DBExpToshiba.UserID + "," +
                                                                           DBExpToshiba.V1 + "," +
                                                                           DBExpToshiba.V2 + "," +
                                                                           DBExpToshiba.V3 + "," +
                                                                           DBExpToshiba.Z2 + "," +
                                                                           DBExpToshiba.Z1 + "," +
                                                                           DBExpToshiba.N2L_A + "," +
                                                                           DBExpToshiba.N2L_B + "," +
                                                                           DBExpToshiba.N2L_C + "," +
                                                                           DBExpToshiba.N2L_AA + "," +
                                                                           DBExpToshiba.N2L_BB + "," +
                                                                           DBExpToshiba.N2L_CC + "," +
                                                                           DBExpToshiba.N2U_A+ "," +
                                                                           DBExpToshiba.N2U_B + "," +
                                                                           DBExpToshiba.N2U_C + "," +
                                                                           DBExpToshiba.N2U_AA + "," +
                                                                           DBExpToshiba.N2U_BB + "," +
                                                                           DBExpToshiba.N2U_CC + "," +
                                                                           DBExpToshiba.OutpL + "," +
                                                                           DBExpToshiba.OutpU + ") Values (" +

                                                                           _pro + "," +
                                                                           Process.InputData.CoolingLower.ToDBObject() + "," +
                                                                           Process.InputData.CoolingUpper.ToDBObject() + "," +
                                                                           Process.Date.ToDBObject() + "," +
                                                                           Process.InputData.G1.ToDBObject() + "," +
                                                                           Process.InputData.G2.ToDBObject() + "," +
                                                                           Process.GlassID.ToDBObject() + "," +
                                                                           Process.GlassName.ToDBObject() + "," +
                                                                           Process.InputData.Gv.ToDBObject() + "," +
                                                                           Process.LowerWorkpiece.ToDBObject() + "," +
                                                                           Process.InputData.P1.ToDBObject() + "," +
                                                                           Process.InputData.P2.ToDBObject() + "," +
                                                                           Process.InputData.P3.ToDBObject() + "," +
                                                                           Process.InputData.PT1.ToDBObject() + "," +
                                                                           Process.Remark.ToDBObject() + "," +
                                                                           Process.InputData.ST1.ToDBObject() + "," +
                                                                           Process.InputData.ST2.ToDBObject() + "," +
                                                                           Process.InputData.T1.ToDBObject() + "," +
                                                                           Process.InputData.T1u.ToDBObject() + "," +
                                                                           Process.InputData.T2.ToDBObject() + "," +
                                                                           Process.InputData.T3.ToDBObject() + "," +
                                                                           Process.InputData.T4.ToDBObject() + "," +
                                                                           Process.InputData.T5.ToDBObject() + "," +
                                                                           Process.InputData.Tv.ToDBObject() + "," +
                                                                           Process.InputData.Tvu.ToDBObject() + "," +
                                                                           Process.UpperWorkpiece.ToDBObject() + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                           Process.InputData.V1.ToDBObject() + "," +
                                                                           Process.InputData.V2.ToDBObject() + "," +
                                                                           Process.InputData.V3.ToDBObject() + "," +
                                                                           Process.InputData.Z2.ToDBObject() + "," +
                                                                           Process.InputData.Z1.ToDBObject() + "," +
                                                                           Process.InputData.N2L_A.ToDBObject() + "," +
                                                                           Process.InputData.N2L_B.ToDBObject() + "," +
                                                                           Process.InputData.N2L_C.ToDBObject() + "," +
                                                                           Process.InputData.N2L_AA.ToDBObject() + "," +
                                                                           Process.InputData.N2L_BB.ToDBObject() + "," +
                                                                           Process.InputData.N2L_CC.ToDBObject() + "," +
                                                                           Process.InputData.N2U_A.ToDBObject() + "," +
                                                                           Process.InputData.N2U_B.ToDBObject() + "," +
                                                                           Process.InputData.N2U_C.ToDBObject() + "," +
                                                                           Process.InputData.N2U_AA.ToDBObject() + "," +
                                                                           Process.InputData.N2U_BB.ToDBObject() + "," +
                                                                           Process.InputData.N2U_CC.ToDBObject() + "," +
                                                                           Process.InputData.OutpL.ToDBObject() + "," +
                                                                           Process.InputData.OutpU.ToDBObject() + ")");

                //TODO: MachineData speichen

                bool success = _myCommunicator.executeTransactedQueries(_queries);

                if (success)
                {
                    FileManager.Instance.Copy(Process.File.Path, @"Data\Toshiba\" + _pro + ".mon");
                }

                return success;

            }

            return false;
        }

        private void saveExpMooreProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();

            PExpMoore Process = process as PExpMoore;

            if (update)
            {
                _queries.AddRange(updateProcessQuality(Process));

                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Atmosphere + " = " + Process.Atmosphere.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.GlassID + " = " + Process.GlassID.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.ResultID + " = " + Process.ResultID.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Cycles + " = " + Process.Cycles.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Force + " = " + Process.Force.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.PressTime + " = " + Process.PressTime.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.ProgramTitle + " = " + Process.ProgramTitle.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.RegionOfInterest + " = " + Process.ROI.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Tmax + " = " + Process.Tmax.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.Tmin + " = " + Process.Tmin.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);
                _queries.Add("Update " + DBExpMoore.Table + " Set " + DBExpMoore.TOut + " = " + Process.TOut.ToDBObject() + " WHERE " + DBExpMoore.ID + "=" + Process.ID);

                foreach (Workpiece wp in Process.Workpieces)
                {
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + Process.ID }));
                    values.Clear();

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }
           
            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 31 + "," + wp.CurrentReferenceNumber + ")");

                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'processed' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);


                    _queries.Add("Update " + DBWorkpieceQuality.Table + " Set " + DBWorkpieceQuality.PID + " = " + _pro.ToDBObject() + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + " is NULL");
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.Corrosion, Value = wp.Quality.Corrosion });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.GlassAdherence, Value = wp.Quality.GlassAdherence });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.MoldScratches, Value = wp.Quality.MoldScratches });
                    values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.OverallResult, Value = wp.Quality.OverallResult });

                    _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieceQuality.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieceQuality.ReferenceNumber, Value = wp.CurrentReferenceNumber + " AND " + DBWorkpieceQuality.PID + "=" + _pro }));
                    
                    values.Clear();
                }

                Process.ID = _pro;

                _queries.Add(saveProcessQuality(process));

                _queries.Add("INSERT INTO " + DBExpMoore.Table + " (" + DBExpMoore.ID + "," +
                                                                          DBExpMoore.UserID + "," +
                                                                          DBExpMoore.Date + "," +
                                                                          DBExpMoore.Atmosphere + "," +
                                                                          DBExpMoore.Cycles + "," +
                                                                          DBExpMoore.GlassID + "," +
                                                                          DBExpMoore.Force + "," +
                                                                          DBExpMoore.LowerWorkpieceID + "," +
                                                                          DBExpMoore.PressTime + "," +
                                                                          DBExpMoore.ProgramTitle + "," +
                                                                          DBExpMoore.RegionOfInterest + "," +
                                                                          DBExpMoore.ResultID + "," +
                                                                          DBExpMoore.Tmax + "," +
                                                                          DBExpMoore.Tmin + "," +
                                                                          DBExpMoore.TOut + "," +
                                                                          DBExpMoore.UpperWorkpieceID + "," +
                                                                          DBExpMoore.Remark + ") Values (" +
                                                                             _pro + "," +
                                                                             Process.UserID.ToDBObject() + "," +
                                                                             Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.Atmosphere.ToDBObject() + "," +
                                                                             Process.Cycles.ToDBObject() + "," +
                                                                             Process.GlassID.ToDBObject() + "," +
                                                                             Process.Force.ToDBObject() + "," +
                                                                             Process.LowerWP.ToDBObject() + "," +
                                                                             Process.PressTime.ToDBObject() + "," +
                                                                             Process.ProgramTitle.ToDBObject() + "," +
                                                                             Process.ROI.ToDBObject() + "," +
                                                                             Process.ResultID.ToDBObject() + "," +
                                                                             Process.Tmax.ToDBObject() + "," +
                                                                             Process.Tmin.ToDBObject() + "," +
                                                                             Process.TOut.ToDBObject() + "," +
                                                                             Process.UpperWP.ToDBObject() + "," +
                                                                             Process.Remark.ToDBObject() + ")");
            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        private List<string> updateProcessQuality(BaseProcess Process)
        {

            List<string> _queries = new List<string>();
            _queries.Add("Update " + DBProcessQuality.Table + " Set " + DBProcessQuality.GlassBreakage + " = " + Process.Quality.GlassBreakage.ToDBObject() + " WHERE " + DBProcessQuality.PID + "=" + Process.ID);
            _queries.Add("Update " + DBProcessQuality.Table + " Set " + DBProcessQuality.GlassPeeling + " = " + Process.Quality.GlassPeeling.ToDBObject() + " WHERE " + DBProcessQuality.PID + "=" + Process.ID);
            _queries.Add("Update " + DBProcessQuality.Table + " Set " + DBProcessQuality.GlassScratches + " = " + Process.Quality.GlassSratches.ToDBObject() + " WHERE " + DBProcessQuality.PID + "=" + Process.ID);
            _queries.Add("Update " + DBProcessQuality.Table + " Set " + DBProcessQuality.OverallResult + " = " + Process.Quality.OverallResult.ToDBObject() + " WHERE " + DBProcessQuality.PID + "=" + Process.ID);
            _queries.Add("Update " + DBProcessQuality.Table + " Set " + DBProcessQuality.GlassPV + " = " + Process.Quality.PV.ToDBObject() + " WHERE " + DBProcessQuality.PID + "=" + Process.ID);

            return _queries;
        }

        private string saveProcessQuality(BaseProcess Process)
        {

            return "INSERT INTO " + DBProcessQuality.Table + " (" + DBProcessQuality.PID + "," +
                                                                          DBProcessQuality.GlassBreakage + "," +
                                                                          DBProcessQuality.GlassPeeling + "," +
                                                                          DBProcessQuality.GlassScratches + "," +
                                                                          DBProcessQuality.GlassPV + "," +
                                                                          DBProcessQuality.OverallResult + ") Values (" +
                                                                          Process.ID + "," +
                                                                           Process.Quality.GlassBreakage.ToDBObject() + "," +
                                                                            Process.Quality.GlassPeeling.ToDBObject() + "," +
                                                                             Process.Quality.GlassSratches.ToDBObject() + "," +
                                                                             Process.Quality.PV.ToDBObject() + "," +
                                                                               Process.Quality.OverallResult.ToDBObject() + ")";
        }

        private void saveDeCoatingCemeconProcess(BaseProcess process, bool update)
        {
            List<string> _queries = new List<string>();

            PDECoatingCemecon Process = process as PDECoatingCemecon;

            if (update)
            {

                _queries.Add("Update " + DBDeCoatingCemecon.Table + " Set " + DBDeCoatingCemecon.CoatingProcessID + " = " + Process.CoatingProcessID.ToDBObject() + " WHERE " + DBDeCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBDeCoatingCemecon.Table + " Set " + DBDeCoatingCemecon.Date + " = " + Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBDeCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBDeCoatingCemecon.Table + " Set " + DBDeCoatingCemecon.Remark + " = " + Process.Remark.ToDBObject() + " WHERE " + DBDeCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBDeCoatingCemecon.Table + " Set " + DBDeCoatingCemecon.UserID + " = " + Process.UserID.ToDBObject() + " WHERE " + DBDeCoatingCemecon.ID + "=" + Process.ID);
                _queries.Add("Update " + DBDeCoatingCemecon.Table + " Set " + DBDeCoatingCemecon.ProcessNumber + " = " + Process.Processnumber.ToDBObject() + " WHERE " + DBDeCoatingCemecon.ID + "=" + Process.ID);

            }
            else
            {
                int _pro = getNextProcessIndex();

                foreach (Workpiece wp in Process.Workpieces)
                {
                    _queries.Add("INSERT INTO " + DBProcessReferenceRelation.Table + " (" + DBProcessReferenceRelation.PID + "," + DBProcessReferenceRelation.MachineID + "," + DBProcessReferenceRelation.RefNumber +
                                   ") VALUES (" + _pro + ", " + 51 + "," + wp.CurrentReferenceNumber + ")");

                    _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'raw' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'terminated' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + process.ProjectID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                    _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + process.IssueID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
                }


                _queries.Add("INSERT INTO " + DBDeCoatingCemecon.Table + " (" + DBDeCoatingCemecon.ID + "," +
                                                                        DBDeCoatingCemecon.UserID + "," +
                                                                         DBDeCoatingCemecon.Date + "," +
                                                                          DBDeCoatingCemecon.CoatingProcessID + "," +
                                                                          DBDeCoatingCemecon.ProcessNumber + "," +
                                                                                   DBDeCoatingCemecon.Remark + ") Values (" +
                                                                          _pro + "," +
                                                                           Process.UserID.ToDBObject() + "," +
                                                                            Process.Date.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             Process.CoatingProcessID.ToDBObject() + "," +
                                                                             Process.Processnumber.ToDBObject() + "," +
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
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.AdherentLayer + " = " + Process.AdherentLayer.GetValueOrDefault().ID.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
                _queries.Add("Update " + DBCoatingCemeconProcess.Table + " Set " + DBCoatingCemeconProcess.ProtectiveLayer + " = " + Process.ProtectiveLayer.GetValueOrDefault().ID.ToDBObject() + " WHERE " + DBCoatingCemeconProcess.ID + "=" + Process.ID);
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
                                                                        Process.AdherentLayer.GetValueOrDefault().ID.ToDBObject() + "," +
                                                                        Process.ProtectiveLayer.GetValueOrDefault().ID.ToDBObject() + "," +
                                                                        Process.Thickness.ToDBObject() + "," +
                                                                        Process.ProgramNumber.ToDBObject() + "," +
                                                                        Process.Remark.ToDBObject() + "," +
                                                                        Process.isDecoating.ToDBObject() + ")");
            }

            _myCommunicator.executeTransactedQueries(_queries);

            this.update();
        }

        #endregion

        public List<FilterCriteria> FilterCriteria
        { get { return m_criteria; } }

        public List<FilterCriteria> FilterCriteriaCoating
        { get { return m_criteriaCoating; } }

        public List<Analysis> getAnalysis(int RefID)
        {
            //ObjectManager.Instance.update();
            DataTable _dt = _myCommunicator.getDataSet("SELECT * FROM " + DBAnalyses.Table +
                                                
                                              //join References
                                              " LEFT JOIN " + DBProcessReferences.Table +
                                              " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                              "=" + DBAnalyses.Table + "." + DBAnalyses.RefNumber +

                                              //join Project
                                              " LEFT JOIN " + DBProjects.Table +
                                              " On " + DBProjects.Table + "." + DBProjects.ID +
                                              "=" + DBProcessReferences.Table + "." + DBProcessReferences.ProjectID +

                                              //join References
                                              " LEFT JOIN " + DBIssues.Table +
                                              " On " + DBIssues.Table + "." + DBIssues.ID +
                                              "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID +

                                              //join References
                                              " LEFT JOIN " + DBUser.Table +
                                              " On " + DBUser.Table + "." + DBUser.ID +
                                              "=" + DBAnalyses.Table + "." + DBAnalyses.UserID +
                
                                              " WHERE " + DBAnalyses.Table+"."+ DBAnalyses.RefNumber + "=" + RefID).Tables[0];

            List<Analysis> m_analyses = new List<Analysis>();

            if (_dt.Rows.Count != 0)
            { 
                for (int i=0; i<_dt.Rows.Count; i++)
                {
                    m_analyses.Add(new Analysis()
                    {
                        ID = _dt.Rows[i].Field<int>(DBAnalyses.ID),
                        Started = _dt.Rows[i].Field<DateTime?>(DBAnalyses.Started),
                        Finished = _dt.Rows[i].Field<DateTime?>(DBAnalyses.Finished),
                        User = ObjectManager.Instance.Users.Find(item => item.ID == _dt.Rows[i].Field<int>(DBUser.ID)),
                        Description = _dt.Rows[i].Field<string>(DBAnalyses.Type),
                        ReferenceNumber = _dt.Rows[i].Field<int>(DBAnalyses.RefNumber),
                        terminated = _dt.Rows[i].Field<bool>(DBAnalyses.isTerminated),
                        Path = FileManager.Instance.StandardDirectory + _dt.Rows[i].Field<int>(DBAnalyses.RefNumber) + "\\Analysen\\" + _dt.Rows[i].Field<string>(DBAnalyses.Type)
                    });
            }}

            return m_analyses;
        }

        public void SaveAnalyses(List<Analysis> Analyses)
        {
            int refID = Analyses[0].ReferenceNumber;
            string description = Analyses[0].Description;
            
            List<string> _queries = new List<string>();

            foreach (var ana in Analyses)
            {
                //save
                if (ana.ID == -1)
                {
                    _queries.Add("INSERT INTO " + DBAnalyses.Table + " (" + DBAnalyses.RefNumber + "," +
                                                                          DBAnalyses.Started + "," +
                                                                          DBAnalyses.Finished + "," +
                                                                          DBAnalyses.UserID + "," +
                                                                          DBAnalyses.isTerminated + "," +
                                                                          DBAnalyses.Type + ") Values (" +
                                                                          ana.ReferenceNumber + "," +
                                                                           ana.Started.ToDBObject() + "," +
                                                                            ana.Finished.ToDBObject() + "," +
                                                                             ana.User.ID.ToDBObject() + "," +
                                                                             ana.terminated.ToDBObject() + "," +
                                                                               ana.Description.ToDBObject() + ")");
                    if (getCurrentStatus(ana.ReferenceNumber)== "processed")
                        _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'analysed' WHERE " + DBProcessReferences.RefNumber + "=" + ana.ReferenceNumber);
                }
                    //update
                else
                {
                    _queries.Add("Update " + DBAnalyses.Table + " Set " + DBAnalyses.Started + " = " + ana.Started.ToDBObject() + " WHERE " + DBAnalyses.ID + "=" + ana.ID);
                    _queries.Add("Update " + DBAnalyses.Table + " Set " + DBAnalyses.Finished + " = " + ana.Finished.ToDBObject() + " WHERE " + DBAnalyses.ID + "=" + ana.ID);
                    _queries.Add("Update " + DBAnalyses.Table + " Set " + DBAnalyses.UserID+ " = " + ana.User.ID+ " WHERE " + DBAnalyses.ID + "=" + ana.ID);
                    _queries.Add("Update " + DBAnalyses.Table + " Set " + DBAnalyses.isTerminated + " = " + ana.terminated + " WHERE " + DBAnalyses.ID + "=" + ana.ID);
                }
            
            }

            bool success = _myCommunicator.executeTransactedQueries(_queries);

            if (success)
            { FileManager.Instance.createDirectory(refID, "Analysen\\"+description); }

            Updater.Instance.forceUpdate();
            //OnUpdateTrigger();
        }

        public string getCurrentStatus(int RefID)
        {

            DataSet _ds = _myCommunicator.getDataSet("SELECT " + DBProcessReferences.Status + " FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.RefNumber + "=" + RefID);
            
            if (_ds.Tables.Count!=0)
            {
                if (_ds.Tables[0].Rows.Count != 0)
                    return _ds.Tables[0].Rows[0].Field<string>(DBProcessReferences.Status);
            }

            return "-1";
        
        }

        public int getCountOf(string Table, string Column, object constraint)
        {
           return _myCommunicator.getNumberOf(Table + " WHERE " + Column + "=" + constraint.ToDBObject());
        }
        public int getCountOf(string Table, string statement)
        {
            return _myCommunicator.getNumberOf(Table + " WHERE " + statement);
        }

        public void setReferenceStatus(int ReferenceNumber, string Status)
        {
            List<string> _queries = new List<string>();

            _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = '" + Status + "' WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber);

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public WorkpieceHistory getWorkpieceHistory(int ReferenceNumber)
        {
            WorkpieceHistory m_history = new WorkpieceHistory();
            m_history.ReferenceNumber = ReferenceNumber;

            DataTable _dt = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table +
                                                            //join Issues
                                                            " LEFT JOIN " + DBIssues.Table +
                                                            " On " + DBIssues.Table + "." + DBIssues.ID +
                                                            "=" + DBProcessReferences.Table + "." + DBProcessReferences.IssueID +
                                                            " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber).Tables[0];

            if (_dt.Rows.Count>0)
            {

                m_history.Status = _dt.Rows[0].Field<string>(DBProcessReferences.Status);
                m_history.Project = ObjectManager.Instance.Projects.Find(item => item.ID == _dt.Rows[0].Field<int?>(DBProcessReferences.ProjectID));
                m_history.Issue = ObjectManager.Instance.Issues.Find(item => item.ID == _dt.Rows[0].Field<int?>(DBProcessReferences.IssueID))?? new Issue();
                m_history.Workpiece = ObjectManager.Instance.getWorkpieceByReference(ReferenceNumber);  //ObjectManager.Instance.Workpieces.Find(item => item.ID == _dt.Rows[0].Field<int>(DBProcessReferences.WorkpiceID));
                m_history.Conclusion = _dt.Rows[0].Field<string>(DBIssues.Conclusion);


                //collect ProcessData
                DataTable _dtProcesses = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferenceRelation.Table + " WHERE " + DBProcessReferenceRelation.RefNumber + "=" + ReferenceNumber).Tables[0];

                foreach(DataRow dr in _dtProcesses.Rows)
                {

                    List<string> TableInfo = MachineTable(dr.Field<int>(DBProcessReferenceRelation.MachineID));

                    int PID = dr.Field<int>(DBProcessReferenceRelation.PID);

                    DataTable _dtt = _myCommunicator.getDataSet("SELECT * FROM " + TableInfo[0] + " WHERE " + TableInfo[1] + "=" + PID).Tables[0];
                    //System.Windows.MessageBox.Show(TableInfo[0] + "," + TableInfo[1]);

                    if (_dtt.Rows.Count > 0)
                    {
                        m_history.Processes.Add(new ProcessMetaData()
                        {
                            Date = _dtt.Rows[0].Field<DateTime>("Date"),
                            User = ObjectManager.Instance.Users.Find(item => item.ID == _dtt.Rows[0].Field<int>("User_ID")),
                            Machine = ObjectManager.Instance.Machines.Find(item => item.ID == dr.Field<int>(DBProcessReferenceRelation.MachineID)),
                            PID = PID
                        });
                    }

                }

                DataTable _dtAnalyses = _myCommunicator.getDataSet("SELECT * FROM " + DBAnalyses.Table + " WHERE " + DBAnalyses.RefNumber + "=" + ReferenceNumber).Tables[0];

                foreach (DataRow dr in _dtAnalyses.Rows)
                {
                    BusinessObjects.Machine m = new Machine();
                    m.Process = 40;
                    m.ID = 40;
                    m.Name = "Analyse (" + dr.Field<string>(DBAnalyses.Type) + ")";
                    m_history.Processes.Add(new ProcessMetaData()
                    {
                        Date = dr.Field<DateTime>("Started"),
                        User = ObjectManager.Instance.Users.Find(item => item.ID == dr.Field<int>("User_ID")),
                        Machine = m,
                        PID = -1
                    });
                }

                //Sort Processes by Machine ID --> Frank Spezial Decoating hinter Analyse
                m_history.Processes = m_history.Processes.OrderBy(item => item.Machine.Process).ToList();

                return m_history;
            
            }

            return null;
            
        }

        public void saveWorkpieceHistory(WorkpieceHistory WPH)
        {
            List<string> _queries = new List<string>();

            
            if (WPH.Project!=null)
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.ProjectID + " = " + WPH.Project.ID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + WPH.ReferenceNumber);
            if (WPH.Issue != null)
            {
                _queries.Add("Update " + DBIssues.Table + " Set " + DBIssues.Conclusion + " = " + WPH.Conclusion.ToDBObject() + " WHERE " + DBIssues.ID + "=" + WPH.Issue.ID);
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.IssueID + " = " + WPH.Issue.ID.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + WPH.ReferenceNumber);
            }

            //if (WPH.Conclusion!="" && WPH.Status!= DBEnum.EnumReference.CANCELLED || WPH.Status != DBEnum.EnumReference.TERMINATED)
            //{
            //    ObjectManager.Instance.ReleaseWorkpiece(WPH.Workpiece, false);
            //}

            _myCommunicator.executeTransactedQueries(_queries);

        }

        /// <summary>
        /// Allocate new status to given referenceNumber
        /// </summary>
        /// <param name="ReferenceNumber"></param>
        /// <param name="newStatus"></param>
        public void skipProcess(int ReferenceNumber, string newStatus)
        {
            List<string> _queries = new List<string>();

            _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = " + newStatus.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber);
            _myCommunicator.executeTransactedQueries(_queries);
            Updater.Instance.forceUpdate();
        }

        /// <summary>
        /// Do new try on workpiece
        /// </summary>
        /// <param name="ReferenceNumber"></param>
        /// <param name="newStatus"></param>
        public void prepareNewTry(int ReferenceNumber, string newStatus)
        {
            List<string> _queries = new List<string>();

            // Get Grinding PV and RA from previus Process
            DataRow dr = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieceQuality.Table + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + ReferenceNumber).Tables[0].Rows[0];

            //Set back status to "newStatus" --> hopefully 'coated'
            _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = " + newStatus.ToDBObject() + " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber);


            _queries.Add("INSERT INTO " + DBWorkpieceQuality.Table + " (" + DBWorkpieceQuality.ReferenceNumber + "," +
                                                                            DBWorkpieceQuality.Grinding_RA + "," +
                                                                            DBWorkpieceQuality.Grinding_PV + ")" +
                                                                            " VALUES (" + ReferenceNumber + "," +
                                                                            dr.Field<int?>(DBWorkpieceQuality.Grinding_RA).ToDBObject() + "," +
                                                                            dr.Field<int?>(DBWorkpieceQuality.Grinding_PV).ToDBObject() + ")");
            _myCommunicator.executeTransactedQueries(_queries);
            Updater.Instance.forceUpdate();
        }

        /// <summary>
        /// Steps over the fist grinding process step
        /// </summary>
        /// <param name="WorkpieceID"></param>
        /// <returns>The created reference number</returns>
        public int skipInitialProcess(Workpiece workpiece)
        {
            List<string> _queries = new List<string>();
            
            int _ref = getNextRefNumber();

            _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID + "," + DBProcessReferences.IssueID + "," + DBProcessReferences.Status +
                            ") VALUES (" + _ref + ", " + workpiece.ID + ", NULL, NULL, " + DBEnum.EnumReference.POLISHED.ToDBObject() + ")");

            _queries.Add("INSERT INTO " + DBWorkpieceQuality.Table + " (" + DBProcessReferences.RefNumber + ") VALUES (" + _ref + ")");

            _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'INPROCESS' WHERE " + DBWorkpieces.ID + "=" + workpiece.ID);

            //check if workpice is oneway and append ReferenceNumber
            if (workpiece.isOneWay)
            {
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Label + " = "+ (workpiece.Label+_ref).ToDBObject() +" WHERE " + DBWorkpieces.ID + "=" + workpiece.ID);
            }

            bool success =_myCommunicator.executeTransactedQueries(_queries);



            if (success)
            {
                if (workpiece.isOneWay)
                {
                    Workpiece ClonedWorpiece = workpiece.clone();
                    ClonedWorpiece.isActive = true;
                    ObjectManager.Instance.saveWorkpiece(ClonedWorpiece, false);
                }
                Updater.Instance.forceUpdate();
                return _ref;
            }
            else
                return -1;
        }

        /// <summary>
        /// Sets status to Cancelled or terminated
        /// </summary>
        /// <param name="workpiece"></param>
        /// <param name="Status"></param>
        public void OverrideReferenceStatus(Workpiece workpiece,string Status)
        {
            if (Status== DBEnum.EnumReference.CANCELLED)
                ObjectManager.Instance.ReleaseWorkpiece(workpiece, true);
            if (Status == DBEnum.EnumReference.TERMINATED)
                ObjectManager.Instance.ReleaseWorkpiece(workpiece, false);

            Updater.Instance.forceUpdate();

        }

        /// <summary>
        /// Checks if a Workpice with given ReferenceNumber was set back to next try
        /// </summary>
        /// <param name="ReferenceNumber"></param>
        /// <returns>True if nextTry</returns>
        public bool checkNewTry(int ReferenceNumber)
        {
            int count = _myCommunicator.getNumberOf(DBWorkpieceQuality.Table + " WHERE " + DBWorkpieceQuality.ReferenceNumber + "=" + ReferenceNumber + " AND " + DBWorkpieceQuality.PID + " is NULL");

            //PID is Null --> first try
            if (count > 0)
                return false;
            //PID is not NULL --> next try
            else
                return true;

        }

        private List<string> MachineTable(int MachineID)
        {
            List<string> m_list = new List<string>();

            switch(MachineID)
            {

                case 11:
                    m_list.Add(DBTurningMoore.Table);
                    m_list.Add(DBTurningMoore.ID);
                    break;
                case 12:
                    m_list.Add(DBGrindingMoore.Table);
                    m_list.Add(DBGrindingMoore.ID);
                    break;
                case 13:
                    m_list.Add(DBGrindingPhoenix.Table);
                    m_list.Add(DBGrindingPhoenix.ID);
                    break;
                case 14:
                    m_list.Add(DBGrindingOther.Table);
                    m_list.Add(DBGrindingOther.ID);
                    break;
                case 21:
                    m_list.Add(DBCoatingCemecon.Table);
                    m_list.Add(DBCoatingCemecon.ID);
                    break;
                case 31:
                    m_list.Add(DBExpMoore.Table);
                    m_list.Add(DBExpMoore.ID);
                    break;
                case 32:
                    m_list.Add(DBExpTestStation.Table);
                    m_list.Add(DBExpTestStation.ID);
                    break;
                case 33:
                    m_list.Add(DBExpCemeCon.Table);
                    m_list.Add(DBExpCemeCon.ID);
                    break;
                case 34:
                    m_list.Add(DBExpToshiba.Table);
                    m_list.Add(DBExpToshiba.ID);
                    break;
                //case 35:
                //    return "";
                case 36:
                    m_list.Add(DBExpOther.Table);
                    m_list.Add(DBExpOther.ID);
                    break;
                case 40:
                    m_list.Add(DBAnalyses.Table);
                    m_list.Add(DBAnalyses.ID);
                    break;
                case 51:
                    m_list.Add(DBDeCoatingCemecon.Table);
                    m_list.Add(DBDeCoatingCemecon.ID);
                    break;

            }
            return m_list;

        }

    }
}
