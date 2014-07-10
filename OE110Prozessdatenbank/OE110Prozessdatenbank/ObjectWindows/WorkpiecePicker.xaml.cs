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
using PDCore.BusinessObjects;
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für WorkpieceAdministration.xaml
    /// </summary>
    public partial class WorkpiecePicker : Window
    {

        ViewModels.PExpMooreVM b_vm;
        int m_wpLocation = 0;

        public WorkpiecePicker(ViewModels.PExpMooreVM vm, int WPLocation)
        {
            InitializeComponent();
            b_vm = vm;
            DataContext = new VMWorkpiecePicker(b_vm);
            m_wpLocation = WPLocation;
           

        }


        private void LV_Workpiece_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                if (m_wpLocation==1)
                {
                    if (b_vm is ViewModels.PExpMooreVM)
                    {
                        (b_vm).UpperWP = b_vm.WorkpiecesUpper.Find(item => item.ID == ((sender as ListView).SelectedItem as Workpiece).ID);
                    }

                    //if (b_vm is ViewModels.PExpTestStationVM)
                    //{
                    //    (b_vm as ViewModels.PExpTestStationVM).LeftWorkpiece = ((sender as ListView).SelectedItem as Workpiece);
                    //}

                }

                if (m_wpLocation == 2)
                {
                    //if (b_vm is ViewModels.PExpTestStationVM)
                    //{
                    //    (b_vm as ViewModels.PExpTestStationVM).CenterWorkpiece = ((sender as ListView).SelectedItem as Workpiece);
                    //}

                }

                if (m_wpLocation == 3)
                {
                    if (b_vm is ViewModels.PExpMooreVM)
                    {
                        (b_vm).LowerWP = b_vm.WorkpiecesLower.Find(item => item.ID == ((sender as ListView).SelectedItem as Workpiece).ID);
                    }

                    //if (b_vm is ViewModels.PExpTestStationVM)
                    //{
                    //    (b_vm as ViewModels.PExpTestStationVM).RightWorkpiece = ((sender as ListView).SelectedItem as Workpiece);
                    //}

                }
                
                this.Close();
            }
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        private void mbt_addWorkpiece_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }
    }

    public class VMWorkpiecePicker : ViewModels.BaseViewModel
    {
        private string m_filter = "";
        ViewModels.PExpMooreVM m_vm;
        public VMWorkpiecePicker(ViewModels.PExpMooreVM vm)
        {
            ObjectManager.Instance.update();
            ObjectManager.Instance.newObjects += Instance_newObjects;
            m_vm = vm;

        }

        //public VMWorkpiecePicker()
        //{
        //    ObjectManager.Instance.update();
        //    ObjectManager.Instance.newObjects += Instance_newObjects;

        //}

        public List<Workpiece> Workpieces
        {
            get
            {
                //return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.Where(item => item.CurrentReferenceNumber.ToString().Contains(Filter) ||
                //                                  item.Material.Name.ToLower().Contains(Filter.ToLower()) ||
                //                                  item.Label.ToLower().Contains(Filter.ToLower())));

                return m_vm.WorkpiecesUpper.FindAll(item => item.CurrentReferenceNumber.ToString().Contains(Filter) ||
                                                  item.Material.Name.ToLower().Contains(Filter.ToLower()) ||
                                                  item.Label.ToLower().Contains(Filter.ToLower()));
                
            }
        }

        void Instance_newObjects()
        {
            ObjectManager.Instance.update();
            NotifyPropertyChanged("Workpieces");
        }

        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; NotifyPropertyChanged("Workpieces"); }
        }
    }
}
