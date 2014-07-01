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
    /// Interaktionslogik für CExpCemeCon.xaml
    /// </summary>
    public partial class CExpCemeCon : Window
    {
        ViewModels.PExpCemeConVM m_vm;
        public CExpCemeCon(int ID, bool update)
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpCemeConVM(ID, update);
            DataContext = m_vm;

            g_quality.Children.Add(m_vm.WorkpieceQualityControl);
            Grid.SetRow(m_vm.WorkpieceQualityControl, 0);

        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cb_glass_changed(object sender, SelectionChangedEventArgs e)
        {
            if (g_quality.Children.Count==2)
                g_quality.Children.RemoveAt(1);
            g_quality.Children.Add(m_vm.ProcessQualityControl);
            Grid.SetRow(m_vm.ProcessQualityControl, 1);
        }
    }
}
