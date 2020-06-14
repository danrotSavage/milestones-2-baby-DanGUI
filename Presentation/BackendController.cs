using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Windows;

namespace Presentation
{

    public class BackendController
    {

        public IService service { get; private set; }

        public BackendController(IService _service)
        {
            this.service = _service;
            service.LoadData();
        }
        public BackendController()
        {
            this.service = new Service();
           service.LoadData();
        }
        public UserModel Login(string username, string password)
        {
            Response<User> user = service.Login(username, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        public void Register(string username, string password,string nickname,string emailHost)
        {
            Response user;
            if (string.IsNullOrEmpty(emailHost)||string.IsNullOrWhiteSpace(emailHost))
                user = service.Register(username, password,nickname);
            else
                user = service.Register(username, password, nickname,emailHost);
            Console.WriteLine("response: "+user.ErrorMessage);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
        }

        public List<MTask> addTask(string email,string title,string dueDate,string description)
        {
            try
            {
                DateTime m = DateTime.Parse(dueDate);
                service.AddTask(email, title, description, m);
                return this.getColumnTasks(email, 0);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public BoardModel GetBoard(string email)
        {
            Response<Board> board = service.GetBoard(email);
            if (board.ErrorOccured)
            {
                MessageBox.Show("failed getting board");
                throw new Exception("could not get board ");
            }
            else
            {
                BoardModel bm = new BoardModel(this, board.Value.emailCreator, board.Value.ColumnsNames);

                return bm;
            }
        }

        internal void reset()
        {
            service.DeleteData();
        }

        public List<MTask> getColumnTasks(string email,string columnName)
        {
            List<MTask> mTasks = new List<MTask>();
            Response<Column> col = service.GetColumn(email, columnName);
            if (col.Value.Tasks!=null)
            {
                foreach (IntroSE.Kanban.Backend.ServiceLayer.Task task in col.Value.Tasks)
                {
                    mTasks.Add(new MTask(task.Id,task.Title, task.Description, task.CreationTime, task.dueDate));


                }
            }
            return mTasks;
        }
        public List<MTask> getColumnTasks(string email,int column)
        {
            List<MTask> mTasks = new List<MTask>();
            Response<Column> col = service.GetColumn(email, column);
            if (col.Value.Tasks != null)
            {
                foreach (IntroSE.Kanban.Backend.ServiceLayer.Task task in col.Value.Tasks)
                {
                    mTasks.Add(new MTask(task.Id,task.Title, task.Description, task.CreationTime, task.dueDate));


                }
            }
            return mTasks;
        }

        public BoardModel RemoveColumn(string email, int columnOrdinal)
        {

            Response resp = service.RemoveColumn(email, columnOrdinal);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            BoardModel b = this.GetBoard(email);

            MessageBox.Show("Column was removed successfully");
            return b;

        }
        public void LimitColumn(string email, int columnOrdinal,int limit)
        {

            Response resp = service.LimitColumnTasks(email, columnOrdinal,limit);
            if (resp.ErrorOccured)
                throw new Exception(resp.ErrorMessage);
            

            MessageBox.Show("Column was Limited successfully");

        }



        public BoardModel AddColumn(string email,int columnOrdinal, string columnName)
        {

            Response<Column> res = service.AddColumn(email, columnOrdinal, columnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);

            }

            BoardModel b = this.GetBoard(email);

            MessageBox.Show("Column was added successfully");
            return b;

        }
        public BoardModel MoveRight(string email, int columnOrdinal)
        {

            Response<Column> res = service.MoveColumnRight(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);

            }

            BoardModel b = this.GetBoard(email);

            MessageBox.Show("Column was moved successfully");
            return b;

        }
        public BoardModel MoveLeft(string email, int columnOrdinal)
        {

            Response<Column> res = service.MoveColumnLeft(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

            BoardModel b = this.GetBoard(email);

            MessageBox.Show("Column was moved successfully");
            return b;

        }
        public void AdvanceTask(string email, int columnOrdinal,int taskId)
        {

            Response res = service.AdvanceTask(email, columnOrdinal,taskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            
            MessageBox.Show("task was moved successfully");
            

        }
    }
}
