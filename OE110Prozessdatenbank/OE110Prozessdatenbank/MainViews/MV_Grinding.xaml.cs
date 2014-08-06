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
using System.Data;
using PDCore.Database;
using System.ComponentModel;

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für F_Grinding.xaml
    /// </summary>
    public partial class MV_Grinding : UserControl
    {
        ViewModels.F_GrindingVM m_vm;
        ProcessWindows.GenericWindow gw;

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public MV_Grinding()
        {
            InitializeComponent();
            m_vm = new ViewModels.F_GrindingVM();
            DataContext = m_vm;

            gw = new ProcessWindows.GenericWindow();
        }

        public void setMachineID(int MID)
        { m_vm.setMachine(MID); }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
            
        }

        private void MaterialListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Polished.SelectedIndex != -1)
            {
                int ID =-1; 

                switch (m_vm.Machine.ID)
                {
                    case 11:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBTurningMoore.ID]);
                        new ProcessWindows.CTurningMoore(ID, true).ShowDialog();
                        break;
                    case 12:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingMoore.ID]);
                        new ProcessWindows.CGrindingMoore(ID, true).ShowDialog();
                        break;
                    case 13:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingPhoenix.ID]);
                        new ProcessWindows.CGrindingPhoenix(ID, true).ShowDialog();
                        break;
                    case 14:
                        ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingOther.ID]);
                        new ProcessWindows.CGrindingOther(ID, true).ShowDialog();
                        break;

                }
              
            }

            
        }

        private void LV_Raw_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if (LV_Raw.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32((LV_Raw.SelectedItem as System.Data.DataRowView)[DBWorkpieces.ID]);

                switch (m_vm.Machine.ID)
                { 
                    case 11:
                        new ProcessWindows.CTurningMoore(ID, false).ShowDialog();
                        break;
                    case 12:
                        new ProcessWindows.CGrindingMoore(ID, false).ShowDialog();
                        break;
                    case 13:
                        new ProcessWindows.CGrindingPhoenix(ID, false).ShowDialog();
                        break;
                    case 14:
                        new ProcessWindows.CGrindingOther(ID, false).ShowDialog();
                        break;
                }
            }
        }

        private void Button_WP_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        private void mi_WorkpieceAdmin_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.WorkpieceAdministration().ShowDialog();
        }

        private void mi_AddWorkpiece_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.AddWorkpiece().ShowDialog();
        }

        private void mi_AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            new Controls.MaterialWindow().ShowDialog();
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

        private void mi_Export_Click(object sender, RoutedEventArgs e)
        {
            switch (m_vm.Machine.ID)
            {
                case 11:
                    List<PDCore.Processes.PTurningMoore> processes = new List<PDCore.Processes.PTurningMoore>();
                    foreach (DataRowView rowview in LV_Polished.Items)
                    {
                        processes.Add(PDCore.Manager.ProcessManager.Instance.getProcess(Convert.ToInt32(rowview[DBTurningMoore.ID]), 11) as PDCore.Processes.PTurningMoore);
                    }
                    //PDCore.Manager.ExportManager.foo(processes);
                    break;
                //case 12:
                //    ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingMoore.ID]);
                //    new ProcessWindows.CGrindingMoore(ID, true).ShowDialog();
                //    break;
                //case 13:
                //    ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingPhoenix.ID]);
                //    new ProcessWindows.CGrindingPhoenix(ID, true).ShowDialog();
                //    break;
                //case 14:
                //    ID = Convert.ToInt32((LV_Polished.SelectedItem as System.Data.DataRowView)[DBGrindingOther.ID]);
                //    new ProcessWindows.CGrindingOther(ID, true).ShowDialog();
                //    break;

            }
        }

        private void cmb_skip_Click(object sender, RoutedEventArgs e)
        {
            if (LV_Raw.SelectedIndex != -1)
            {

                string label = ((LV_Raw.SelectedItem as DataRowView)[DBWorkpieces.Label]).ToString();

                

                if (MessageBox.Show(Properties.Messages.q_skipWorkpieceGrinding,"",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int ID = Convert.ToInt32((LV_Raw.SelectedItem as DataRowView)[DBWorkpieces.ID]);
                    PDCore.Manager.ProcessManager.Instance.skipInitialProcess(PDCore.Manager.ObjectManager.Instance.getWorkpiece(ID));
                }
            }

        }

        #region ContxtMenuHelper

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

        #endregion


        private void ListView_Header_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);

            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                LV_Raw.Items.SortDescriptions.Clear();
                
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);

            switch (newDir)
            {
                case ListSortDirection.Descending:
                    m_vm.SortString = " ORDER BY " + column.Tag.ToString() + " DESC";
                    break;
                default:
                    m_vm.SortString = " ORDER BY " + column.Tag.ToString() + " ASC";
                    break;
            }

        }
        




    }
}
