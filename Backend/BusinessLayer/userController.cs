using IntroSE.Kanban.Backend;
using IntroSE.Kanban.Backend.DAL;
using IntroSE.Kanban.Backend.DAL.DTO;
using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Globalization;

[assembly: InternalsVisibleTo("KanbanUnitTesting")]

namespace IntroSE.Kanban.Backend.BussinessLayer
{
    public class UserController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, UserB> userDic;
        private UserDalController usercont = new UserDalController();

        public Dictionary<string, UserB> UserDic
        {
            get{return userDic;}
        }
        private string loggenIn;
        private UserDalController cont = new UserDalController();

        public User getUser(string email)
        {
            User user = new User(userDic[email].Email, userDic[email].Nickname);
            return user;
        }
        public UserController()
        {
            userDic = new Dictionary<string, UserB>();
            loggenIn = null;
        }
        public bool loggedIn(string email)
        {
            if ((email == loggenIn))
            {
                return true;
            }
                return false;
        }
        public void LoadData()
        {
            Dictionary<string,UserDTO> users=cont.LoadUsers();
            foreach (KeyValuePair<string,UserDTO> user in users)
            {
                UserB b = new UserB(user.Value.Email, user.Value.Password, user.Value.Nickname,user.Value.BoardMail);
                userDic.Add(b.Email, b);
            }

        }
        public void DeleteData()
        {
            usercont.Destroy();
            loggenIn = null;
            userDic = new Dictionary<string, UserB>();
        }
        public void register(string email, string password, string nickname)
        {
            string newEmail = email.ToLower();
            if (!EmailVerify(newEmail))
            {
                throw new KanbanException("illegal email");
            }
            if (userDic.ContainsKey(newEmail))
            {
                throw new KanbanException("email already in use");
            }
            if (!legalPass(password))
            {
                throw new KanbanException("illegal password");
            }
            if (!nickVerify(nickname))
            {
                throw new KanbanException("illegal nickname length");
            }
            UserB m = new UserB(newEmail, password, nickname);
            userDic.Add(newEmail, m);
             UserDTO u = new UserDTO(newEmail, password, nickname, newEmail);
             bool ans=cont.Insert(u);
             if (!ans)
                throw new Exception("could not save new user");
             
        }
        public void register(string email, string password, string nickname,string boardMail)
        {
            string newEmail = email.ToLower();
            string newboardMail= boardMail.ToLower();

            if (!EmailVerify(newEmail))
            {
                throw new KanbanException("illegal email");
            }
            if (userDic.ContainsKey(newEmail))
            {
                throw new KanbanException("email already in use");
            }
            if (!legalPass(password))
            {
                throw new KanbanException("illegal password");
            }
            if (!nickVerify(nickname))
            {
                throw new KanbanException("illegal nickname length");
            }

                UserB m = new UserB(newEmail, password, nickname,boardMail);
                userDic.Add(newEmail, m);
                UserDTO u = new UserDTO(newEmail, password, nickname,boardMail);
                bool ans=cont.Insert(u);
                if (!ans)
                    throw new Exception("could not save new user");
        }
        public User login(string email, string password)
        {
            string newm = email.ToLower();
            if (loggenIn != null)
            {
                if (loggedIn(newm))
                    throw new KanbanException("the user is already logged in");
                throw new KanbanException("another user is already logged in");
            }
            if (!userDic.ContainsKey(newm))
            {
                    throw new KanbanException("email is wrong");
            }
            if (!userDic[newm].login(password))
            {
                    throw new KanbanException("password is wrong");
            }
            loggenIn = newm;
            User output = new User(newm, userDic[newm].Nickname);
            return output;  
        }

        public void logout(string email)
        {
            if (email == null)
                throw new KanbanException("email is invalid");
            email = email.ToLower();
            if (email == loggenIn)
            {
                loggenIn = null;
            }
            else
            {
                throw new KanbanException("Can not logout someone that isn't logged in");

            }
        }
        private bool legalPass(string password)
        {
            int MINLENGTH = 5;
            int MAXLENGTH = 25;

            if (password == null || (password.Length < MINLENGTH) || (password.Length > MAXLENGTH))
                return false;

            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCaseLetter = true;
                else if (char.IsLower(c)) hasLowerCaseLetter = true;
                else if (char.IsDigit(c)) hasDecimalDigit = true;
                if (hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit)
                    break;
            }

            bool isValid = hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit
                        ;
            return isValid;


        }
        public bool EmailVerify(string email) //Makes sure that the input email is valid.
        { 
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                if (e.Message != null) { }
                return false;
            }
            catch (ArgumentException e)
            {
                if (e.Message != null) { }
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public bool nickVerify(string nickname)
        {
            if (nickname == null || nickname.Length == 0)
                return false;
            return true;
        }
    }
}

