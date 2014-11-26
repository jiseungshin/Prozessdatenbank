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

namespace Fixes
{
    /// <summary>
    /// Interaktionslogik für FixWindow.xaml
    /// </summary>
    public partial class FixWindow : Window
    {
        ProcessFixes m_processFixes;

        public FixWindow()
        {
            InitializeComponent();
            g_action.IsEnabled = false;
            m_processFixes = new ProcessFixes();
        }

        private void bt_continue_Click(object sender, RoutedEventArgs e)
        {
            g_action.IsEnabled = true;
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_machineData_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Befehl wirklich ausführen?","Kritische Aktion",MessageBoxButton.YesNo,MessageBoxImage.Warning)== MessageBoxResult.Yes)
                m_processFixes.ImportToshibaMData();
        }

        private void bt_references_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Befehl wirklich ausführen?", "Kritische Aktion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                m_processFixes.insertReferences();
        }
    }
}
