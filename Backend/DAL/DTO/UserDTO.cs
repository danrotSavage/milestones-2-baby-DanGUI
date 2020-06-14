using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL.DTO
{
    class UserDTO :DTObj
    {
        private string email;
        private string password;
        private string nickname;
        private string boardMail;

        public const string UserPasswordColumn = "Password";
        public const string UserNicknameColumn = "Nickname";
        public const string UserEmailColumn = "Email";
        public const string BoardMailColumn = "BoardMail";

        public UserDTO(string email) : base(new UserDalController())
        {
            this.email = email;
        }
        public UserDTO(string email, string password, string nickname,string BoardMail): base (new UserDalController())
        {

            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.boardMail = BoardMail;
            this.Id = 0;
        }
        public UserDTO(string email, string password, string nickname) : base(new UserDalController())
        {
            this.email = email;
            this.password = password;
            this.nickname = nickname;
            this.boardMail = email;
            this.Id = 0;

        }
        public string Email
        {
            get { return email; }
            set {
                Controller.Update(email,UserEmailColumn,value);
                email = value;
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                Controller.Update(email, UserPasswordColumn, value);
                password = value;
            }
        }
        public string Nickname
        {
            get { return nickname; }
            set
            {
                Controller.Update(email, UserNicknameColumn, value);
                nickname = value;
            }
        }
        public string BoardMail
        {
            get { return boardMail; }
            set
            {
                this.boardMail = value;
                Controller.Update(email,BoardMailColumn,value);
            }
        }

    }
}
