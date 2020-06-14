using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RegisterViewModel:NotifiableObject
    {
        public BackendController Controller { get; private set; }
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
        private string nickname;
        public string Nickname
        {
            get => nickname;
            set
            {
                this.nickname = value;
                RaisePropertyChanged("Nickname");
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
            }
        }
        private string message= "Registered successfully";
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }
        private string email_Host;
        public string Email_Host
        {
            get => email_Host;
            set
            {
                this.email_Host = value;
                RaisePropertyChanged("Email_Host");
            }
        }
        public void Register()
        {
            Message = "";
            try
            {
                Controller.Register(Email, Password,Nickname,Email_Host);
                Message = "Resgistered succesfully";

            }
            catch (Exception e)
            {
                Message = e.Message;
                Console.WriteLine("error: "+e.Message);
            }
        }
        public RegisterViewModel()
        {
            Controller = new BackendController();
            this.Email = "";
            this.password = "";
            this.nickname = "";
            this.Message = "";
        }
        public RegisterViewModel(BackendController controller)
        {
            this.Controller = controller;
            this.Email = "";
            this.password = "";
            this.nickname = "";
            this.Message = "";
        }
    }
}
