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

namespace OE110Prozessdatenbank.ProcessWindows
{
    /// <summary>
    /// Interaktionslogik für CExpTestStation.xaml
    /// </summary>
    public partial class CExpTestStation : Window
    {
        ViewModels.PExpTestStationVM m_vm;
        public CExpTestStation(int ID)
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpTestStationVM(ID);
            DataContext = m_vm;

            cb_Glass.IsEnabled = true;
            cb_WPLeft.IsEnabled = false;
            cb_WPRight.IsEnabled = false;
            cb_WPCenter.IsEnabled = false;
            
        }

        public CExpTestStation()
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpTestStationVM();
            DataContext = m_vm;

        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void cb_WPLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!m_set.Contains(1))
            {
                m_set.Add(1);
            }
            setQualityControls();
        }

        private void cb_WPCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!m_set.Contains(2))
            {
                m_set.Add(2);
            }
            setQualityControls();
        }

        private void cb_WPRight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!m_set.Contains(3))
            {
                m_set.Add(3);
            }
            setQualityControls();
        }

        private void cb_Glass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!m_set.Contains(4))
            {
                m_set.Add(4);
            }
            setQualityControls();
        }

        private List<int> m_set = new List<int>();

        private void setQualityControls()
        {
            g_quality.Children.Clear();
            for (int i = 0; i < m_set.Count; i++)
            {
                if (m_set[i] == 1)
                {
                    g_quality.Children.Add(m_vm.WP_LeftControl);
                    Grid.SetRow(m_vm.WP_LeftControl, i);
                    continue;
                }
                if (m_set[i] == 2)
                {
                    g_quality.Children.Add(m_vm.WP_CenterControl);
                    Grid.SetRow(m_vm.WP_CenterControl, i);
                    continue;
                }
                if (m_set[i] == 3)
                {
                    g_quality.Children.Add(m_vm.WP_RightControl);
                    Grid.SetRow(m_vm.WP_RightControl, i);
                    continue;
                }
                if (m_set[i] == 4)
                {
                    g_quality.Children.Add(m_vm.ProcessQualityControl);
                    Grid.SetRow(m_vm.ProcessQualityControl, i);
                    continue;
                }
            }
        }

        private void TextBox_PreviewDecimalTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Input.DecimalRegex.IsMatch((sender as TextBox).Text + e.Text);
        }
    }
}
