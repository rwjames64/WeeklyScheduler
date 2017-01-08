using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Task
{
    class ScheduledTask : Task
    {
        public ScheduledTask(string title, string description, string time) : base(title, description)
        {
            Time = time;
        }

        public string Time { get; private set; }
    }
}
