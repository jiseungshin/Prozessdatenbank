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
using OE110Prozessdatenbank.ViewModels;
using PDCore.Manager;
using System.Collections.ObjectModel;
using PDCore.Processes;
using PDCore.BusinessObjects;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für PhoenixProcessAdministration.xaml
    /// </summary>
    public partial class PhoenixProcessAdministration : Window
    {
        public PhoenixProcessAdministration()
        {
            InitializeComponent();
            DataContext = new PhoenixProcessAdministrationVM();
        }

        private void LV_Processes_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                new ObjectWindows.AddPhoenixProcess(((sender as ListView).SelectedItem) as PDCore.Processes.PGrindingPhoenixProcess).ShowDialog();
            }
        }

        private void mbt_addWorkpiece_Click(object sender, RoutedEventArgs e)
        {

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

        private void mbt_addProcess_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddPhoenixProcess().ShowDialog();
        }

    }

    public class PhoenixProcessAdministrationVM : BaseViewModel
    {
        private string m_filter = "";

        public PhoenixProcessAdministrationVM()
        {
            ObjectManager.Instance.newObjects += Instance_newObjects;
            ObjectManager.Instance.update(PDCore.Database.DBPhoenixProcesses.Table);
        }

        void Instance_newObjects()
        {
            NotifyPropertyChanged("PhoenixProcesses");
        }

        public ObservableCollection<PGrindingPhoenixProcess> PhoenixProcesses
        {
            get
            {
                if (m_filter == "")
                    return new ObservableCollection<PGrindingPhoenixProcess>(ObjectManager.Instance.PhoenixProcesses);
                else
                    return new ObservableCollection<PGrindingPhoenixProcess>(ObjectManager.Instance.PhoenixProcesses.Where(item => item.Description.ToLower().Contains(Filter.ToLower()) || item.Ra.ToString().ToLower().Contains(Filter.ToLower())));
            }
        }

        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; NotifyPropertyChanged("PhoenixProcesses"); }
        }
    }
}
