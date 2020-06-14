using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BussinessLayer;

namespace WpfApp1.Model
{
    public class MTask
    {
        private int id;
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        private Service service;
        private String email;
        private int colID;

        public MTask(TaskB t, string email, int colID)
        {//while loading a board
            this.colID = colID;
            this.email = email;
            id = t.Id;
            title = t.Title;
            description = t.Description;
            creationTime = t.CreationTime;
            dueDate = t.DueDate;
            service = new Service();
        }

        public MTask(string Title, string Description, DateTime CreationTime, DateTime DueDate)
        {
            title = Title;
            description = Description;
            creationTime = CreationTime;
            dueDate = DueDate;
            service = new Service();
        }
        public MTask(string Title, string Description)
        {
            title = Title;
            description = Description;
            
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                //String s = service.UpdateTaskTitle(email, colID, id, title).Message;
                //need to continue
            }
        }
        public string Description
        {
            get => description; set
            {
                description = value;
                //String s = service.UpdateTaskDescription(email, colID, id, description).Message;
            }
        }
        public DateTime CreationTime
        {
            get => creationTime;
        }
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                //String s = service.UpdateTaskTitle(email, colID, id, title).Message;
            }
        }
    }
}