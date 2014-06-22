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
using PDCore.BusinessObjects;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CIssue.xaml
    /// </summary>
    public partial class CIssue : Window
    {
        ViewModels.OIssueVM m_vm;
        public CIssue(Issue issue)
        {
            InitializeComponent();
            m_vm = new ViewModels.OIssueVM(issue);
            DataContext = m_vm;
        }

        public CIssue(int ID)
        {
            InitializeComponent();
            m_vm = new ViewModels.OIssueVM(ID);
            DataContext = m_vm;
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
