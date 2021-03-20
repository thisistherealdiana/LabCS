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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        V4MainCollection V4MC = new V4MainCollection();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = V4MC;
        }
        
        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (V4MC.ChangesWereMade == true)
            {
                MessageBoxResult res = MessageBox.Show("Данные могут быть потеряны. Продолжить?\nНажмите \"Да\", если желаете продолжить без сохранения.\nНажмите \"Нет\", если хотите сохранить текущие данные в файле.", "", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Yes)
                {
                    V4MC = new V4MainCollection();
                }
                if (res == MessageBoxResult.No)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                    if (dlg.ShowDialog() == true)
                    {
                        string filename = dlg.FileName;
                        V4MC.Save(filename);
                    }
                }
            }
            else
            {
                V4MC = new V4MainCollection();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (V4MC.ChangesWereMade == true)
            {
                MessageBoxResult res = MessageBox.Show("Данные могут быть потеряны. Продолжить?\nНажмите \"Да\", если желаете продолжить без сохранения." +
                                                        "\nНажмите \"Нет\", если хотите сохранить текущие данные в файле.", "", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    flag = false;
                }
                else if (res == MessageBoxResult.No)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        string filename = dlg.FileName;
                        V4MC.Save(filename);
                    }
                }
            }
            if (flag == true)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    string filename = dlg.FileName;
                    V4MC = Load(filename);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                V4MC.Save(filename);
            }
        }

        private void Add_Defaults_Click(object sender, RoutedEventArgs e)
        {
            V4DataCollection item1 = new V4DataCollection("first", 111d);
            item1.InitRandom(2, 4, 4, 0, 9);
            //Grid2D gr = new Grid2D();
            Grid2D gr = new Grid2D(1, 2, 3, 5);
            V4DataOnGrid item2 = new V4DataOnGrid("second", 87d, gr);
            item2.InitRandom(1, 9);
            V4DataCollection item3 = new V4DataCollection("third", 3d);
            item3.InitRandom(2, 4, 4, 0, 9);

            V4MC.Add(item1);
            V4MC.Add(item2);
            V4MC.Add(item3);
        }

        private void Add_Default_V4DataCollection_Click(object sender, RoutedEventArgs e)
        {
            V4DataCollection item = new V4DataCollection("random V4DC", 9d);
            item.InitRandom(5, 4, 4, 0, 9);
            V4MC.Add(item);
        }

        private void Add_Default_V4DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            //Grid2D gr = new Grid2D();
            Grid2D gr = new Grid2D(1,2,3,5);

            V4DataOnGrid item = new V4DataOnGrid("random V4DOG", 8d, gr);
            item.InitRandom(1, 9);
            V4MC.Add(item);
        }

        private void Add_Element_from_File_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                V4DataCollection item = new V4DataCollection(filename);
                V4MC.Add(item);
            }
            else
            {
                MessageBox.Show("Произошла ошибка при чтении из файла.");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //listBox_Main.Items.Remove(listBox_Main.Items[listBox_Main.SelectedIndex]);
            if (listBox_Main.SelectedItem.GetType().Name == "V4DataOnGrid")
            {
                V4DataOnGrid item = (V4DataOnGrid)listBox_Main.SelectedItem;
                string id = item.Info;
                double w = item.Frequency;
                V4MC.Remove(id, w);
            }
            else if (listBox_Main.SelectedItem.GetType().Name == "V4DataCollection")
            {
                V4DataCollection item = (V4DataCollection)listBox_Main.SelectedItem;
                string id = item.Info;
                double w = item.Frequency;
                V4MC.Remove(id, w);
            }
        }

        private void FilterByV4DataOnFrid(object sender, FilterEventArgs args)
        {
            if (args.Item != null)
            {
                if (args.Item.GetType().Name == "V4DataOnGrid") args.Accepted = true;
                else args.Accepted = false;
            }
        }
        private void FilterByV4DataCollection(object sender, FilterEventArgs args)
        {
            if (args.Item != null)
            {
                if (args.Item.GetType().Name == "V4DataCollection") args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (V4MC.ChangesWereMade == true)
            {
                MessageBoxResult res = MessageBox.Show("Данные могут быть потеряны. Продолжить?\nНажмите \"Да\", если желаете продолжить без сохранения.\n" +
                                                        "Нажмите \"Нет\", если хотите сохранить текущие данные в файле.", "", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.No)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        string filename = dlg.FileName;
                        V4MC.Save(filename);
                    }
                }
            }
        }
    }
}
