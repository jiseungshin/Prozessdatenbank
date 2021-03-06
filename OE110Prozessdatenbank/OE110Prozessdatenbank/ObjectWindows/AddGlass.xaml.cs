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

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für AddGlass.xaml
    /// </summary>
    public partial class AddGlass : Window
    {
        ViewModels.OGlassVM m_vm;
        public AddGlass()
        {
            InitializeComponent();
            m_vm = new ViewModels.OGlassVM();
            DataContext = m_vm;
        }

        public AddGlass(int GID)
        {
            InitializeComponent();
            m_vm = new ViewModels.OGlassVM(GID);
            DataContext = m_vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_params_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.GlassStructureWindow(m_vm).ShowDialog();
        }
    }
}
