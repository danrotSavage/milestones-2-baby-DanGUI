using System;
using System.Collections.Generic;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.IO;
using System.Linq;

namespace IntroSE.Kanban.Backend.DAL
{
    class TaskDalController : DalController
    {
        private const string TaskTableName = "Task";


        public TaskDalController() : base(TaskTableName)
        {
           
        }

        public bool Insert(TaskDTO task)
        {
          using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();

                    command.CommandText = $"INSERT INTO {TaskTableName} ({TaskDTO.IDColumnName} ,{TaskDTO.TaskEmailColumn},{TaskDTO.TaskTitleColumn},{TaskDTO.TaskCreationDateColumn},{TaskDTO.TaskDueDateColumn},{TaskDTO.TaskBodyColumn},{TaskDTO.TaskColumnId}) " +
                        $"VALUES (@idVal,@emailVal,@titleVal,@creationDateVal,@dueDateVal,@bodyVal,@columnIdVal);";


                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.Id);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", task.Email);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter creationDateParam = new SQLiteParameter(@"creationDateVal", task.CreationDate);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter bodyParam = new SQLiteParameter(@"bodyVal", task.Body);
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnIdVal", task.ColumnId);
                    SQLiteParameter boardMailParam = new SQLiteParameter(@"boardMaildVal", task.BoardMail);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(creationDateParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(bodyParam);
                    command.Parameters.Add(columnIdParam);
                    command.Parameters.Add(boardMailParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    if (e.Message != null) { }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

                return res > 0;

            }
        }
        public bool Update(int id,string title, string email, string columnName, string insertValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TaskTableName} set [{columnName}]=@colNameVal where Email=@emailVal AND Title=@titleVal AND Id=@idVal"

                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"colNameVal", insertValue));
                    command.Parameters.Add(new SQLiteParameter(@"titleVal", title));
                    command.Parameters.Add(new SQLiteParameter(@"emailVal", email));
                    command.Parameters.Add(new SQLiteParameter(@"idVal", id));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e.Message != null) { }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;

        }
        public bool Delete(string title, string email, int id)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TaskTableName} where Email={email} AND Title={title} AND Id={id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e.Message != null) { }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public List<TaskDTO> LoadTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            return result;
        }

        protected override DTObj ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO ret = new TaskDTO(reader.GetInt32(0),reader.GetString(1),reader.GetString(2),Convert.ToDateTime(reader.GetString(3)), Convert.ToDateTime(reader.GetString(4)), reader.GetString(5),reader.GetInt32(6), reader.GetString(7));

            return ret;

        }
    }
}
