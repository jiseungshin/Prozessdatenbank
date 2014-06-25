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

namespace OE110Prozessdatenbank.MainWindows
{
    /// <summary>
    /// Interaktionslogik für F_PostProcessing.xaml
    /// </summary>
    public partial class F_PostProcessing : UserControl
    {
        public F_PostProcessing()
        {
            InitializeComponent();
            DataContext = new ViewModels.F_PostProcessingVM();
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

        private void cmb_conclusion_Click(object sender, RoutedEventArgs e)
        {
            int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
            new Controls.CConclusion(ID).ShowDialog();
        }

        private void cmb_decoating_Click(object sender, RoutedEventArgs e)
        {
            int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
            new Controls.CDecoating(ID).ShowDialog();
        }

        private void cmb_analyse_Click(object sender, RoutedEventArgs e)
        {
            int ID = Convert.ToInt32(((listview).SelectedItem as DataRowView)[DBProcessReferences.RefNumber]);
            new Controls.CAnalyses(ID).ShowDialog();
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
    }
}
