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
        MainWindows.F_Grinding gg;
        MainWindows.F_Coating ff;
        public MainWindow()
        {
            InitializeComponent();
            ff = new MainWindows.F_Coating();
            ff.Show();
            gg = new MainWindows.F_Grinding();
            gg.Show();
            //this.Close();
            //Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //ProcessWindows.GenericWindow gq = new ProcessWindows.GenericWindow();
            //gq.contentGrid.Children.Add(new Controls.CExpMoore(9));
            //gq.ShowDialog();

            new Controls.WorkpieceAdministration().ShowDialog();

            //new Controls.CProject(5).ShowDialog();
            //new Controls.CoatingSPAdministration().ShowDialog();
            //List<int?> test = PDCore.Manager.ObjectManager.Instance.getProjectIssueIDS(2);
            //// MessageBox.Show(PDCore.Manager.ProcessManager.Instance.getNextProcessIndex().ToString());
           //// PDCore.Manager.ProcessManager.Instance
           // PTurningMoore tt = PDCore.Manager.ProcessManager.Instance.getProcessByReference(9, 1) as PTurningMoore;

           // Workpiece wp = new Workpiece();
           // wp.ID = 9;

           // //PTurningMoore tt = new PTurningMoore();
           // tt.UserID = 2;
           // tt.Date = DateTime.Now;
           // tt.Remark = "MYSQL Rocks44 Update";
           // tt.Radius = 888;
           // tt.Speed = 33;

           // PDCore.Manager.ProcessManager.Instance.saveProcess(tt, null, true);
            //MainWindows.F_Grinding ff = new MainWindows.F_Grinding();
            //new MainWindows.F_Grinding().ShowDialog();
            //ff.ShowDialog();

        }


    }
}
