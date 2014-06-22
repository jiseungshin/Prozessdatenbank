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
using PDCore.Processes;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CQuality.xaml
    /// </summary>
    public partial class CProcessQuality : UserControl
    {
        public CProcessQuality(BaseProcess process)
        {
            InitializeComponent();
            DataContext = new ViewModels.ProcessQualityVM(process);
        }
    }
}
