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
    /// Interaktionslogik für CGrindingMoore.xaml
    /// </summary>
    public partial class CGrindingMoore : Window
    {
        ViewModels.PGrindingMooreVM m_vm;
        public CGrindingMoore(int refID, bool update)
        {
            InitializeComponent();
            InitializeComponent();
            m_vm = new ViewModels.PGrindingMooreVM(refID, update);
            DataContext = m_vm;

            if (update)
                cb_process.IsEnabled = false;
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
            e.Handled = !Input.DecimalRegex.IsMatch((sender as TextBox).Text + e.Text);
        }
    }
}
