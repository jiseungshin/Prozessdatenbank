﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PDCore.BusinessObjects;
using PDCore.Manager;
using PDCore.Database;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.ViewModels
{
    public class F_GrindingVM : BaseViewModel
    {

        private Machine m_Machine = null;
        private string m_rawFilter = "";
        private string m_PolshedFilter = "";
        private string m_PolishedKrit = DBProjects.Table + "." + DBProjects.Name;
        private FilterCriteria m_polishedCriterium = ProcessManager.Instance.FilterCriteria[0];
        private string m_sortString="";
        private string m_sortStringProcessed = "";

        public F_GrindingVM()
        {
            //Updater.Instance.newData += Instance_newData;
            ProcessManager.Instance.newProcesses += Instance_newProcesses;
            ObjectManager.Instance.update(DBWorkpieces.Table);
            ObjectManager.Instance.update(DBProjects.Table);
            ObjectManager.Instance.update(DBIssues.Table);
            ObjectManager.Instance.update(DBUser.Table);
            ObjectManager.Instance.update(DBMachine.Table);
            
            m_Machine = ObjectManager.Instance.Machines[0];
        }

        void Instance_newProcesses()
        {
            NotifyPropertyChanged("DataRaw");
            NotifyPropertyChanged("DataPolished");
        }

        public string searchRaw
        {
            set
            {


            }
        }

        public void setMachine(int ID)
        {
            try
            {
                Machine = Machines.Single(item => item.ID == ID);
                
            }
            catch 
            { 
                //int a = 0; 
            }
        }

        public Machine Machine
        {
            get
            {
                return m_Machine;
            }
            set
            {
                m_Machine = value;
                NotifyPropertyChanged("Machine"); NotifyPropertyChanged("DataPolished");
            }
        }

        public ObservableCollection<Machine> Machines
        {
            get { return new ObservableCollection<Machine>(ObjectManager.Instance.Machines.FindAll(item => item.Process == 1)); }
        }

        public DataSet DataRaw
        {
            get {return ProcessManager.Instance.getData(Queries.QueryRaw + m_rawFilter + m_sortString); }
        }

        public ObservableCollection<FilterCriteria> FilterCriteria
        { get { return new ObservableCollection<PDCore.Database.FilterCriteria>(ProcessManager.Instance.FilterCriteria); } }

        public FilterCriteria Criterium
        {
            get { return m_polishedCriterium; }
            set { m_polishedCriterium = value; }
        }

        public DataSet DataPolished
        {
            get
            {
                switch (Machine.ID)
                {
                    case 11:
                        return ProcessManager.Instance.getData(Queries.QueryTurningMoore + m_PolshedFilter + m_sortStringProcessed);
                    case 12:
                        return ProcessManager.Instance.getData(Queries.QueryGrindingMoore + m_PolshedFilter + m_sortStringProcessed);
                    case 13:
                        return ProcessManager.Instance.getData(Queries.QueryGrindingPhoenix + m_PolshedFilter + m_sortStringProcessed);
                    case 14:
                        return ProcessManager.Instance.getData(Queries.QueryGrindingOther + m_PolshedFilter + m_sortStringProcessed);

                }
                return null;
            }
        }

        public string RawFilter
        {
            set 
            {
                if (value != "")
                {
                    m_rawFilter = " AND (" + DBWorkpieces .Table+ "." + DBWorkpieces.Label + " LIKE ('%" + value + "%')" +
                              " OR " + DBMAterial.Table+ "." + DBMAterial.Name + " LIKE ('%" + value + "%')" +
                              " OR " + DBUser.Table+ "." + DBUser.Token + " LIKE ('%" + value + "%'))";
                }
                else
                    m_rawFilter = value;
            
                NotifyPropertyChanged("DataRaw");
            }
        }

        public string PolishedKriterium
        {
            set { m_PolishedKrit = value; }
        }
        public string PolishedFilter
        {
            set
            {
                if (value != "")
                {
                    m_PolshedFilter = " WHERE " + m_polishedCriterium.DatabaseField + " LIKE ('%" + value + "%')";
                }
                else
                    m_PolshedFilter = value;

                NotifyPropertyChanged("DataPolished");
            }
        }

        public string SortString
        {
            set { m_sortString = value; NotifyPropertyChanged("DataRaw"); }
        }

        public string SortStringProcessed
        {
            set { m_sortStringProcessed = value; NotifyPropertyChanged("DataPolished"); }
        }

        
    }
}
