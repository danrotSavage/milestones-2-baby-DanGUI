using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL.DTO
{
    class BoardDTO :DTObj
    {
        private string email;
        private int taskCounter;

        public const string BoardEmailColumn = "Email";
        public const string TaskCounterColumn="TaskCounter";

        public BoardDTO(string email) : base(new UserDalController())
        {
            this.email = email;
            Id = 0;
        }

        public BoardDTO(string email,int TaskCounter) : base(new UserDalController())
        {
            this.email = email;
            this.taskCounter = TaskCounter;
            Id = 0;
        }
        
        public string Email
        {
            get { return email; }
            set {
                Controller.Update(email,BoardEmailColumn,value);
                email = value;
            }
        }
        public int TaskCounter
        {
            get { return taskCounter; }
            set
            {
                Controller.Update(email, TaskCounterColumn, value.ToString());
                taskCounter = value;
            }
        }

    }
}
