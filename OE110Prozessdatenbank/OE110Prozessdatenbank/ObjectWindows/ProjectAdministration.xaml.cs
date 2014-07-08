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
using System.Collections.ObjectModel;
using PDCore.BusinessObjects;
using PDCore.Manager;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für ProjectAdministration.xaml
    /// </summary>
    public partial class ProjectAdministration : Window
    {
        public ProjectAdministration()
        {
            InitializeComponent();
            DataContext = new VMProjectAdmin();
        }

        private void LV_Project_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {

                int ID = (((sender as ListView).SelectedItem as Project).ID);
                new ObjectWindows.CProject(ID).ShowDialog();

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

        private void mbt_AddProject_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.CProject().ShowDialog();
        }
    }

    public class VMProjectAdmin : ViewModels.BaseViewModel
    {
        public VMProjectAdmin()
        {
            ObjectManager.Instance.updateProjects();
            ObjectManager.Instance.updateUser();
            ObjectManager.Instance.newObjects += Instance_newObjects;

        }

        void Instance_newObjects()
        {
            NotifyPropertyChanged("Projects");
        }

        public ObservableCollection<Project> Projects
        { get { return new ObservableCollection<Project>(ObjectManager.Instance.Projects); } }
    }
}
