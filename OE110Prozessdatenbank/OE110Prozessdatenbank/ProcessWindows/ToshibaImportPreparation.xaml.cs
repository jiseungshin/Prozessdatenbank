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
using PDCore.Processes;

namespace OE110Prozessdatenbank.ProcessWindows
{
    /// <summary>
    /// Interaktionslogik für ToshibaImportPreparation.xaml
    /// </summary>
    public partial class ToshibaImportPreparation : Window
    {
        ViewModels.PToshibaImportVM m_vm;
        public ToshibaImportPreparation()
        {
            InitializeComponent();
            m_vm = new ViewModels.PToshibaImportVM();
            DataContext = m_vm;
        }

        private void bt_imoport_Click(object sender, RoutedEventArgs e)
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

                new MainWindows.ToshibaProcessImport(m_vm).Show();
                this.Close();



            }
        }
    }
}
