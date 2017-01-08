using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Task
{
    class ScheduledTask : Task, IComparable<ScheduledTask>
    {
        private string amPm;
        private int hour;
        private int minute;

        public ScheduledTask(string title, string description, int hour, int minute, string amPm) : base(title, description)
        {
            this.amPm = amPm;
            this.hour = hour;
            this.minute = minute;
        }

        public string Time
        {
            get
            {
                return hour.ToString("00") + ":" + minute.ToString("00") + " " + amPm;
            }
        }

        public int CompareTo(ScheduledTask task)
        {
            int result = amPm.CompareTo(task.amPm);

            if (result == 0)
            {
                int hourA = (hour == 12 ? 0 : hour);
                int hourB = (task.hour == 12 ? 0 : task.hour);
                result = hourA - hourB;
            }

            if (result == 0)
            {
                result = minute - task.minute;
            }

            if (result == 0)
            {
                result = Title.CompareTo(task.Title);
            }

            if (result == 0)
            {
                result = Description.CompareTo(task.Description);
            }

            return result;
        }
    }
}
