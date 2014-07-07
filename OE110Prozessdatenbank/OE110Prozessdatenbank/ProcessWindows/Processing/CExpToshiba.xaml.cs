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

namespace OE110Prozessdatenbank.ProcessWindows
{
    /// <summary>
    /// Interaktionslogik für CExpToshiba.xaml
    /// </summary>
    public partial class CExpToshiba : Window
    {
        ViewModels.PToshibaVM m_vm;
        int gridIndex = 0;
        public CExpToshiba(int PID)
        {
            InitializeComponent();
            m_vm = new ViewModels.PToshibaVM(PID);
            DataContext = m_vm;

            if (m_vm.Process.UpperWorkpiece != null)
            {
                try
                {
                    g_quality.Children.Add(m_vm.WP_UpperControl);
                    Grid.SetRow(m_vm.WP_UpperControl, gridIndex);
                    gridIndex++;
                }
                catch { }
            }
            if (m_vm.Process.LowerWorkpiece != null)
            {
                try
                {
                    g_quality.Children.Add(m_vm.WP_LowerControl);
                    Grid.SetRow(m_vm.WP_LowerControl, gridIndex);
                    gridIndex++;
                }
                catch { }
            }
            if (m_vm.Glass != null)
            {
                try
                {
                    g_quality.Children.Add(m_vm.ProcessQualityControl);
                    Grid.SetRow(m_vm.ProcessQualityControl, gridIndex);
                }
                catch { }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
