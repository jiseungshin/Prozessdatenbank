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
    /// Interaktionslogik für UserAdministration.xaml
    /// </summary>
    public partial class UserAdministration : Window
    {
        public UserAdministration()
        {
            InitializeComponent();
            DataContext = new VMUsreAdministration();
        }

        private void LV_Users_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new AddUser(((sender as ListView).SelectedItem as User).ID).ShowDialog();
            }
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            new AddUser().ShowDialog();
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

    public class VMUsreAdministration : ViewModels.BaseViewModel
    {

        public VMUsreAdministration()
        {
            //ObjectManager.Instance.update();
            ObjectManager.Instance.newObjects += Instance_newObjects;
        }

        public ObservableCollection<User> Users
        {
            get { return new ObservableCollection<User>(ObjectManager.Instance.Users); }
        }

        void Instance_newObjects()
        {
            //ObjectManager.Instance.update();
            NotifyPropertyChanged("Users");
        }
    }
}
