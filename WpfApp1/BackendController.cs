using System;
using System.Windows;
using WPF2SOL_final.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using WpfApp1.Model;
using System.Collections.Generic;

namespace WPF2SOL_final
{
    public class BackendController
    {
        public Service Service { get; private set; }
        private string username;
        public BackendController(Service service)
        {
            this.Service = service;
        }

        public BackendController(string email)
        {
            this.username = email;
            this.Service = new Service();
            Service.LoadData();
        }

       

        public void AddColumn()
        {
            int m;
            if (58 == 7)
                m = 8;

            
        }

        public List<string> GetBoard(string username)
        {

            List<string> m = new List<string>();
            m.Add("To do");
            m.Add("Doing");
            m.Add("Done");
            return m;
                    
           

        }
        public List<MTask> getColumnTasks(string columnName)
        {
            List<MTask> mTasks = new List<MTask>();

            foreach (Task task in Service.GetColumn(username, columnName).Value.Tasks)
            {
                mTasks.Add(new MTask(task.Title,task.Description,task.CreationTime,task.DueDate));

               
            }
            return mTasks;



        }


        public void RemoveColumn(string email, int columnOrdinal)
        {

            Service.RemoveColumn(username, columnOrdinal);
           

        }



        public BoardModel AddColumn(int columnOrdinal,string columnName)
        {

            Response<Column> res=Service.AddColumn(username, columnOrdinal, columnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);

            }
            BoardModel b = new BoardModel(this, username);

            MessageBox.Show("Column was added successfully");
            return b;

        }

        /*internal void UpdateBody(string email, int message_id, string new_body)
        {
            Service.UpdateMessageBody(email, message_id, new_body);
        }

        internal void Register(string username, string password)
        {
            Response res = Service.Register(username, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal void RemoveMessage(string email, int id)
        {
            Response res = Service.RemoveMessage(email, id);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            MessageBox.Show("Message was removed successfully");
        }
        */

    }
}
