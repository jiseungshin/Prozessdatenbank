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

namespace OE110Prozessdatenbank.Controls
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
            new Controls.AddGlass().ShowDialog();
        }

        private void LV_Glass_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new Controls.AddGlass(((sender as ListView).SelectedItem as Glass).ID).ShowDialog();
            }
        }
    }

    public class VMGlassAdministration : ViewModels.BaseViewModel
    {

        public VMGlassAdministration()
        {
            ObjectManager.Instance.update();
            Updater.Instance.newData += Instance_newData;
        }

        public ObservableCollection<Glass> Glasses
        {
            get { return new ObservableCollection<Glass>(ObjectManager.Instance.Glasses); }
        }

        void Instance_newData()
        {
            ObjectManager.Instance.update();
            NotifyPropertyChanged("Glasses");
        }
    }

}
