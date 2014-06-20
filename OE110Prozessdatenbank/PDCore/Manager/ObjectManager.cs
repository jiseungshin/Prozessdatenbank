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

            if (_myCommunicator.checkConnection())
            { 
            }

        }

         void _myCommunicator_MessageThrown(Communicator.MessageType mType, Exception Message)
        {
            System.Windows.MessageBox.Show(Message.Message);
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
         private List<Material> m_materials = new List<Material>();

        public void update()
        {
            getProjects();
            getUser();
            getMachines();
            getPhoenixProcesses();
            getMaterials();
            getWorkpieces();
            getIssues();
        }

        private void getProjects()
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " ORDER BY " + DBProjects.Name);
            m_projects.Clear();
            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_projects.Add(new Project() { ID = row.Field<int>(DBProjects.ID), Description = row.Field<string>(DBProjects.Name) });
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
                m_users.Add(new User() { ID = row.Field<int>(DBUser.ID), FirstName = row.Field<string>(DBUser.FirstName), LastName = row.Field<string>(DBUser.LastName), Token = row.Field<string>(DBUser.Token) });
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
                    //PurchaseDate = row.Field<DateTime>(DBWorkpieces.PurchaseDate),
                    Geometry = row.Field<string>(DBWorkpieces.Geometry),
                    isOneWay = row.Field<bool>(DBWorkpieces.isOneWay),
                    KindOfProbe = row.Field<string>(DBWorkpieces.KindOfProbe)
                });
            }
        }



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

        public Workpiece getWorkpieceByProcessID(int PID)
        {
            //DataSet _ds = _myCommunicator.getDataSet("SELECT * from " + DBProcessReferenceRelation.Table +
            //                                                " where " + DBProcessReferences.RefNumber + "=" + RefNumber);

            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferenceRelation.Table +

                                                        //join References
                                                        " LEFT JOIN " + DBProcessReferences.Table +
                                                        " On " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber +
                                                        "=" + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.RefNumber+
                                                        " where " + DBProcessReferenceRelation.Table + "." + DBProcessReferenceRelation.PID + "=" + PID);

            int wp_ID = _ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

            return getWorkpiece(wp_ID);
        }

        public Workpiece getWorkpieceByReference(int RefNumber)
        {

            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table +" where " + DBProcessReferences.RefNumber + "=" + RefNumber);

            int wp_ID = _ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.WorkpiceID);

            return getWorkpiece(wp_ID);
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
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Label + " = " + wp.Label.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.MaterialID + " = " + wp.Material.ID.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.Geometry + " = " + wp.Geometry.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.BatchNumber + " = " + wp.BatchNumber.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.isOneWay + " = " + wp.isOneWay.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.KindOfProbe + " = " + wp.KindOfProbe.ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
                _queries.Add("Update " + DBWorkpieces.Table + " Set " + DBWorkpieces.PurchaseDate + " = " + wp.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + " WHERE " + DBWorkpieces.ID + "=" + wp.ID);
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
                                                                                     DBWorkpieces.Status + ") Values (" +
                                                                            wp.Label.ToDBObject() + "," +
                                                                            wp.BatchNumber.ToDBObject() + "," +
                                                                            wp.Geometry.ToDBObject() + "," +
                                                                            wp.isOneWay.ToDBObject() + "," +
                                                                            wp.KindOfProbe.ToDBObject() + "," +
                                                                            wp.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss").ToDBObject() + "," +
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


    }
}
