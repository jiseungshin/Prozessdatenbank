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
using System.Runtime.InteropServices;
using System.Windows.Interop;


namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für PostProcessingPicker.xaml
    /// </summary>
    public partial class PostProcessingPicker : Window
    {
        int ID;
        public PostProcessingPicker(int RefID)
        {

            AllowsTransparency = true;
            //Opacity = 0.7;
            ID = RefID;
            InitializeComponent();
            Title.Content = "Vorgang " + RefID;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            { this.Close(); }
        }

        private void bt_analyse_Click(object sender, RoutedEventArgs e)
        {
            new Controls.CAnalyses(ID).Show();
            this.Close();
        }

        private void bt_decoating_Click(object sender, RoutedEventArgs e)
        {
            new Controls.CDecoating(ID).Show();
            this.Close();
        }

        private void conclusion_Click(object sender, RoutedEventArgs e)
        {
            new Controls.CConclusion(ID).Show();
            this.Close();
        }
    }

    
}
