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
using System.Data;
using PDCore.Database;

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für F_Grinding.xaml
    /// </summary>
    public partial class MV_Grinding : UserControl
    {
        ViewModels.F_GrindingVM m_vm;
        ProcessWindows.GenericWindow gw;
        public MV_Grinding()
        {
            InitializeComponent();
            m_vm = new ViewModels.F_GrindingVM();
            DataContext = m_vm;

            gw = new ProcessWindows.GenericWindow();
        }

        public void setMachineID(int MID)
        { m_vm.setMachine(MID); }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
            
        }

        private void MaterialListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Polished.SelectedIndex != -1)
            {
                int ID =-1; 

                switch (m_vm.Machine.ID)
                {
                    case 11:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBTurningMoore.ID]);
                        new ProcessWindows.CTurningMoore(ID, true).ShowDialog();
                        break;
                    case 12:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingMoore.ID]);
                        new ProcessWindows.CGrindingMoore(ID, true).ShowDialog();
                        break;
                    case 13:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingPhoenix.ID]);
                        new ProcessWindows.CGrindingPhoenix(ID, true).ShowDialog();
                        break;
                    case 14:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingOther.ID]);
                        new ProcessWindows.CGrindingOther(ID, true).ShowDialog();
                        break;

                }
              
            }

            
        }

        private void LV_Raw_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Raw.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32((LV_Raw.SelectedItem as System.Data.DataRowView)[DBWorkpieces.ID]);

                switch (m_vm.Machine.ID)
                { 
                    case 11:
                        new ProcessWindows.CTurningMoore(ID, false).ShowDialog();
                        break;
                    case 12:
                        new ProcessWindows.CGrindingMoore(ID, false).ShowDialog();
                        break;
                    case 13:
                        new ProcessWindows.CGrindingPhoenix(ID, false).ShowDialog();
                        break;
                    case 14:
                        new ProcessWindows.CGrindingOther(ID, false).ShowDialog();
                        break;
                }
            }
        }

        private void Button_WP_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        private void mi_WorkpieceAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.WorkpieceAdministration().ShowDialog();
        }

        private void mi_AddWorkpiece_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        private void mi_AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }


    }
}
