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
using PDCore.BusinessObjects;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CExpOther.xaml
    /// </summary>
    public partial class CProject : Window
    {
        ViewModels.OProjectVM m_vm;
        public CProject(int ID)
        {
            InitializeComponent();
            m_vm = new ViewModels.OProjectVM(ID);
            DataContext = m_vm;
        }

        public CProject()
        {
            InitializeComponent();
            m_vm = new ViewModels.OProjectVM();
            DataContext = m_vm;
            bt_addIssue.IsEnabled = false;
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LV_Issues_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new CIssue((sender as ListView).SelectedItem as Issue).ShowDialog();
            }
        }

        private void bt_addIssue_Click(object sender, RoutedEventArgs e)
        {
            new CIssue(m_vm.ProjectID).ShowDialog();
        }
    }
}
