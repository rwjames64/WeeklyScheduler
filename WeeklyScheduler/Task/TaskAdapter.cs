using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WeeklyScheduler.Task
{
    /// <summary>
    /// Adapter class for storing and reading data from the Tasks database file.
    /// </summary>
    public class TaskAdapter
    {
        private static readonly string DIRECTORY_NAME = "data";
        private static readonly string FILE_NAME = DIRECTORY_NAME + "\\tasks.xml";
        private static readonly string INIT_XML = "<?xml version='1.0' ?><tasks></tasks>";

        private static TaskAdapter instance;

        private XmlDocument document;
        private XmlElement root;
        private SortedSet<string> titleSet;

        private TaskAdapter()
        {
            document = new XmlDocument();

            if (!System.IO.File.Exists(FILE_NAME))
            {
                document.LoadXml(INIT_XML);
                System.IO.Directory.CreateDirectory(DIRECTORY_NAME);
                document.Save(FILE_NAME);
            }
            else
            {
                document.Load(FILE_NAME);
            }

            root = document.DocumentElement;
        }

        public static TaskAdapter GetInstance()
        {
            if (instance == null)
            {
                instance = new TaskAdapter();
            }

            return instance;
        }

        /// <summary>
        /// Add a Task to the database file.
        /// </summary>
        /// <param name="task"></param>
        /// <returns>bool indicating if the addition was successful</returns>
        public bool AddTask(Task task)
        {
            bool addTask = false;

            if (GetElementWithTitle(task.Title) == null && task.Title != null && task.Title != "")
            {
                XmlElement taskElement = document.CreateElement("task");
                taskElement.SetAttribute("title", task.Title);
                taskElement.SetAttribute("description", task.Description);
                root.AppendChild(taskElement);

                document.Save(FILE_NAME);
                addTask = true;
            }

            return addTask;
        }

        public void RemoveAllTasks()
        {
            root.RemoveAll();
        }

        /// <summary>
        /// Remove a Task from the database file.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>bool indicating if the removal was successful</returns>
        public bool RemoveTask(string title)
        {
            bool removed = false;
            XmlElement element = GetElementWithTitle(title);

            if (element != null)
            {
                root.RemoveChild(element);

                document.Save(FILE_NAME);
                removed = true;
            }

            return removed;
        }

        /// <summary>
        /// Get a Task from the database. Returns null if no Task contains the provided title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Task GetTask(string title)
        {
            Task task = null;
            XmlElement element = GetElementWithTitle(title);

            if (element != null)
            {
                string description = element.GetAttribute("description");
                task = new WeeklyScheduler.Task.Task(title, description);
            }

            return task;
        }

        /// <summary>
        /// Returns a collection of all task titles in the database.
        /// </summary>
        /// <returns></returns>
        public List<string> TaskTitles()
        {
            List<string> titles = new List<string>();

            foreach(XmlElement element in root.GetElementsByTagName("task"))
            {
                titles.Add(element.GetAttribute("title"));
            }

            return titles;
        }

        private XmlElement GetElementWithTitle(string title)
        {
            XmlElement element = null;

            foreach (XmlElement e in root.GetElementsByTagName("task"))
            {
                if (e.GetAttribute("title") == title)
                {
                    element = e;
                }
            }

            return element;
        }
    }
}
