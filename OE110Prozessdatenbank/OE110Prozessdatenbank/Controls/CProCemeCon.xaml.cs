using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using PDCore.Processes;
using PDCore.Manager;
using PDCore.Database;
using OE110Prozessdatenbank.Commands;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CProMoore.xaml
    /// </summary>
    public partial class CProSemeCon : UserControl
    {

        private ProcessWindows.GenericWindow m_window;

        public CProSemeCon()
        {
            InitializeComponent();
            DataContext = new VMProcessingCemeCon();
            
        }

        private void LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBExpCemeCon.ID]);

                new ProcessWindows.CExpCemeCon(ID, true).ShowDialog();
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void mbt_newProcess_Click(object sender, RoutedEventArgs e)
        {

            if ((LV_Coated).SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((LV_Coated).SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);

                new ProcessWindows.CExpCemeCon(ID, false).ShowDialog();

            }

        }

        private void LV_Coated_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);

                new ProcessWindows.CExpCemeCon(ID, false).ShowDialog();

            }
        }
    }

    public class VMProcessingCemeCon : ViewModels.BaseViewModel
    {


        private string m_filter = "";
        private string m_coatedFilter = "";
        private FilterCriteria m_criteria = ProcessManager.Instance.FilterCriteria[0];
        public VMProcessingCemeCon()
        {
            Updater.Instance.newData += Instance_newData;
        }

        void Instance_newData()
        {
            NotifyPropertyChanged("ProcessedData");
            NotifyPropertyChanged("CoatedData");
        }

        public DataSet ProcessedData
        {
            get 
            {
                return ProcessManager.Instance.getData(Queries.QueryProcessedCemeCon + m_filter);
            }
        }

        public DataSet CoatedData
        {
            get { return ProcessManager.Instance.getData(Queries.QueryCoated +" WHERE "+DBProcessReferences.Table+"."+DBProcessReferences.Status+"='coated' " + m_coatedFilter); }
        }

        public ObservableCollection<FilterCriteria> FilterCriteria
        { get { return new ObservableCollection<PDCore.Database.FilterCriteria>(ProcessManager.Instance.FilterCriteria); } }

        public FilterCriteria Criterium
        {
            set { m_criteria = value; }
        }

        public string Filter
        {
            set
            {
                if (value != "")
                {
                    m_filter = " AND " + m_criteria.DatabaseField + " LIKE ('%" + value + "%')";
                }
                else
                    m_filter = value;

                NotifyPropertyChanged("ProcessedData");
            }
        }

        public string CoatedFilter
        {
            set
            {
                if (value != "")
                {
                    m_coatedFilter = " AND (" + DBWorkpieces.Table + "." + DBWorkpieces.Label + " LIKE ('%" + value + "%')" +
                              " OR " + DBProcessReferences.Table + "." + DBProcessReferences.RefNumber + " LIKE ('%" + value + "%')" +
                              " OR " + DBMAterial.Table + "." + DBMAterial.Name + " LIKE ('%" + value + "%'))";
                }
                else
                    m_coatedFilter = value;

                NotifyPropertyChanged("CoatedData");
            }
        }



    }
}
