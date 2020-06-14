using System;
using IntroSE.Kanban.Backend.BussinessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntroSE.Kanban.Backend.ServiceLayer

{
    public class BoardService 
    {
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoardController bController;
        private UserService uService;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:IntroSE.Kanban.Backend.ServiceLayer.BoardService"/> class.
        /// </summary>
        /// <param name="u">U.</param>
        public BoardService(UserService u)
        {
            this.bController = new BoardController(u.getUcontroller());
            this.uService = u;
        }


        public Response Register(string email)
        {
            try
            {
                bController.AddBoard(email);
                log.Info("board registered successfully to the new user");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("Board could not be added" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Board could not be added" + e.Message);
                return new Response(e.Message);
            }
        }
        public Response Register(string email, string password,string nickname,string emailHost)
        {
            try
            {
                if (bController.hasBoard(emailHost))
                    this.uService.Register(email,password,nickname,emailHost);
                log.Info("board registered successfully to the new user");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("Board could not be added" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Board could not be added" + e.Message);
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Gets the board.
        /// </summary>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        public Response<Board> GetBoard(string email)
        {
            Board rtn;
            try
            {
                rtn = bController.getBoard(email);
                log.Info("Managed to retrieve the board");
                return new Response<Board>(rtn);
            }
            catch (KanbanException e)
            {
                log.Warn("bs couldnt get board after kanban exception" + e.Message);
                return new Response<Board>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("bs couldnt get board after kanban exception" + e.Message);
                return new Response<Board>(e.Message);
            }
        }

        public Response DeleteData()
            {
                try
                {
                    bController.DeleteData();
                    log.Info("all board data has been successfully deleted");
                    return new Response();
                }
                catch (KanbanException e)
                {
                    log.Warn("data could not be deleted, error:" + e.Message);
                    return new Response(e.Message);
                }
                catch (Exception e)
                {
                    log.Error("data could not be deleted, error:" + e.Message);
                    return new Response(e.Message);
                }
            }

        public Response RemoveColumn(string email, int columnOrdinal)
        {
            try
            {
                bController.RemoveColumn(email, columnOrdinal);
                log.Info("Column successfully removed");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("Column could not be deleted: error" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Column could not be deleted: error" + e.Message);
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
        {
            try
            {
                Column rtn = bController.AddColumn(email, columnOrdinal, Name);
                log.Info("Column added successfully");
                return new Response<Column>(rtn);
            }
            catch (KanbanException e)
            {
                log.Warn("Column could not be added: error" + e.Message);
                return new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Column could not be added: error" + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        internal Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            try
            {
                bController.DeleteTask(email,columnOrdinal,taskId);
                return new Response();
            }
            catch(KanbanException e)
            {
                log.Warn("Column could not be added: error" + e.Message);
                return new Response<Column>(e.Message);
            }
            catch(Exception e)
            {
                log.Error("Column could not be added: error" + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        /// <returns>A response object with a value set to the column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(string email, int columnOrdinal)
        {
            try
            {
                Column rtn = bController.MoveColumnRight(email, columnOrdinal);
                return new Response<Column>(rtn);
            }
            catch (KanbanException e)
            {
                log.Warn("Column could not be Moved properly: error" + e.Message);
                return new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("Column could not be Moved properly: error" + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the columns</param>
        /// <returns>A response object with a value set to the column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
        {
            try
            {
                Column rtn = bController.MoveColumnLeft(email, columnOrdinal);
                return new Response<Column>(rtn);
            }
            catch (KanbanException e)
            {
                log.Warn("Column could not be Moved properly: error" + e.Message);
                return new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Warn("Column could not be Moved properly: error" + e.Message);
                return new Response<Column>(e.Message);
            }
        }

        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="columnName">Column name.</param>
        public Response<Column> GetColumn(string email, string columnName) {
            
            try
            {
                Column rtn = bController.getColumn(email, columnName);
                log.Info("managed to retrieve the column");
                return new Response<Column>(rtn);
            }
            catch (KanbanException e)
            {
                log.Warn("column not retrieved, Error:" + e.Message);
            return new Response<Column>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("column not retrieved, Error:" + e.Message);
                return new Response<Column>(e.Message);
            }
        }
        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="column">Column.</param>
        public Response<Column> GetColumn(string email, int column)
        {
            Column rtn;
            try
            {
                rtn = bController.getColumn(email, column);
                log.Info("managed to retrieve the column");
                return new Response<Column>(rtn);
            }
            catch (KanbanException e)
            {
                log.Warn("failed to get Column" + e.Message);
                return new Response<Column>(e.Message);
            }
            catch (Exception e)
            { 
                log.Error("failed to get Column" + e.Message);
                return new Response<Column>(e.Message);
            }
        }





        /// <summary>
        /// Adds the task.
        /// </summary>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="title">Title.</param>
        /// <param name="description">Description.</param>
        /// <param name="dueDate">Due date.</param>
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            try
            {
                Task output = bController.addTask(email, title, description, dueDate);
                return new Response<Task>(output);
            }
            catch (KanbanException e)
            {
                log.Warn("bs failed to add the task, Error" + e.Message);
                return new Response<Task>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("bs failed to add the task, Error" + e.Message);
                return new Response<Task>(e.Message);
            }
        }

        /// <summary>
        /// Limits the column tasks.
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="columnOrdinal">Column ordinal.</param>
        /// <param name="limit">Limit.</param>
        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            try
            {
                bController.setLimit(email, limit, columnOrdinal);
                log.Info("applied limit the amount of tasks in a column");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("could not set limit, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("could not set limit, Error:" + e.Message);
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Updates the task due date.
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="column">Column.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="dueDate">Due date.</param>
        public Response UpdateTaskDueDate(string email, int column, int id, DateTime dueDate)
        {

            try
            {
                bController.UpdateTaskDueDate(email, column, id, dueDate);
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("DueDate could not be updated, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            { 
                log.Error("DueDate could not be updated, Error:" + e.Message);
                return new Response(e.Message);
            }


        }

        /// <summary>
        /// Updates the task title.
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="column">Column.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="title">Title.</param>
        public Response UpdateTaskTitle(string email, int column, int id, string title)
        {
            try
            {
                bController.UpdateTaskTitle(email, column, id, title);
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("task's title could not be updated, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("task's title could not be updated, Error:" + e.Message);
                return new Response(e.Message);
            }

        }


        /// <summary>
        /// Updates the task description.
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="column">Column.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="description">Description.</param>
        public Response UpdateTaskDescription(string email, int column, int id, string description)
        {
            try
            {
                bController.UpdateTaskDescription(email, column, id, description);
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("task's description could not be updated, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("task's description could not be updated, Error:" + e.Message);
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Advances the task.
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        /// <param name="email">Email.</param>
        /// <param name="column">Column.</param>
        /// <param name="id">Identifier.</param>
        public Response AdvanceTask(string email, int column, int id )
        {
             try
            {
                bController.AdvanceTask(email, column, id);
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("task could not be advanced, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("task could not be advanced, Error:" + e.Message);
                return new Response(e.Message);
            }
        }



        /// <summary>
        /// Loads the boards data.
        /// </summary>
        public Response loadBoardsData(UserService us)
        {
            try
            {
            bController = new BoardController(us.getUcontroller());
            bController.LoadData();
            bController.Ucontroller = us.getUcontroller();
            return new Response();
            }
            catch(KanbanException e)
            {
                log.Warn("could not load the boards, Error:" +e.Message);
                return new Response(e.Message);
            }
            catch(Exception e)
            {
                log.Error("could not load the boards, Error:" +e.Message);
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Assign a tasl to another user.
        /// </summary>
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                bController.AssignTask(email,columnOrdinal,taskId,emailAssignee);
                return new Response();
            }
            catch(KanbanException e)
            {
                log.Warn("could not load the boards, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch(Exception e)
            {
                log.Error("could not load the boards, Error:" + e.Message);
                return new Response(e.Message);
            }
        }
    }
}
