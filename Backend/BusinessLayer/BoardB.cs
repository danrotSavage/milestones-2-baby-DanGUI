using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DAL;
using IntroSE.Kanban.Backend.DAL.DTO;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTesting")]

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class BoardB
    {
        private string email;
        private ColumnB[] columns;
        private int tasids;
        private TaskDalController taskDalController=new TaskDalController();
        private ColumnDalController columnDalController = new ColumnDalController();
        private BoardDalController BoardDalController = new BoardDalController();

        public BoardB(string email)
        {
            this.email = email;
            tasids = 0;
            columns = new ColumnB[3];
            columns[0] = new ColumnB("backlog",email);
            columns[1] = new ColumnB("in progress", email);
            columns[2] = new ColumnB("done", email);
            for (int i = 0; i < 3; i++)
            {
                insertColumnIntoDatabse(columns[i], i, -1,this.Email);
            }

        }

        public BoardB(string email, int size,int tasids)
        {
            this.email = email;
            this.tasids = tasids;
            columns = new ColumnB[size];

        }

        public void InsertColumn(ColumnB col, int place)
        {
            columns[place] = col;
        }
       
        public int Tasids
        {
            get => tasids;
            set
            {
                tasids = value;
                BoardDalController.Update(email,BoardDTO.TaskCounterColumn,value);

            }
        }
        public ColumnB[] Columns{ get{ return columns;} set{columns=value;}}

        public string Email => this.email;

        public void insertColumnIntoDatabse(ColumnB b, int place, int limit,string email)
        {
            ColumnDTO col = new ColumnDTO(email,b.Name,place,b.Limit,b.taskcounter);
            columnDalController.Insert(col);
        }

        public Task addTask(string email,string title, string description, DateTime dueDate)
        {
            Task tas = columns[0].add(email,tasids,title, description, dueDate,email);
            Tasids++;
                return tas;
        }

        public TaskB getTask(int taskId)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].hasTask(taskId))
                {

                    return columns[i].getTask(taskId);
                }
            }
            return null;
        }

        public bool hasTask(int taskId)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].hasTask(taskId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool setLimt(int lim, int ColumnOrdinal)
        {
            if (columns[ColumnOrdinal].setLimit(lim))
            {
                return true;
            }
            return false;
        }

        public bool UpdateTaskDueDate(int column, int id, DateTime dueDate)
        {
            if (columns[column].UpdateTaskDueDate(id, dueDate) && (column < columns.Length - 1)
    )
            {
                return true;
            }
            return false;
        }

        public bool UpdateTaskTitle(int column, int id, string title)
        {
            if (columns[column].UpdateTaskTitle(id, title) && (column < columns.Length - 1))
            {
                return true;
            }
            return false;
        }

        public bool UpdateTaskDescription(int column, int id, string description)
        {
            if (columns[column].UpdateTaskDescription(id, description) && (column < columns.Length - 1))
            {
                return true;
            }
            return false;
        }
        public bool AdvanceTask(int column, int id)
        {
            if (columns[column].getTask(id) != null & (column < columns.Length - 1))
            {
                TaskB removed = columns[column].removeTask(id);
                if (removed != null)
                {
                    columns[column + 1].add(removed);
                    return true;
                }
            }
            return false;

        }

        public Column getColumn(int column)
        {
            if (column >= 0 & column < columns.Length)
                return columns[column].getColumn();
            throw new KanbanException("no such column");

        }

        public Column getColumn(string ColumnOrdinal)
        {
            foreach (ColumnB col in columns)
            {
                if (col.Name == ColumnOrdinal)
                    return col.getColumn();
            }
            throw new KanbanException("no such column");
        }

        public Column AddColumn(int ColumnOrdinal, string Name)
        {
            if (Name == null || Name.Length == 0 | Name.Length > 15)
                throw new KanbanException("the new Columns name length is illegal");
            ColumnB[] ncolumns = new ColumnB[columns.Length + 1];
            for (int i = 0; i < ColumnOrdinal; i++)
            {
                ncolumns[i] = columns[i];
            }
            ncolumns[ColumnOrdinal] = new ColumnB(Name,email);
            for (int i = ColumnOrdinal + 1; i < ncolumns.Length; i++)
            {
                ncolumns[i] = columns[i-1];
            }
            columns = ncolumns;

            insertColumnIntoDatabse(columns[ColumnOrdinal], ColumnOrdinal,-1,email);
            for (int i = ColumnOrdinal+1; i < columns.Length; i++)
            {
                columnDalController.Update(columns[i].Name, email, ColumnDTO.PlaceColumnName, i.ToString());
            }
            return columns[ColumnOrdinal].getColumn();
        }

        public Column AddColumn(int ColumnOrdinal, ColumnB col)
        {
            if (ColumnOrdinal > columns.Length || ColumnOrdinal < 0)
                throw new KanbanException(" cannot remove column with illegal ordinal");
            ColumnB[] ncolumns = new ColumnB[columns.Length + 1];
            for (int i = 0; i < ColumnOrdinal; i++)
            {
                ncolumns[i] = columns[i];
            }
            ncolumns[ColumnOrdinal] = col;
            for (int i = ColumnOrdinal + 1; i < columns.Length; i++)
            {
                ncolumns[i + 1] = columns[i];
            }
            columns = ncolumns;

            insertColumnIntoDatabse(col, ColumnOrdinal, col.Limit, email);
            for (int i = ColumnOrdinal + 1; i < columns.Length; i++)
            {
                columnDalController.Update(columns[i].Name,email, ColumnDTO.PlaceColumnName, i.ToString());
            }
            return columns[ColumnOrdinal].getColumn();
        }

        public void RemoveColumn(int ColumnOrdinal)
        {
            if (ColumnOrdinal > columns.Length - 1 || ColumnOrdinal < 0)
                throw new KanbanException(" cannot remove column with illegal ordinal");
            if (columns.Length <= 2)
                throw new KanbanException("connot remove column from board with two or less columns");
            if (ColumnOrdinal >= columns.Length)
                throw new KanbanException("column ordinal is illegal");
            ColumnB[] ncolumns = new ColumnB[columns.Length - 1];

            ColumnB removed = columns[ColumnOrdinal];
            Dictionary<int, TaskB> tas = removed.Tasks;
            int tascount = tas.Count;
            if (tascount != 0)
            {
                if (ColumnOrdinal != 0)
                {
                    if (columns[ColumnOrdinal - 1].TaskLimit==-1||columns[ColumnOrdinal - 1].TaskLimit >= columns[ColumnOrdinal - 1].taskcounter + tascount)
                    {
                        foreach (KeyValuePair<int, TaskB> task in tas)
                        {
                            columns[ColumnOrdinal - 1].add(task.Value);
                            taskDalController.Update(task.Value.Id, task.Value.Title, task.Value.Email, TaskDTO.TaskColumnId, columns[ColumnOrdinal - 1].Id.ToString());
                        }
                    }
                    else
                        throw new KanbanException(" could not move task from column bacause there is not enought place");
                }
                else
                {
                    if (columns[ColumnOrdinal + 1].TaskLimit == -1 || columns[ColumnOrdinal + 1].TaskLimit >= columns[ColumnOrdinal + 1].taskcounter + tascount)
                    {
                        foreach (KeyValuePair<int, TaskB> task in tas)
                        {
                            columns[ColumnOrdinal + 1].add(task.Value);
                            taskDalController.Update(task.Value.Id, task.Value.Title, task.Value.Email, TaskDTO.TaskColumnId, columns[ColumnOrdinal + 1].Id.ToString());

                        }
                    }
                    else
                        throw new KanbanException(" could not move task from column bacause there is not enought place");
                }
            }
            for (int i = 0; i < ColumnOrdinal; i++)
            {
                ncolumns[i] = columns[i];
            }
            for (int i = ColumnOrdinal; i < ncolumns.Length; i++)
            {
                ncolumns[i] = columns[i + 1];
            }
            columns = ncolumns;
            columnDalController.Delete(removed.Id,email);
            for (int i = ColumnOrdinal; i < columns.Length; i++)
            {
                columnDalController.Update(columns[i].Name, email, ColumnDTO.PlaceColumnName, i.ToString());
            }
        }

        public Column MoveColumnRight(int ColumnOrdinal)
        {
            if (ColumnOrdinal >= 0& ColumnOrdinal <= columns.Length - 1)
            {
                ColumnB col = columns[ColumnOrdinal];
                columns[ColumnOrdinal]=columns[ColumnOrdinal+1];
                columns[ColumnOrdinal+1]=col;
                columnDalController.Update(columns[ColumnOrdinal].Name, email, ColumnDTO.PlaceColumnName, ColumnOrdinal+1.ToString());
                columnDalController.Update(columns[ColumnOrdinal+1].Name,email, ColumnDTO.PlaceColumnName, ColumnOrdinal.ToString());
                return col.getColumn();
            }
            else
                throw new KanbanException("con not move right the first column");
        }

        public Column MoveColumnLeft(int ColumnOrdinal)
        {
            if (ColumnOrdinal >= 0 & ColumnOrdinal <= columns.Length - 1)
            {
                ColumnB col = columns[ColumnOrdinal];
                columns[ColumnOrdinal]=columns[ColumnOrdinal-1];
                columns[ColumnOrdinal-1]=col;
                columnDalController.Update(columns[ColumnOrdinal].Name,email, ColumnDTO.PlaceColumnName, (ColumnOrdinal-1).ToString());
                columnDalController.Update(columns[ColumnOrdinal-1].Name, email, ColumnDTO.PlaceColumnName, ColumnOrdinal.ToString());
                return col.getColumn();
            }
            else
                throw new KanbanException(" con not move left the last column");
        }

        internal TaskB removeTask(int taskId)
        {
            foreach(ColumnB col in columns)
            {
                if (col.hasTask(taskId))
                    return col.removeTask(taskId);
            }
            throw new KanbanException("no such task in this user's board");
        }
    }
}
