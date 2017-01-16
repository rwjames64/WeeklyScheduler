using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Task
{
    public class Task
    {
        public Task(string title)
        {
            Title = title;
        }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value.Trim();
            }
        }
    }
}
