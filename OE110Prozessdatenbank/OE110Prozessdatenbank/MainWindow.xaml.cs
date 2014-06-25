﻿using System;
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
    public partial class MainWindow : Window, ILogin
    {
        Brush m_selectedColor;
        Brush m_hoverColor;

        private MainWindows.F_Grinding m_grindingControl;
        private UIElement m_coatingControl;
        private UIElement m_processingControlMoore;
        private UIElement m_processingControlTest;
        private UIElement m_analysingControl;
        private UIElement m_processingControlToshiba;


        public MainWindow()
        {
            InitializeComponent();
            lbLoginName.Content = "";
            userIcon.Visibility = System.Windows.Visibility.Hidden;
            //#FF5EB45A
            var converter = new System.Windows.Media.BrushConverter();
            m_selectedColor = (Brush)converter.ConvertFromString("#FF509E5E");
            m_hoverColor = (Brush)converter.ConvertFromString("#FF5EB45A");

            m_grindingControl = new MainWindows.F_Grinding();
            m_coatingControl = new MainWindows.F_Coating();
            m_processingControlMoore = new Controls.CProMoore();
            m_processingControlToshiba = new Controls.CProToshiba();
            m_processingControlTest = new Controls.CProTestStation();
            m_analysingControl = new MainWindows.F_PostProcessing();

            this.Closing += MainWindow_Closing;

            g_content.Children.Add(new MainWindows.Welcome(this));


            g_selector.Visibility = System.Windows.Visibility.Collapsed;
            
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           Environment.Exit(0);
        }

        public void showSelector()
        {
            g_selector.Visibility = System.Windows.Visibility.Visible;
            Cursor = Cursors.Wait;
            m_grindingControl = new MainWindows.F_Grinding();
            m_coatingControl = new MainWindows.F_Coating();
            m_processingControlTest = new Controls.CProTestStation();
            m_processingControlMoore = new Controls.CProMoore();
            m_analysingControl = new MainWindows.F_PostProcessing();
            m_processingControlToshiba = new Controls.CProToshiba();
            
            if (PDCore.Manager.UserManager.CurrentUser!=null)
            {
                lbLoginName.Content = PDCore.Manager.UserManager.CurrentUser.FirstName + " " + PDCore.Manager.UserManager.CurrentUser.LastName;
                userIcon.Visibility = System.Windows.Visibility.Visible;
                int id = Convert.ToInt32(PDCore.Manager.UserManager.CurrentUser.MachineID);
                switch(id)
                {
                    case 1:
                        this.changeWindow(lb_1, id);
                        break;
                    case 2:
                        this.changeWindow(lb_1, id);
                        break;
                    case 3:
                        this.changeWindow(lb_1, id);
                        break;
                    case 4:
                        this.changeWindow(lb_1, id);
                        break;
                    case 5:
                        this.changeWindow(lb_2, id);
                        break;
                    case 6:
                        this.changeWindow(lb_3, id);
                        break;
                    case 7:
                        this.changeWindow(lb_3, id);
                        break;
                    case 8:
                        this.changeWindow(lb_3, id);
                        break;
                    case 9:
                        this.changeWindow(lb_31, id);
                        break;
                    case 10:
                        this.changeWindow(lb_4, id);
                        break;
                    case 11:
                        this.changeWindow(lb_4, id);
                        break;

                }
            }
            else
            {
                lbLoginName.Content = "";
                userIcon.Visibility = System.Windows.Visibility.Hidden;
            }

            Cursor = Cursors.Arrow;

        }
        public void hideSelector()
        {
            g_selector.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void lb_MouseDown(object sender, MouseButtonEventArgs e)
        {

            changeWindow(sender as Label,0);
        }

        private void changeWindow(Label lb, int machine)
        {
            lb_1.Background = Brushes.Transparent;
            lb_2.Background = Brushes.Transparent;
            lb_3.Background = Brushes.Transparent;
            lb_4.Background = Brushes.Transparent;

            lb_1.Foreground = Brushes.Black;
            lb_2.Foreground = Brushes.Black;
            lb_3.Foreground = Brushes.Black;
            lb_4.Foreground = Brushes.Black;

            m_selectedLabel = lb;
            if (lb != null)
            {
                lb.Background = m_selectedColor;
                lb.Foreground = Brushes.WhiteSmoke;
            }
            maxiLeave();
            g_selector.Visibility = System.Windows.Visibility.Visible;
            switch (lb.Name)
            {
                case "lb_1":
                    g_content.Children.Clear();
                    m_grindingControl.setMachineID(machine);
                    g_content.Children.Add(m_grindingControl);
                    lb.Background = m_selectedColor;
                    lb.Foreground = Brushes.WhiteSmoke;
                    break;
                case "lb_2":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_coatingControl);
                    lb.Background = m_selectedColor;
                    lb.Foreground = Brushes.WhiteSmoke;
                    break;
                case "lb_32":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlMoore);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_35":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlTest);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_31":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlToshiba);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_4":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_analysingControl);
                    lb.Background = m_selectedColor;
                    lb.Foreground = Brushes.WhiteSmoke;
                    break;
                default:
                    g_content.Children.Clear();
                    g_content.Children.Add(new MainWindows.Welcome(this));
                    g_selector.Visibility = System.Windows.Visibility.Collapsed;
                    break;

            }
        }

        private Label m_selectedLabel;

        private void lb_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = m_hoverColor;
            (sender as Label).Foreground = Brushes.WhiteSmoke;
            if (m_selectedLabel != null)
            {
                m_selectedLabel.Background = m_selectedColor;
                m_selectedLabel.Foreground = Brushes.WhiteSmoke;
            }
        }

        private void lb_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = Brushes.White;
            (sender as Label).Foreground = Brushes.Black;
            if (m_selectedLabel != null)
            {
                m_selectedLabel.Background = m_selectedColor;
                m_selectedLabel.Foreground = Brushes.WhiteSmoke;
            }

        }

        private void mi_gotoLogin_Click(object sender, RoutedEventArgs e)
        {
            changeWindow(new Label(),0);
        }

        private void lb_MouseEnter_extended(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = m_hoverColor;
            (sender as Label).Foreground = Brushes.WhiteSmoke;
            //g_maxi.Height = 180;
            lb_31.Visibility = System.Windows.Visibility.Visible;
            lb_32.Visibility = System.Windows.Visibility.Visible;
            lb_33.Visibility = System.Windows.Visibility.Visible;
            lb_34.Visibility = System.Windows.Visibility.Visible;
            lb_35.Visibility = System.Windows.Visibility.Visible;
        }

        private void g_maxiLeave(object sender, MouseEventArgs e)
        {
            maxiLeave();   
        }

        private void maxiLeave()
        {
            if (lb_3 != m_selectedLabel)
            {
                lb_3.Background = Brushes.White;
                lb_3.Foreground = Brushes.Black;
            }

            lb_31.Visibility = System.Windows.Visibility.Collapsed;
            lb_32.Visibility = System.Windows.Visibility.Collapsed;
            lb_33.Visibility = System.Windows.Visibility.Collapsed;
            lb_34.Visibility = System.Windows.Visibility.Collapsed;
            lb_35.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void lb_MouseLeave1(object sender, MouseEventArgs e)
        {

        }

        private void mbt_addProject_Click(object sender, RoutedEventArgs e)
        {
            new Controls.CProject(2).ShowDialog();
        }

        private void Debug_click(object sender, RoutedEventArgs e)
        {            

           
        }


        


    }

    public interface ILogin
    {

        void showSelector();
        void hideSelector();
    }
}
