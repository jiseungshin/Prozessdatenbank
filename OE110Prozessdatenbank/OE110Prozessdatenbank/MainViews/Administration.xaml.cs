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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PDCore.Manager;

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für Welcome.xaml
    /// </summary>
    public partial class Administration : UserControl
    {

        public Administration()
        {
            InitializeComponent();
            ObjectManager.Instance.update();

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

        private void mbt_getHistory_Click(object sender, RoutedEventArgs e)
        {
            new Controls.ReferencePicker().ShowDialog();
        }

        private void mbt_getCoatProcesses_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.CoatingSPAdministration().ShowDialog();
        }

        private void mbt_getGlasses_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.GlassAdministration().ShowDialog();
        }

        private void mbt_getWorkpieces_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.WorkpieceAdministration().ShowDialog();
        }

        private void mbt_getProjects_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.ProjectAdministration().ShowDialog();
        }

        private void mbt_getUser_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.UserAdministration().ShowDialog();
        }

    }

    
}
