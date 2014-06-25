using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.ComponentModel;
using PDCore.Processes;

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für ToshibaImportProgress.xaml
    /// </summary>
    public partial class ToshibaImportProgress : Window
    {
        ViewModels.PToshibaImportVM m_vm;
        public ToshibaImportProgress(ViewModels.PToshibaImportVM vm)
        {
            InitializeComponent();
             m_vm =vm;
            m_bw = new BackgroundWorker();
            m_bw.WorkerReportsProgress = true;
            m_bw.DoWork += m_bw_DoWork;
            m_bw.ProgressChanged += m_bw_ProgressChanged;
            this.Topmost = true;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        int pathCount;
        double step;

        void m_bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lb_process.Content = "Import: " + e.UserState.ToString();
            pb_process.Value += e.ProgressPercentage;
            this.Topmost = false;
        }

        void m_bw_DoWork(object sender, DoWorkEventArgs e)
        {

            List<string> paths = e.Argument as List<string>;


            foreach (var path in paths)
                {
                    var file = PDCore.ToshibaImport.IO.getMonFileData(path);
                    PToshiba process = PDCore.ToshibaImport.Analytics.AnalyseProcess(file);
                    process.File = file;
                    //m_vm.addProcess(process);

                    //m_bw.ReportProgress(Convert.ToInt32(step), path);
            }

            //if (MessageBox.Show("Import abgeschlossen! Die Prozessdaten sind nun im Frontend verfügbar", "Datenbankimport", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
            //{
            //    this.Close();
            //}
        }

        private BackgroundWorker m_bw;

        public void startImport(List<string> path)
        {
            pathCount = path.Count;
            step = 100 / pathCount;
            m_bw.RunWorkerAsync(path);
        }
    }
}
