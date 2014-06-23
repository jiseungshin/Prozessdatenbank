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
            Updater.Instance.newData += Instance_newData;
        }

        void Instance_newData()
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
            get { return ProcessManager.Instance.getData(Queries.QueryCoated + m_coatedFilter); }
        }

        public string PolishedFilter
        {
            set
            {
                if (value != "")
                {
                    m_polishedFilter = " AND " + DBWorkpieces.Table + "." + DBWorkpieces.Label + " LIKE ('%" + value + "%')" +
                              " OR " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber + " LIKE ('%" + value + "%')" +
                              " OR " + DBMAterial.Table + "." + DBMAterial.Name + " LIKE ('%" + value + "%')";
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
                    m_coatedFilter = " AND " + m_CoatedCriterium.DatabaseField + " LIKE ('%" + value + "%')";
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
