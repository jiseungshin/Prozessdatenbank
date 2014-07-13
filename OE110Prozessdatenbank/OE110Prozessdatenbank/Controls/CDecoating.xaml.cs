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
    /// Interaktionslogik für CDecoating.xaml
    /// </summary>
    public partial class CDecoating : Window
    {
        public CDecoating(int RefID)
        {
            InitializeComponent();
            DataContext = new ViewModels.PDeCoatingCemeconVM(RefID);
        }

        private void bt_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewDecimalTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Input.DecimalRegex.IsMatch((sender as TextBox).Text + e.Text);
        }
    }
}
