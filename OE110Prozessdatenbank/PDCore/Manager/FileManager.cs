﻿using System;
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

        private string StandardDir = "";

        FileManager()
        {
            List<string> m_data = IO.SimpleIO.getClearText(@"connection.txt");

            //new instance of MySQL communication class 
            _myCommunicator = new MySQLCommunicator();
            _myCommunicator.Password = m_data[3]; //PDCore.Properties.Settings.Default.Password;
            _myCommunicator.Server = m_data[0]; //PDCore.Properties.Settings.Default.Server;
            _myCommunicator.User = m_data[2]; //PDCore.Properties.Settings.Default.User;
            _myCommunicator.Database = m_data[1]; //PDCore.Properties.Settings.Default.Database;

            StandardDir = m_data[4];



            //Recieve database-errors
            _myCommunicator.MessageThrown += _myCommunicator_MessageThrown;

            Updater.Instance.newData += Instance_newData;

            if (_myCommunicator.checkConnection())
            { 
            }

        }

        public string StandardDirectory
        { get { return StandardDir; } }

        void Instance_newData(params string[] values)
        {
            //throw new NotImplementedException();
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

        //public void createProjectDirectory(Project project)
        //{
        //    Directory.CreateDirectory(StandardDir + project.Description);
        //    //Directory.CreateDirectory(@"" + project.Description);
        //}

        //public void updateProjectDirectory(Project Project)
        //{
        //    if (Project.Description!=Project.OLDDescription)
        //    {
        //        try
        //        {
        //            Directory.Move(StandardDir + Project.OLDDescription, StandardDir + Project.Description);
        //        }
        //        catch { System.Windows.MessageBox.Show("Das Umbenennen des Ordners konnte nicht ausgeführt werden, da der Zugruff verweigert wurde! Der Ordner muss nun manuell umbenannt werden!"); }

        //    }

        //}

        //public void createIssueDirectory(Issue Issue)
        //{
        //    string Projectname = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + Issue.ProjectID).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
        //    Directory.CreateDirectory(StandardDir + Projectname + "\\" + Issue.Description);
        //}

        //public void updateIssueDirectoryName(Issue Issue)
        //{

        //    if (Issue.Description != Issue.OLDDescription)
        //    {
        //        try
        //        {
        //            string Projectname = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + Issue.ProjectID).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
        //            Directory.Move(StandardDir + Projectname + "\\" + Issue.OLDDescription, StandardDir + Projectname + "\\" + Issue.Description);
        //        }
        //        catch { System.Windows.MessageBox.Show("Das Umbenennen des Ordners konnte nicht ausgeführt werden, da der Zugruff verweigert wurde! Der Ordner muss nun manuell umbenannt werden!"); }
        //    }
        //}

        public void createReferenceDirectory(int ReferenceNumber)
        {
            DataTable _reference = (_myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber).Tables[0]);

            //string ProjectName = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.ProjectID)).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
            //string IssueName = (_myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.IssueID)).Tables[0].Rows[0]).Field<string>(DBIssues.Description);

            Directory.CreateDirectory(StandardDir /*+ ProjectName + "\\" + IssueName + "\\"*/ + ReferenceNumber);
        }

        public void createDirectory(int ReferenceNumber, string Name)
        {
            DataTable _reference = (_myCommunicator.getDataSet("SELECT * FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.RefNumber + "=" + ReferenceNumber).Tables[0]);

            string ProjectName = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.ProjectID)).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
            string IssueName = (_myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ID + "=" + _reference.Rows[0].Field<int>(DBProcessReferences.IssueID)).Tables[0].Rows[0]).Field<string>(DBIssues.Description);

            Directory.CreateDirectory(StandardDir +/* ProjectName + "\\" + IssueName + "\\" +*/ ReferenceNumber + "\\" + Name);
        }

        //public void createProcessDirectory(int ProjectID, int IssueID, int PID, string Name, bool hidden)
        //{

        //    string ProjectName = (_myCommunicator.getDataSet("SELECT * FROM " + DBProjects.Table + " WHERE " + DBProjects.ID + "=" + ProjectID).Tables[0].Rows[0]).Field<string>(DBProjects.Name);
        //    string IssueName = (_myCommunicator.getDataSet("SELECT * FROM " + DBIssues.Table + " WHERE " + DBIssues.ID + "=" + IssueID).Tables[0].Rows[0]).Field<string>(DBIssues.Description);

        //    DirectoryInfo di = Directory.CreateDirectory(StandardDir + ProjectName + "\\" + IssueName + "\\" + PID + " - " + Name);

        //    if(hidden)
        //        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden; 

        //}
        public string getDirPth(int ReferenceNumber)
        {
            try
            {
                string[] dir = Directory.GetDirectories(StandardDir, ReferenceNumber.ToString(), SearchOption.AllDirectories);
                return dir[0];
            }
            catch { return ""; }
        }

        public void Copy(string source, string dest)
        {
            File.Copy(source, dest, true);
        }
    }
}
