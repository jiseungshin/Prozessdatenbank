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

        ViewModels.PAnalysisVM m_analysisVM;
        public AddAnalysis(int RefID)
        {
            InitializeComponent();
            m_analysisVM = new ViewModels.PAnalysisVM(RefID);
            DataContext = m_analysisVM;
        }

        public AddAnalysis(PDCore.Processes.Analysis Analysis)
        {
            InitializeComponent();
            m_analysisVM = new ViewModels.PAnalysisVM(Analysis);
            DataContext = m_analysisVM;

            cb_analysis.IsEnabled = false;
            //cb_user.IsEnabled = false;
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
