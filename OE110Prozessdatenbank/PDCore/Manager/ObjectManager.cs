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


        private ObservableCollection<Project> m_projects = new ObservableCollection<Project>();
        private ObservableCollection<User> m_users = new ObservableCollection<User>();

        public ObservableCollection<Project> getProjects()
        {
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " ORDER BY " + DBProjects.Name);
            m_projects.Clear();
            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_projects.Add(new Project() { ID = row.Field<int>(DBProjects.ID), Description = row.Field<string>(DBProjects.Name) });
            }

            return m_projects;
        }

        public ObservableCollection<User> getUser()
        {
            m_users.Clear();
            DataSet _ds = _myCommunicator.getDataSet("SELECT * FROM " + DBUser.Table + " ORDER BY " + DBUser.LastName);

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                m_users.Add(new User() { ID = row.Field<int>(DBUser.ID), FirstName = row.Field<string>(DBUser.FirstName), LastName = row.Field<string>(DBUser.LastName), Token = row.Field<string>(DBUser.Token) });
            }

            return m_users;
        }

    }
}
