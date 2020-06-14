using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    interface IBoardService
    {
        Response Register(string email);
        Response<Board> GetBoard(string email);
        Response DeleteData();
        Response RemoveColumn(string email, int columnOrdinal);
        Response<Column> AddColumn(string email, int columnOrdinal, string Name);
        Response<Column> MoveColumnRight(string email, int columnOrdinal);
        Response<Column> MoveColumnLeft(string email, int columnOrdinal);
        Response<Column> GetColumn(string email, string columnName);
        Response<Column> GetColumn(string email, int column);
        Response<Task> AddTask(string email, string title, string description, DateTime dueDate);
        Response LimitColumnTasks(string email, int columnOrdinal, int limit);
        Response UpdateTaskDueDate(string email, int column, int id, DateTime dueDate);
        Response UpdateTaskTitle(string email, int column, int id, string title);
        Response UpdateTaskDescription(string email, int column, int id, string description);
        Response AdvanceTask(string email, int column, int id);
        Response loadBoardsData(UserService us);
    }
}
