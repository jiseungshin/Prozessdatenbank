using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PDCore.Manager;
using PDCore.Database;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace OE110Prozessdatenbank.ViewModels
{
    public class F_PostProcessingVM : BaseViewModel
    {

        private string m_processedConstraint = "ProcessReferences.Status='processed'";
        private string m_AnalysedCostraint = "ProcessReferences.Status='analysed'";
        private string m_DecoatedConstraint = "ProcessReferences.Status='decoated'";
        private string m_terminatedContraint = "ProcessReferences.Status='terminated' OR ProcessReferences.Status='cancelled'";
        private string m_FullConstraint = " WHERE (ProcessReferences.Status='processed' "+
                                            "OR ProcessReferences.Status='analysed' "+
                                            "OR ProcessReferences.Status='terminated' " +
                                            "OR ProcessReferences.Status='cancelled' " +
                                            "OR ProcessReferences.Status='decoated') " ;

        private string m_Filter = "";

        private string m_sortString = "";

        private bool m_p = true;
        private bool m_a = true;
        private bool m_d = true;
        private bool m_t = true;

        public F_PostProcessingVM()
        {
            ProcessManager.Instance.newProcesses += Instance_newProcesses;
        }

        void Instance_newProcesses()
        {
            NotifyPropertyChanged("Data");
        }

        public DataTable Data
        {
            get
            {
                DataSet _ds = ProcessManager.Instance.getData(Queries.QueryPostProcessing + m_FullConstraint + m_Filter + m_sortString);
                if (_ds.Tables[0].Rows.Count > 0)
                {
                    _ds.Tables[0].Columns.Add("analysed", typeof(bool));
                    _ds.Tables[0].Columns.Add("decoated", typeof(bool));
                    _ds.Tables[0].Columns.Add("terminated", typeof(bool));

                    foreach (DataRow dr in _ds.Tables[0].Rows)
                    {

                        if (ProcessManager.Instance.getCountOf(DBAnalyses.Table, DBAnalyses.RefNumber, dr.Field<int>(DBProcessReferences.RefNumber)) > 0)
                            dr["analysed"] = true;
                        else
                            dr["analysed"] = false;

                        if (ProcessManager.Instance.getCountOf(DBProcessReferenceRelation.Table, DBProcessReferenceRelation.RefNumber + " = " + dr.Field<int>(DBProcessReferences.RefNumber) + " AND Machine_ID=51") > 0)
                            dr["decoated"] = true;
                        else
                            dr["decoated"] = false;

                        if (dr.Field<string>(DBIssues.Conclusion) != null)
                            dr["terminated"] = true;
                        else
                            dr["terminated"] = false;
                    }
                }



                return _ds.Tables[0];
            }
        }

        public bool Processed
        {
            set
            {
                if (value == true)
                {
                    m_processedConstraint = " ProcessReferences.Status='processed' ";
                }
                else
                    m_processedConstraint = "";

                generateFullConstraint();
                m_p = value;
            }
            get { return m_p; }
        }

        public bool Analysed
        {
            set
            {
                if (value == true)
                {
                    m_AnalysedCostraint = " ProcessReferences.Status='analysed' ";
                }
                else
                    m_AnalysedCostraint = "";

                generateFullConstraint();
                m_a = value;
            }
            get { return m_a; }
        }

        public bool Decoated
        {
            set
            {
                if (value == true)
                {
                    m_DecoatedConstraint = " ProcessReferences.Status='decoated' ";
                }
                else
                    m_DecoatedConstraint = "";

                generateFullConstraint();
                m_d = value;
            }
            get
            {
                return m_d;
            }
        }

        public bool Terminated
        {
            set
            {
                if (value == true)
                {
                    m_terminatedContraint = " ProcessReferences.Status='terminated' OR ProcessReferences.Status='cancelled' ";
                }
                else
                    m_terminatedContraint = "";

                generateFullConstraint();
                m_t = value;
            }
            get
            {
                return m_t;
            }
        }

        private void generateFullConstraint()
        {
            m_FullConstraint = " WHERE (";

            if (m_processedConstraint != "")
            {
                m_FullConstraint += m_processedConstraint;
                m_FullConstraint += " OR ";
            }

            if (m_AnalysedCostraint != "")
            {
                m_FullConstraint += m_AnalysedCostraint; m_FullConstraint += " OR ";
            }

            if (m_DecoatedConstraint != "")
            {
                m_FullConstraint += m_DecoatedConstraint; m_FullConstraint += " OR ";
            }

            if (m_terminatedContraint != "")
            {
                m_FullConstraint += m_terminatedContraint; m_FullConstraint += " OR ";
            }

            //m_FullConstraint += "ProcessReferences.Status='cancelled' " +
            //                    "OR ProcessReferences.Status='terminated')";

            m_FullConstraint = m_FullConstraint.Remove(m_FullConstraint.Length - 4, 3);
            m_FullConstraint += ")";

            if (m_processedConstraint == "" && m_AnalysedCostraint == "" && m_DecoatedConstraint == "" && m_terminatedContraint == "")
                m_FullConstraint = " WHERE ProcessReferences.ReferenceNumber=-1";

            NotifyPropertyChanged("Data");

        }

        public string Filter
        {
            set
            {
                if (value != "")
                {
                    m_Filter = " AND (" + DBWorkpieces.Table + "." + DBWorkpieces.Label + " LIKE ('%" + value + "%')" +
                              " OR " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber + " LIKE ('%" + value + "%')" +
                              " OR " + DBMAterial.Table + "." + DBMAterial.Name + " LIKE ('%" + value + "%'))";
                }
                else
                    m_Filter = value;

                NotifyPropertyChanged("Data");
            }
        }

        public string SortString
        {
            set { m_sortString = value; NotifyPropertyChanged("Data"); }
        }

    }
    
}
