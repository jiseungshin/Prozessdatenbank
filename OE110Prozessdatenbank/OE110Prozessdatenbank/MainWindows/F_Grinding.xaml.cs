﻿using System;
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

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für F_Grinding.xaml
    /// </summary>
    public partial class F_Grinding : Window
    {
        ViewModels.F_GrindingVM m_vm;
        ProcessWindows.GenericWindow gw;
        public F_Grinding()
        {
            InitializeComponent();
            m_vm = new ViewModels.F_GrindingVM();
            DataContext = m_vm;

            gw = new ProcessWindows.GenericWindow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
            
        }

        private void MaterialListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Polished.SelectedIndex != -1)
            {
                gw = new ProcessWindows.GenericWindow();
                gw.contentGrid.Children.Clear();
                int ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);

                switch (m_vm.Machine.ID)
                {
                    case 1:
                        gw.contentGrid.Children.Add(new Controls.CTurningMoore(ID, true));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                    case 2:
                        gw.contentGrid.Children.Add(new Controls.CGrindingMoore(ID, true));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                    case 3:
                         gw.contentGrid.Children.Add(new Controls.CGrindingPhoenix(ID, true));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                    case 4:
                        gw.contentGrid.Children.Add(new Controls.CGrindingOther(ID, true));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;

                }

                gw.ShowDialog();
              
            }

            
        }

        private void LV_Raw_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Raw.SelectedIndex != -1)
            {

                gw = new ProcessWindows.GenericWindow();
                gw.contentGrid.Children.Clear();
                int ID = Convert.ToInt32((LV_Raw.SelectedItem as System.Data.DataRowView)[DBWorkpieces.ID]);

                switch (m_vm.Machine.ID)
                { 
                    case 1:
                        gw.contentGrid.Children.Add(new Controls.CTurningMoore(ID, false));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                    case 2:
                        gw.contentGrid.Children.Add(new Controls.CGrindingMoore(ID, false));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                    case 3:
                        gw.contentGrid.Children.Add(new Controls.CGrindingPhoenix(ID, false));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                    case 4:
                        gw.contentGrid.Children.Add(new Controls.CGrindingOther(ID, false));
                        gw.Title = "Prozessdaten " + m_vm.Machine.Name;
                        break;
                }

                gw.ShowDialog();
            }
        }

        private void Button_WP_Click(object sender, RoutedEventArgs e)
        {
            new Controls.AddWorkpiece().ShowDialog();
        }
    }
}
