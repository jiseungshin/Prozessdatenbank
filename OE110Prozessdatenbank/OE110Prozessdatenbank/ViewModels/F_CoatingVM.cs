using System;
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
    public class F_CoatingVM : BaseViewModel
    {

        private string m_polishedFilter = "";
        private string m_coatedFilter = "";
        private FilterCriteria m_CoatedCriterium = ProcessManager.Instance.FilterCriteria[0];

        public F_CoatingVM()
        {
            ProcessManager.Instance.update();
            ProcessManager.Instance.newProcesses += Instance_newProcesses;

        }

        void Instance_newProcesses()
        {
            NotifyPropertyChanged("DataPolished");
            NotifyPropertyChanged("DataCoated");
        }
        public DataSet DataPolished
        {
            get { return ProcessManager.Instance.getData(Queries.QueryGrinded + m_polishedFilter); }
        }

        public DataSet DataCoated
        {
            get
            {
                DataSet _ds = ProcessManager.Instance.getData(Queries.QueryCoated + m_coatedFilter);
                //_ds.Tables[0].Columns.Add("al", typeof(string));
                //_ds.Tables[0].Columns.Add("pl", typeof(string));
                //foreach(DataRow dr in _ds.Tables[0].Rows)
                //{
                //    //AdherentLayer = Layer.Find(item => item.ID == row.Field<int>(DBCoatingCemeconProcess.AdherentLayer)),
                //    //ProtectiveLayer = Layer.Find(item => item.ID == row.Field<int>(DBCoatingCemeconProcess.ProtectiveLayer)),

                //    //int id = dr.Field<int>(DBCoatingCemeconProcess.AdherentLayer);

                //    dr["al"] = ProcessManager.Instance.Layer.Find(item => item.ID == dr.Field<int>(DBCoatingCemeconProcess.AdherentLayer)).Structure;
                //    dr["pl"] = ProcessManager.Instance.Layer.Find(item => item.ID == dr.Field<int>(DBCoatingCemeconProcess.ProtectiveLayer)).Structure;

                //}

                return _ds;
            }
        }

        public string PolishedFilter
        {
            set
            {
                if (value != "")
                {
                    m_polishedFilter = " AND (" + DBWorkpieces.Table + "." + DBWorkpieces.Label + " LIKE ('%" + value + "%')" +
                              " OR " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber + " LIKE ('%" + value + "%')" +
                              " OR " + DBMAterial.Table + "." + DBMAterial.Name + " LIKE ('%" + value + "%'))";
                }
                else
                    m_polishedFilter = value;

                NotifyPropertyChanged("DataPolished");
            }
        }

        public string CoatedFilter
        {
            set
            {
                if (value != "")
                {
                    m_coatedFilter = " WHERE " + m_CoatedCriterium.DatabaseField + " LIKE ('%" + value + "%')";
                }
                else
                    m_coatedFilter = value;

                NotifyPropertyChanged("DataCoated");
            }
        } 

        public ObservableCollection<FilterCriteria> FilterCriteria
        { get { return new ObservableCollection<PDCore.Database.FilterCriteria>(ProcessManager.Instance.FilterCriteriaCoating); } }

        public FilterCriteria Criterium
        {
            get { return m_CoatedCriterium; }
            set { m_CoatedCriterium = value; }
        }
    }
}
