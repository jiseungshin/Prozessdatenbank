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
using PDCore.BusinessObjects;
using PDCore.Manager;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für GlassAdministration.xaml
    /// </summary>
    public partial class GlassAdministration : Window
    {
        public GlassAdministration()
        {
            InitializeComponent();
            DataContext = new VMGlassAdministration();
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddGlass().ShowDialog();
        }

        private void LV_Glass_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new AddGlass(((sender as ListView).SelectedItem as Glass).ID).ShowDialog();
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

    }

    public class VMGlassAdministration : ViewModels.BaseViewModel
    {

        private string m_filter = "";

        public VMGlassAdministration()
        {
            ObjectManager.Instance.update();
            ObjectManager.Instance.newObjects += Instance_newObjects;
        }

        public ObservableCollection<Glass> Glasses
        {
            get 
            { 
                if (m_filter=="")
                    return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses);
                else
                    return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses.Where(item => item.Comapany.ToLower().Contains(Filter.ToLower()) || item.Name.ToLower().Contains(Filter.ToLower())));
            }
        }

        void Instance_newObjects()
        {
            ObjectManager.Instance.update();
            NotifyPropertyChanged("Glasses");
        }

        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; NotifyPropertyChanged("Glasses"); }
        }
    }

}
