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
using System.Drawing;

namespace OE110Prozessdatenbank.ProcessWindows
{
    /// <summary>
    /// Interaktionslogik für CExpMoore.xaml
    /// </summary>
    public partial class CExpMoore : Window
    {
        ViewModels.PExpMooreVM m_vm;
        bool m_update = false;
        public CExpMoore()
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpMooreVM();
            DataContext = m_vm;
           
        }

        public CExpMoore(int PID)
        {
            InitializeComponent();
            m_update = true;
            m_vm = new ViewModels.PExpMooreVM(PID);
            DataContext = m_vm;

            bt_findLower.Visibility = System.Windows.Visibility.Hidden;
            bt_findUpper.Visibility = System.Windows.Visibility.Hidden;
            cb_glass.IsEnabled = true;
            
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void cb_glass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!m_update)
            {
                if (!m_set.Contains(3))
                {
                    m_set.Add(3);
                }

                setQualityControls();
            }
        }

        private List<int> m_set = new List<int>();

        private void setQualityControls()
        {
            g_quality.Children.Clear();
            for (int i = 0; i < m_set.Count; i++)
            { 
                if (m_set[i]==1)
                {
                    g_quality.Children.Add(m_vm.WP_LowerControl);
                    Grid.SetRow(m_vm.WP_LowerControl, i);
                    continue;
                }
                if (m_set[i] == 2)
                {
                    g_quality.Children.Add(m_vm.WP_UpperControl);
                    Grid.SetRow(m_vm.WP_UpperControl, i);
                    continue;
                }
                if (m_set[i] == 3)
                {
                    g_quality.Children.Add(m_vm.ProcessQualityControl);
                    Grid.SetRow(m_vm.ProcessQualityControl, i);
                    continue;
                }
            }
        }

        ObjectWindows.WorkpiecePicker UpperWPPicker;
        ObjectWindows.WorkpiecePicker LowerWPPicker;
        private void bt_findUpper_Click(object sender, RoutedEventArgs e)
        {
            UpperWPPicker = new ObjectWindows.WorkpiecePicker(m_vm.WorkpiecesUpper);
            
            UpperWPPicker.Closing += owp_Closing;
            UpperWPPicker.Show();
        }

        void owp_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (UpperWPPicker.Workpice != null)
            {
                m_vm.UpperWP = UpperWPPicker.Workpice;
                if (!m_set.Contains(2))
                {
                    m_set.Add(2);
                }

                setQualityControls();
            }
        }

        private void bt_findLower_Click(object sender, RoutedEventArgs e)
        {
            LowerWPPicker = new ObjectWindows.WorkpiecePicker(m_vm.WorkpiecesLower);
            
            LowerWPPicker.Closing += LowerWPPicker_Closing;
            LowerWPPicker.Show();
        }

        void LowerWPPicker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LowerWPPicker.Workpice != null)
            {
                m_vm.LowerWP = LowerWPPicker.Workpice;
                if (!m_set.Contains(1))
                {
                    m_set.Add(1);
                }
                setQualityControls();
            }
        }

        private void window_loaded(object sender, RoutedEventArgs e)
        {
            if (m_update)
            {
                //lower
                if (m_vm.LowerWP != null)
                {
                    if (!m_set.Contains(1))
                    {
                        m_set.Add(1);
                    }
                    setQualityControls();
                }
                //upper
                if (m_vm.UpperWP != null)
                {
                    if (!m_set.Contains(2))
                    {
                        m_set.Add(2);
                    }

                    setQualityControls();
                }
                //glass
                if (m_vm.Glass != null)
                {
                    if (!m_set.Contains(3))
                    {
                        m_set.Add(3);
                    }

                    setQualityControls();
                }
            }

        }

        private void TextBox_PreviewDecimalTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Input.DecimalRegex.IsMatch((sender as TextBox).Text + e.Text);
        }

       
    }
}
