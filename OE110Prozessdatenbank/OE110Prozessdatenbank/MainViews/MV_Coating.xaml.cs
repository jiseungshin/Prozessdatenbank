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
using PDCore.Database;
using System.Data;

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für F_Coating.xaml
    /// </summary>
    public partial class MV_Coating : UserControl
    {
        public MV_Coating()
        {
            InitializeComponent();
            DataContext = new ViewModels.F_CoatingVM();
        }

        private void LV_Raw_DoubvleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);
                new ProcessWindows.CCoatingCemecon(ID, false).ShowDialog();

            }
        }

        private void MaterialListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((sender as ListView).SelectedItem as System.Data.DataRowView)[DBCoatingCemecon.ID]);
                new ProcessWindows.CCoatingCemecon(ID, true).ShowDialog();
            }
        }

        private void mbt_addcoatingProcess_Click(object sender, RoutedEventArgs e)
        {
            new ObjectWindows.CoatingStandardProcessWindow().ShowDialog();
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

        private void cmb_skip_Click(object sender, RoutedEventArgs e)
        {
            if (LD_Polished.SelectedIndex != -1)
            {

                //int? projectID = (LD_Polished.SelectedItem as System.Data.DataRowView).Row.Field<int?>(DBProcessReferences.ProjectID);
                //int? IssueID = (LD_Polished.SelectedItem as System.Data.DataRowView).Row.Field<int?>(DBProcessReferences.IssueID);

                //if (projectID != null && IssueID != null)
                //{
                    
                    if (MessageBox.Show(Properties.Messages.q_skipWorkpieceCoating, "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        int RefNumber = Convert.ToInt32((LD_Polished.SelectedItem as System.Data.DataRowView)[DBProcessReferences.RefNumber]);

                        {
                            PDCore.Manager.ProcessManager.Instance.skipProcess(RefNumber, DBEnum.EnumReference.COATED);
                        }
                    }
                //}

                //else
                //{
                //    MessageBox.Show(Properties.Messages.e_NoProjectNoIssue, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
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

        private void cmb_cancel_Click(object sender, RoutedEventArgs e)
        {
            if (LD_Polished.SelectedIndex != -1)
            {
                if (MessageBox.Show(Properties.Messages.q_CancelReference, "Vorgang abbrechen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    int ID = Convert.ToInt32(((LD_Polished).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                    PDCore.Manager.ProcessManager.Instance.OverrideReferenceStatus(PDCore.Manager.ObjectManager.Instance.getWorkpieceByReference(ID), DBEnum.EnumReference.CANCELLED);
                }
            }
        }

        
    }
}
