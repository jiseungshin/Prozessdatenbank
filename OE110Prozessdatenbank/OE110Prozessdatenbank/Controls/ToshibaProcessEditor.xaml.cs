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
using System.Collections.ObjectModel;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für ToshibaProcessEditor.xaml
    /// </summary>
    public partial class ToshibaProcessEditor : Window
    {
        ViewModels.PToshibaImportVM m_vm;
        private List<string> m_countSelection;
        public ToshibaProcessEditor(ViewModels.PToshibaImportVM vm, int index)
        {
            InitializeComponent();
            m_countSelection = new List<string>();
            generateCountSelector(vm.Processes.Count - index);
            m_vm = vm;
            DataContext = this;// vm.Processes[index];
            //cb1.DataContext = this;

            
        }

        public ObservableCollection<string> CountSelectionArray
        { get { return new ObservableCollection<string>(m_countSelection); } }

        public int Selection
        {
            set
            {
                if (MessageBox.Show("Wert wirklich übernehmen?") == MessageBoxResult.OK)
                {

                    //int a = 0;
                }

            }
            get { return 0; }
        }

       // int sel = 0;

        private void generateCountSelector(int count)
        {
            m_countSelection.Add("---Übernehmen---");
            m_countSelection.Add("Übernehmen für alle");
            for (int i=1; i<count; i++)
            {
                if (i == 1)
                    m_countSelection.Add("Übernehmen für den nächsten");
                else
                    m_countSelection.Add("Übernehmen für die nächsten " + i);
            }
        }

        private void cmb_analyse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        private void TextBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        private void EditStatusCm_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    TextBox g = cm.PlacementTarget as TextBox;
                    if (g != null)
                    {
                        MessageBox.Show(g.Text); // Will print red
                    }
                }
            }
        }
    }
}
