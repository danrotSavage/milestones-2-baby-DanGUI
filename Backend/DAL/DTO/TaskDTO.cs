using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL.DTO
{

    class TaskDTO :DTObj
    {
        private string title;
        private DateTime creationDate;
        private DateTime dueDate;
        private string body;
        private string email;
        private int columnId;
        private string boardMail;

        public const string TaskColumnId = "ColumnId";
        public const string TaskEmailColumn = "Email";
        public const string TaskTitleColumn = "Title";
        internal TaskDalController cont = new TaskDalController();

        public const string TaskCreationDateColumn = "CreationDate";
        public const string TaskDueDateColumn = "DueDate";
        public const string TaskBodyColumn = "Body";
        public const string BoardMailColumn = "BoardMail";
        public TaskDTO(int id, string email,string title, DateTime creationDate, DateTime dueDate,string body, int columnId,string boardMail):base( new TaskDalController())
        {
            Id = id;
            this.title = title;
            this.creationDate = creationDate;
            this.dueDate = dueDate;
            this.body = body;
            this.columnId = columnId;
            this.email = email;
            this.boardMail = boardMail;
        }

        public TaskDTO(int id) : base(new TaskDalController())
        {
            Id = id;
        }
        public string Title
        {
            get{ return title; }
            set {
               cont.Update(this.Id,this.title,this.email, TaskTitleColumn, value);
                this.title = value;
            }
        }

        public string Body
        {
            get { return body; }
            set {
                cont.Update(this.Id, this.title, this.email, TaskBodyColumn, value);
                this.body = value;
            }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                cont.Update(this.Id, this.title, this.email, TaskBodyColumn, value.ToString());
                this.dueDate = value;
            }
        }

        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                cont.Update(this.Id, this.title, this.email, TaskBodyColumn, value.ToString());
                this.creationDate = value;
            }
        }

        public int ColumnId
        {
            get { return columnId; }
            set
            {
                cont.Update(this.Id, this.title, this.email, TaskColumnId, value.ToString());
                this.columnId = value;
            }
        }

        public string Email
        {
            get { return email; }
            
        }
        public string BoardMail
        {
            get { return boardMail; }
        }
    }
}
