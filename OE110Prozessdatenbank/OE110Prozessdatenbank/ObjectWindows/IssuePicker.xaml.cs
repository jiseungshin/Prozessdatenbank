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
    public partial class IssuePicker : Window
    {
        OReferenceVM m_vm;
        public IssuePicker(OReferenceVM vm)
        {
            InitializeComponent();
            int projectID = 0;
            if (vm.Project != null)
                projectID = vm.Project.ID;
            
            DataContext = new VMIssuePicker(projectID);
            m_vm = vm;
        }

        private void LV_Project_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                if (MessageBox.Show(Properties.Messages.q_editIssue, "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    m_vm.Issue = (sender as ListView).SelectedItem as Issue;
                    this.Close();
                }
            }
        }
    }

    public class VMIssuePicker : ViewModels.BaseViewModel
    {
        int m_projectID = 0;
        public VMIssuePicker(int ProjectID)
        {
            m_projectID = ProjectID;
        }

        public ObservableCollection<Issue> Issues
        { get { return new ObservableCollection<Issue>(ObjectManager.Instance.Issues.FindAll(item=>item.ProjectID==m_projectID)); } }
    }
}
