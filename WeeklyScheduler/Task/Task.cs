﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Task
{
    public class Task
    {
        public Task(string title, string description)
        {
            Title = title;
            Description = description;
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

        private string description;
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value.Trim();
            }
        }
    }
}
