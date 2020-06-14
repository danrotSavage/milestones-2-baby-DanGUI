using Presentation;
using Presentation.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Presentation.view
{

    internal class BoardViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private UserModel user;
        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(user.Email.Contains("achiya") ? Colors.Blue : Colors.Red);
            }
        }
        public BoardModel board;
        public BoardModel Board
        {
            get => board;
            private set
            {
                board = value;
                RaisePropertyChanged("Board");
            }
        }
        private ColumnModel _selectedMessage;
        private MTask _selectedTask;
        public MTask SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                EnableForwardTask = value != null;


                RaisePropertyChanged("SelectedTask");

            }
        }

        public void AddColumn(string columnTitle,string columnOrdinal)
        {
            try
            {
                int ord = Int32.Parse(columnOrdinal);
                Board = Controller.AddColumn(user.Email,ord, columnTitle);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot add new Column message. " + e.Message);
            }
        }
        public int getColumnOrdinal(string columnTitle)
        {
            int i = 0;
            foreach (ColumnModel col in board.ColumnNames)
            {
                if (col.Name == columnTitle)
                    return i;
                i++;
            }
            return i;
        }

        internal void LimitColumn()
        {
            throw new NotImplementedException();
        }

        public ColumnModel SelectedMessage
        {
            get
            {
                return _selectedMessage;
            }
            set
            {
                _selectedMessage = value;
                EnableForwardColumn = value != null;
                if (value!=null)
                    Tasks =   new ObservableCollection<MTask>(Controller.getColumnTasks(user.Email,value.Name));


                RaisePropertyChanged("SelectedMessage");

            }
        }

        public void addTask(string title, string dueDate, string description)
        {
            try
            {
                this.Tasks = new ObservableCollection<MTask>(Controller.addTask(user.Email, title, dueDate, description));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            }

        private ObservableCollection<MTask> tasks;
        public ObservableCollection<MTask> Tasks
        {
            get => tasks;
            set
            {
                if (value != null)
                {
                    this.tasks = value;
                    RaisePropertyChanged("Tasks");
                }
            }
        }
        
        private bool _enableForwardColumn = false;
        public bool EnableForwardColumn
        {
            get => _enableForwardColumn;
            private set
            {
                _enableForwardColumn = value;
                RaisePropertyChanged("EnableForwardColumn");
            }
        }
        private bool _enableForwardTask = false;
        public bool EnableForwardTask
        {
            get => _enableForwardTask;
            private set
            {
                _enableForwardTask = value;
                RaisePropertyChanged("EnableForwardTask");
            }
        }

        internal void Logout()
        {

        }

        public BoardViewModel(UserModel user,BackendController cont)
        {
            this.Controller =cont;
            this.user = user;
            Board =Controller.GetBoard(user.Email);
           
        }
        public BoardViewModel(UserModel user)
        {
            this.Controller = new BackendController();
            this.user = user;
            Board = Controller.GetBoard(user.Email);

        }

        public void RemoveColumn()
        {

            try
            {
                Board = Controller.RemoveColumn(user.Email, getColumnOrdinal(SelectedMessage.Name));
                Tasks = null;
                SelectedMessage = null;
                EnableForwardColumn = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot remove message. " + e.Message);
            }
        }
        public void MoveRight()
        {

            try
            {
                Board = Controller.MoveRight(user.Email, getColumnOrdinal(SelectedMessage.Name));
                Tasks = new ObservableCollection<MTask>(Controller.getColumnTasks(user.Email, 0));
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot move column. " + e.Message);
            }
        }
        public void MoveLeft()
        {

            try
            {
                Board = Controller.MoveLeft(user.Email, getColumnOrdinal(SelectedMessage.Name));
                Tasks = new ObservableCollection<MTask>(Controller.getColumnTasks(user.Email, 0));
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot move column " + e.Message);
            }
        }
        public void AdvanceTask()
        {

            try
            {
                Controller.AdvanceTask(user.Email, getColumnOrdinal(SelectedMessage.Name),SelectedTask.Id);
                Tasks = new ObservableCollection<MTask>(Controller.getColumnTasks(user.Email, 0));
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot advance task " + e.Message);
            }
        }
        public void LimitColumn(string limit)
        {

            try
            {
                Controller.LimitColumn(user.Email, getColumnOrdinal(SelectedMessage.Name),Int32.Parse(limit));
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot limit column " + e.Message);
            }
        }
      


    }
}