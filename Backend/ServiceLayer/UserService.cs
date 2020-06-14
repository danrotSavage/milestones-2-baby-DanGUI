using System;
using IntroSE.Kanban.Backend.BussinessLayer;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private UserController uController;

        public UserService() {
            this.uController = new UserController();
        }
        public UserController getUcontroller()
        {
            return uController;
        }
        public Response Register(string email, string password, string nickname)
        {
            try
            {
                uController.register(email, password, nickname);
                log.Info("user has been registered");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("user could not be registered" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("user could not be registered" + e.Message);
                return new Response(e.Message);
            }
        }
        public Response Register(string email, string password, string nickname, string boardMail)
        {
            try
            {
                uController.register(email, password, nickname, boardMail);
                log.Info("user has been registered");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("user could not be registered" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("user could not be registered" + e.Message);
                return new Response(e.Message);
            }
        }
        public Response DeleteData()
        {
            try
            {
                uController.DeleteData();
                log.Info("all users have been deleted");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("data could not be deleted, Error:" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Warn("data could not be deleted, Error:" + e.Message);
                return new Response(e.Message);
            }
        }


        public Response Logout(string email)
        {
            try
            {
                uController.logout(email);
                log.Info("user has been logged out");
                return new Response();
            }
            catch (KanbanException e)
            {
                log.Warn("user could not be logged out" + e.Message);
                return new Response(e.Message);
            }
            catch (Exception e)
            {
                log.Error("user could not be logged out" + e.Message);
                return new Response(e.Message);
            }
        }

        public Response<User> Login(string email, string password)
        {
            try
            {
                User output = uController.login(email, password);
                log.Info("user has logged in");
                return new Response<User>(output);
            }
            catch (KanbanException e)
            {
                log.Warn("user could not be logged in, Error:" + e.Message);
                return new Response<User>(e.Message);
            }
            catch (Exception e)
            {
                log.Error("user could not be logged in, Error:" + e.Message);
                return new Response<User>(e.Message);
            }
        }


        public Response loadUsersData()
        {
            try
            {
                uController.LoadData();
                log.Info("users Data has been loaded");
                return new Response();
            }            
            catch(KanbanException e)
            {
                log.Warn("could not load users, error:" +e.Message);
                return new Response(e.Message);
            }
            catch(Exception e)
            {
                log.Error("could not load users, error:" +e.Message);
                return new Response(e.Message);
            }
        }
        internal Response<User> getUser(string email)
        {
            try
            {
                return new Response<User>(uController.getUser(email));
            }
            catch (KanbanException e)
            {
                return new Response<User>(e.Message);
            }
            catch (Exception e)
            {
                return new Response<User>(e.Message);
            }
        }
    }
}
