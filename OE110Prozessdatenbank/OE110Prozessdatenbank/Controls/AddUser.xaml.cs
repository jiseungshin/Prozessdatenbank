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
    /// Interaktionslogik für AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
            DataContext = new ViewModels.OUserVM();
        }

        public AddUser(int UID)
        {
            InitializeComponent();
            DataContext = new ViewModels.OUserVM(UID);
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
