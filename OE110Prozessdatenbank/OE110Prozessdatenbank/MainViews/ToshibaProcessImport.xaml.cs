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
using PDCore.Manager;
using PDCore.Processes;
using System.Diagnostics;

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für ToshibaProcessImport.xaml
    /// </summary>
    public partial class ToshibaProcessImport : Window
    {
        ViewModels.PToshibaImportVM m_vm;
        public ToshibaProcessImport()
        {
            InitializeComponent();
            m_vm = new ViewModels.PToshibaImportVM();
            DataContext = m_vm;
            
        }

        public ToshibaProcessImport(ViewModels.PToshibaImportVM vm)
        {
            InitializeComponent();
            m_vm = vm;
            DataContext = m_vm;

        }

        private void bt_import_Click(object sender, RoutedEventArgs e)
        {
           
            System.Windows.Forms.OpenFileDialog _ofd = new System.Windows.Forms.OpenFileDialog();

            _ofd.Filter = "mon files (*.mon)|*.mon|Alle Deteien (*.*)|*.*";
            _ofd.FilterIndex = 1;
            _ofd.RestoreDirectory = true;
            _ofd.Multiselect = true;


            System.Windows.Forms.DialogResult result = _ofd.ShowDialog();
            this.UpdateLayout();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.Wait;
                int id = 1;
                foreach (var path in _ofd.FileNames)
                {

                    var file = PDCore.ToshibaImport.IO.getMonFileData(path);
                    PToshiba process = PDCore.ToshibaImport.Analytics.AnalyseProcess(file);
                    process.File = file;

                    m_vm.addProcess(process, id);

                    id++;
                    
                }

                Cursor = Cursors.Arrow;
               

            }
        }

        private void loadData(string[] data)
        {

        }
        ProcessWindows.ToshibaProcessEditor m_processWindow;
        private void LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                try
                {
                    int index = (sender as ListView).SelectedIndex;
                    new ProcessWindows.ToshibaProcessEditor(m_vm, index).ShowDialog();

                }
                catch { }

            }
        }

        private void bt_openProcessParams_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void bt_openProcessParams_Click(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void bt_importToDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Messages.q_importToshiba, "Hinweis", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                List<PToshiba> processes = new List<PToshiba>();
                foreach (var pvm in m_vm.Processes)
                {
                    processes.Add(pvm.Process);
                }

                PDCore.Manager.ProcessManager.Instance.importToshibaProcesses(processes);
                this.Close();
            }
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LV_Processes_KeyDown(object sender, KeyEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                if (e.Key == Key.Delete)
                {
                    if (MessageBox.Show("Das Entfernen von Elementen ist zurzeit aus technischen Gründen nicht möglich","Hinweis",MessageBoxButton.OK,MessageBoxImage.Warning)== MessageBoxResult.Yes)
                    {
                        //m_vm.RemoveProcess((sender as ListView).SelectedIndex);
                    }
                }

            }
        }
    }
}
