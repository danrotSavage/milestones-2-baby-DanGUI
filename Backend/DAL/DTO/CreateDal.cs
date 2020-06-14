/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;



    class CreateDal
    {
        static void Main(string[] args)
        {

         

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            Console.WriteLine(path);
            string connectionString = $"Data Source= {path} + ; Version=3";


            const string Id = "id";


            const string EmailForeignKey = "Emailforeign";
            const string ColumnName = "ColumnName";
            const string PlaceName = "Place";
            const string LimitName = "ColumnLimit";

            const string UserTableName = "User";
            const string EmailColumn = "Email";
            const string PasswordColumn = "Password";
            const string NicknameColumn = "Nickname";

            const string TaskTableName = "Task";
            const string TaskInColumn = "Column";
            const string TaskTitle = "Title";
            const string TaskCreationDate = "Creation Date";
            const string TaskDueDate = "Due Date";
            const string TaskBody = "Body";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(null, connection))
                {
                    int res = -1;

                    try
                    {
                    


                        command.CommandText = $"CREATE TABLE {UserTableName} ({Id} INTEGER PRIMARY KEY AUTOINCREMENT,{EmailColumn} TEXT PRIMARY KEY, {PasswordColumn} TEXT, {NicknameColumn} TEXT)";
                        command.Prepare();
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("2");

                        command.CommandText = $"CREATE TABLE {BoardTableName} ( {ColumnName} TEXT , {PlaceName} TEXT,{LimitName} INT, {EmailForeignKey} TEXT, FOREIGN KEY ({EmailForeignKey}) REFERENCES {UserTableName} ({EmailColumn}))";
                        command.Prepare();
                        command.ExecuteNonQuery();
                        command.CommandText = $"CREATE TABLE {TaskTableName} ({TaskID} INTEGER, {TaskInColumn} INTEGER, {TaskTitle} TEXT, {TaskCreationDate} TEXT, {TaskDueDate} TEXT, {TaskBody} TEXT, PRIMARY KEY({TaskID}))";
                        command.Prepare();
                        command.ExecuteNonQuery();
                        SQLiteParameter idParam = new SQLiteParameter(@"idVal", 5);
                        SQLiteParameter carNameParam = new SQLiteParameter(@"carName", "no one can stop me im all the way up");

                        SQLiteParameter priceParam = new SQLiteParameter(@"priceVal", 32400);
                        SQLiteParameter carnameParam = new SQLiteParameter(@"carnameVal", "vroom vroom");




                        command.Parameters.Add(idParam);
                        command.Parameters.Add(carNameParam);
                        command.Parameters.Add(priceParam);
                        command.Parameters.Add(carnameParam);







                        Console.WriteLine("3");

                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(command.CommandText);
                        Console.WriteLine(e.ToString());
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                        Console.WriteLine(res);
                    }

                }










                Console.ReadLine();
            }

        }
    
}
*/