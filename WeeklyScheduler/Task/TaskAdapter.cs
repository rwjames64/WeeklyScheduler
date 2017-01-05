using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Task
{
    /// <summary>
    /// Adapter class for storing and reading data from the Tasks database file.
    /// </summary>
    public class TaskAdapter
    {
        private static TaskAdapter instance;

        private TaskAdapter()
        {
            // TODO
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
            return false;
        }

        /// <summary>
        /// Remove a Task from the database file.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>bool indicating if the removal was successful</returns>
        public bool RemoveTask(string title)
        {
            return false;
        }

        /// <summary>
        /// Get a Task from the database. Returns null if no Task contains the provided title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Task GetTask(string title)
        {
            return null;
        }

        /// <summary>
        /// Returns a collection of all task titles in the database sorted alphabetically.
        /// </summary>
        /// <returns></returns>
        public ICollection<string> TaskTitles()
        {
            SortedSet<string> titles = new SortedSet<string>();
            return titles;
        }
    }
}
