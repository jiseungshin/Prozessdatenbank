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

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für AddAnalysis.xaml
    /// </summary>
    public partial class AddAnalysis : Window
    {
        public AddAnalysis(int RefID, bool update)
        {
            InitializeComponent();
            DataContext = new ViewModels.PAnalysisVM(RefID);
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
