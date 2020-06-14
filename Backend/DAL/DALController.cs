using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.Data.SQLite;

using System.IO;

namespace IntroSE.Kanban.Backend.DAL

{
    internal abstract class DalController
    {
        protected readonly string connectionString;
        private readonly string tableName;
        public DalController(string tableName)
        {

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            connectionString = $"Data Source= {path}; Version=3;";
            this.tableName = tableName;
        }
        public string ConnectionString
        {
            get { return connectionString; }
        }
        public bool Update(string email, string columnName, object insertValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{columnName}]=@{columnName} where Email={email}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, insertValue));
                    connection.Open();
                    res=command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    if (e.Message!=null) { }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        
    }
        public bool Destroy()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                {
                    try
                    {
                        command.CommandText = $"delete from {tableName}";
                        command.Prepare();
                        connection.Open();
                        command.ExecuteNonQuery();
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
                return res > 0;

            }
        }
        public List<DTObj> Select()
        {
            List<DTObj> results = new List<DTObj>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);         
                command.CommandText = $"select * from {tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                finally
                {
                    if (dataReader != null){
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }
                
            }
            return results;
            }
      
        protected abstract DTObj ConvertReaderToObject(SQLiteDataReader reader);


    }
}
