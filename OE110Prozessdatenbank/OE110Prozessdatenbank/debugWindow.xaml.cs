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

namespace OE110Prozessdatenbank
{
    /// <summary>
    /// Interaktionslogik für debugWindow.xaml
    /// </summary>
    public partial class debugWindow : Window
    {
        public debugWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PDCore.Manager.ProcessManager.Instance.ImportToshibaMData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PDCore.Manager.ProcessManager.Instance.insertReferences();
        }
    }
}
