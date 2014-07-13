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

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für HistoryViewer.xaml
    /// </summary>
    public partial class HistoryViewer : Window
    {
        ViewModels.OReferenceVM m_vm;
        int refID;
        public HistoryViewer(int RefID)
        {
            InitializeComponent();
            m_vm = new ViewModels.OReferenceVM(RefID);
            DataContext = m_vm;
            refID = RefID;
        }

        private void LV_History_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex !=-1)
            {
                PDCore.BusinessObjects.ProcessMetaData md = (sender as ListView).SelectedItem as PDCore.BusinessObjects.ProcessMetaData;

                switch(md.Machine.ID)
                {
                    case 11:
                        new ProcessWindows.CTurningMoore(md.PID, true).ShowDialog();
                        break;
                    case 12:
                        new ProcessWindows.CGrindingMoore(md.PID, true).ShowDialog();
                        break;
                    case 13:
                        new ProcessWindows.CGrindingPhoenix(md.PID, true).ShowDialog();
                        break;
                    case 14:
                        new ProcessWindows.CGrindingOther(md.PID, true).ShowDialog();
                        break;
                    case 21:
                        new ProcessWindows.CCoatingCemecon(md.PID, true).ShowDialog();
                        break;
                    case 31:
                        new ProcessWindows.CExpMoore(md.PID).ShowDialog();
                        break;
                    case 32:
                        new ProcessWindows.CExpTestStation(md.PID).ShowDialog();
                        break;
                    case 33:
                        new ProcessWindows.CExpCemeCon(md.PID,true).ShowDialog();
                        break;
                    case 40:
                        new Controls.CAnalyses(m_vm.ReferenceNumber).ShowDialog();
                        break;
                    case 51:
                        new Controls.CDecoating(m_vm.ReferenceNumber).ShowDialog();
                        break;

                }

            }
        }

        private void bt_cancelProcess_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
