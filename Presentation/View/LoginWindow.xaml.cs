using System.Windows;
using Presentation.Model;
using Presentation.ViewModel;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel loginViewModel;

        public LoginWindow()
        {
            InitializeComponent();
            this.loginViewModel = new LoginViewModel();
            this.DataContext = loginViewModel;
        }

        public LoginWindow(BackendController cont)
        {
            InitializeComponent();
            this.loginViewModel = new LoginViewModel(cont);
            this.DataContext = loginViewModel;
        }

        private void ToRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow reg = new RegisterWindow(loginViewModel.Controller);
            this.Close();
            reg.ShowDialog();
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = loginViewModel.Login();
            if (user != null)
            {
                BoardWindow boardView = new BoardWindow(user,loginViewModel.Controller);
                this.Close();
                boardView.Show();
            }
        }
    }
    }
