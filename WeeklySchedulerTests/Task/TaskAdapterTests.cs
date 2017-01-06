using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeeklyScheduler.Task;

namespace WeeklySchedulerTests.Task
{
    /// <summary>
    /// Summary description for TaskAdapterTests
    /// </summary>
    [TestClass]
    public class TaskAdapterTests
    {
        public TaskAdapterTests()
        {
            // intentionally left blank
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddGetRemoveTask()
        {
            TaskAdapter adapter = TaskAdapter.GetInstance();
            WeeklyScheduler.Task.Task task1 = new WeeklyScheduler.Task.Task("title", "description");
            WeeklyScheduler.Task.Task task2;

            Assert.IsTrue(adapter.AddTask(task1), "Unable to add new task");
            task2 = adapter.GetTask(task1.Title);
            Assert.IsNotNull(task2, "Unable to get added task");
            Assert.AreEqual(task2.Title, task1.Title, "GetTask did not return the proper task");
            Assert.AreEqual(task2.Description, task1.Description, "GetTask did not return the proper task");
            Assert.IsTrue(adapter.RemoveTask("title"), "Unable to remove task");
            Assert.IsNull(adapter.GetTask("title"), "Task was not removed properly");
        }

        [TestMethod]
        public void TestAddBlankTask()
        {
            TaskAdapter adapter = TaskAdapter.GetInstance();
            WeeklyScheduler.Task.Task task = new WeeklyScheduler.Task.Task("", "");

            Assert.IsFalse(adapter.AddTask(task), "Adding task with blank title did not fail as expected");
        }

        [TestMethod]
        public void TestAddDuplicateTask()
        {
            TaskAdapter adapter = TaskAdapter.GetInstance();
            WeeklyScheduler.Task.Task task = new WeeklyScheduler.Task.Task("title", "description");

            Assert.IsTrue(adapter.AddTask(task), "Unable to add new task");
            Assert.IsFalse(adapter.AddTask(task), "Adding duplicate task did not fail as expected");
        }

        [TestMethod]
        public void TestRemoveTaskNotFound()
        {
            TaskAdapter adapter = TaskAdapter.GetInstance();

            Assert.IsFalse(adapter.RemoveTask(""), "Removing a task not found in the database did not fail as expected");
        }

        [TestMethod]
        public void TestTaskTitles()
        {
            TaskAdapter adapter = TaskAdapter.GetInstance();
            WeeklyScheduler.Task.Task task1 = new WeeklyScheduler.Task.Task("one", "");
            WeeklyScheduler.Task.Task task2 = new WeeklyScheduler.Task.Task("two", "");
            WeeklyScheduler.Task.Task task3 = new WeeklyScheduler.Task.Task("three", "");

            adapter.AddTask(task1);
            adapter.AddTask(task2);
            adapter.AddTask(task3);

            List<string> titles = adapter.TaskTitles();

            Assert.AreEqual(3, titles.Count);

            adapter.RemoveTask(task1.Title);
            adapter.RemoveTask(task2.Title);
            adapter.RemoveTask(task3.Title);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TaskAdapter.GetInstance().RemoveAllTasks();
        }
    }
}
