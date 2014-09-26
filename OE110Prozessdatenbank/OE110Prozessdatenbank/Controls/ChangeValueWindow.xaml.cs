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

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für ChangeValueWindow.xaml
    /// </summary>
    public partial class ChangeValueWindow : Window
    {
        ViewModels.BaseViewModel m_vm;
        public ChangeValueWindow(ViewModels.BaseViewModel vm, string variable)
        {
            InitializeComponent();
            m_vm = vm;
            this.Title = "Neuen Wert eingeben";
            ViewModels.PToshibaVM vvv = m_vm as ViewModels.PToshibaVM;
            Val = vvv.LensName;

            DataContext = this;
        }

        string m_value;

        public string Val
        { get { return m_value; } set { m_value = value; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            ViewModels.PToshibaVM vvv = m_vm as ViewModels.PToshibaVM;

            vvv.LensName = Val;
            vvv.NotifyPropertyChanged("LensName");
            this.Close();


            
        }
    }
}
