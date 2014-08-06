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

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für Welcome.xaml
    /// </summary>
    public partial class Welcome : UserControl
    {
        ILogin login;
        public Welcome(ILogin control)
        {
            InitializeComponent();
            ObjectManager.Instance.update(PDCore.Database.DBUser.Table);
            cb_user.DataContext = ObjectManager.Instance.Users.FindAll(item => item.isActive).OrderBy(item=>item.LastName);
            //MessageBox.Show(Environment.UserName);
            cb_user.SelectedItem = ObjectManager.Instance.Users.Find(item => item.Token.ToUpper() == Environment.UserName.ToUpper());

            login = control;
        }

        private void bt_NoLogin_Click(object sender, RoutedEventArgs e)
        {
            cb_user.Visibility = System.Windows.Visibility.Hidden;
            bt_login.Visibility = System.Windows.Visibility.Hidden;
            bt_NoLogin.Visibility = System.Windows.Visibility.Hidden;
            PDCore.Manager.UserManager.CurrentUser = null;
            login.showSelector();
        }

        private void bt_login_Click(object sender, RoutedEventArgs e)
        {
            PDCore.Manager.UserManager.CurrentUser = cb_user.SelectedItem as PDCore.BusinessObjects.User;
            login.showSelector();
        }
    }

    
}
