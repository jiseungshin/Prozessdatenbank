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
using OE110Prozessdatenbank.ViewModels;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für ProjectAdministration.xaml
    /// </summary>
    public partial class ProjectPicker : Window
    {
        OReferenceVM m_vm;
        public ProjectPicker(OReferenceVM vm)
        {
            InitializeComponent();
            DataContext = new VMProjectPicker();
            m_vm = vm;
        }

        private void LV_Project_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                if (MessageBox.Show(Properties.Messages.q_editProject, "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    m_vm.Project = (sender as ListView).SelectedItem as Project;
                    this.Close();
                }
            }
        }
    }

    public class VMProjectPicker : ViewModels.BaseViewModel
    {
        public VMProjectPicker()
        {
            ObjectManager.Instance.update(PDCore.Database.DBProjects.Table);
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
