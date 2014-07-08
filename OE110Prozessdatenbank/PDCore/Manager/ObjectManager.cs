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
            _myCommunicator.Password = PDCore.Properties.Settings.Default.Password;
            _myCommunicator.Server = PDCore.Properties.Settings.Default.Server;
            _myCommunicator.User = PDCore.Properties.Settings.Default.User;
            _myCommunicator.Database =PDCore.Properties.Settings.Default.Database;

            //Recieve database-errors
            _myCommunicator.MessageThrown += _myCommunicator_MessageThrown;

            Updater.Instance.newData += Instance_newData;

            if (_myCommunicator.checkConnection())
            { 
            }

        }

        void Instance_newData()
        {
            update();
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

        public void updateProjects()
        { getProjects(); getIssues(); }

        public void updateUser()
        { getUser(); }

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
                m_PhoenixProcesses.Add(new PGrindingPhoenixProcess() { ID = row.Field<int>(DBPhoenixProcesses.ID), Description = row.Field<string>(DBPhoenixProcesses.Description) });
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

                                                        //join materials
                                                        " LEFT JOIN " + DBMAterial.Table +
                                                        " On " + DBMAterial.Table + "." + DBMAterial.ID +
                                                        "=" + DBWorkpieces.Table + "." + DBWorkpieces.MaterialID);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                mm = new Material() { ID = row.Field<int>(DBMAterial.ID), Name = row.Field<string>(DBMAterial.Name) };
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
                    Status = row.Field<string>(DBWorkpieces.Status),
                    isActive = row.Field<bool>(DBWorkpieces.isActive)
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
        { get { return m_users; } }

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

            Workpiece _wp = getWorkpiece(wp_ID);
            _wp.CurrentRefereneNumber = RefNumber;

            _wp.Quality = new WorkpieceQuality()
            {
                Corrosion = _dsWPQuality.Field<int?>(DBWorkpieceQuality.Corrosion),
                MoldScratches = _dsWPQuality.Field<int?>(DBWorkpieceQuality.MoldScratches),
                GlassAdherence = _dsWPQuality.Field<int?>(DBWorkpieceQuality.GlassAdherence),
                OverallResult = _dsWPQuality.Field<int?>(DBWorkpieceQuality.OverallResult)
            };

            return _wp;
        }

        public Workpiece getWorkpieceByReference(int RefNumber, int PID)
        {

            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " where " + DBProcessReferences.RefNumber + "=" + RefNumber);
            DataRow _dsWPQuality = _myCommunicator.getDataSet("SELECT * FROM " + DBWorkpieceQuality.Table + " where " + DBWorkpieceQuality.ReferenceNumber + "=" + RefNumber +" AND "+DBWorkpieceQuality.PID+"="+PID).Tables[0].Rows[0];

            int wp_ID = _ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

            Workpiece _wp = getWorkpiece(wp_ID);
            _wp.CurrentRefereneNumber = RefNumber;

            _wp.Quality = new WorkpieceQuality()
            {
                Corrosion = _dsWPQuality.Field<int>(DBWorkpieceQuality.Corrosion),
                MoldScratches = _dsWPQuality.Field<int>(DBWorkpieceQuality.MoldScratches),
                GlassAdherence = _dsWPQuality.Field<int>(DBWorkpieceQuality.GlassAdherence),
                OverallResult = _dsWPQuality.Field<int>(DBWorkpieceQuality.OverallResult)
            };

            return _wp;
        }

        public Material getMaterial(int MatID)
        {
            return m_materials.Find(item => item.ID == MatID);
        }

        public void saveWorkpiece(Workpiece wp, bool update)
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


                _queries.Add(MySQLCommunicator.BuildUpdateQuery(DBWorkpieces.Table, values, new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.ID, Value = wp.ID }));
            }
            else
            {
                _queries.Add("INSERT INTO " + DBWorkpieces.Table + " (" + DBWorkpieces.Label + "," +
                                                                           DBWorkpieces.BatchNumber + "," +
                                                                           DBWorkpieces.Geometry + "," +
                                                                           DBWorkpieces.isOneWay + "," +
                                                                           DBWorkpieces.KindOfProbe + "," +
                                                                           DBWorkpieces.PurchaseDate + "," +
                                                                           DBWorkpieces.MaterialID + "," +
                                                                           DBWorkpieces.isActive + "," +
                                                                                     DBWorkpieces.Status + ") Values (" +
                                                                            wp.Label.ToDBObject() + "," +
                                                                            wp.BatchNumber.ToDBObject() + "," +
                                                                            wp.Geometry.ToDBObject() + "," +
                                                                            wp.isOneWay.ToDBObject() + "," +
                                                                            wp.KindOfProbe.ToDBObject() + "," +
                                                                            wp.PurchaseDate.ToDBObject() + "," +
                                                                            wp.Material.ID.ToDBObject() + "," +
                                                                             wp.isActive.ToDBObject() + ",'raw')");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

        public void saveWorkpiece(Workpiece wp, bool update, int projectID, int IssueID)
        {

            List<string> _queries = new List<string>();

            

            
            if (update)
            {
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Label + " = " + wp.Label.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.MaterialID + " = " + wp.Material.ID.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Geometry + " = " + wp.Geometry.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.BatchNumber + " = " + wp.BatchNumber.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.isOneWay + " = " + wp.isOneWay.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.KindOfProbe + " = " + wp.KindOfProbe.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.PurchaseDate + " = " + wp.PurchaseDate.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.isActive + " = " + wp.isActive.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
            }
            else
            {

                int _ref = ProcessManager.Instance.getNextRefNumber();

                _queries.Add("INSERT INTO " + DBProcessReferences.Table + " (" + DBProcessReferences.RefNumber + "," + DBProcessReferences.WorkpiceID + "," + DBProcessReferences.ProjectID + "," + DBProcessReferences.IssueID + "," + DBProcessReferences.Status +
                                ") VALUES (" + _ref + ", " + wp.ID + ", " + projectID.ToDBObject() + ", " + IssueID.ToDBObject() + ", 'raw')");
                _queries.Add("INSERT INTO " + DBWorkpieceQuality.Table + " (" + DBProcessReferences.RefNumber + ") VALUES (" + _ref + ")");
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'INPROCESS' WHERE " + DBWorkpieces.ID + "=" + wp.ID);

                _queries.Add("INSERT INTO " + DBWorkpieces.Table + " (" + DBWorkpieces.Label + "," +
                                                                           DBWorkpieces.BatchNumber + "," +
                                                                           DBWorkpieces.Geometry + "," +
                                                                           DBWorkpieces.isOneWay + "," +
                                                                           DBWorkpieces.KindOfProbe + "," +
                                                                           DBWorkpieces.PurchaseDate + "," +
                                                                           DBWorkpieces.isActive + "," +
                                                                           DBWorkpieces.MaterialID + "," +
                                                                                     DBWorkpieces.Status + ") Values (" +
                                                                            wp.Label.ToDBObject() + "," +
                                                                            wp.BatchNumber.ToDBObject() + "," +
                                                                            wp.Geometry.ToDBObject() + "," +
                                                                            wp.isOneWay.ToDBObject() + "," +
                                                                            wp.KindOfProbe.ToDBObject() + "," +
                                                                            wp.PurchaseDate.ToDBObject() + "," +
                                                                            wp.isActive.ToDBObject() + "," +
                                                                             wp.Material.ID.ToDBObject() + ",'raw')");




            }

            _myCommunicator.executeTransactedQueries(_queries);
        }

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
                _queries.Add("Update " + DBProjects.Table + " Set " + DBProjects.Name + " = " + project.Description.ToDBObject() + " WHERE " + DBProjects.ID + "=" + project.ID);
                _queries.Add("Update " + DBProjects.Table + " Set " + DBProjects.Remark + " = " + project.Remark.ToDBObject() + " WHERE " + DBProjects.ID + "=" + project.ID);
                _queries.Add("Update " + DBProjects.Table + " Set " + DBProjects.Finished + " = " + project.Finished.ToDBObject() + " WHERE " + DBProjects.ID + "=" + project.ID);
                _queries.Add("Update " + DBProjects.Table + " Set " + DBProjects.Started + " = " + project.Started.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBProjects.ID + "=" + project.ID);
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
            if (success && !update)
            {
                FileManager.Instance.createProjectDirectory(project);
            }
            if (success && update)
            {
                FileManager.Instance.updateProjectDirectory(project);
            }
        }

        public void saveIssue(Issue issue, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                _queries.Add("Update " + DBIssues.Table + " Set " + DBIssues.Description + " = " + issue.Description.ToDBObject() + " WHERE " + DBIssues.ID + "=" + issue.ID);
                _queries.Add("Update " + DBIssues.Table + " Set " + DBIssues.Remark + " = " + issue.Remark.ToDBObject() + " WHERE " + DBIssues.ID + "=" + issue.ID);
                _queries.Add("Update " + DBIssues.Table + " Set " + DBIssues.ProjectID + " = " + issue.ProjectID.ToDBObject() + " WHERE " + DBIssues.ID + "=" + issue.ID);
                
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
            if (success && !update)
            {
                FileManager.Instance.createIssueDirectory(issue);
            }
            if (success && update)
            {
                FileManager.Instance.updateIssueDirectoryName(issue);
            }
        }

        public void saveUser(User user, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                _queries.Add("Update " + DBUser.Table + " Set " + DBUser.FirstName + " = " + user.FirstName.ToDBObject() + " WHERE " + DBUser.ID + "=" + user.ID);
                _queries.Add("Update " + DBUser.Table + " Set " + DBUser.LastName + " = " + user.LastName.ToDBObject() + " WHERE " + DBUser.ID + "=" + user.ID);
                _queries.Add("Update " + DBUser.Table + " Set " + DBUser.Token + " = " + user.Token.ToDBObject() + " WHERE " + DBUser.ID + "=" + user.ID);
                _queries.Add("Update " + DBUser.Table + " Set " + DBUser.isActive + " = " + user.isActive.ToDBObject() + " WHERE " + DBUser.ID + "=" + user.ID);
                _queries.Add("Update " + DBUser.Table + " Set " + DBUser.MachineID + " = " + user.MachineID.ToDBObject() + " WHERE " + DBUser.ID + "=" + user.ID);

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

        public void saveGlass(Glass glass, bool update)
        {
            List<string> _queries = new List<string>();

            if (update)
            {
                _queries.Add("Update " + DBGlasses.Table + " Set " + DBGlasses.Description + " = " + glass.Name.ToDBObject() + " WHERE " + DBGlasses.ID + "=" + glass.ID);
                _queries.Add("Update " + DBGlasses.Table + " Set " + DBGlasses.Company + " = " + glass.Comapany.ToDBObject() + " WHERE " + DBGlasses.ID + "=" + glass.ID);

            }
            else
            {
                _queries.Add("INSERT INTO " + DBGlasses.Table + " (" + DBGlasses.Description + "," +
                                                                                     DBGlasses.Company + ") Values (" +
                                                                            glass.Name.ToDBObject() + "," +
                                                                             glass.Comapany.ToDBObject() + ")");




            }

            _myCommunicator.executeTransactedQueries(_queries);
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
            DataSet _ds = _myCommunicator.getDataSet(Queries.QueryCoatedReferences);

            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                m_coatedWorkpieces.Add(getWorkpieceByReference(dr.Field<int>(DBProcessReferences.RefNumber)));
            }
        }

        public List<Workpiece> CoatedWorkpieces
        { get { return m_coatedWorkpieces; } }

        public void ReleaseWorkpiece(Workpiece wp, bool cancelled)
        {
            List<string> _queries = new List<string>();

            _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Status + " = 'raw' WHERE " + DBWorkpieces.ID + "=" + wp.ID);
            if (cancelled)
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'cancelled' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);
            else
                _queries.Add("Update " + DBProcessReferences.Table + " Set " + DBProcessReferences.Status + " = 'terminated' WHERE " + DBProcessReferences.RefNumber + "=" + wp.CurrentRefereneNumber);

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
