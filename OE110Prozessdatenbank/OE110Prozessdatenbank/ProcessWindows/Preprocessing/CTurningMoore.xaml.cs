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
    /// Interaktionslogik für CTurningMoore.xaml
    /// </summary>
    public partial class CTurningMoore : Window
    {

        ViewModels.PTurningMooreVM m_vm;
        public CTurningMoore(int refID, bool update)
        {
            InitializeComponent();
            m_vm = new ViewModels.PTurningMooreVM(refID, update);
            DataContext = m_vm;

            if (update)
                cb_process.IsEnabled = false;
           
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void lb_workpiece_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ObjectWindows.AddWorkpiece tt = new ObjectWindows.AddWorkpiece(m_vm.Workpiece.ID);
            tt.g_content.IsEnabled = false;
            tt.Title = m_vm.Workpiece.Label;
            tt.ShowDialog();
        }
    }
}
