using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL.DTO
{
    class ColumnDTO:DTObj
    {
        private string columnName;
        private int limit;
        private string email;
        private int taskCounter;

        public const string PlaceColumnName = "Place";
        public const string LimColumnName = "Lim";
        public const string ColumnNameColumn = "Name";
        public const string EmailColumnName = "Email";
        public const string TaskCounterName="TaskCounter";





        public ColumnDTO(string email,string columnName, int place, int limit,int TaskCounter) :base( new ColumnDalController())
        {
            this.columnName = columnName;
            this.Id = place;
            this.limit = limit;
            this.email = email;
            this.TaskCounter=TaskCounter;
        }
        public int TaskCounter
        {
            get{return taskCounter;}
            set {this.taskCounter=value;}
        }
        public int Limit
        {
            get { return limit; }
            set
            {
                limit = value;
            }
        }
        public string ColumnName
        {
            get { return columnName; }
          
        }
        public string Email
        {
            get { return email; }

        }
    }
}
