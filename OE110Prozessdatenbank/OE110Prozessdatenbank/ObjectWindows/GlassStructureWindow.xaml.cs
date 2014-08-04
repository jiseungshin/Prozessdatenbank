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

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für GlassStructureWindow.xaml
    /// </summary>
    public partial class GlassStructureWindow : Window
    {
        OGlassVM m_vm;
        public GlassStructureWindow(OGlassVM vm)
        {
            InitializeComponent();
            m_vm = vm;
            DataContext = m_vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Properties.Messages.n_SaveGlassParameters, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
            this.Close();
        }
    }
}
