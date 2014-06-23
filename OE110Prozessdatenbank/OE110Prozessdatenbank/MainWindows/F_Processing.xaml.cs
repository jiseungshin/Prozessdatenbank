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

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für F_Processing.xaml
    /// </summary>
    public partial class F_Processing : Window
    {
        public F_Processing(int MachineID)
        {
            InitializeComponent();

            switch (MachineID)
            { 
                case 6:
                    this.g_content.Children.Add(new Controls.CProMoore());
                    break;
            }
        }

        

        private void mi_AddMaterial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_AddWorkpiece_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mi_WorkpieceAdmin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
