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
using WPF2SOL_final.Model;
using WPF2SOL_final.view;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow(UserModel user)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(user);
            this.viewModel = (MainViewModel)DataContext;

        }

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            viewModel.AddColumn(ColumnTitle.Text);
        }

        private void DeleteColumn(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            throw new Exception();
        }
    }
}
