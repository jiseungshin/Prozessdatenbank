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
using System.Reflection;

namespace OE110Prozessdatenbank.Controls
{
    /// <summary>
    /// Interaktionslogik für ToshibaProcessEditor.xaml
    /// </summary>
    public partial class ToshibaProcessEditor : Window
    {
        ViewModels.PToshibaImportVM m_vm;
        private List<string> m_countSelection;
        private List<MenuItem> m_items;
        int index = 0;
        public ToshibaProcessEditor(ViewModels.PToshibaImportVM vm, int index)
        {
            InitializeComponent();
            m_countSelection = new List<string>();
            m_items = new List<MenuItem>();
            generateCountSelector(vm.Processes.Count - index);
            this.index = index;
            m_vm = vm;
            DataContext = this;// vm.Processes[index];
            //cb1.DataContext = this;

            
        }

        public ObservableCollection<string> CountSelectionArray
        { get { return new ObservableCollection<string>(m_countSelection); } }

        public ObservableCollection<MenuItem> Items
        { get { return new ObservableCollection<MenuItem>(m_items); } }

        public ViewModels.PToshibaVM VM
        { get { return m_vm.Processes[index]; } }


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
                {
                    m_countSelection.Add("Übernehmen für den nächsten");
                    MenuItem item = new MenuItem();
                    item.Header = "den nächsten";
                    item.Name = "mi_"+i.ToString(); 
                    m_items.Add(item);
                }
                else
                {
                    m_countSelection.Add("Übernehmen für die nächsten " + i);
                    MenuItem item = new MenuItem();
                    item.Header = "die nächsten " + i;
                    item.Name = "mi_" + i.ToString();
                    item.Click += item_Click;
                    m_items.Add(item);
                }
            }
        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            string name = mnu.Name;


            object t = (ContextMenu)mnu.Parent;

            test = Convert.ToInt32(name.Remove(0, 3));
        }

        int test;

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

       

        private void take_all(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            TextBox sp = null;
            if (mnu != null)
            {
                sp = ((ContextMenu)mnu.Parent).PlacementTarget as TextBox;
            }
        }

        private void takespec(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            TextBox sp = null;
            if (mnu != null)
            {
                //MenuItem bb;
                //bb = ((MenuItem)mnu.Parent) as MenuItem;
                sp = ((ContextMenu)mnu.Parent).PlacementTarget as TextBox;

                Binding myBinding = BindingOperations.GetBinding(sp, TextBox.TextProperty);

                string dd = myBinding.Path.Path.Remove(0, 3);
                
                //PropertyInfo property = typeof(ViewModels.PToshibaVM).GetProperty(myBinding.ElementName);

                System.Reflection.PropertyInfo prop = typeof(ViewModels.PToshibaVM).GetProperty(dd);

                

                
                
                for (int i = index + 1; i < index + test+1; i++)
                {
                    //object value = prop.GetValue(m_vm.Processes[i], null);

                    prop.SetValue(m_vm.Processes[i], Convert.ToDouble(sp.Text.Replace('.', ',')), null);
                    
                    //m_vm.Processes[i].P1 = Convert.ToDouble(sp.Text.Replace('.',','));
                }

                

                //property.SetValue(Convert.ToDouble(sp.Text.Replace('.',',')), "...", null);

            }

            //MessageBox.Show(" Textbox:" + sp.Name);
        }

    }
}
