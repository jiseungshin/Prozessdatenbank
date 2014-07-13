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
            cb_WPPicker.Visibility = System.Windows.Visibility.Hidden;
            cb_RefPicker.DataContext = ProcessManager.Instance.getData("SELECT * FROM " + DBProcessReferences.Table);
            cb_WPPicker.DataContext = ProcessManager.Instance.getData("SELECT * FROM " + DBWorkpieces.Table);
        }

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rb_reference.IsChecked)
            {
                if ((cb_RefPicker.SelectedItem as System.Data.DataRowView) != null)
                {
                    new ObjectWindows.HistoryViewer((cb_RefPicker.SelectedItem as System.Data.DataRowView).Row.Field<int>(DBProcessReferences.RefNumber)).Show();
                    this.Close();
                }

                else
                { MessageBox.Show(Properties.Messages.e_UnknownReference, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
            else
            {
                if ((cb_WPPicker.SelectedItem as System.Data.DataRowView) != null)
                {
                    int ID = (cb_WPPicker.SelectedItem as System.Data.DataRowView).Row.Field<int>(DBWorkpieces.ID);
                    DataSet ds = PDCore.Manager.ProcessManager.Instance.getData("SELECT * FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.WorkpiceID + "=" +
                    ID + " AND NOT (" + DBProcessReferences.Status + "=" + DBEnum.EnumReference.TERMINATED.ToDBObject() + " OR " +
                    DBProcessReferences.Status + "=" + DBEnum.EnumReference.CANCELLED.ToDBObject() + " OR " +
                    DBProcessReferences.Status + "=" + DBEnum.EnumReference.RAW.ToDBObject() + ")");

                    if (ds.Tables[0].Rows.Count>0)
                    {
                        int reference = ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.RefNumber);
                        new ObjectWindows.HistoryViewer(reference).Show();
                        this.Close();
                    }

                    else
                        MessageBox.Show(Properties.Messages.n_WorkpieceNotInUse, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    
                    
                }

                else
                { MessageBox.Show(Properties.Messages.e_UnknownReference, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        private void rb_reference_Click(object sender, RoutedEventArgs e)
        {
            cb_WPPicker.Visibility = System.Windows.Visibility.Hidden;
            cb_RefPicker.Visibility = System.Windows.Visibility.Visible;
        }

        private void rb_workpiece_Click(object sender, RoutedEventArgs e)
        {
            cb_WPPicker.Visibility = System.Windows.Visibility.Visible;
            cb_RefPicker.Visibility = System.Windows.Visibility.Hidden;
            
        }
    }
}
