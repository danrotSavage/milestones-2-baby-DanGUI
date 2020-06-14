using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IntroSE.Kanban.Backend.ServiceLayer.Tests
{

    public class ServiceTests
    {
        Service service;

        [SetUp]
        public void initService()
        {
            service = new Service();


        }
        [Test]
        public void Register()
        {
            Response res = service.Register("bardamri0@gmail.com", "Bar123", "barb");
            Assert.IsNull(res.ErrorMessage, "failed to register new user");
        }
        [Test]
        public void RegisterTwoUsersToOneBoard()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            Response res = service.Register("bardamri1@gmail.com", "Bar123", "barb", "bardamri0@gmail.com");
            Assert.IsNull(res.ErrorMessage, "failed to register new user");
        }
        [TestCase("bardamri0gmail.com", "Bar123", "barb")]
        [TestCase("bardamri0gmail.com", "", "barb")]
        [TestCase("bardamri0@gmail.com", "ar123", "barb")]
        [TestCase("bardamri0@gmail.com", "Bar123", "")]
        [TestCase("bardamri0@gmail.com", "Bar123", null)]
        [TestCase("bardamri0@gmail.com", null, "barb")]
        [TestCase(null, "Bar123", "barb")]
        [TestCase("bardamri0@gmail.com", "Ba13", "barb")]
        [TestCase("", "Bar123", "barb")]
        public void RegisterEdgeCase(string email, string password, string nickname)
        {
            Response res = service.Register(email, password, nickname);
            Assert.IsNotNull(res.ErrorMessage, "failed to register new user");
        }
        [TestCase("bardamri1gmail.com", "Bar123", "barb", "bardamri0@gmail.com")]
        [TestCase("bardamri1gmail.com", "Bar123", "barb", "bardamri2@gmail.com")]
        [TestCase("bardamri1gmail.com", "", "barb", "bardamri0@gmail.com")]
        [TestCase("bardamri1@gmail.com", "ar123", "barb", "bardamri0@gmail.com")]
        [TestCase("bardamri1@gmail.com", "Bar123", "", "bardamri0@gmail.com")]
        [TestCase("bardamri1@gmail.com", "Bar123", null, "bardamri0@gmail.com")]
        [TestCase("bardamri1@gmail.com", null, "barb", "bardamri0@gmail.com")]
        [TestCase(null, "Bar123", "barb", "bardamri0@gmail.com")]
        [TestCase("bardamri1@gmail.com", "Ba13", "barb", "bardamri0@gmail.com")]
        [TestCase("", "Bar123", "barb", "bardamri0@gmail.com")]
        public void RegisterSecondUserEdgeCase(string email, string password, string nickname, string boardMail)
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            Response res = service.Register(email, password, nickname);
            Assert.IsNotNull(res.ErrorMessage, "failed to register new user");
        }
        [Test]
        public void Login()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            User user = service.Login("bardamri0@gmail.com", "Bar123").Value;
            User user2 = new User("bardamri0@gmail.com", "barb");
            Assert.AreEqual(user2, user, "login valid user test failed");
        }
        [Test]
        public void Login2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb", "bardamri0@gmail.com");
            User user = service.Login("bardamri1@gmail.com", "Bar123").Value;
            User user2 = new User("bardamri1@gmail.com", "barb");
            Assert.AreEqual(user2, user, "login valid user test failed");
        }
        [Test]
        public void LoadData()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.LoadData();
            User res = service.getUser("bardamri0@gmail.com").Value;
            Assert.AreEqual(res, new User("bardamri0@gmail.com", "barb"), "Load user method failed");
        }
        [Test]
        public void GetBoardTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Board board = service.GetBoard("bardamri0@gmail.com").Value;
            List<string> cols = new List<string> { "backlog", "in progress", "done" };

            IReadOnlyCollection<string> columns = new ReadOnlyCollection<string>(cols);
            Board b = new Board(columns, "bardamri0@gmail.com");
            Assert.AreEqual(b, board, "failed to get board");
        }

        [Test]
        public void LimitColumnTasksTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Response res = service.LimitColumnTasks("bardamri0@gmail.com", 0, 3);
            Assert.IsNull(res.ErrorMessage, "failed to set limit in column");
        }
        [TestCase(2)]
        [TestCase(-1)]
        public void LimitColumnTasksTest2(int i)
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            Response res = service.LimitColumnTasks("bardamri0@gmail.com", 0, i);
            Assert.IsNull(res.ErrorMessage, "failed to set limit in column");
        }
        [Test]
        public void LimitColumnTasksTest3()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            Response res = service.LimitColumnTasks("bardamri0@gmail.com", 0, 1);
            Assert.IsNotNull(res.ErrorMessage, "failed to set limit in column");
        }
        [Test]
        public void AddTaskTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;

            Task tas = new Task(task.Id, DateTime.Now, "new title", "new description", "bardamri0@gmail.com", DateTime.MaxValue);

            Assert.AreEqual(expected: tas, actual: task, "failed to add a valid task");
        }
        [Test]
        public void AddSecondTask()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, DateTime.Now, "second title", "second description", "bardamri0@gmail.com", DateTime.MaxValue);

            Assert.AreEqual(expected: tas, actual: task, " failed to add a valid second task");
        }
        [Test]
        public void AddTaskEdgeCase()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Response<Task> task = service.AddTask("bardamri0@gmail.com", "", "second description", DateTime.MaxValue);
            Assert.AreEqual("could not add task because task details are illegal", task.ErrorMessage, " added an empty title edge case task ");
        }
        public void AddTaskEdgeCase2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, DateTime.Now, "second title", "second description", "bardamri0@gmail.com", DateTime.MaxValue);

            Assert.IsInstanceOf<KanbanException>(task, " added a wrong mail edge case task");
        }
        public void AddTaskEdgeCase3()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, DateTime.Now, "second title", "second description", "bardamri0@gmail.com", DateTime.MaxValue);
            Assert.IsInstanceOf<KanbanException>(task, " added an edge case task non logged in user");
        }
        public void AddTaskEdgeCase4()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "descriptiofvndf;ojsnjdfbsjdnb;sldkfkbn;sdkfbnbs;ldfkflbns;ldgfkbbns;lgfdgflbkns;dgbklnsd;dglblkns;glglbkns;gdfkbns;fgllkbns;lgfdkbns;flggkbns;flgkbnsfggns;flkgbnkbs;flgfhndhnkbnsf;lfgkbnsf;lfgkbnsf;lgkbnbsf;lgkbnsfl;gbnsf;lgbkns;flglkbns;lfgbns;kgbns;lgnbs;lfgfkbns;flgkbns;lfkfgbnmnergsthdfhyhndgghme", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, DateTime.Now, "second title", "second description", "bardamri0@gmail.com", DateTime.MaxValue);
            Assert.IsInstanceOf<KanbanException>(task, " added too large description edge case  task");
        }
        [Test]
        public void UpdateTaskDueDateTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.UpdateTaskDueDate("bardamri0@gmail.com", 0, tas.Id, new DateTime(2021, 11, 12));
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to update task due date");
        }

        [Test]
        public void UpdateTaskTitleTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.UpdateTaskTitle("bardamri0@gmail.com", 0, tas.Id, "other title");
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to update task title");
        }

        [Test]
        public void UpdateTaskDescriptionTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.UpdateTaskDescription("bardamri0@gmail.com", 0, tas.Id, "new description");
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to update task description");
        }

        [Test]
        public void AdvanceTaskTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri0@gmail.com", 0, task.Id);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to advance task");
        }
        [Test]
        public void AdvanceTaskEdgeTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri01@gmail.com", 0, task.Id);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, "advance task with wrong mail");
        }
        [Test]
        public void AdvanceTaskEdgeTest2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri0@gmail.com", 1, task.Id);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, " advanced wrong column task");
        }
        [Test]
        public void AdvanceTaskEdgeTest3()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri0@gmail.com", 1, 3);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, " advanced wrong id task");
        }
        [Test]
        public void GetColumnTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Response<Column> col = service.GetColumn("bardamri0@gmail.com", "backlog");
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "backlog", 100);
            Assert.AreEqual(expected: col2, col.Value, "failed to get column");

        }
        [Test]
        public void GetColumnTest2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Response<Column> col = service.GetColumn("bardamri0@gmail.com", 0);
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "backlog", 100);
            Assert.AreEqual(expected: col2, col.Value, "failed to get column");

        }

        [Test]
        public void RemoveColumnTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Response res = service.RemoveColumn("bardamri0@gmail.com", 0);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 0);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest3()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AdvanceTask("bardamri0@gmail.com", 0, 0);
            service.AdvanceTask("bardamri0@gmail.com", 0, 1);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 1);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest4()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "third title", "third description", DateTime.MaxValue);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 0);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest5()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "third title", "third description", DateTime.MaxValue);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 1);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest6()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.LimitColumnTasks("bardamri0@gmail.com", 1, 2);
            service.AdvanceTask("bardamri0@gmail.com", 0, 0);
            service.AdvanceTask("bardamri0@gmail.com", 0, 1);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 0);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        public void RemoveColumnTest7()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.LimitColumnTasks("bardamri0@gmail.com", 0, 3);
            service.AdvanceTask("bardamri0@gmail.com", 0, 0);
            service.AdvanceTask("bardamri0@gmail.com", 0, 1);
            service.AdvanceTask("bardamri0@gmail.com", 0, 2);
            service.AdvanceTask("bardamri0@gmail.com", 0, 3);
            service.AdvanceTask("bardamri0@gmail.com", 0, 4);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 1);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void AddColumnTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Column col = service.AddColumn("bardamri0@gmail.com", 1, "shem").Value;
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "shem", 100);
            Assert.AreEqual(expected: col2, actual: col, " failed to add new column");
        }

        [Test]
        public void MoveColumnRightTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Column col = service.MoveColumnRight("bardamri0@gmail.com", 0).Value;
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "backlog", 100);
            Assert.AreEqual(expected: col2, actual: col, "failed to move column right");
        }

        [Test]
        public void MoveColumnLeftTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Column col = service.MoveColumnLeft("bardamri0@gmail.com", 2).Value;
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "done", 100);
            Assert.AreEqual(expected: col2, actual: col, "failed to move column left");
        }

        [Test]
        public void AssignTaskTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            service.Login("bardamri1@gmail.com", "Bar123");
            Task tas=service.AddTask("bardamri1@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AssignTask("bardamri1@gmail.com",0,tas.Id, "bardamri0@gmail.com");
            Assert.IsNull(res.ErrorMessage,"failed to assign task to another user ");
        }
        [Test]
        public void AssignTaskTestEdgeCase()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            service.Login("bardamri1@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri1@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AssignTask("bardamri2@gmail.com", 0, tas.Id, "bardamri0@gmail.com");
            Assert.IsNotNull(res.ErrorMessage, "failed by assigning task with wrong input to another user ");
        }
        [Test]
        public void AssignTaskTestEdgeCase2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            service.Login("bardamri1@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri1@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AssignTask("bardamri1@gmail.com", 0, tas.Id, "bardamri2@gmail.com");
            Assert.IsNotNull(res.ErrorMessage, "failed by assigning task with wrong input to another user ");
        }
        [Test]
        public void AssignTaskTestEdgeCase3()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            Task tas = service.AddTask("bardamri1@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AssignTask("bardamri1@gmail.com", 0, tas.Id, "bardamri0@gmail.com");
            Assert.IsNotNull(res.ErrorMessage, "failed by assigning task with wrong input to another user ");
        }
        [Test]
        public void AssignTaskTestEdgeCase4()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            service.Login("bardamri1@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri1@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AssignTask("bardamri1@gmail.com", 1, tas.Id, "bardamri0@gmail.com");
            Assert.IsNotNull(res.ErrorMessage, "failed by assigning task with wrong input to another user ");
        }
        [Test]
        public void AssignTaskTestEdgeCase5()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            service.Login("bardamri1@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri1@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AssignTask("bardamri1@gmail.com", 0, 3, "bardamri0@gmail.com");
            Assert.IsNotNull(res.ErrorMessage, "failed by assigning task with wrong input to another user ");
        }
        [Test]
        public void AssignTaskTestEdgeCase6()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Register("bardamri1@gmail.com", "Bar123", "barb");
            service.Login("bardamri1@gmail.com", "Bar123");
            Response res = service.AssignTask("bardamri1@gmail.com", 0, 3, "bardamri0@gmail.com");
            Assert.IsNotNull(res.ErrorMessage, "failed by assigning task with wrong input to another user ");
        }
        [Test]
        public void DeleteTaskTest()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.DeleteTask("bardamri0@gmail.com",0,tas.Id);
            Assert.IsNull(res.ErrorMessage, "failed to delete task ");
        }
        [Test]
        public void DeleteTaskTestEdgeCase()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.DeleteTask("bardamri@gmail.com", 0, tas.Id);
            Assert.IsNotNull(res.ErrorMessage, "failed by deletint task with wrong input ");
        }
        [Test]
        public void DeleteTaskTestEdgeCase2()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.DeleteTask("bardamri0@gmail.com", 0, tas.Id);
            Assert.IsNotNull(res.ErrorMessage, "failed by deletint task with wrong input ");
        }
        [Test]
        public void DeleteTaskTestEdgeCase3()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.DeleteTask("bardamri0@gmail.com",1, tas.Id);
            Assert.IsNotNull(res.ErrorMessage, "failed by deletint task with wrong input ");
        }
        [Test]
        public void DeleteTaskTestEdgeCase4()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.DeleteTask("bardamri0@gmail.com", 0, 5);
            Assert.IsNotNull(res.ErrorMessage, "failed by deletint task with wrong input ");
        }
        [Test]
        public void DeleteTaskTestEdgeCase5()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Response res = service.DeleteTask("bardamri0@gmail.com", 0, 0);
            Assert.IsNotNull(res.ErrorMessage, "failed by deletint task with wrong input ");
        }
        [TearDown]
        public void deletedate()
        {
            service.DeleteData();
        }
    }
}