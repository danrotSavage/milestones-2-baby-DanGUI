using System;
using IntroSE.Kanban.Backend.DAL;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTesting")]

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class TaskB
    {
        private int id;
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        private TaskDalController cont = new TaskDalController();
        private string email;
        private string boardMail;

        public TaskB(string email, int id,string title, DateTime creationDate, DateTime dueDate,string body,string boardMail)
        {
            this.email = email;
            this.id=id;
            this.title = title;
            this.description = body;
            this.creationTime = creationDate;
            this.dueDate = dueDate;
            this.boardMail = boardMail;

        }
        public string Email
        {
            get { return email; } set{ email = value; } 
        }
        public int Id
        {
            get { return id; }
            set {id =value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value;
                cont.Update(id,title,email, TaskDTO.TaskTitleColumn, value);
            }
        }
        public string Description
        {
            get { return description; }
            set { description = value;
                cont.Update(id,title, email, TaskDTO.TaskBodyColumn, value);
            }
        }
        // check
        public DateTime CreationTime
        {
            get { return creationTime; }
        }
        public DateTime DueDate
        {
            get { return dueDate; }
            set {
                dueDate = value;
                cont.Update( id, title,email, TaskDTO.TaskDueDateColumn, value.ToString());

            }
        }
        public string BoardMail
        {
            get { return boardMail;}

        }
    }
}
