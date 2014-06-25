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
using System.Diagnostics;


namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CAnalyses.xaml
    /// </summary>
    public partial class CAnalyses : Window
    {
        int m_RefID = -1;
        ViewModels.PAnalysesVM m_vm;
        public CAnalyses(int ID)
        {
            InitializeComponent();
            m_RefID = ID;
            m_vm = new ViewModels.PAnalysesVM(ID);
            DataContext = m_vm;
        }

        private void LV_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                new AddAnalysis(((sender as ListView).SelectedItem as PDCore.Processes.Analysis)).Show();
            }
        }

        private void bt_openExplorer_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe",(sender as Button).Uid);
        }

        private void bt_add_Click(object sender, RoutedEventArgs e)
        {
            new AddAnalysis(m_RefID).ShowDialog();
        }
    }
}
