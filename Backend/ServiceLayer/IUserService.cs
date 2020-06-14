using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    interface IUserService
    {
        Response Register(string email, string password, string nickname);
        Response DeleteData();
        Response Logout(string email);
        Response<User> Login(string email, string password);
        Response loadUsersData();
    }
}