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
using PDCore.Database;
using System.Data;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für ReferencePicker.xaml
    /// </summary>
    public partial class ReferencePicker : Window
    {
        public ReferencePicker()
        {
            InitializeComponent();
            cb_RefPicker.DataContext = ProcessManager.Instance.getData("SELECT * FROM " + DBProcessReferences.Table);
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            if ((cb_RefPicker.SelectedItem as System.Data.DataRowView)!=null)
            {
                new ObjectWindows.HistoryViewer((cb_RefPicker.SelectedItem as System.Data.DataRowView).Row.Field<int>(DBProcessReferences.RefNumber)).Show();
                this.Close();
            }

            else
            { MessageBox.Show("Die eingegebene Vorgangsnummer ist nicht vorhanden!","Hinweis",MessageBoxButton.OK,MessageBoxImage.Error); }
        }
    }
}
