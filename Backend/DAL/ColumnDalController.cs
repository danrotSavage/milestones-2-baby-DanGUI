using System;
using System.Collections.Generic;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.IO;
using System.Linq;

namespace IntroSE.Kanban.Backend.DAL
{
    class ColumnDalController : DalController
    {
        private const string ColumnTableName = "Column";


        public ColumnDalController() : base(ColumnTableName)
        {
           
        }

        public bool Insert(ColumnDTO column)
        {


            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);

                try
                {

                    connection.Open();

                    command.CommandText = $"INSERT INTO {ColumnTableName} ({ColumnDTO.EmailColumnName},{ColumnDTO.ColumnNameColumn},{ColumnDTO.PlaceColumnName},{ColumnDTO.LimColumnName},{ColumnDTO.TaskCounterName})" +
                        $"VALUES (@emailVal,@nameVal,@placeVal,@limVal,@taskCounter);";


                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", column.Email);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", column.ColumnName);
                    SQLiteParameter placeParam = new SQLiteParameter(@"placeVal", column.Id);
                    SQLiteParameter limParam = new SQLiteParameter(@"limVal", column.Limit);
                    SQLiteParameter taskCounetrParam = new SQLiteParameter(@"taskCounter", column.TaskCounter);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(placeParam);
                    command.Parameters.Add(limParam);
                    command.Parameters.Add(taskCounetrParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e != null)
                        res = -1;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                
                return res > 0;

            }
        }

        public bool Delete(int id, string email)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {ColumnTableName} where Email='{email}' AND Place={id}"
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
        public bool Update(string name,string email, string columnName, string insertValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {ColumnTableName} set [{columnName}]=@colNameVal where Name=@nameVal AND Email=@emailVal"

                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"colNameVal", insertValue));
                    command.Parameters.Add(new SQLiteParameter(@"nameVal", name));
                    command.Parameters.Add(new SQLiteParameter(@"emailVal",email));
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
        public List<ColumnDTO> LoadColumns()
        {
            List<ColumnDTO> result = Select().Cast<ColumnDTO>().ToList();
            return result;
        }
        protected override DTObj ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO ret = new ColumnDTO(reader.GetString(0),reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3),  reader.GetInt32(4));
            return ret;

        }
        public int Count(string email)
        {
            int res = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                {

                    try
                    {
                        command.CommandText = $"SELECT * FROM {ColumnTableName} WHERE {ColumnDTO.EmailColumnName}=@emailVal";
                        SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", email);
                        command.Parameters.Add(emailParam);
                        command.Prepare();
                        connection.Open();
                        SQLiteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            res += 1;
                        }
                    }
                    catch (Exception e)
                    {
                        if (e != null)
                            res = -1;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
            }
            return res;
        } 
    }
}