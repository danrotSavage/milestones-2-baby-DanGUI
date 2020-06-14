
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
            service.Register("bardamri0@gmail.com", "Bar123", "barb");

        }
        [Test]
        public void Login()
        {

            User user=service.Login("bardamri0@gmail.com", "Bar123").Value;
            User user2 = new User("bardamri0@gmail.com","barb");
            Assert.AreEqual(user2, user, "login valid user test failed");
        }
        [Test]
        public void LoadData()
        {
            service.LoadData();
            User res= service.getUser("bardamri0@gmail.com").Value;
            Assert.AreEqual(res, new User("bardamri0@gmail.com", "barb"),"Load user method failed");
        }
        [Test]
        public void GetBoardTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Board board = service.GetBoard("bardamri0@gmail.com").Value;
            List<string> cols= new List<string> {"backlog","in progress","done" };

            IReadOnlyCollection<string> columns = new ReadOnlyCollection<string>(cols);
            Board b = new Board(columns);
            Assert.AreEqual(b,board,"failed to get board");
        }

        [Test]
        public void LimitColumnTasksTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Response res = service.LimitColumnTasks("bardamri0@gmail.com", 0, 3);
            Assert.IsNull(res.ErrorMessage, "failed to set limit in column");
        }
        [TestCase(2)]
        [TestCase(-1)]
        public void LimitColumnTasksTest2(int i)
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            Response res = service.LimitColumnTasks("bardamri0@gmail.com", 0, i);
            Assert.IsNull(res.ErrorMessage, "failed to set limit in column");
        }
        [Test]
        public void LimitColumnTasksTest3()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            Response res = service.LimitColumnTasks("bardamri0@gmail.com", 0,1);
            Assert.IsNotNull(res.ErrorMessage, "failed to set limit in column");
        }
        [Test]
        public void AddTaskTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, task.CreationTime, "new title", "new description");
            Assert.AreEqual(expected: tas, actual: task, "failed to add a valid task");
        }
        [Test]
        public void AddSecondTask()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, task.CreationTime, "second title", "second description");
            Assert.AreEqual(expected: tas, actual: task, " failed to add a valid second task");
        }
        [Test]
        public void AddTaskEdgeCase()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Response<Task> task = service.AddTask("bardamri0@gmail.com", "", "second description", DateTime.MaxValue);
            Assert.AreEqual ("could not add task because task details are illegal", task.ErrorMessage, " added an empty title edge case task ");
        }
        public void AddTaskEdgeCase2()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri01@gmail.com", "second title", "second description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, task.CreationTime, "second title", "second description");
            Assert.IsInstanceOf<KanbanException>(task, " added a wrong mail edge case task");
        }
        public void AddTaskEdgeCase3()
        {
         
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, task.CreationTime, "second title", "second description");
            Assert.IsInstanceOf<KanbanException>(task, " added an edge case task non logged in user");
        }
        public void AddTaskEdgeCase4()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            Task task = service.AddTask("bardamri0@gmail.com", "second title", "descriptiofvndf;ojsnjdfbsjdnb;sldkfkbn;sdkfbnbs;ldfkflbns;ldgfkbbns;lgfdgflbkns;dgbklnsd;dglblkns;glglbkns;gdfkbns;fgllkbns;lgfdkbns;flggkbns;flgkbnsfggns;flkgbnkbs;flgfhndhnkbnsf;lfgkbnsf;lfgkbnsf;lgkbnbsf;lgkbnsfl;gbnsf;lgbkns;flglkbns;lfgbns;kgbns;lgnbs;lfgfkbns;flgkbns;lfkfgbnmnergsthdfhyhndgghme", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, task.CreationTime, "second title", "second description");
            Assert.IsInstanceOf<KanbanException>(task, " added too large description edge case  task");
        }
        [Test]
        public void UpdateTaskDueDateTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas=service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.UpdateTaskDueDate("bardamri0@gmail.com",0,tas.Id,new DateTime(2021,11,12));
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to update task due date");
        }

        [Test]
        public void UpdateTaskTitleTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.UpdateTaskTitle("bardamri0@gmail.com", 0, tas.Id,"other title");
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to update task title");
        }

        [Test]
        public void UpdateTaskDescriptionTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task tas = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.UpdateTaskDescription("bardamri0@gmail.com", 0, tas.Id, "new description");
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to update task description");
        }

        [Test]
        public void AdvanceTaskTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri0@gmail.com",0,task.Id);
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
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri0@gmail.com", 1, task.Id);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, " advanced wrong column task");
        }
        [Test]
        public void AdvanceTaskEdgeTest3()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Response res = service.AdvanceTask("bardamri0@gmail.com", 1,3);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, " advanced wrong id task");
        }
        [Test]
        public void GetColumnTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Response<Column> col = service.GetColumn("bardamri0@gmail.com", "backlog");
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column( tasks,"backlog",-1);
            Assert.AreEqual(expected:col2,col.Value,"failed to get column");

        }
        [Test]
        public void GetColumnTest2()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Response<Column> col = service.GetColumn("bardamri0@gmail.com", 0);
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "backlog", -1);
            Assert.AreEqual(expected: col2, col.Value, "failed to get column");

        }

        [Test]
        public void RemoveColumnTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Response res = service.RemoveColumn("bardamri0@gmail.com",0);
            Assert.AreEqual(expected:null,actual:res.ErrorMessage,"failed to remove column");
        }
        [Test]
        public void RemoveColumnTest2()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 0);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest3()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AdvanceTask("bardamri0@gmail.com",0,0);
            service.AdvanceTask("bardamri0@gmail.com", 0, 1);
            Response res = service.RemoveColumn("bardamri0@gmail.com",1);
            Assert.AreEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        [Test]
        public void RemoveColumnTest4()
        {
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
            service.Login("bardamri0@gmail.com", "Bar123");
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.AddTask("bardamri0@gmail.com", "second title", "second description", DateTime.MaxValue);
            service.LimitColumnTasks("bardamri0@gmail.com",1,2);
            service.AdvanceTask("bardamri0@gmail.com", 0, 0);
            service.AdvanceTask("bardamri0@gmail.com", 0, 1);
            Response res = service.RemoveColumn("bardamri0@gmail.com", 0);
            Assert.AreNotEqual(expected: null, actual: res.ErrorMessage, "failed to remove column");
        }
        public void RemoveColumnTest7()
        {
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
            service.Login("bardamri0@gmail.com", "Bar123");
            Column col=service.AddColumn("bardamri0@gmail.com",1,"shem").Value;
            IReadOnlyCollection <Task>tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "shem", -1);
            Assert.AreEqual(expected:col2 ,actual:col," failed to add new column");
        }

        [Test]
        public void MoveColumnRightTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Column col = service.MoveColumnRight("bardamri0@gmail.com", 0).Value;
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks, "backlog", -1);
            Assert.AreEqual(expected:col2, actual: col, "failed to move column right");
        }

        [Test]
        public void MoveColumnLeftTest()
        {
            service.Login("bardamri0@gmail.com", "Bar123");
            Column col = service.MoveColumnLeft("bardamri0@gmail.com",2).Value;
            IReadOnlyCollection<Task> tasks = new ReadOnlyCollection<Task>(new List<Task>());
            Column col2 = new Column(tasks,"done",-1);
            Assert.AreEqual(expected:col2, actual: col, "failed to move column left");
        }
        [TearDown]
        public void deletedate()
        {
            service.DeleteData();
        }
    }
}