using System;
using IntroSE.Kanban.Backend.DAL;
using IntroSE.Kanban.Backend.DAL.DTO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTesting")]

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class UserB
    {
        private string email;
        private string password;
        private string nickname;
        private string boardMail; 
        private UserDalController cont = new UserDalController();

        public UserB(string email, string password, string nickname)
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.boardMail = email;
        }
        public UserB(string email, string password, string nickname,string boardMail)
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.boardMail = boardMail;
        }
        public void editPass(string newPass, string oldPass)
        {
            if (oldPass==this.password)
            {
                this.password = newPass;
                cont.Update(Email, UserDTO.UserPasswordColumn, newPass);
            }
        }

        public bool login(string pass)
        {
            return this.password == pass;


        }
        public string Email => this.email;
        public string Nickname => this.nickname;
        public string BoardMail => this.boardMail;
    }
}
