using System;
using System.Collections.Generic;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.IO;
using System.Linq;

namespace IntroSE.Kanban.Backend.DAL
{
    class UserDalController : DalController
    {
        private const string UserTableName = "User";
        

        public UserDalController() : base(UserTableName)
        {
            
        }

        public bool Insert(UserDTO user)
        {
            int res = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                {

                    try
                    {
                        command.CommandText = $"INSERT INTO {UserTableName} ({UserDTO.UserEmailColumn},{UserDTO.UserPasswordColumn},{UserDTO.UserNicknameColumn},{UserDTO.BoardMailColumn})" +
                            $"VALUES (@emailVal,@passwordVal,@nicknameVal,@boardMailVal)";

                        SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                        SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                        SQLiteParameter nicknameParam = new SQLiteParameter(@"nicknameVal", user.Nickname);
                        SQLiteParameter boardMailParam = new SQLiteParameter(@"boardMailVal", user.BoardMail);
                        command.Parameters.Add(emailParam);
                        command.Parameters.Add(passwordParam);
                        command.Parameters.Add(nicknameParam);
                        command.Parameters.Add(boardMailParam);
                        command.Prepare();
                        connection.Open();

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

                }
            }

            return res > 0;

        }


       public Dictionary<string,UserDTO> LoadUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            Dictionary<string,UserDTO> output=new Dictionary<string, UserDTO>();
            foreach(UserDTO user in result)
                {
                output.Add(user.Email,user);
                }
            return output;
        }
        protected override DTObj ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO ret = new UserDTO(reader.GetString(0), reader.GetString(1),  reader.GetString(2), reader.GetString(3));

            return ret;

        }

    }
}
