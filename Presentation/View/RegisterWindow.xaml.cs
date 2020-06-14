using System.Windows;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterViewModel registerViewModel = new RegisterViewModel();

        public RegisterWindow(BackendController controller)
        {
            InitializeComponent();
            this.registerViewModel = new RegisterViewModel(controller);
            this.DataContext = registerViewModel;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
           registerViewModel.Register();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow(registerViewModel.Controller);
            this.Close();
            login.ShowDialog();
        }
    }
}
