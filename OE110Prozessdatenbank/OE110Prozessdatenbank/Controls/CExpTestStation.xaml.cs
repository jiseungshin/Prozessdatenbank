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
    /// Interaktionslogik für CExpTestStation.xaml
    /// </summary>
    public partial class CExpTestStation : UserControl
    {
        ViewModels.PExpTestStationVM m_vm;
        public CExpTestStation(int ID, bool update)
        {
            InitializeComponent();
            m_vm = new ViewModels.PExpTestStationVM(ID, update);
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
    }
}
