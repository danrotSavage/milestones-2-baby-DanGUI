using Presentation.Model;
using Presentation.view;
using System;
using System.Windows;
using System.Windows.Controls;


namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardViewModel viewModel;
        public BoardWindow(UserModel user,BackendController controller)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(user,controller);
            this.DataContext = viewModel;

        }

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            
            viewModel.AddColumn(ColumnTitle.Text,ColumnOrdinal.Text);
        }

        private void DeleteColumn(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
        }

        

        private void AddTask(object sender, RoutedEventArgs e)
        {
            viewModel.addTask(taskTitle.Text,datePickerBoard.Text,description.Text);
        }

        private void LimitColumn(object sender, RoutedEventArgs e)
        {
            viewModel.LimitColumn(ColumnLimit.Text);
        }

        private void MoveRight(object sender, RoutedEventArgs e)
        {
            viewModel.MoveRight();
        }

        private void MoveLeft(object sender, RoutedEventArgs e)
        {
            viewModel.MoveLeft();
        }

        private void AdvanceTask(object sender, RoutedEventArgs e)
        {
            viewModel.AdvanceTask();
        }
    }
}
