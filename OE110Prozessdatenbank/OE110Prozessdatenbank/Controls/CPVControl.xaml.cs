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

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für CPVControl.xaml
    /// </summary>
    public partial class CPVControl : UserControl
    {
        private VMProcessPressQuality m_vm;
        public CPVControl()
        {
            InitializeComponent();
            m_vm = new VMProcessPressQuality();
            DataContext = m_vm;
        }

        private void rb_g_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_vm.quali = 0;
            m_vm.NotifyPropertyChanged("BGood");
            m_vm.NotifyPropertyChanged("BOkay");
            m_vm.NotifyPropertyChanged("BBad");
        }

        private void rb_o_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_vm.quali = 1;
            m_vm.NotifyPropertyChanged("BGood");
            m_vm.NotifyPropertyChanged("BOkay");
            m_vm.NotifyPropertyChanged("BBad");
        }

        private void rb_b_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_vm.quali = 2;
            m_vm.NotifyPropertyChanged("BGood");
            m_vm.NotifyPropertyChanged("BOkay");
            m_vm.NotifyPropertyChanged("BBad");
        }

        public Grid C
        { get { return g_content; } }
    }

    public class VMProcessPressQuality :ViewModels.BaseViewModel
    {
        public int quali = 0;
        public Brush BGood
        {
            get
            {
                if (quali == 0)
                    return Brushes.Green;
                else
                    return Brushes.Transparent;

            }
        }

        public Brush BOkay
        {
            get
            {
                if (quali == 1)
                    return Brushes.Orange;
                else
                    return Brushes.Transparent;

            }
        }

        public Brush BBad
        {
            get
            {
                if (quali == 2)
                    return Brushes.Red;
                else
                    return Brushes.Transparent;

            }
        }
    }


}
