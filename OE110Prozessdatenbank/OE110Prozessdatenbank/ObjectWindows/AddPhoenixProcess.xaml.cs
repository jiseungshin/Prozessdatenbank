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
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für AddPhoenixProcess.xaml
    /// </summary>
    public partial class AddPhoenixProcess : Window
    {
        private OPheonixProcessVM m_vm;

        public AddPhoenixProcess()
        {
            InitializeComponent();
            m_vm = new OPheonixProcessVM();
            DataContext = m_vm;
        }

        public AddPhoenixProcess(PDCore.Processes.PGrindingPhoenixProcess process)
        {
            InitializeComponent();
            m_vm = new OPheonixProcessVM(process);
            DataContext = m_vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
