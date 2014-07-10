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
using OE110Prozessdatenbank.ViewModels;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CConclusion.xaml
    /// </summary>
    public partial class CConclusion : Window
    {
        public CConclusion(int RefID)
        {
            InitializeComponent();
            DataContext = new OReferenceVM(RefID);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_Save_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
