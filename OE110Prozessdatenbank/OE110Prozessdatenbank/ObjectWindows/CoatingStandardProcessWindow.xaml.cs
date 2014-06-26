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
using PDCore.Processes;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für CoatingStandardProcessWindow.xaml
    /// </summary>
    public partial class CoatingStandardProcessWindow : Window
    {
        public CoatingStandardProcessWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.PCemeconStandarProcessVM();
        }

        public CoatingStandardProcessWindow(PCoatingCemeconProcess process)
        {
            InitializeComponent();
            DataContext = new ViewModels.PCemeconStandarProcessVM(process);
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
