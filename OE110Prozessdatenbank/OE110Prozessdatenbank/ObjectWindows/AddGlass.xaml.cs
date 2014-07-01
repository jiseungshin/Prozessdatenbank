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
    /// Interaktionslogik für AddGlass.xaml
    /// </summary>
    public partial class AddGlass : Window
    {
        public AddGlass()
        {
            InitializeComponent();
            DataContext = new ViewModels.OGlassVM();
        }

        public AddGlass(int GID)
        {
            InitializeComponent();
            DataContext = new ViewModels.OGlassVM(GID);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
