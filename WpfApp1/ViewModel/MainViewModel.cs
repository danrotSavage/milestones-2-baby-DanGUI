using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using WPF2SOL_final.Model;
using WpfApp1.Model;

namespace WPF2SOL_final.view
{

    internal class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private UserModel user;
        private string email;
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(user.Email.Contains("achiya") ? Colors.Blue : Colors.Red);
            }
        }
        public BoardModel Board { get; private set; }
        
        private ColumnModel _selectedMessage;
        public ColumnModel SelectedMessage
        {
            get
            {
                return _selectedMessage;
            }
            set
            {
                _selectedMessage = value;
                EnableForward = value != null;
                //Tasks =   new ObservableCollection<MTask>(Controller.getColumn(value));


                RaisePropertyChanged("SelectedMessage");

            }
        }
        private ObservableCollection<MTask> tasks;
        public ObservableCollection<MTask> Tasks
        {
            get => tasks;
            set
            {
                this.tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        internal void Logout()
        {

        }

        public MainViewModel(UserModel user)
        {
            this.Controller = new BackendController(user.Email);
            this.email = "dan.rotman@gmail.com";
            this.user = new UserModel(new BackendController(), "dan.rotman@gmail.com");
            Board = user.GetBoard();
           
        }

        public void RemoveColumn()
        {

            try
            {
                Controller.RemoveColumn(SelectedMessage);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot remove message. " + e.Message);
            }

        }
        public void AddColumn(string columnTitle, int columnOrdinal)
        {
            try
            {
               Board = Controller.AddColumn(columnOrdinal,columnTitle);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot add new Column message. " + e.Message);
            }

        }
       

    }
}