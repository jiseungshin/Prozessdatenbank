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
using PDCore.Database;

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für F_Coating.xaml
    /// </summary>
    public partial class MV_Coating : UserControl
    {
        public MV_Coating()
        {
            InitializeComponent();
            DataContext = new ViewModels.F_CoatingVM();
        }

        private void LV_Raw_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);
                new ProcessWindows.CCoatingCemecon(ID, false).ShowDialog();

            }
        }

        private void MaterialListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBCoatingCemecon.ID]);
                new ProcessWindows.CCoatingCemecon(ID, true).ShowDialog();
            }
        }

        private void mbt_addcoatingProcess_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.CoatingStandardProcessWindow().ShowDialog();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        
    }
}
