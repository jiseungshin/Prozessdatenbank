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
using System.Windows.Shapes;
using PDCore.Manager;
using PDCore.Processes;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für CoatingSPAdministration.xaml
    /// </summary>
    public partial class CoatingSPAdministration : Window
    {
        public CoatingSPAdministration()
        {
            InitializeComponent();
            DataContext = new VMCoatingProcesses();
            
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
       

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            ObjectWindows.CoatingStandardProcessWindow _pw = new ObjectWindows.CoatingStandardProcessWindow();
            _pw.ShowDialog();
        }

        private void LV_Processes_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                ObjectWindows.CoatingStandardProcessWindow _pw = new ObjectWindows.CoatingStandardProcessWindow((sender as ListView).SelectedItem as PCoatingCemeconProcess);
                _pw.ShowDialog();
            }
        }
    }

    public class VMCoatingProcesses : ViewModels.BaseViewModel
    {

        public VMCoatingProcesses()
        {
            ProcessManager.Instance.update();
            Updater.Instance.newData += Instance_newData;
        }

        public ObservableCollection<PCoatingCemeconProcess> Processes
        {
            get { return new ObservableCollection<PCoatingCemeconProcess>(ProcessManager.Instance.CemeConStandardProcesses); }
        }

        void Instance_newData()
        {
            NotifyPropertyChanged("Processes");
        }
    }
}
