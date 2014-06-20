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

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für F_Coating.xaml
    /// </summary>
    public partial class F_Coating : Window
    {
        ViewModels.F_CoatingVM m_vm;
        ProcessWindows.GenericWindow gw;
        public F_Coating()
        {
            InitializeComponent();
            DataContext = new ViewModels.F_CoatingVM();
        }

        private void LV_Raw_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                gw = new ProcessWindows.GenericWindow();
                gw.contentGrid.Children.Clear();
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);


                gw.contentGrid.Children.Add(new Controls.CCoatingCemecon(ID, false));
                gw.Title = "Prozessdaten CemeCon";

                gw.ShowDialog();

            }
        }

        private void MaterialListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                gw = new ProcessWindows.GenericWindow();
                gw.contentGrid.Children.Clear();
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBCoatingCemecon.ID]);


                gw.contentGrid.Children.Add(new Controls.CCoatingCemecon(ID, true));
                gw.Title = "Prozessdaten CemeCon";

                gw.ShowDialog();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Controls.CoatingStandardProcessWindow().ShowDialog();
        }
    }
}
