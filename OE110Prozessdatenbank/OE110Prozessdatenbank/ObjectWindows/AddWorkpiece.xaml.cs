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
    /// Interaktionslogik für AddWorkpiece.xaml
    /// </summary>
    public partial class AddWorkpiece : Window
    {
        public AddWorkpiece()
        {
            InitializeComponent();
            DataContext = new ViewModels.OWorkpieceVM();
        }

        public AddWorkpiece(int ID)
        {
            InitializeComponent();
            DataContext = new ViewModels.OWorkpieceVM(ID);
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //e.Handled = !Input.CharacterRegex.IsMatch((sender as TextBox).Text + e.Text);
        }
    }
}
