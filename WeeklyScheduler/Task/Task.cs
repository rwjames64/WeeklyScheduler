using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Task
{
    class Task
    {
        public string Title
        {
            get
            {
                return Title;
            }

            set
            {
                Title = value.Trim();
            }
        }

        public string Description
        {
            get
            {
                return Description;
            }

            set
            {
                Description = value.Trim();
            }
        }
    }
}
