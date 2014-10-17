using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Database;
using PDCore.BusinessObjects;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using PDCore.Processes;

namespace PDCore.Manager
{
    public class ObjectManager
    {

        private MySQLCommunicator _myCommunicator;

        #region instancing

        static ObjectManager instance = null;
        static readonly object padlock = new object();

        ObjectManager()
        {
            //new instance of MySQL communication class 
            _myCommunicator = new MySQLCommunicator();
            _myCommunicator.Password = IO.SimpleIO.getClearText(@"connection.txt")[3]; //PDCore.Properties.Settings.Default.Password;
            _myCommunicator.Server = IO.SimpleIO.getClearText(@"connection.txt")[0]; //PDCore.Properties.Settings.Default.Server;
            _myCommunicator.User = IO.SimpleIO.getClearText(@"connection.txt")[2]; //PDCore.Properties.Settings.Default.User;
            _myCommunicator.Database = IO.SimpleIO.getClearText(@"connection.txt")[1]; //PDCore.Properties.Settings.Default.Database;

            //Recieve database-errors
            _myCommunicator.MessageThrown += _myCommunicator_MessageThrown;

            Updater.Instance.newData += Instance_newData;

            if (_myCommunicator.checkConnection())
            { 
            }

        }

        void Instance_newData(params string[] values)
        {
            if (values.Length == 0)
            {
                update();
                
            }
            else
            {
                if (values.Contains(DBMAterial.Table))
                {
                    getMaterials();
                }
            }

            OnUpdateTrigger();
        }

        public delegate void UpdateHandler();
        public event UpdateHandler newObjects;

        private void OnUpdateTrigger()
        {
            if (newObjects != null)
                newObjects();
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
                 System.Windows.MessageBox.Show(Message.Message,"Hinweis",System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);

             }
            
        }

         public static ObjectManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ObjectManager();
                    }
                    return instance;
                }
            }
        }

        #endregion


         private List<Project> m_projects = new List<Project>();
         private List<Issue> m_issues = new List<Issue>();
         private List<User> m_users = new List<User>();
         private List<Machine> m_machines = new List<Machine>();
         private List<PGrindingPhoenixProcess> m_PhoenixProcesses = new List<PGrindingPhoenixProcess>();

         private List<Workpiece> m_worlpieces = new List<Workpiece>();
         private List<Workpiece> m_coatedWorkpieces = new List<Workpiece>();
         private List<Material> m_materials = new List<Material>();
         private List<Glass> m_glasses = new List<Glass>();
         

        public void update()
        {
            getProjects();
            getUser();
            getMachines();
            getPhoenixProcesses();
            getMaterials();
            getWorkpieces();
            getIssues();
            getGlasses();
            getCoatedWorkpieces();
        }

        public void update(string Table)
        {
            if (Table == DBUser.Table)
                getUser();
            if (Table == DBProjects.Table)
                getProjects();
            if (Table == DBIssues.Table)
                getIssues();
            if (Table == DBMAterial.Table)
                getMaterials();
            if (Table == DBGlasses.Table)
                getGlasses();
            if (Table == DBWorkpieces.Table)
            {
                getWorkpieces();
                getCoatedWorkpieces();
                
            }
            if (Table == DBPhoenixProcesses.Table)
                getPhoenixProcesses();

            if (Table == DBMachine.Table)
                getMachines();
        }

        private void getGlasses()
        {
            m_glasses.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBGlasses.Table + " ORDER BY " + DBGlasses.Description);

            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                m_glasses.Add(new Glass()
                {
                    ID = dr.Field<int>(DBGlasses.ID),
                    Name = dr.Field<string>(DBGlasses.Description),
                    Comapany = dr.Field<string>(DBGlasses.Company)
                });
            }
        }

        private void getProjects()
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " ORDER BY " + DBProjects.Name);
            m_projects.Clear();
            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                
                Project _p = new Project() { ID = row.Field<int>(DBProjects.ID), Description = row.Field<string>(DBProjects.Name) };
                _p.User = Users.Find(item => item.ID == row.Field<int>(DBProjects.UserID));
                _p.Remark = row.Field<string>(DBProjects.Remark);
                _p.Started = row.Field<DateTime>(DBProjects.Started);
                _p.Finished = row.Field<DateTime?>(DBProjects.Finished);

                DataSet _dsIssue = _myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ProjectID + "=" + _p.ID);

                foreach (DataRow dr in _dsIssue.Tables[0].Rows)
                {
                    _p.Issues.Add(new Issue()
                    {
                        ID = dr.Field<int>(DBIssues.ID),
                        Description = dr.Field<string>(DBIssues.Description),
                        ProjectID = dr.Field<int>(DBIssues.ProjectID),
                        Remark = dr.Field<string>(DBIssues.Remark)
                    });
                    
                }
                
                m_projects.Add(_p);
            }

        }

        private void getIssues()
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " ORDER BY " + DBIssues.Description);
            m_issues.Clear();
            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_issues.Add(new Issue() { ID = row.Field<int>(DBIssues.ID), Description = row.Field<string>(DBIssues.Description), ProjectID = row.Field<int>(DBIssues.ProjectID) });
            }

        }

       

        private void getUser()
        {
            m_users.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBUser.Table + " ORDER BY " + DBUser.LastName);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_users.Add(new User() { 
                    ID = row.Field<int>(DBUser.ID), 
                    FirstName = row.Field<string>(DBUser.FirstName),
                    LastName = row.Field<string>(DBUser.LastName), 
                    Token = row.Field<string>(DBUser.Token),
                    isActive = row.Field<bool>(DBUser.isActive),
                    MachineID = row.Field<int?>(DBUser.MachineID)
                });
            }

        }

        private void getMachines()
        {
            m_machines.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBMachine.Table + " ORDER BY " + DBMachine.Name);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_machines.Add(new Machine() { ID = row.Field<int>(DBMachine.ID), Name = row.Field<string>(DBMachine.Name), Process = row.Field<int>(DBMachine.Process) });
            }
        }

        private void getPhoenixProcesses()
        {
            m_PhoenixProcesses.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBPhoenixProcesses.Table + " ORDER BY " + DBPhoenixProcesses.ID);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_PhoenixProcesses.Add(new PGrindingPhoenixProcess() { 
                                    ID = row.Field<int>(DBPhoenixProcesses.ID), 
                                    Description = row.Field<string>(DBPhoenixProcesses.Description),
                                    Remark = row.Field<string>(DBPhoenixProcesses.Remark),
                                    Ra = row.Field<double>(DBPhoenixProcesses.Ra)});
            }
        }

        private void getMaterials()
        {
            m_materials.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBMAterial.Table + " ORDER BY " + DBMAterial.Name);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_materials.Add(new Material() { ID = row.Field<int>(DBMAterial.ID), Name = row.Field<string>(DBMAterial.Name) });
            }
        }

        private void getWorkpieces()
        {
            m_worlpieces.Clear();
            Material mm;
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieces.Table +

                                                        //join user
                                                        " LEFT JOIN " + DBUser.Table +
                                                        " On " + DBUser.Table + "." + DBUser.ID +
                                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.Initiator_ID +
                                                        
                                                        //join materials
                                                        " LEFT JOIN " + DBMAterial.Table +
                                                        " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                mm = new Material() { ID = row.Field<int>(DBMAterial.ID), Name = row.Field<string>(DBMAterial.Name) };
                
                //Frank spezial Status aufgeschlüsselt für Werkstückverwaltung
                string Status = row.Field<string>(DBWorkpieces.Status);

                DataSet _dsStatus = _myCommunicator.getDataSet("SELECT Status FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.WorkpiceID + "=" + row.Field<int>(DBWorkpieces.ID));// + " AND 'Status' not in ('terminated') AND 'Status' not in ('cancelled')");
                if (_dsStatus.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow rowStatus in _dsStatus.Tables[0].Rows)
                    {
                        Status = rowStatus.Field<string>(DBProcessReferences.Status);
                    }
                }

                m_worlpieces.Add(new Workpiece()
                {
                    ID = row.Field<int>(DBWorkpieces.ID),
                    Label = row.Field<string>(DBWorkpieces.Label),
                    Material = mm,
                    BatchNumber = row.Field<string>(DBWorkpieces.BatchNumber),
                    PurchaseDate = row.Field<DateTime?>(DBWorkpieces.PurchaseDate),
                    Geometry = row.Field<string>(DBWorkpieces.Geometry),
                    isOneWay = row.Field<bool>(DBWorkpieces.isOneWay),
                    KindOfProbe = row.Field<string>(DBWorkpieces.KindOfProbe),
                    Status = Status,//row.Field<string>(DBWorkpieces.Status),
                    isActive = row.Field<bool>(DBWorkpieces.isActive),
                    InitiatorID = row.Field<int?>(DBWorkpieces.Initiator_ID),
                    Remark = row.Field<string>(DBWorkpieces.Remark)
                });
            }
        }


        public List<Glass> Glasses
        { get { return m_glasses; } }

        public List<Material> Materials
        { get { return m_materials; } }

        public List<Workpiece> Workpieces
        { get { return m_worlpieces; } }

        public List<PGrindingPhoenixProcess> PhoenixProcesses
        {
            get { return m_PhoenixProcesses; }
        }
        public List<Machine> Machines
        { get { return m_machines; } }

        public List<User> Users
        { get { return m_users.OrderBy(item=>item.Token).ToList(); } }

        public List<Project> Projects
        { get { return m_projects; } }

        public List<Issue> Issues
        { get { return m_issues; } }

       
        public Workpiece getWorkpiece(int WPID)
        {
            return m_worlpieces.Find(item => item.ID == WPID); 
        }

        public Workpiece getWorkpieceByReference(int RefNumber)
        {

            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table +" where " + DBProcessReferences.RefNumber + "=" + RefNumber);
            DataRow _dsWPQuality = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieceQuality.Table + " where " + DBWorkpieceQuality.ReferenceNumber + "=" + RefNumber).Tables[0].Rows[0];


            int wp_ID = _ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

            Workpiece _wp; //;= getWorkpiece(wp_ID);
            //---------------------------------------------------------------------------------------------------------------
            Material mm;
            DataRow row = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieces.Table +

                                                        //join materials
                                                        " LEFT JOIN " + DBMAterial.Table +
                                                        " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID + " WHERE " + DBWorkpieces.ID + "=" + wp_ID).Tables[0].Rows[0];


            mm = new Material() { ID = row.Field<int>(DBMAterial.ID), Name = row.Field<string>(DBMAterial.Name) };
            _wp = new Workpiece()
            {
                ID = row.Field<int>(DBWorkpieces.ID),
                Label = row.Field<string>(DBWorkpieces.Label),
                Material = mm,
                BatchNumber = row.Field<string>(DBWorkpieces.BatchNumber),
                PurchaseDate = row.Field<DateTime?>(DBWorkpieces.PurchaseDate),
                Geometry = row.Field<string>(DBWorkpieces.Geometry),
                isOneWay = row.Field<bool>(DBWorkpieces.isOneWay),
                KindOfProbe = row.Field<string>(DBWorkpieces.KindOfProbe),
                Status = row.Field<string>(DBWorkpieces.Status),
                isActive = row.Field<bool>(DBWorkpieces.isActive),
                Remark = row.Field<string>(DBWorkpieces.Remark)
            };

            //-------------------------------------------------------------------------------------------------------------------------------
            _wp.CurrentReferenceNumber = RefNumber;

            //_wp.Quality = new WorkpieceQuality()
            //{
            //    Corrosion = _dsWPQuality.Field<int?>(DBWorkpieceQuality.Corrosion),
            //    MoldScratches = _dsWPQuality.Field<int?>(DBWorkpieceQuality.MoldScratches),
            //    GlassAdherence = _dsWPQuality.Field<int?>(DBWorkpieceQuality.GlassAdherence),
            //    OverallResult = _dsWPQuality.Field<int?>(DBWorkpieceQuality.OverallResult),
            //    PID = _dsWPQuality.Field<int?>(DBWorkpieceQuality.PID)
            //};

            return _wp;
        }

        public Workpiece getWorkpieceByReference(int RefNumber, int? PID)
        {
             DataRow _dsWPQuality;
           
            if (PID!=null)
                _dsWPQuality = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieceQuality.Table + " where " + DBWorkpieceQuality.ReferenceNumber + "=" + RefNumber +" AND "+DBWorkpieceQuality.PID+"="+Convert.ToInt32(PID)).Tables[0].Rows[0];
           else
                _dsWPQuality = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieceQuality.Table + " where " + DBWorkpieceQuality.ReferenceNumber + "=" + RefNumber + " AND " + DBWorkpieceQuality.PID + " IS NULL").Tables[0].Rows[0];


            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + "=" + RefNumber);
           

            int wp_ID = _ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

            Workpiece _wp; //;= getWorkpiece(wp_ID);
            //---------------------------------------------------------------------------------------------------------------
            Material mm;
            DataRow row = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieces.Table +

                                                        //join materials
                                                        " LEFT JOIN " + DBMAterial.Table +
                                                        " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID + " WHERE " + DBWorkpieces.ID + "=" + wp_ID).Tables[0].Rows[0];


                mm = new Material() { ID = row.Field<int>(DBMAterial.ID), Name = row.Field<string>(DBMAterial.Name) };
                _wp = new Workpiece()
                {
                    ID = row.Field<int>(DBWorkpieces.ID),
                    Label = row.Field<string>(DBWorkpieces.Label),
                    Material = mm,
                    BatchNumber = row.Field<string>(DBWorkpieces.BatchNumber),
                    PurchaseDate = row.Field<DateTime?>(DBWorkpieces.PurchaseDate),
                    Geometry = row.Field<string>(DBWorkpieces.Geometry),
                    isOneWay = row.Field<bool>(DBWorkpieces.isOneWay),
                    KindOfProbe = row.Field<string>(DBWorkpieces.KindOfProbe),
                    Status = row.Field<string>(DBWorkpieces.Status),
                    isActive = row.Field<bool>(DBWorkpieces.isActive)
                };
            
            //-------------------------------------------------------------------------------------------------------------------------------

            _wp.CurrentReferenceNumber = RefNumber;

            _wp.Quality = new WorkpieceQuality()
            {
                Corrosion = _dsWPQuality.Field<int?>(DBWorkpieceQuality.Corrosion),
                MoldScratches = _dsWPQuality.Field<int?>(DBWorkpieceQuality.MoldScratches),
                GlassAdherence = _dsWPQuality.Field<int?>(DBWorkpieceQuality.GlassAdherence),
                OverallResult = _dsWPQuality.Field<int?>(DBWorkpieceQuality.OverallResult),
                PID = _dsWPQuality.Field<int?>(DBWorkpieceQuality.PID)
            };

            return _wp;
        }

        public Material getMaterial(int MatID)
        {
            return m_materials.Find(item => item.ID == MatID);
        }

        public void saveWorkpiece(Workpiece wp, bool update, int status = 0)
        {

            List<string> _queries = new List<string>();

            if (update)
            {
                List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.Label, Value = wp.Label });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.MaterialID, Value = wp.Material.ID });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.Geometry, Value = wp.Geometry });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.BatchNumber, Value = wp.BatchNumber });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.isOneWay, Value = wp.isOneWay });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.KindOfProbe, Value = wp.KindOfProbe });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.PurchaseDate, Value = wp.PurchaseDate });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.isActive, Value = wp.isActive });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.Remark, Value = wp.Remark });


                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieces.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.ID, Value = wp.ID }));
                _myCommunicator.executeTransactedQueries(_queries);
            
            }
            else
            {
                _queries.Add("INSERT INTO " + DBWorkpieces.Table + " (" + DBWorkpieces.Label + "," +
                                                                           DBWorkpieces.Initiator_ID + "," +
                                                                           DBWorkpieces.BatchNumber + "," +
                                                                           DBWorkpieces.Geometry + "," +
                                                                           DBWorkpieces.isOneWay + "," +
                                                                           DBWorkpieces.KindOfProbe + "," +
                                                                           DBWorkpieces.PurchaseDate + "," +
                                                                           DBWorkpieces.MaterialID + "," +
                                                                           DBWorkpieces.isActive + "," +
                                                                           DBWorkpieces.Remark + "," +
                                                                                     DBWorkpieces.Status + ") Values (" +
                                                                            wp.Label.ToDBObject() + "," +
                                                                            wp.InitiatorID.ToDBObject() + "," +
                                                                            wp.BatchNumber.ToDBObject() + "," +
                                                                            wp.Geometry.ToDBObject() + "," +
                                                                            wp.isOneWay.ToDBObject() + "," +
                                                                            wp.KindOfProbe.ToDBObject() + "," +
                                                                            wp.PurchaseDate.ToDBObject() + "," +
                                                                            wp.Material.ID.ToDBObject() + "," +
                                                                             wp.isActive.ToDBObject() + ","+ 
                                                                             wp.Remark.ToDBObject() + ",'raw')");


                int WPID = Convert.ToInt32(_myCommunicator.executeQuery(_queries[0]));

                if (WPID!=-1 && status!=0)
                {
                    wp.ID = WPID;
                    int reference = -1;
                    switch(status)
                    {
                        case 1:
                            reference = ProcessManager.Instance.skipInitialProcess(wp);
                            if (reference != -1)
                                FileManager.Instance.createReferenceDirectory(reference);
                            break;
                        case 2:
                            reference = ProcessManager.Instance.skipInitialProcess(wp);
                            if (reference != -1)
                            {
                                FileManager.Instance.createReferenceDirectory(reference);
                                ProcessManager.Instance.skipProcess(reference, DBEnum.EnumReference.COATED);
                            }
                            break;
                    }
                }

            }

            
        }

        //public void saveWorkpiece(Workpiece wp, bool update, int projectID, int IssueID)
        //{

        //    List<string> _queries = new List<string>();

            

            
        //    if (update)
        //    {
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Label + " = " + wp.Label.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.MaterialID + " = " + wp.Material.ID.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Geometry + " = " + wp.Geometry.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.BatchNumber + " = " + wp.BatchNumber.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.isOneWay + " = " + wp.isOneWay.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.KindOfProbe + " = " + wp.KindOfProbe.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.PurchaseDate + " = " + wp.PurchaseDate.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.isActive + " = " + wp.isActive.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
        //    }
        //    else
        //    {

        //        int _ref = ProcessManager.Instance.getNextRefNumber();

        //        _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID + "," + DBProcessReferences.IssueID + "," + DBProcessReferences.Status +
        //                        ") VALUES (" + _ref + ", " + wp.ID + ", " + projectID.ToDBObject() + ", " + IssueID.ToDBObject() + ", 'raw')");
        //        _queries.Add("INSERT INTO " + DBWorkpieceQuality.Table + " (" + DBProcessReferences.RefNumber + ") VALUES (" + _ref + ")");
        //        _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'INPROCESS' WHERE " + DBWorkpieces.ID + "=" + wp.ID);

        //        _queries.Add("INSERT INTO " + DBWorkpieces.Table + " (" + DBWorkpieces.Label + "," +
        //                                                                   DBWorkpieces.BatchNumber + "," +
        //                                                                   DBWorkpieces.Geometry + "," +
        //                                                                   DBWorkpieces.isOneWay + "," +
        //                                                                   DBWorkpieces.KindOfProbe + "," +
        //                                                                   DBWorkpieces.PurchaseDate + "," +
        //                                                                   DBWorkpieces.isActive + "," +
        //                                                                   DBWorkpieces.MaterialID + "," +
        //                                                                             DBWorkpieces.Status + ") Values (" +
        //                                                                    wp.Label.ToDBObject() + "," +
        //                                                                    wp.BatchNumber.ToDBObject() + "," +
        //                                                                    wp.Geometry.ToDBObject() + "," +
        //                                                                    wp.isOneWay.ToDBObject() + "," +
        //                                                                    wp.KindOfProbe.ToDBObject() + "," +
        //                                                                    wp.PurchaseDate.ToDBObject() + "," +
        //                                                                    wp.isActive.ToDBObject() + "," +
        //                                                                     wp.Material.ID.ToDBObject() + ",'raw')");




        //    }

        //    _myCommunicator.executeTransactedQueries(_queries);
        //}

        public void saveMaterial(Material material, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                _queries.Add("Update " + DBMAterial.Table + " Set " + DBMAterial.Name + " = " + material.Name.ToDBObject() + " WHERE " + DBMAterial.ID + "=" + material.ID);
            }
            else
            {
                _queries.Add("INSERT INTO " + DBMAterial.Table + " (" + DBMAterial.Name + ") Values (" + material.Name.ToDBObject() + ")");

            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public int? getProjectID(int RefNumber)
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + "=" + RefNumber);

            return _ds.Tables[0].Rows[0].Field<int?>(DBProcessReferences.ProjectID);
        }

        public int? getIssueID(int RefNumber)
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + "=" + RefNumber);

            return _ds.Tables[0].Rows[0].Field<int?>(DBProcessReferences.IssueID);
        }

        public void saveProject(Project project, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBProjects.Name, Value = project.Description });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBProjects.Remark, Value = project.Remark });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBProjects.Finished, Value = project.Finished });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBProjects.Started, Value = project.Started });

                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBProjects.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBProjects.ID, Value = project.ID }));

            }
            else
            {
                _queries.Add("INSERT INTO " + DBProjects.Table + " (" + DBProjects.Name + "," +
                                                                        DBProjects.UserID + "," +
                                                                           DBProjects.Remark + "," +
                                                                           DBProjects.Started + "," +
                                                                                     DBProjects.Finished + ") Values (" +
                                                                            project.Description.ToDBObject() + "," +
                                                                            project.User.ID.ToDBObject() + "," +
                                                                            project.Remark.ToDBObject() + "," +
                                                                            project.Started.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
                                                                             project.Finished.ToDBObject()+")");




            }

            bool success = _myCommunicator.executeTransactedQueries(_queries);
            //if (success && !update)
            //{
            //    FileManager.Instance.createProjectDirectory(project);
            //}
            //if (success && update)
            //{
            //    FileManager.Instance.updateProjectDirectory(project);
            //}
        }

        public void saveIssue(Issue issue, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {

                List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBIssues.Description, Value = issue.Description});
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBIssues.Remark, Value = issue.Remark });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBIssues.ProjectID, Value = issue.ProjectID });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBIssues.Conclusion, Value = issue.Conclusion });

                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBIssues.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBIssues.ID, Value = issue.ID }));
                
            }
            else
            {
                _queries.Add("INSERT INTO " + DBIssues.Table + " (" + DBIssues.Description + "," +
                                                                           DBIssues.Remark + "," +
                                                                                     DBIssues.ProjectID + ") Values (" +
                                                                            issue.Description.ToDBObject() + "," +
                                                                            issue.Remark.ToDBObject() + "," +
                                                                             issue.ProjectID.ToDBObject() + ")");




            }

            bool success = _myCommunicator.executeTransactedQueries(_queries);
            //if (success && !update)
            //{
            //    FileManager.Instance.createIssueDirectory(issue);
            //}
            //if (success && update)
            //{
            //    FileManager.Instance.updateIssueDirectoryName(issue);
            //}
        }

        public void saveUser(User user, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBUser.FirstName, Value = user.FirstName });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBUser.LastName, Value = user.LastName });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBUser.Token, Value = user.Token });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBUser.isActive, Value = user.isActive });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBUser.MachineID, Value = user.MachineID });


                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBUser.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBUser.ID, Value = user.ID }));

            }
            else
            {
                _queries.Add("INSERT INTO " + DBUser.Table + " (" + DBUser.FirstName + "," +
                                                                           DBUser.LastName + "," +
                                                                           DBUser.Token + "," +
                                                                           DBUser.MachineID + "," +
                                                                                     DBUser.isActive + ") Values (" +
                                                                            user.FirstName.ToDBObject() + "," +
                                                                            user.LastName.ToDBObject() + "," +
                                                                            user.Token.ToDBObject() + "," +
                                                                            user.MachineID.ToDBObject() + "," +
                                                                             user.isActive.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public void saveLayer(string name)
        {
             List<string> _queries = new List<string>();

             _queries.Add("INSERT INTO " + DBCoatingLayers.Table + " (" + DBCoatingLayers.Layer + ") Values (" + name.ToDBObject() + ")");
             _myCommunicator.executeTransactedQueries(_queries);
        }

        public void saveGlass(Glass glass, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                #region update
                List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();

                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Description, Value = glass.Name });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Company, Value = glass.Comapany });

                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Abrasion, Value = glass.Abrasion });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Acid_Resistance, Value = glass.Acid_Resistance_RA });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Acid_Resistance_SR, Value = glass.Acid_Resistance_SR });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Aluminium_fluoride, Value = glass.Aluminium_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Aluminiumoxid, Value = glass.Aluminiumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Annealing_Point, Value = glass.Annealing_Point });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Antimonoxid, Value = glass.Antimonoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Barium_fluoride, Value = glass.Barium_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Bariumoxid, Value = glass.Bariumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Boroxid, Value = glass.Boroxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Caesiumoxid, Value = glass.Caesiumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Calcium_fluoride, Value = glass.Calcium_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Calciumoxid, Value = glass.Calciumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Fluor, Value = glass.Fluor });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Germaniumoxid, Value = glass.Germaniumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Kaliumoxid, Value = glass.Kaliumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Knoop_Hardness, Value = glass.Knoop_Hardness });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Lanthanoxid, Value = glass.Lanthanoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Lanthanum_fluoride, Value = glass.Lanthanum_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Lithiumoxid, Value = glass.Lithiumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Magnecium_fluoride, Value = glass.Magnecium_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Molding_Temperature, Value = glass.Molding_Temperature });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Natriumoxid, Value = glass.Natriumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Niobpentoxid, Value = glass.Niobpentoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Phospahte_Resistance, Value = glass.Phospahte_Resistance });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Phosphorus_oxide, Value = glass.Phosphorus_oxide });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Photoelastic_Constant, Value = glass.Photoelastic_Constant });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Siliziumdioxid, Value = glass.Siliziumdioxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Softening_Point, Value = glass.Softening_Point });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Specific_Gravity, Value = glass.Specific_Gravity });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Strain_Point, Value = glass.Strain_Point });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Strontium_fluoride, Value = glass.Strontium_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Strontiumoxid, Value = glass.Strontiumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Tantaloxid, Value = glass.Tantaloxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Tellurium, Value = glass.Tellurium });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Titanoxid, Value = glass.Titanoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Transformation_Temperature, Value = glass.Transformation_Temperature });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Tungsten_Oxid, Value = glass.Tungsten_Oxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Wärmeausdehnungskoeffizient, Value = glass.Wärmeausdehnungskoeffizient });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Wärmeleitfähigkeit, Value = glass.Wärmeleitfähigkeit });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Water_Resistance, Value = glass.Water_Resistance });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Weathering_Resistance, Value = glass.Weathering_Resistance });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Wismutoxid, Value = glass.Wismutoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Wolframtrioxid, Value = glass.Wolframtrioxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Yield_Point, Value = glass.Yield_Point });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Yttrium_fluoride, Value = glass.Yttrium_fluoride });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Yttriumoxid, Value = glass.Yttriumoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Zinkoxid, Value = glass.Zinkoxid });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.Zirkonoxid, Value = glass.Zirkonoxid });

                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBGlasses.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBGlasses.ID, Value = glass.ID }));
                #endregion
            }
            else
            {
                #region save
                _queries.Add("INSERT INTO " + DBGlasses.Table + " (" + DBGlasses.Description + "," +
                                                                       DBGlasses.Company + "," +
                                                                       DBGlasses.Abrasion + "," +
                                                                       DBGlasses.Acid_Resistance + "," +
                                                                       DBGlasses.Acid_Resistance_SR + "," +
                                                                       DBGlasses.Aluminium_fluoride + "," +
                                                                       DBGlasses.Aluminiumoxid + "," +
                                                                       DBGlasses.Annealing_Point + "," +
                                                                       DBGlasses.Antimonoxid + "," +
                                                                       DBGlasses.Barium_fluoride + "," +
                                                                       DBGlasses.Bariumoxid + "," +
                                                                       DBGlasses.Boroxid + "," +
                                                                       DBGlasses.Caesiumoxid + "," +
                                                                       DBGlasses.Calcium_fluoride + "," +
                                                                       DBGlasses.Calciumoxid + "," +
                                                                       DBGlasses.Fluor + "," +
                                                                       DBGlasses.Germaniumoxid + "," +
                                                                       DBGlasses.Kaliumoxid + "," +
                                                                       DBGlasses.Knoop_Hardness + "," +
                                                                       DBGlasses.Lanthanoxid + "," +
                                                                       DBGlasses.Lanthanum_fluoride + "," +
                                                                       DBGlasses.Lithiumoxid + "," +
                                                                       DBGlasses.Magnecium_fluoride + "," +
                                                                       DBGlasses.Molding_Temperature + "," +
                                                                       DBGlasses.Natriumoxid + "," +
                                                                       DBGlasses.Niobpentoxid + "," +
                                                                       DBGlasses.Phospahte_Resistance + "," +
                                                                       DBGlasses.Phosphorus_oxide + "," +
                                                                       DBGlasses.Photoelastic_Constant + "," +
                                                                       DBGlasses.Siliziumdioxid + "," +
                                                                       DBGlasses.Softening_Point + "," +
                                                                       DBGlasses.Specific_Gravity + "," +
                                                                       DBGlasses.Strain_Point + "," +
                                                                       DBGlasses.Strontium_fluoride + "," +
                                                                       DBGlasses.Strontiumoxid + "," +
                                                                       DBGlasses.Tantaloxid + "," +
                                                                       DBGlasses.Tellurium + "," +
                                                                       DBGlasses.Titanoxid + "," +
                                                                       DBGlasses.Transformation_Temperature + "," +
                                                                       DBGlasses.Tungsten_Oxid + "," +
                                                                       DBGlasses.Wärmeausdehnungskoeffizient + "," +
                                                                       DBGlasses.Wärmeleitfähigkeit + "," +
                                                                       DBGlasses.Water_Resistance + "," +
                                                                       DBGlasses.Weathering_Resistance + "," +
                                                                       DBGlasses.Wismutoxid + "," +
                                                                       DBGlasses.Wolframtrioxid + "," +
                                                                       DBGlasses.Yield_Point + "," +
                                                                       DBGlasses.Yttrium_fluoride + "," +
                                                                       DBGlasses.Yttriumoxid + "," +
                                                                       DBGlasses.Zinkoxid + "," +
                                                                       DBGlasses.Zirkonoxid+ ") Values (" +
                                                                            glass.Name.ToDBObject() + "," +
                                                                            glass.Comapany.ToDBObject() + "," +
                                                                            glass.Abrasion.ToDBObject() + "," +
                                                                            glass.Acid_Resistance_RA.ToDBObject() + "," +
                                                                            glass.Acid_Resistance_SR.ToDBObject() + "," +
                                                                            glass.Aluminium_fluoride.ToDBObject() + "," +
                                                                            glass.Aluminiumoxid.ToDBObject() + "," +
                                                                            glass.Annealing_Point.ToDBObject() + "," +
                                                                            glass.Antimonoxid.ToDBObject() + "," +
                                                                            glass.Barium_fluoride.ToDBObject() + "," +
                                                                            glass.Bariumoxid.ToDBObject() + "," +
                                                                            glass.Boroxid.ToDBObject() + "," +
                                                                            glass.Caesiumoxid.ToDBObject() + "," +
                                                                            glass.Calcium_fluoride.ToDBObject() + "," +
                                                                            glass.Calciumoxid.ToDBObject() + "," +
                                                                            glass.Fluor.ToDBObject() + "," +
                                                                            glass.Germaniumoxid.ToDBObject() + "," +
                                                                            glass.Kaliumoxid.ToDBObject() + "," +
                                                                            glass.Knoop_Hardness.ToDBObject() + "," +
                                                                            glass.Lanthanoxid.ToDBObject() + "," +
                                                                            glass.Lanthanum_fluoride.ToDBObject() + "," +
                                                                            glass.Lithiumoxid.ToDBObject() + "," +
                                                                            glass.Magnecium_fluoride.ToDBObject() + "," +
                                                                            glass.Molding_Temperature.ToDBObject() + "," +
                                                                            glass.Natriumoxid.ToDBObject() + "," +
                                                                            glass.Niobpentoxid.ToDBObject() + "," +
                                                                            glass.Phospahte_Resistance.ToDBObject() + "," +
                                                                            glass.Phosphorus_oxide.ToDBObject() + "," +
                                                                            glass.Photoelastic_Constant.ToDBObject() + "," +
                                                                            glass.Siliziumdioxid.ToDBObject() + "," +
                                                                            glass.Softening_Point.ToDBObject() + "," +
                                                                            glass.Specific_Gravity.ToDBObject() + "," +
                                                                            glass.Strain_Point.ToDBObject() + "," +
                                                                            glass.Strontium_fluoride.ToDBObject() + "," +
                                                                            glass.Strontiumoxid.ToDBObject() + "," +
                                                                            glass.Tantaloxid.ToDBObject() + "," +
                                                                            glass.Tellurium.ToDBObject() + "," +
                                                                            glass.Titanoxid.ToDBObject() + "," +
                                                                            glass.Transformation_Temperature.ToDBObject() + "," +
                                                                            glass.Tungsten_Oxid.ToDBObject() + "," +
                                                                            glass.Wärmeausdehnungskoeffizient.ToDBObject() + "," +
                                                                            glass.Wärmeleitfähigkeit.ToDBObject() + "," +
                                                                            glass.Water_Resistance.ToDBObject() + "," +
                                                                            glass.Weathering_Resistance.ToDBObject() + "," +
                                                                            glass.Wismutoxid.ToDBObject() + "," +
                                                                            glass.Wolframtrioxid.ToDBObject() + "," +
                                                                            glass.Yield_Point.ToDBObject() + "," +
                                                                            glass.Yttrium_fluoride.ToDBObject() + "," +
                                                                            glass.Yttriumoxid.ToDBObject() + "," +
                                                                            glass.Zinkoxid.ToDBObject() + "," +
                                                                            glass.Zirkonoxid.ToDBObject() +")");


                #endregion

            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public void savePheonixProcess(PGrindingPhoenixProcess process, bool update)
        {
            List<string> _queries = new List<string>();
            if (update)
            {
                List<MySQLCommunicator.ColumnValuePair> values = new List<MySQLCommunicator.ColumnValuePair>();
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBPhoenixProcesses.Description, Value = process.Description });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBPhoenixProcesses.Remark, Value = process.Remark });
                values.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBPhoenixProcesses.Ra, Value = process.Ra });


                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBPhoenixProcesses.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBPhoenixProcesses.ID, Value = process.ID }));
            }
            else
            {
                _queries.Add("INSERT INTO " + DBPhoenixProcesses.Table + " (" + DBPhoenixProcesses.ID + "," +
                                                                               DBPhoenixProcesses.Description + "," +
                                                                               DBPhoenixProcesses.Ra + "," +
                                                                               DBPhoenixProcesses.Remark + ") Values (" +
                                                                                process.ID.ToDBObject() + "," +
                                                                                process.Description.ToDBObject() + "," +
                                                                                process.Ra.ToDBObject() + "," +
                                                                                 process.Remark.ToDBObject() + ")");
            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public Glass getGlass(int ID)

        {
            DataRow dr = _myCommunicator.getDataSet("SELECT * FROM " + DBGlasses.Table + " WHERE " + DBGlasses.ID+"="+ID).Tables[0].Rows[0];

            Glass m_glass = new Glass()
            {
                ID = dr.Field<int>(DBGlasses.ID),
                Name = dr.Field<string>(DBGlasses.Description),
                Comapany = dr.Field<string>(DBGlasses.Company),

                Abrasion = dr.Field<double?>(DBGlasses.Abrasion),
                Acid_Resistance_RA = dr.Field<double?>(DBGlasses.Acid_Resistance),
                Acid_Resistance_SR = dr.Field<double?>(DBGlasses.Acid_Resistance_SR),
                Aluminium_fluoride = dr.Field<double?>(DBGlasses.Aluminium_fluoride),
                Aluminiumoxid = dr.Field<double?>(DBGlasses.Aluminiumoxid),
                Annealing_Point = dr.Field<double?>(DBGlasses.Annealing_Point),
                Antimonoxid = dr.Field<double?>(DBGlasses.Antimonoxid),
                Barium_fluoride = dr.Field<double?>(DBGlasses.Barium_fluoride),
                Bariumoxid = dr.Field<double?>(DBGlasses.Bariumoxid),
                Boroxid = dr.Field<double?>(DBGlasses.Boroxid),
                Caesiumoxid = dr.Field<double?>(DBGlasses.Caesiumoxid),
                Calcium_fluoride = dr.Field<double?>(DBGlasses.Calcium_fluoride),
                Calciumoxid = dr.Field<double?>(DBGlasses.Calciumoxid),
                Fluor = dr.Field<double?>(DBGlasses.Fluor),
                Germaniumoxid = dr.Field<double?>(DBGlasses.Germaniumoxid),
                Kaliumoxid = dr.Field<double?>(DBGlasses.Kaliumoxid),
                Knoop_Hardness = dr.Field<double?>(DBGlasses.Knoop_Hardness),
                Lanthanoxid = dr.Field<double?>(DBGlasses.Lanthanoxid),
                Lanthanum_fluoride = dr.Field<double?>(DBGlasses.Lanthanum_fluoride),
                Lithiumoxid = dr.Field<double?>(DBGlasses.Lithiumoxid),
                Magnecium_fluoride = dr.Field<double?>(DBGlasses.Magnecium_fluoride),
                Molding_Temperature = dr.Field<double?>(DBGlasses.Molding_Temperature),
                Natriumoxid = dr.Field<double?>(DBGlasses.Natriumoxid),
                Niobpentoxid = dr.Field<double?>(DBGlasses.Niobpentoxid),
                Phospahte_Resistance = dr.Field<double?>(DBGlasses.Phospahte_Resistance),
                Phosphorus_oxide = dr.Field<double?>(DBGlasses.Phosphorus_oxide),
                Photoelastic_Constant = dr.Field<double?>(DBGlasses.Photoelastic_Constant),
                Siliziumdioxid = dr.Field<double?>(DBGlasses.Siliziumdioxid),
                Softening_Point = dr.Field<double?>(DBGlasses.Softening_Point),
                Specific_Gravity = dr.Field<double?>(DBGlasses.Specific_Gravity),
                Strain_Point = dr.Field<double?>(DBGlasses.Strain_Point),
                Strontium_fluoride = dr.Field<double?>(DBGlasses.Strontium_fluoride),
                Strontiumoxid = dr.Field<double?>(DBGlasses.Strontiumoxid),
                Tantaloxid = dr.Field<double?>(DBGlasses.Tantaloxid),
                Tellurium = dr.Field<double?>(DBGlasses.Tellurium),
                Titanoxid = dr.Field<double?>(DBGlasses.Titanoxid),
                Transformation_Temperature = dr.Field<double?>(DBGlasses.Transformation_Temperature),
                Tungsten_Oxid = dr.Field<double?>(DBGlasses.Tungsten_Oxid),
                Wärmeausdehnungskoeffizient = dr.Field<double?>(DBGlasses.Wärmeausdehnungskoeffizient),
                Wärmeleitfähigkeit = dr.Field<double?>(DBGlasses.Wärmeleitfähigkeit),
                Water_Resistance = dr.Field<double?>(DBGlasses.Water_Resistance),
                Weathering_Resistance = dr.Field<double?>(DBGlasses.Weathering_Resistance),
                Wismutoxid = dr.Field<double?>(DBGlasses.Wismutoxid),
                Wolframtrioxid = dr.Field<double?>(DBGlasses.Wolframtrioxid),
                Yield_Point = dr.Field<double?>(DBGlasses.Yield_Point),
                Yttrium_fluoride = dr.Field<double?>(DBGlasses.Yttrium_fluoride),
                Yttriumoxid = dr.Field<double?>(DBGlasses.Yttriumoxid),
                Zinkoxid = dr.Field<double?>(DBGlasses.Zinkoxid),
                Zirkonoxid = dr.Field<double?>(DBGlasses.Zirkonoxid)
            };

            return m_glass;

        }

        public User getUser(int UID)
        {
            DataRow _dr = _myCommunicator.getDataSet("SELECT * FROM " + DBUser.Table + " WHERE " + DBUser.ID + "=" + UID).Tables[0].Rows[0];

            return new User()
            {
                ID = _dr.Field<int>(DBUser.ID),
                FirstName = _dr.Field<string>(DBUser.FirstName),
                LastName = _dr.Field<string>(DBUser.LastName),
                Token = _dr.Field<string>(DBUser.Token),
                isActive = _dr.Field<bool>(DBUser.isActive),
            };

        }

        private void getCoatedWorkpieces()
        {
            m_coatedWorkpieces.Clear();
            DataSet _ds1 = _myCommunicator.getDataSet(Queries.QueryCoatedReferences);

            foreach (DataRow dr in _ds1.Tables[0].Rows)
            {
                int RefNumber = dr.Field<int>(DBProcessReferences.RefNumber);

                DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + "=" + RefNumber);
                DataRow _dsWPQuality = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieceQuality.Table + " where " + DBWorkpieceQuality.ReferenceNumber + "=" + RefNumber +" AND "+DBWorkpieceQuality.PID+" is NULL").Tables[0].Rows[0];

                int wp_ID = _ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

                Workpiece _wp = getWorkpiece(wp_ID);
                _wp.CurrentReferenceNumber = RefNumber;

                _wp.Reference.Project = Projects.Find(item => item.ID == dr.Field<int?>(DBProcessReferences.ProjectID)) ?? new Project();
                _wp.Reference.Issue = Issues.Find(item => item.ID == dr.Field<int?>(DBProcessReferences.IssueID)) ?? new Issue();

                _wp.Quality = new WorkpieceQuality()
                {
                    Corrosion = _dsWPQuality.Field<int?>(DBWorkpieceQuality.Corrosion),
                    MoldScratches = _dsWPQuality.Field<int?>(DBWorkpieceQuality.MoldScratches),
                    GlassAdherence = _dsWPQuality.Field<int?>(DBWorkpieceQuality.GlassAdherence),
                    OverallResult = _dsWPQuality.Field<int?>(DBWorkpieceQuality.OverallResult),
                    PID = _dsWPQuality.Field<int?>(DBWorkpieceQuality.PID)
                };

                m_coatedWorkpieces.Add(_wp);
            }

        }

        public List<Workpiece> CoatedWorkpieces
        { get { return m_coatedWorkpieces; } }

        public void ReleaseWorkpiece(Workpiece wp, bool cancelled)
        {
            List<string> _queries = new List<string>();

            _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'raw' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
            if (cancelled)
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'cancelled' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);
            else
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'terminated' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentReferenceNumber);

            if (wp.isOneWay)
            {
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.isActive + " = 0 WHERE " + DBWorkpieces.ID + "=" + wp.ID);
            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public void Remove(BusinessObject obj)
        {
            string _query = "";
            if (obj is BusinessObjects.Material)
            {
                _query = "DELETE FROM " + DBMAterial.Table + " WHERE " + DBMAterial.ID + "=" + obj.ID;
            }

            _myCommunicator.executeTransactedQueries(new List<string>() { _query });

        }


    }
}
