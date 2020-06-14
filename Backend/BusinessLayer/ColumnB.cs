using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IntroSE.Kanban.Backend.DAL;
using IntroSE.Kanban.Backend.DAL.DTO;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTesting")]

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class ColumnB
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;
        private Dictionary<int, TaskB> tasks {get; set;}
        private int limit;
        private int taskCounter;
        private int id;
        private string email;
        private ColumnDalController cont = new ColumnDalController();
        private TaskDalController taskDalController = new TaskDalController();

        public Dictionary<int, TaskB> Tasks { get => tasks; set => tasks = value; }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int taskcounter
        {
            get { return this.taskCounter; }
        }
        public int TaskLimit
        {
            get { return this.limit; }
           
        }
        public int Limit
        {
            get { return limit; }
        }
        public ColumnB(string name,string email)
        {
            this.taskCounter = 0;
            this.tasks = new Dictionary<int, TaskB>();
            this.name = name;
            this.limit = 100;
            this.email=email;

        }
        public ColumnB(string columnName, int place, int limit,int TaskCounter)
        {
            this.name=columnName;
            this.id=place;
            this.limit=limit;
            this.taskCounter=TaskCounter;
            this.tasks = new Dictionary<int, TaskB>();
        }
        public bool setLimit(int lim)
        {
            if (taskCounter > lim & lim!=-1)
                return false;
            this.limit = lim;
            cont.Update(this.name, email, ColumnDTO.LimColumnName, lim.ToString());
            return true;
        }
        public string Name
        {
            get { return name; }
            set {
                this.name = value;
                cont.Update(name,email , ColumnDTO.ColumnNameColumn, value);

            }
        }
        public TaskB getTask(int taskId)
        {
            if (hasTask(taskId))
                return tasks[taskId];

            return null;

        }

        public Task add(string addEmail,int id,string title, string description, DateTime dueDate,string assignto)
        {
            if (taskCounter < limit|limit==-1)
            {
                
                TaskB tas = new TaskB(addEmail,id,title,DateTime.Now,dueDate,description, assignto);
                tasks.Add(tas.Id, tas);
                TaskDTO addto = new TaskDTO(tas.Id,addEmail,tas.Title,tas.CreationTime,tas.DueDate,tas.Description,this.id,this.email);
                taskDalController.Insert(addto);
                taskCounter++;
                cont.Update(name,email, ColumnDTO.TaskCounterName,taskCounter.ToString());
                return new Task(tas.Id,tas.CreationTime, tas.Title,tas.Description,tas.Email,tas.DueDate);

            }
            throw new KanbanException("you reached to the max number of tasks allowed");
        }
        public Task add(TaskB tas)
        {
            if (taskCounter < limit | limit == -1)
            {
                taskCounter++;
                tasks.Add(tas.Id, tas);
                taskDalController.Update(tas.Id,tas.Title,tas.Email,TaskDTO.TaskColumnId,this.id.ToString());
                cont.Update(name,email, ColumnDTO.TaskCounterName, taskCounter.ToString());
                return new Task(tas.Id, tas.CreationTime, tas.Title, tas.Description,tas.Email,tas.DueDate);

            }
            throw new KanbanException("you reached to the max number of tasks allowed");
        }
        public TaskB removeTask(int id)
        {
            TaskB rtn;
            if (tasks.ContainsKey(id))
            {
                rtn = tasks[id];
                tasks.Remove(id);
                taskCounter = taskCounter - 1;
                taskDalController.Delete(rtn.Title,email,rtn.Id);
                return rtn;
            }
            return null;
        }
        public bool isFull()
        {
            if (limit == -1)
                return false;
            else
                return limit == taskCounter;
        }
        public bool hasTask(int taskId)
        {
            return tasks.ContainsKey(taskId);
        }
        public bool UpdateTaskDueDate(int id, DateTime dueDate)
        {
            if (tasks.ContainsKey(id))
            {
                tasks[id].DueDate = dueDate;
                return true;
            }
            return false;
        }
        public bool UpdateTaskTitle(int id, string title)
        {
            if (tasks.ContainsKey(id))
            {
                tasks[id].Title = title;
                return true;
            }
            return false;



        }
        public bool UpdateTaskDescription(int id, string description)
        {
            if (tasks.ContainsKey(id))
            {
                tasks[id].Description = description;
                return true;
            }
            return false;
        }

        public Column getColumn()
        {
            List<Task> lis = new List<Task>();
            if (tasks.Count != 0)
            {
                foreach (KeyValuePair<int, TaskB> entry in tasks)
                {
                    Task t = new Task(entry.Value.Id, entry.Value.CreationTime, entry.Value.Title, entry.Value.Description, entry.Value.BoardMail, entry.Value.DueDate);

                    lis.Add(t);

                }
            }
            IReadOnlyCollection<Task> readon = new ReadOnlyCollection<Task>(lis);
            Column c = new Column(readon, name, limit);
            return c;

        }
    }
}
