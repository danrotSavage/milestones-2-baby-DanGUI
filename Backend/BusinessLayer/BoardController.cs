using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DAL;
using IntroSE.Kanban.Backend.DAL.DTO;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTesting")]

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class BoardController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, BoardB> boards;
        private UserController uController;
        private ColumnDalController columnCont = new ColumnDalController();
        private TaskDalController taskCont = new TaskDalController();
        private BoardDalController boardCont = new BoardDalController();

        public BoardController(UserController userController)
        {
            boards = new Dictionary<string, BoardB>();
            this.uController = userController;
        }
        public void AddBoard(string email)
        {
            email = email.ToLower();
            BoardB newb = new BoardB(email);
            boards.Add(email, newb);
            BoardDTO b = new BoardDTO(email,0);
            boardCont.Insert(b);

        }
        public UserController Ucontroller
        {
            get { return this.uController; }
            set { this.uController = value; }
        }
        public Board getBoard(string email)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (!uController.loggedIn(email))
                throw new KanbanException("cannot get board of not logged in user");
            if (hasBoard(mail))
            {
                List<string> m = new List<string>();
                foreach (ColumnB col in boards[mail].Columns)
                {
                    m.Add(col.Name);
                }
                IReadOnlyCollection<string> k = new ReadOnlyCollection<string>(m);
                return new Board(k,mail);
            }
            throw new KanbanException("email does not exist");
        }

        public void LoadData()
        {
            try
            {
                foreach (KeyValuePair<string,BoardDTO> board in boardCont.LoadBoards())
                {
                    int size = columnCont.Count(board.Value.Email);
                    BoardB temp = new BoardB(board.Value.Email, size,board.Value.TaskCounter);
                    boards.Add(temp.Email, temp);
                }
                foreach (ColumnDTO col in columnCont.LoadColumns())
                {
                    ColumnB col2 = new ColumnB(col.ColumnName, col.Id,col.Limit,col.TaskCounter);
                    boards[col.Email].Columns[col.Id]=col2;
                }
                foreach (TaskDTO task in taskCont.LoadTasks())
                {
                    TaskB temp = new TaskB(task.Email,task.Id,task.Title,task.CreationDate,task.DueDate,task.Body,task.BoardMail);
                    boards[task.Email].Columns[task.ColumnId].Tasks.Add(temp.Id,temp);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void DeleteData()
        {
            columnCont.Destroy();
            taskCont.Destroy();
            boardCont.Destroy();
            this.boards=new Dictionary<string, BoardB>();
        }
        public bool hasBoard(string email)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (boards.ContainsKey(mail))
            {
                return true;
            }
            return false;


        }

        public Task addTask(string email, string title, string description, DateTime dueDate)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (!uController.UserDic.ContainsKey(email)||!boards.ContainsKey(mail))
                throw new KanbanException(" board or user do not exists");
            if ((Ucontroller.loggedIn(email)) & (0 < title.Length & title.Length <= 50) & (description == null || description.Length <= 300)&dueDate>DateTime.Now)
            {
                Task outp = boards[mail].addTask(email,title, description, dueDate);
                return outp;
            }
            else
            {
                throw new KanbanException("could not add task because task details are illegal");
            }
        }
        public TaskB getTask(string email, int taskId)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (!Ucontroller.loggedIn(email))
                throw new KanbanException("user is not logged in");
                return boards[mail].getTask(taskId);
        }

        public bool hasTask(string email, int taskId)
        {

            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!Ucontroller.loggedIn(email))
                throw new KanbanException("user is not logged in");
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            return boards[mail].hasTask(taskId);
        }

        internal void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            else
            {
                email = email.ToLower();
                string mail = uController.UserDic[email].BoardMail;
                if (!Ucontroller.loggedIn(email))
                    throw new KanbanException("user is not logged in");
                else
                {
                    if (!boards.ContainsKey(mail))
                        throw new KanbanException("board dont exists");
                    else
                    {
                        if(Boards[mail].Columns.Length -1< columnOrdinal)
                            throw new KanbanException("illegal colum ordinal");
                        else
                        {
                            if (!Boards[mail].Columns[columnOrdinal].hasTask(taskId))
                                throw new KanbanException("no such task in this column ordinal to delete delete ");
                            else
                            {
                                if (Boards[mail].Columns[columnOrdinal].getTask(taskId).Email != email)
                                    throw new KanbanException("cannot edit other user's tasks");
                                Boards[mail].Columns[columnOrdinal].removeTask(taskId);
                            }
                            
                        }

                    }
                }
            }
        }

        public Dictionary<string, BoardB> Boards
        {
            get => boards;
            set => boards = value;
        }
        public void setLimit(string email, int lim, int ColumnOrdinal)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (mail != email)
                throw new KanbanException("only boards creator can edit columns");
            if (Ucontroller.loggedIn(email))
            {
                if (!boards[mail].setLimt(lim, ColumnOrdinal))

                    throw new KanbanException("limit can not be lower than amount of tasks.");

            }
            else
            {

                throw new KanbanException("user not logged in");
            }
        }
        public void UpdateTaskDueDate(string email, int column, int id, DateTime dueDate)
        {
            if (email == null||email=="")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (Ucontroller.loggedIn(email))
            {
                if (boards[mail].Columns[column].getTask(id).Email != email)
                    throw new KanbanException("cannot edit other user's tasks");
                if (!boards[mail].UpdateTaskDueDate(column, id, dueDate))
                {

                    throw new KanbanException("task does not exist");
                }
            }
            else
            {
                throw new KanbanException("user not logged in");
            }
        }
        public void UpdateTaskTitle(string email, int column, int id, string title)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (Ucontroller.loggedIn(email))
            {
                if (boards[mail].Columns[column].getTask(id).Email != email)
                    throw new KanbanException("cannot edit other user's tasks");
                if (!boards[mail].UpdateTaskTitle(column, id, title))
                {

                    throw new KanbanException("task does not exist");
                }
            }
            else
            {
                throw new KanbanException("user not logged in");
            }
        }

        public void UpdateTaskDescription(string email, int column, int id, string description)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (Ucontroller.loggedIn(email))
            {
                if (boards[mail].Columns[column].getTask(id).Email != email)
                    throw new KanbanException("cannot edit other user's tasks");
                if (!boards[mail].UpdateTaskDescription(column, id, description))
                {

                    throw new KanbanException("task does not exist");
                }
            }
            else
            {
                throw new KanbanException("user not logged in");
            }
        }

        public void AdvanceTask(string email, int column, int id)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (boards[mail].Columns[column].getTask(id).Email != email)
                throw new KanbanException("cannot edit other user's tasks");
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (!boards.ContainsKey(email))
                throw new KanbanException("email is not exist");
            if (Ucontroller.loggedIn(email))
            {
                if (!boards[mail].AdvanceTask(column, id))
                {
                    throw new KanbanException("task does not exist");
                }
            }
            else
            {
                throw new KanbanException("user not logged in");
            }
        }

        public Column getColumn(string email, int column)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (Ucontroller.loggedIn(email))
            {
                Column rtn = boards[mail].getColumn(column);

                return rtn;
            }
            throw new KanbanException("user not logged in");
        }
        public Column getColumn(string email, string column)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            string newm = email.ToLower();
            string mail = uController.UserDic[newm].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (Ucontroller.loggedIn(newm))
            {
                try
                {
                    return  boards[mail].getColumn(column);
                }
                catch (KanbanException e)
                {
                    throw new KanbanException(e.Message);

                }
                catch (Exception e)
                {
                    throw new KanbanException(e.Message);

                }

            }
            else
            {
                throw new KanbanException("user is not logged in");
            }
        }
        public Column AddColumn(string email, int ColumnOrdinal, string Name)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            else
            {
                if (mail != email)
                    throw new KanbanException("only boards creator can change columns");
                if (uController.loggedIn(email))
                {
                    return boards[mail].AddColumn(ColumnOrdinal, Name);
                }
                else
                    throw new KanbanException("there was a try to remove a column to a user that is not logged in");

            }
        }
        public void RemoveColumn(string email, int ColumnOrdinal)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (boards.ContainsKey(email))
            {
                if (mail != email)
                    throw new KanbanException("only boards creator can change columns");
                if (uController.loggedIn(email))
                {
                    boards[mail].RemoveColumn(ColumnOrdinal);
                }
                else
                    throw new KanbanException("there was a try to remove a column to a user that is not logged in");
            }
            else
                throw new KanbanException("there is no board for this email");
        }
        public Column MoveColumnRight(string email, int ColumnOrdinal)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (boards.ContainsKey(mail))
            {
                if (mail != email)
                    throw new KanbanException("only boards creator can change columns");
                if (uController.loggedIn(email))
                {
                    return boards[mail].MoveColumnRight(ColumnOrdinal);
                }
                else
                    throw new KanbanException("there was a try to move a column to a user that is not logged in");
            }
            else
                throw new KanbanException("there is no board for this email");
        }
        public Column MoveColumnLeft(string email, int ColumnOrdinal)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;
            if (!boards.ContainsKey(mail))
                throw new KanbanException("board dont exists");
            if (boards.ContainsKey(mail))
            {
                if (mail != email)
                    throw new KanbanException("only boards creator can change columns");
                if (uController.loggedIn(email))
                {
                    return boards[mail].MoveColumnLeft(ColumnOrdinal);
                }
                else
                    throw new KanbanException("there was a try to move a column to a user that is not logged in");
            }
            else
                throw new KanbanException("there is no board for this email");
        }

        internal void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (email == null || email == "")
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            string mail = uController.UserDic[email].BoardMail;

            if (!boards.ContainsKey(emailAssignee))
                throw new KanbanException("no such user for add task to");
            else
            {
                if (!Ucontroller.loggedIn(email))
                    throw new KanbanException("cannot add sign to user who is not logged in");
                else
                {
                    if (boards[email].Columns.Length - 1 < columnOrdinal)
                        throw new KanbanException(" illegal column ordinal");
                    if (!boards[email].Columns[columnOrdinal].hasTask(taskId))
                        throw new KanbanException(" task do not exist");
                    TaskB task = boards[email].Columns[columnOrdinal].removeTask(taskId);
                    if (task.BoardMail != email)
                        throw new KanbanException("only boards creator can change columns");
                    Task tas=boards[mail].Columns[0].add(mail, taskId,task.Title,task.Description,task.DueDate, emailAssignee);

                }
            }
        }
    }
}
