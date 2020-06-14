using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KanbanUnitTest")]

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;
        internal User(string email, string nickname)
        {
            this.Email = email;
            this.Nickname = nickname;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;
            else
            {
                User user = (User)obj;
                if (user.Email == this.Email && user.Nickname == this.Nickname)
                    return true;
                else
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
