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
    /// Interaktionslogik für WorkpieceAdministration.xaml
    /// </summary>
    public partial class WorkpieceAdministration : Window
    {
        public WorkpieceAdministration()
        {
            InitializeComponent();
            DataContext = new VMWorkpieceAdministration();
        }

        private void LV_Workpiece_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new ObjectWindows.AddWorkpiece(((sender as ListView).SelectedItem as Workpiece).ID).ShowDialog();
            }
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }
    }

    public class VMWorkpieceAdministration : ViewModels.BaseViewModel
    {

        public VMWorkpieceAdministration()
        {
            ObjectManager.Instance.update();
            Updater.Instance.newData += Instance_newData;
        }

        public ObservableCollection<Workpiece> Workpieces
        {
            get { return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces); }
        }

        void Instance_newData()
        {
            ObjectManager.Instance.update();
            NotifyPropertyChanged("Workpieces");
        }
    }
}
