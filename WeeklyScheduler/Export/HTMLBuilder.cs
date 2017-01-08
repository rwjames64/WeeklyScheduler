using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyScheduler.Task;

namespace WeeklyScheduler.Export
{
    static class HTMLBuilder
    {
        /// <summary>
        /// Generates HTML as a string.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="tasks">Each List of ScheduledTask represents one day of the week. It is
        /// assumed there will be seven of these lists in order of the days of the week starting on Sunday.</param>
        /// <returns></returns>
        public static string GenerateHTML(string name, DateTime date, List<List<ScheduledTask>> tasks)
        {
            StringBuilder html = new StringBuilder();

            html.Append(GenerateCSS());
            html.Append(GenerateHeader(name, date));
            html.Append(GenerateContent(tasks));
            html.Append(GenerateFooter());

            return html.ToString();
        }

        private static string GenerateCSS()
        {
            StringBuilder css = new StringBuilder();

            css.Append("<DOCTYPE html>");
            css.Append("<style>");
            css.Append("</style>");

            return css.ToString();
        }

        private static string GenerateHeader(string name, DateTime date)
        {
            StringBuilder header = new StringBuilder();

            header.Append("<body>");
            header.Append("<div id=\"Header\">");

            header.Append("<span class=\"name\">");
            header.Append(name);
            header.Append("</span>");

            header.Append("<span class=\"title\">");
            header.Append("WEEKLY SCHEDULE");
            header.Append("</span>");

            header.Append("<span class=\"date\">");
            header.Append(date.ToShortDateString());
            header.Append("</span>");

            header.Append("</div>");

            return header.ToString();
        }

        private static string GenerateContent(List<List<ScheduledTask>> tasks)
        {
            StringBuilder content = new StringBuilder();

            content.Append("<div id=\"Content\">");
            content.Append(GenerateDay("Sunday", tasks[0]));
            content.Append(GenerateDay("Monday", tasks[1]));
            content.Append(GenerateDay("Tuesday", tasks[2]));
            content.Append(GenerateDay("Wednesday", tasks[3]));
            content.Append(GenerateDay("Thursday", tasks[4]));
            content.Append(GenerateDay("Friday", tasks[5]));
            content.Append(GenerateDay("Saturday", tasks[6]));
            content.Append("</div>");

            return content.ToString();
        }

        private static string GenerateDay(string name, List<ScheduledTask> tasks)
        {
            StringBuilder day = new StringBuilder();

            day.Append("<div class=\"day\">");
            day.Append(name);
            day.Append("<div class=\"border\">");
            day.Append("<div class=\"scheduledTask\">");
            
            foreach (ScheduledTask task in tasks)
            {
                day.Append("<p>" + task.Time + "</p>");
                day.Append("<p>" + task.Title + "</p>");
                day.Append("<p>" + task.Description + "</p>");
            }

            day.Append("</div>");
            day.Append("</div>");
            day.Append("</div>");

            return day.ToString();
        }

        private static string GenerateFooter()
        {
            StringBuilder footer = new StringBuilder();

            footer.Append("<div id=\"Footer\">");
            footer.Append("Manager").Append("<hr />");
            footer.Append("Supervisor").Append("<hr />");
            footer.Append("Supervisor").Append("<hr />");
            footer.Append("Employee").Append("<hr />");
            footer.Append("</div>");
            footer.Append("</body></html>");

            return footer.ToString();
        }
    }
}
