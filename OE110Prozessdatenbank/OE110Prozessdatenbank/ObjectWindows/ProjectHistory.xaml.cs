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

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für ProjectHistory.xaml
    /// </summary>
    public partial class ProjectHistory : Window
    {
        public ProjectHistory()
        {
            InitializeComponent();
            DataContext = new ViewModels.ProjectHistoryVM();
        }

        private void LV_PHistory_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_PHistory.SelectedIndex!=-1)
            {

                new ObjectWindows.HistoryViewer((LV_PHistory.SelectedItem as PDCore.BusinessObjects.WorkpieceHistory).ReferenceNumber).ShowDialog();

            }
        }
    }
}
