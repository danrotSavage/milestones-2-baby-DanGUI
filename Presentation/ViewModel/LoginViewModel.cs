using Presentation.Model;
using System;
using IntroSE.Kanban.Backend;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        public BackendController Controller { get; set; }
        public Brush BorderBrushPassword {
            get { return string.IsNullOrWhiteSpace(password)?Brushes.Red:Brushes.Gray; }
        
        }
        private string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                this._Email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string password;
        public string Password
        {
            get => password;
            set
            {
                this.password = value;
                RaisePropertyChanged("Password");
                RaisePropertyChanged("BorderBrushPassword");
             
            }
        }
        private string message;
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }
        public UserModel Login()
        {
            try
            {
                return Controller.Login(Email,Password);
            }
            catch(KanbanException e)
            {
                Message = e.Message;
                return null;
            }
            catch(Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
        public LoginViewModel()
        {
            Controller = new BackendController();
            this.Email = "";
            this.password = "";
            this.Message = "";
        }

        public LoginViewModel(BackendController cont)
        {
            this.Email = "";
            this.password = "";
            this.Message = "";
            Controller = cont;
        }
    }
}
