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

        private void bt_import_Click(object sender, RoutedEventArgs e)
        {
           
            System.Windows.Forms.OpenFileDialog _ofd = new System.Windows.Forms.OpenFileDialog();

            _ofd.Filter = "mon files (*.mon)|*.mon|Alle Deteien (*.*)|*.*";
            _ofd.FilterIndex = 1;
            _ofd.RestoreDirectory = true;
            _ofd.Multiselect = true;


            System.Windows.Forms.DialogResult result = _ofd.ShowDialog();
            Cursor = Cursors.Wait;
            this.UpdateLayout();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.Wait;
               // ToshibaImportProgress io = new ToshibaImportProgress(m_vm);
                int id = 1;
                foreach (var path in _ofd.FileNames)
                {

                    var file = PDCore.ToshibaImport.IO.getMonFileData(path);
                    PToshiba process = PDCore.ToshibaImport.Analytics.AnalyseProcess(file);
                    process.File = file;

                    m_vm.addProcess(process, id);

                    id++;
                    
                }

                //io.Show();
                //io.startImport(_ofd.FileNames.ToList());

           // new Controls.CPVControl().ShowDialog();
                Cursor = Cursors.Arrow;
               

            }
        }

        private void loadData(string[] data)
        {

        }

        private void LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex!=-1)
            {
                int index = (sender as ListView).SelectedIndex;
                new Controls.ToshibaProcessEditor(m_vm, index).Show();

            }
        }

        private void bt_openProcessParams_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void bt_openProcessParams_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((sender as Label).Parent.ToString()); ;
        }
    }
}
