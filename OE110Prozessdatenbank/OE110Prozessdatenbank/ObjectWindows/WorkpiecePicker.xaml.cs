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

        Workpiece m_workpice = null;
        public WorkpiecePicker(ObservableCollection<Workpiece> workpieces)
        {
            InitializeComponent();
            DataContext = new VMWorkpiecePicker(workpieces);
        }


        private void LV_Workpiece_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {


                m_workpice = ((sender as ListView).SelectedItem as Workpiece);
          
                this.Close();
            }
        }

        public Workpiece Workpice
        { get { return m_workpice; } }

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
        ObservableCollection<Workpiece> m_workpieces;
        public VMWorkpiecePicker(ObservableCollection<Workpiece> workpieces)
        {
            ObjectManager.Instance.update(PDCore.Database.DBWorkpieces.Table);
            ObjectManager.Instance.newObjects += Instance_newObjects;
            m_workpieces = workpieces;

        }

        //public VMWorkpiecePicker()
        //{
        //    ObjectManager.Instance.update();
        //    ObjectManager.Instance.newObjects += Instance_newObjects;

        //}

        public ObservableCollection<Workpiece> Workpieces
        {
            get
            {
                //return new ObservableCollection<Workpiece>(ObjectManager.Instance.CoatedWorkpieces.Where(item => item.CurrentReferenceNumber.ToString().Contains(Filter) ||
                //                                  item.Material.Name.ToLower().Contains(Filter.ToLower()) ||
                //                                  item.Label.ToLower().Contains(Filter.ToLower())));

                return new ObservableCollection<PDCore.BusinessObjects.Workpiece>(m_workpieces.ToList().FindAll(item => item.CurrentReferenceNumber.ToString().Contains(Filter) ||
                                                  item.Material.Name.ToLower().Contains(Filter.ToLower()) ||
                                                  item.Label.ToLower().Contains(Filter.ToLower()) ||
                                                  item.Reference.Project.Description.ToLower().Contains(Filter.ToLower()) ||
                                                  item.Reference.Issue.Description.ToLower().Contains(Filter.ToLower())));
                
            }
        }

        void Instance_newObjects()
        {
            //ObjectManager.Instance.update();
            NotifyPropertyChanged("Workpieces");
        }

        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; NotifyPropertyChanged("Workpieces"); }
        }
    }
}
