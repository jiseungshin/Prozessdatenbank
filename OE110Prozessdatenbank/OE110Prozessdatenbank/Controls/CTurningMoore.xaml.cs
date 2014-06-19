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

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CTurningMoore.xaml
    /// </summary>
    public partial class CTurningMoore : UserControl
    {

        ViewModels.PTurningMooreVM m_vm;
        public CTurningMoore(int refID)
        {
            InitializeComponent();
            m_vm = new ViewModels.PTurningMooreVM(refID);
            DataContext = m_vm;
        }
    }
}
