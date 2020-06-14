using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace KanbanUnitTest
{

    [TestClass]
    public class UnitTest1
    {
        Service service = new Service();


        [TestMethod]
        public void canAddTask()
        {
            service.Register("bardamri0@gmail.com", "Bar123", "barb");
            service.Login("bardamri0@gmail.com", "Bar123");
            Task task;
            task = service.AddTask("bardamri0@gmail.com", "new title", "new description", DateTime.MaxValue).Value;
            Task tas = new Task(task.Id, DateTime.Now, DateTime.MaxValue, "new title", "new description");
            Assert.AreEqual<Task>(expected: tas, actual: task, "added task is incorrect");
        }
    }
}
