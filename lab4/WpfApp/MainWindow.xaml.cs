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
                MessageBoxResult res = MessageBox.Show("Данные могут быть потеряны. Продолжить?\nНажмите \"Да\", если желаете продолжить без сохранения." +
                                                        "\nНажмите \"Нет\", если хотите сохранить текущие данные в файле.", "", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Yes)
                {
                    V4MC = new V4MainCollection();
                    DataContext = V4MC;
                }
                if (res == MessageBoxResult.No)
                {
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                    if (dlg.ShowDialog() == true)
                    {
                        string filename = dlg.FileName;
                        V4MC.Save(filename);
                        V4MC.ChangesWereMade = false;
                    }
                }
            }
            else
            {
                V4MC = new V4MainCollection();
                DataContext = V4MC;
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
                        V4MC.ChangesWereMade = false;
                        flag = false;
                    }
                }
            }
            if (flag == true)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    try
                    {
                        string filename = dlg.FileName;
                        V4MC = V4MainCollection.Load(filename);
                        DataContext = null;
                        DataContext = V4MC;
                        V4MC.ChangesWereMade = true;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    string filename = dlg.FileName;
                    V4MC.Save(filename);
                    V4MC.ChangesWereMade = false;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void Add_Defaults_Click(object sender, RoutedEventArgs e)
        {
            V4MC.AddDefaults();
            DataContext = null;
            DataContext = V4MC;
        }

        private void Add_Default_V4DataCollection_Click(object sender, RoutedEventArgs e)
        {
            V4DataCollection item = new V4DataCollection("random V4DC", 9d);
            item.InitRandom(5, 4, 4, 0, 9);
            V4MC.Add(item);
            DataContext = null;
            DataContext = V4MC;
        }

        private void Add_Default_V4DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            //Grid2D gr = new Grid2D();
            Grid2D gr = new Grid2D(1,2,3,5);

            V4DataOnGrid item = new V4DataOnGrid("random V4DOG", 8d, gr);
            item.InitRandom(1, 9);
            V4MC.Add(item);
            DataContext = null;
            DataContext = V4MC;
        }

        private void Add_Element_from_File_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                V4DataCollection item = new V4DataCollection(filename);
                V4MC.Add(item);
                DataContext = null;
                DataContext = V4MC;
            }
            else
            {
                MessageBox.Show("Произошла ошибка при чтении из файла.");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            V4MC.Remove2((V4Data)listBox_Main.SelectedItem);
            DataContext = null;
            DataContext = V4MC;
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
                        V4MC.ChangesWereMade = false;
                    }
                }
            }
        }
    }
}
