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
using PDCore.Database;
using System.Threading;
using System.Windows.Markup;
using System.Globalization;

namespace OE110Prozessdatenbank
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILogin
    {
        Brush m_selectedColor;
        Brush m_hoverColor;

        private MainViews.MV_Grinding m_grindingControl;
        private UIElement m_coatingControl;
        private UIElement m_processingControlMoore;
        private UIElement m_processingControlTest;
        private UIElement m_analysingControl;
        private UIElement m_processingControlToshiba;
        private UIElement m_processingControlCemeCon;
        private UIElement m_processingControlFueller;

        private Label m_adminLabel = new Label();


        public MainWindow()
        {
            InitializeComponent();

            //Set culture to german
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


            m_adminLabel.Name = "lb_0";
            lbLoginName.Content = "";
            userIcon.Visibility = System.Windows.Visibility.Hidden;
            //#FF5EB45A
            var converter = new System.Windows.Media.BrushConverter();
            m_selectedColor = (Brush)converter.ConvertFromString("#FF509E5E");
            m_hoverColor = (Brush)converter.ConvertFromString("#FF5EB45A");

            m_grindingControl = new MainViews.MV_Grinding();
            m_coatingControl = new MainViews.MV_Coating();
            m_processingControlMoore = new MainViews.MV_ProMoore();
            m_processingControlToshiba = new MainViews.MV_ProToshiba();
            m_processingControlTest = new MainViews.MV_ProTestStation();
            m_processingControlFueller = new MainViews.MV_ProFueller();
            m_analysingControl = new MainViews.MV_PostProcessing();
            m_processingControlCemeCon = new Controls.CProSemeCon();

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
            m_grindingControl = new MainViews.MV_Grinding();
            m_coatingControl = new MainViews.MV_Coating();
            m_processingControlTest = new MainViews.MV_ProTestStation();
            m_processingControlMoore = new MainViews.MV_ProMoore();
            m_analysingControl = new MainViews.MV_PostProcessing();
            m_processingControlToshiba = new MainViews.MV_ProToshiba();
            
            if (PDCore.Manager.UserManager.CurrentUser!=null)
            {
                lbLoginName.Content = PDCore.Manager.UserManager.CurrentUser.FirstName + " " + PDCore.Manager.UserManager.CurrentUser.LastName;
                userIcon.Visibility = System.Windows.Visibility.Visible;
                int id = Convert.ToInt32(PDCore.Manager.UserManager.CurrentUser.MachineID);
                switch(id)
                {
                    case 0:
                        this.changeWindow(m_adminLabel, 0);
                        break;
                    case 11:
                        this.changeWindow(lb_1, id);
                        break;
                    case 12:
                        this.changeWindow(lb_1, id);
                        break;
                    case 13:
                        this.changeWindow(lb_1, id);
                        break;
                    case 14:
                        this.changeWindow(lb_1, id);
                        break;
                    case 21:
                        this.changeWindow(lb_2, id);
                        break;
                    case 31:
                        this.changeWindow(lb_31, id);
                        break;
                    case 32:
                        this.changeWindow(lb_32, id);
                        break;
                    case 33:
                        this.changeWindow(lb_33, id);
                        break;
                    case 34:
                        this.changeWindow(lb_34, id);
                        break;
                    case 35:
                        this.changeWindow(lb_35, id);
                        break;
                    //case 36:
                    //    this.changeWindow(lb_36, id);
                    //    break;
                    case 41:
                        this.changeWindow(lb_4, id);
                        break;
                    case 51:
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
                case "lb_0":
                    g_content.Children.Clear();
                    g_content.Children.Add(new MainViews.Administration());
                    break;
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
                case "lb_31":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlMoore);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_32":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlTest);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_34":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlToshiba);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_33":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlCemeCon);
                    lb_3.Background = m_selectedColor;
                    lb_3.Foreground = Brushes.WhiteSmoke;
                    m_selectedLabel = lb_3;
                    break;
                case "lb_35":
                    g_content.Children.Clear();
                    g_content.Children.Add(m_processingControlFueller);
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
            new ObjectWindows.CProject().ShowDialog();
        }

        private void Debug_click(object sender, RoutedEventArgs e)
        {
            PDCore.Manager.Updater.Instance.forceUpdate();

            //var tt = PDCore.Manager.ProcessManager.Instance.getWorkpieceHistory(1);
            //var tt = PDCore.Manager.FileManager.Instance.getDirPth(1);
            //PDCore.Processes.PTurningMoore p = PDCore.Manager.ProcessManager.Instance.getProcess(15,11) as PDCore.Processes.PTurningMoore;
            //PDCore.Manager.ExportManager.foo(new List<PDCore.Processes.PTurningMoore>() { p });
            //List<MySQLCommunicator.ColumnValuePair> tt = new List<MySQLCommunicator.ColumnValuePair>();

            //tt.Add(new MySQLCommunicator.ColumnValuePair(){Culumn=DBWorkpieces.Label,Value="huhu"});
            //tt.Add(new MySQLCommunicator.ColumnValuePair() { Culumn = DBWorkpieces.PurchaseDate, Value = DateTime.Now });
            //tt.Add(new MySQLCommunicator.ColumnValuePair(){Culumn=DBWorkpieces.isActive,Value=true});

            //string q =PDCore.Database.MySQLCommunicator.BuildUpdateQuery(DBWorkpieces.Table,tt,new MySQLCommunicator.ColumnValuePair(){Culumn=DBWorkpieces.ID,Value=1});

            ////PDCore.Manager.Updater.Instance.foo("hallo", "huhu");
            //MessageBox.Show(PDCore.Manager.ProcessManager.Instance.checkNewTry(2).ToString());
            //int a = 0;

            //List<PDCore.BusinessObjects.Workpiece> wps = PDCore.Manager.ObjectManager.Instance.Workpieces;

            //new ObjectWindows.WorkpiecePicker().ShowDialog();
        }

        private void mbt_CoatingAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.CoatingSPAdministration().ShowDialog();
        }

        private void mbt_ProjectAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.ProjectAdministration().ShowDialog();
        }

        private void mbt_Admininstration_Click(object sender, RoutedEventArgs e)
        {
            g_content.Children.Clear();
            g_content.Children.Add(new MainViews.Administration());
        }

        private void mbt_WorkpieceAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.WorkpieceAdministration().ShowDialog();
        }

        private void mbt_UserAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.UserAdministration().ShowDialog();
        }

        private void mbt_GlassAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.GlassAdministration().ShowDialog();
        }

        private void mbt_MaterialAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.MaterialAdministration().ShowDialog();
        }

        private void mbt_addMaterial_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
        }

        private void mbt_addWorkpiece_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        private void mi_Terminate_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Anwendung jetzt schließen?", "", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void mbt_GetReference_Click(object sender, RoutedEventArgs e)
        {
            new Controls.ReferencePicker().ShowDialog();
        }

        private void mbt_GetRefereProjectHistory_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.ProjectHistory().ShowDialog();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                new Controls.ReferencePicker().ShowDialog();
            }

            if (e.Key == Key.P && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                new ObjectWindows.ProjectHistory().ShowDialog();
            }

            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                new debugWindow().ShowDialog();
            }
        }

        private void mbt_PhoenixProcessAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.PhoenixProcessAdministration().ShowDialog();
        }

       


        


    }

    public interface ILogin
    {

        void showSelector();
        void hideSelector();
    }
}
