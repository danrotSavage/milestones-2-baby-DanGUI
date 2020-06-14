using System;
using System.Collections.Generic;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.IO;
using System.Linq;

namespace IntroSE.Kanban.Backend.DAL
{
    class BoardDalController : DalController
    {
        private const string BoardTableName = "Board";
        

        public BoardDalController() : base(BoardTableName)
        {
            
        }

        public bool Insert(BoardDTO board)
        {
            int res = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                {

                    try
                    {
                        command.CommandText = $"INSERT INTO {BoardTableName} ({BoardDTO.BoardEmailColumn},{BoardDTO.TaskCounterColumn})" +
                            $"VALUES (@emailVal,@taskCounterVal)";

                        SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", board.Email);
                        SQLiteParameter taskCounterParam = new SQLiteParameter(@"taskCounterVal", board.TaskCounter);
                        command.Parameters.Add(emailParam);
                        command.Parameters.Add(taskCounterParam);
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


       public Dictionary<string,BoardDTO> LoadBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            Dictionary<string, BoardDTO> output=new Dictionary<string, BoardDTO>();
            foreach(BoardDTO board in result)
                {
                output.Add(board.Email, board);
                }
            return output;
        }
        protected override DTObj ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO ret = new BoardDTO(reader.GetString(0), reader.GetInt32(1));
            return ret;

        }

    }
}
