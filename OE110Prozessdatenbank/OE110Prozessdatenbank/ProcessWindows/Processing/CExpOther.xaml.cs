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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OE110Prozessdatenbank.ProcessWindows
{
    /// <summary>
    /// Interaktionslogik für CExpOther.xaml
    /// </summary>
    public partial class CExpOther : Window
    {
        public CExpOther(int ID, bool update)
        {
            InitializeComponent();
            DataContext = new ViewModels.PExpOtherVM(ID, update);
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
