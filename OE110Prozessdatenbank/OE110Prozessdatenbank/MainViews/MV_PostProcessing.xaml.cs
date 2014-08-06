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
using OE110Prozessdatenbank.Properties;
using System.ComponentModel;

namespace OE110Prozessdatenbank.MainViews
{
    /// <summary>
    /// Interaktionslogik für F_PostProcessing.xaml
    /// </summary>
    public partial class MV_PostProcessing : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private ViewModels.F_PostProcessingVM m_vm;

        public MV_PostProcessing()
        {
            InitializeComponent();
            m_vm = new ViewModels.F_PostProcessingVM();
            DataContext = m_vm;
        }

        private void LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

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

        private void ListView_Header_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);

            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                listview.Items.SortDescriptions.Clear();
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
                    m_vm.SortString = /*" ORDER BY " +*/ column.Tag.ToString() + " DESC";
                    break;
                default:
                    m_vm.SortString = /*" ORDER BY " +*/ column.Tag.ToString() + " ASC";
                    break;
            }

        }

        private void cmb_conclusion_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                new Controls.CConclusion(ID).ShowDialog();
            }
        }

        private void cmb_decoating_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                new Controls.CDecoating(ID).ShowDialog();
            }
        }
        private void cmb_analyse_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                new Controls.CAnalyses(ID).ShowDialog();
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

        private void mbt_gotoAnalyse_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                new Controls.CAnalyses(ID).ShowDialog();
            }
            else
                MessageBox.Show(Messages.e_NoSelectedListElement, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void mbt_gotoDecoating_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                new Controls.CDecoating(ID).ShowDialog();
            }
            else
                MessageBox.Show(Messages.e_NoSelectedListElement, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void mbt_gotoConclusion_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                new Controls.CConclusion(ID).ShowDialog();
            }
            else
                MessageBox.Show(Messages.e_NoSelectedListElement, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void cmb_cancel_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                string status = ((listview).SelectedItem as DataRowView).Row.Field<string>(DBProcessReferences.Status);

                if (status == DBEnum.EnumReference.CANCELLED || status == DBEnum.EnumReference.TERMINATED)
                {
                    MessageBox.Show(Messages.e_cannotCancelProcess, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    if (MessageBox.Show(Messages.q_CancelReference, "Vorgang abbrechen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                        PDCore.Manager.ProcessManager.Instance.OverrideReferenceStatus(PDCore.Manager.ObjectManager.Instance.getWorkpieceByReference(ID), DBEnum.EnumReference.CANCELLED);
                    }
                }
            }
        }

        private void cmb_terminate_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                 string status = ((listview).SelectedItem as DataRowView).Row.Field<string>(DBProcessReferences.Status);

                 if (status == DBEnum.EnumReference.CANCELLED || status == DBEnum.EnumReference.TERMINATED)
                 {
                     MessageBox.Show(Messages.e_cannnotDoNextTry, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Error);

                 }
                 else
                 {
                     if (MessageBox.Show(Messages.q_TerminateReference, "Vorgang beenden", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                     {
                         int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
                         PDCore.Manager.ProcessManager.Instance.OverrideReferenceStatus(PDCore.Manager.ObjectManager.Instance.getWorkpieceByReference(ID), DBEnum.EnumReference.CANCELLED);
                     }
                 }
            }
        }

        private void cmb_nextTry_Click(object sender, RoutedEventArgs e)
        {
            if (listview.SelectedIndex != -1)
            {
                string status = ((listview).SelectedItem as DataRowView).Row.Field<string>(DBProcessReferences.Status);

                if (status == DBEnum.EnumReference.CANCELLED || status == DBEnum.EnumReference.TERMINATED)
                {
                     MessageBox.Show(Messages.e_cannnotDoNextTry, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Error);
                   
                }
                else
                {
                    if (MessageBox.Show(Messages.q_NextTry, "Neuer Versuch", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);

                        PDCore.Manager.ProcessManager.Instance.prepareNewTry(ID, DBEnum.EnumReference.COATED);
                    }
                }
            }
        }
    }
}
