namespace Presentation.Model
{
    public class UserModel:NotifiableModelObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public UserModel(BackendController Controller, string email):base(Controller)
        {
            this.Email = email;
        }
    }
}