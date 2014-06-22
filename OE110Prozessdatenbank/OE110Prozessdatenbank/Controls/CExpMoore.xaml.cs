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

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CExpMoore.xaml
    /// </summary>
    public partial class CExpMoore : UserControl
    {
        ViewModels.PExpMooreVM m_vm;
        public CExpMoore()
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpMooreVM();
            DataContext = m_vm;
        }

        public CExpMoore(int PID)
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpMooreVM(PID);
            DataContext = m_vm;

            cb_upperWP.IsEnabled = false;
            cb_lowerWP.IsEnabled = false;
            cb_glass.IsEnabled = false;
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cb_lowerWP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = g_quality.Children.Count;
            
            g_quality.Children.Add(m_vm.WP_LowerControl);
            Grid.SetRow(m_vm.WP_LowerControl, count);

            g_quality.Height = (count+1) * 120;
            
        }

        private void cb_upperWP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = g_quality.Children.Count;

            g_quality.Children.Add(m_vm.WP_UpperControl);
            Grid.SetRow(m_vm.WP_UpperControl, count);

            g_quality.Height = (count + 1) * 120;
        }
    }
}
