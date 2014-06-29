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
            for (int i=1; i<count; i++)
            {
                if (i == 1)
                {
                    MenuItem item = new MenuItem();
                    item.Header = "den nächsten";
                    item.Name = "mi_"+i.ToString();
                    item.Click += item_Click;
                    m_items.Add(item);
                }
                else
                {
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
                sp = ((ContextMenu)mnu.Parent).PlacementTarget as TextBox;

                Binding myBinding = BindingOperations.GetBinding(sp, TextBox.TextProperty);

                string boundComponent = myBinding.Path.Path.Remove(0, 3);


                System.Reflection.PropertyInfo prop = typeof(ViewModels.PToshibaVM).GetProperty(boundComponent);
      
                for (int i = index + 1; i < index + test+1; i++)
                {
                    prop.SetValue(m_vm.Processes[i], Convert.ToDouble(sp.Text.Replace('.', ',')), null);                   
                }

                

                //property.SetValue(Convert.ToDouble(sp.Text.Replace('.',',')), "...", null);

            }

            //MessageBox.Show(" Textbox:" + sp.Name);
        }

    }
}
