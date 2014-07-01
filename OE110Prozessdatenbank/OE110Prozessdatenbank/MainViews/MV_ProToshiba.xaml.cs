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

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für CProMoore.xaml
    /// </summary>
    public partial class MV_ProToshiba : UserControl
    {

        private ProcessWindows.GenericWindow m_window;

        public MV_ProToshiba()
        {
            InitializeComponent();
            //DataContext = new VMProcessingMoore();
            
        }

        private void LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBExpMoore.ID]);
                //m_window = new ProcessWindows.GenericWindow();
                //m_window.contentGrid.Children.Add(new Controls.CExpMoore(ID));
                //m_window.Title = "Versuch - Moore";
                //m_window.IconType = IconManager.IconType.ProcessIcon;
                //m_window.ShowDialog();
                //new ProcessWindows.CExpMoore(ID).ShowDialog();
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
            new MainWindows.ToshibaProcessImport().ShowDialog();

        }
    }

    public class VMProcessingMoore : ViewModels.BaseViewModel
    {


        private string m_filter = "";
        private FilterCriteria m_criteria = ProcessManager.Instance.FilterCriteria[0];
        public VMProcessingMoore()
        {
            Updater.Instance.newData += Instance_newData;
        }

        void Instance_newData()
        {
            NotifyPropertyChanged("ProcessedData");
        }

        public DataSet ProcessedData
        {
            get 
            {
                return ProcessManager.Instance.getData(Queries.QueryProcessedMoore + m_filter);
            }
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



    }
}
