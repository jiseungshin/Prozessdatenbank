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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OE110Prozessdatenbank.ProcessWindows
{
    /// <summary>
    /// Interaktionslogik für CCoatingCemecon.xaml
    /// </summary>
    public partial class CCoatingCemecon : Window
    {
        ViewModels.PCoatingCemeconVM m_vm;

        public CCoatingCemecon(int refID, bool update)
        {
            InitializeComponent();
            m_vm = new ViewModels.PCoatingCemeconVM(refID, update);
            DataContext = m_vm;
            if (update)
                cb_takeProcessData.IsEnabled = false;
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Input.IntegerRegex.IsMatch((sender as TextBox).Text + e.Text);
        }
    }
}
