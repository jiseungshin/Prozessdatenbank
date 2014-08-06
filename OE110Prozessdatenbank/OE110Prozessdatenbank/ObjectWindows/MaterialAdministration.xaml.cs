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
using System.Collections.ObjectModel;
using PDCore.BusinessObjects;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für MaterialAdministration.xaml
    /// </summary>
    public partial class MaterialAdministration : Window
    {
        public MaterialAdministration()
        {
            InitializeComponent();
            DataContext = new VMMaterialAdministration();
        }

        private void mbt_addMaterial_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
        }

        private void LV_Materials_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new Controls.MaterialWindow(((sender as ListView).SelectedItem as Material).ID).ShowDialog();
            }
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

        private void LV_Workpiece_KeyDown(object sender, KeyEventArgs e)
        {
            if (LV_Workpiece.SelectedIndex!=-1)
            {
                if (e.Key == Key.Delete)
                {
                    if (MessageBox.Show(Properties.Messages.q_removeMaterial,"",MessageBoxButton.YesNo,MessageBoxImage.Warning)== MessageBoxResult.Yes)
                    {
                        ObjectManager.Instance.Remove(LV_Workpiece.SelectedItem as Material);
                        Updater.Instance.forceUpdate(PDCore.Database.DBMAterial.Table);
                    }
                }
            }
        }
    }

    public class VMMaterialAdministration : ViewModels.BaseViewModel
    {

        public VMMaterialAdministration()
        {
            ObjectManager.Instance.update(PDCore.Database.DBMAterial.Table);
            ObjectManager.Instance.newObjects += Instance_newObjects;
        }

        public ObservableCollection<Material> Materials
        {
            get { return new ObservableCollection<Material>(ObjectManager.Instance.Materials); }
        }

        void Instance_newObjects()
        {
            //ObjectManager.Instance.update();
            NotifyPropertyChanged("Materials");
        }
    }
}
