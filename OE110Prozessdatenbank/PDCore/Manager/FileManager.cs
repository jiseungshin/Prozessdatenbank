using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Database;
using PDCore.BusinessObjects;
using System.IO;
using System.Data;

namespace PDCore.Manager
{
    public class FileManager
    {
        private MySQLCommunicator _myCommunicator;

        #region instancing

        static FileManager instance = null;
        static readonly object padlock = new object();

        FileManager()
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
            //update();
        }

         void _myCommunicator_MessageThrown(Communicator.MessageType mType, Exception Message)
        {
             //1062 duplicate number
            System.Windows.MessageBox.Show(Message.Message);
        }

         public static FileManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FileManager();
                    }
                    return instance;
                }
            }
        }

        #endregion

        public void createProjectDirectory(Project project)
        {
            Directory.CreateDirectory(Properties.Settings.Default.StandardDirPath + project.Description);
        }
        public void createIssueDirectory(Issue Issue)
        {
            string Projectname = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + Issue.ProjectID).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
            Directory.CreateDirectory(Properties.Settings.Default.StandardDirPath + Projectname + "\\" + Issue.Description);
        }
        public void createReferenceDirectory(int ReferenceNumber)
        {
            DataTable _reference = (_myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber).Tables[0]);

            string ProjectName = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.ProjectID)).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
            string IssueName = (_myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.IssueID)).Tables[0].Rows[0]).Field<string>(DBIssues.Description);

            Directory.CreateDirectory(Properties.Settings.Default.StandardDirPath + ProjectName + "\\" + IssueName + "\\" + ReferenceNumber);
        }

        public void createDirectory(int ReferenceNumber, string Name)
        {
            DataTable _reference = (_myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber).Tables[0]);

            string ProjectName = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.ProjectID)).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
            string IssueName = (_myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.IssueID)).Tables[0].Rows[0]).Field<string>(DBIssues.Description);

            Directory.CreateDirectory(Properties.Settings.Default.StandardDirPath + ProjectName + "\\" + IssueName + "\\" + ReferenceNumber + "\\" + Name);
        }

        public void createProcessDirectory(int ProjectID, int IssueID, int PID, string Name, bool hidden)
        {

            string ProjectName = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + ProjectID).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
            string IssueName = (_myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ID + "=" + IssueID).Tables[0].Rows[0]).Field<string>(DBIssues.Description);

            DirectoryInfo di = Directory.CreateDirectory(Properties.Settings.Default.StandardDirPath + ProjectName + "\\" + IssueName + "\\" + PID + " - " + Name);

            if(hidden)
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden; 

        }
    }
}
