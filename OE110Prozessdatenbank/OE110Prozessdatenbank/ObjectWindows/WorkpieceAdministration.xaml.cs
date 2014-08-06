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
using System.Data;
using PDCore.Database;

namespace OE110Prozessdatenbank.ObjectWindows
{
    /// <summary>
    /// Interaktionslogik für WorkpieceAdministration.xaml
    /// </summary>
    public partial class WorkpieceAdministration : Window
    {
        public WorkpieceAdministration()
        {
            InitializeComponent();
            DataContext = new VMWorkpieceAdministration();
        }

        private void LV_Workpiece_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                new ObjectWindows.AddWorkpiece(((sender as ListView).SelectedItem as Workpiece).ID).ShowDialog();
            }
        }

        private void bt_Add_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
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

        private void mbt_addWorkpiece_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        void OnListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
                return;

            ListViewItem item = MyVisualTreeHelper.FindParent<ListViewItem>((DependencyObject)e.OriginalSource);
            if (item == null)
                return;

            if (item.Focusable && !item.IsFocused)
                item.Focus();
        }

        public static class MyVisualTreeHelper
        {
            static bool AlwaysTrue<T>(T obj) { return true; }

            /// <summary>
            /// Finds a parent of a given item on the visual tree. If the element is a ContentElement or FrameworkElement 
            /// it will use the logical tree to jump the gap.
            /// If not matching item can be found, a null reference is returned.
            /// </summary>
            /// <typeparam name="T">The type of the element to be found</typeparam>
            /// <param name="child">A direct or indirect child of the wanted item.</param>
            /// <returns>The first parent item that matches the submitted type parameter. If not matching item can be found, a null reference is returned.</returns>
            public static T FindParent<T>(DependencyObject child) where T : DependencyObject
            {
                return FindParent<T>(child, AlwaysTrue<T>);
            }

            public static T FindParent<T>(DependencyObject child, Predicate<T> predicate) where T : DependencyObject
            {
                DependencyObject parent = GetParent(child);
                if (parent == null)
                    return null;

                // check if the parent matches the type and predicate we're looking for
                if ((parent is T) && (predicate((T)parent)))
                    return parent as T;
                else
                    return FindParent<T>(parent);
            }

            static DependencyObject GetParent(DependencyObject child)
            {
                DependencyObject parent = null;
                if (child is Visual || child is System.Windows.Media.Media3D.Visual3D)
                    parent = VisualTreeHelper.GetParent(child);

                // if fails to find a parent via the visual tree, try to logical tree.
                return parent ?? LogicalTreeHelper.GetParent(child);
            }
        }

        private void cmb_goToReference_Click(object sender, RoutedEventArgs e)
        {
            if (LV_Workpiece.SelectedIndex!=-1)
            {
                int ID = (LV_Workpiece.SelectedItem as Workpiece).ID;
                DataSet ds = PDCore.Manager.ProcessManager.Instance.getData("SELECT * FROM " + DBProcessReferences.Table + " WHERE " + DBProcessReferences.WorkpiceID + "=" +
                ID + " AND NOT (" + DBProcessReferences.Status + "=" + DBEnum.EnumReference.TERMINATED.ToDBObject() + " OR " +
                DBProcessReferences.Status + "=" + DBEnum.EnumReference.CANCELLED.ToDBObject() + " OR " +
                DBProcessReferences.Status + "=" + DBEnum.EnumReference.RAW.ToDBObject() + ")");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int reference = ds.Tables[0].Rows[0].Field<int>(DBProcessReferences.RefNumber);
                    new ObjectWindows.HistoryViewer(reference).Show();
                }

                else
                    MessageBox.Show(Properties.Messages.n_WorkpieceNotInUse, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    
            }
        }
    }

    public class VMWorkpieceAdministration : ViewModels.BaseViewModel
    {

        private string m_filter = "";

        public VMWorkpieceAdministration()
        {
            //ObjectManager.Instance.update();
            ObjectManager.Instance.newObjects += Instance_newObjects;
        }

        public ObservableCollection<Workpiece> Workpieces
        {
            get 
            { 
                if (m_filter=="")
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces);
                else
                    return new ObservableCollection<Workpiece>(ObjectManager.Instance.Workpieces.Where(item => item.Label.ToLower().Contains(Filter.ToLower()) || item.Material.Name.ToLower().Contains(Filter.ToLower())));
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
