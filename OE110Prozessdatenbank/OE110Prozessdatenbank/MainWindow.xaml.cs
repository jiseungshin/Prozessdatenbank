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
using PDCore.BusinessObjects;
using PDCore.Processes;

namespace OE110Prozessdatenbank
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brush m_selectedColor;
        Brush m_hoverColor;

        private UIElement m_grindingControl;
        private UIElement m_coatingControl;
        private UIElement m_processingControl;
        private UIElement m_analysingControl;


        public MainWindow()
        {
            InitializeComponent();
            //#FF5EB45A
            var converter = new System.Windows.Media.BrushConverter();
            m_selectedColor = (Brush)converter.ConvertFromString("#FF509E5E");
            m_hoverColor = (Brush)converter.ConvertFromString("#FF5EB45A");

            m_grindingControl = new MainWindows.F_Grinding();
            m_coatingControl = new MainWindows.F_Coating();
            m_processingControl = new Controls.CProMoore();

            this.Closing += MainWindow_Closing;

            g_content.Children.Add(m_grindingControl);
            lb_1.Background = m_selectedColor;
            lb_1.Foreground = Brushes.WhiteSmoke;
            m_selectedLabel = lb_1;
            
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           Environment.Exit(0);
        }


        private void lb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lb_1.Background = Brushes.Transparent;
            lb_2.Background = Brushes.Transparent;
            lb_3.Background = Brushes.Transparent;
            lb_4.Background = Brushes.Transparent;

            lb_1.Foreground = Brushes.Black;
            lb_2.Foreground = Brushes.Black;
            lb_3.Foreground = Brushes.Black;
            lb_4.Foreground = Brushes.Black;

            m_selectedLabel = (sender as Label);

            (sender as Label).Background = m_selectedColor;
            (sender as Label).Foreground = Brushes.WhiteSmoke;
            
            switch((sender as Label).Name)
            {
                case "lb_1":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_grindingControl);
                    break;
                case "lb_2":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_coatingControl);
                    break;
                case "lb_3":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControl);
                    break;
                case "lb_4":
                    break;
                default:
                    break;

            }

        }

        private Brush m_1;
        private Brush m_2;
        private Label m_selectedLabel;

        private void lb_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = m_hoverColor;
            (sender as Label).Foreground = Brushes.WhiteSmoke;

            m_selectedLabel.Background = m_selectedColor;
            m_selectedLabel.Foreground = Brushes.WhiteSmoke;
        }

        private void lb_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = Brushes.Transparent;
            (sender as Label).Foreground = Brushes.Black;

            m_selectedLabel.Background = m_selectedColor;
            m_selectedLabel.Foreground = Brushes.WhiteSmoke;
        }


        


    }
}
