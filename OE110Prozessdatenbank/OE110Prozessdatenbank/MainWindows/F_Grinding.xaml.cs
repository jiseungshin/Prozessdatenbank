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
    /// Interaktionslogik für F_Grinding.xaml
    /// </summary>
    public partial class F_Grinding : Window
    {
        public F_Grinding()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Add(new Controls.CTurningMoore(9));
        }
    }
}
