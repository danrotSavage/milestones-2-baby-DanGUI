using IntroSE.Kanban.Backend.BussinessLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IntroSE.Kanban.Backend;
using Moq;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Tests
{
    
    public class UnitTest2
    {
        Mock<BoardB> board1;
        BoardController bc;


        [SetUp]
        public void init()
        {
            board1 = new Mock<BoardB>();
            bc = new BoardController();
            bc.addBoard(board1.Object);
        }

        [Test]
        public void TestMethod1()
        {
            //Arrange
            board1.Setup(m => m.addTask(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),It.IsAny<DateTime>())).Returns((string email,string title,string description,DateTime duedate)=>new Task(0,DateTime.Now,title,description,email,duedate));

            //Act
            Task result = bc.addTask(board1.Object.Email, "title", "description", DateTime.MaxValue);

            //Assert

            Assert.AreEqual(expected: new Task(0, DateTime.Now, "title", "description", board1.Object.Email, DateTime.MaxValue), actual: result);

        }
    }
}
