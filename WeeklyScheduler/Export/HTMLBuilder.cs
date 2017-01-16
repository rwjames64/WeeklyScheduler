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

            // body styles
            css.Append("body { ");
            css.Append("max-width: 1100px; ");
            css.Append("margin: 0 auto; ");
            css.Append("} ");

            // Header styles
            css.Append("#Header { ");
            css.Append("font-size: 2em; ");
            css.Append("margin: 2em; ");
            css.Append("text-align: center; ");
            css.Append("} ");

            css.Append("#Header span { ");
            css.Append("margin: 0 1.5em; ");
            css.Append("} ");

            // Content styles
            css.Append(".border { ");
            css.Append("border: 1px solid black; ");
            css.Append("height: 500px; ");
            css.Append("text-align: left; ");
            css.Append("} ");

            css.Append(".day { ");
            css.Append("float: left; ");
            css.Append("margin: 5px; ");
            css.Append("text-align: center; ");
            css.Append("width: 147px; ");
            css.Append("} ");

            css.Append(".scheduledTask { ");
            css.Append("margin: 0.5em; ");
            css.Append("} ");

            css.Append("p { ");
            css.Append("margin: 0; ");
            css.Append("} ");

            // Footer styles
            css.Append("#Footer hr { ");
            css.Append("border-color: black; ");
            css.Append("border-style: solid; ");
            css.Append("border-top: 0; ");
            css.Append("display: inline-block; ");
            css.Append("margin-top: 30px; ");
            css.Append("position: relative; ");
            css.Append("top: 1em; ");
            css.Append("width: 460px; ");
            css.Append("} ");

            css.Append("#Footer span { ");
            css.Append("display: inline-block; ");
            css.Append("padding-right: 0.2em; ");
            css.Append("text-align: right; ");
            css.Append("width: 80px; ");
            css.Append("} ");

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
            
            foreach (ScheduledTask task in tasks)
            {
                day.Append("<div class=\"scheduledTask\">");
                if (task.Time != "")
                {
                    day.Append("<p>" + task.Time + "</p>");
                }
                day.Append("<p>" + task.Title + "</p>");
                day.Append("</div>");
            }

            day.Append("</div>");
            day.Append("</div>");

            return day.ToString();
        }

        private static string GenerateFooter()
        {
            StringBuilder footer = new StringBuilder();

            footer.Append("<div id=\"Footer\">");
            footer.Append("<span>Manager</span>").Append("<hr />");
            footer.Append("<span>Supervisor</span>").Append("<hr />");
            footer.Append("<span>Supervisor</span>").Append("<hr />");
            footer.Append("<span>Employee</span>").Append("<hr />");
            footer.Append("</div>");
            footer.Append("</body></html>");

            return footer.ToString();
        }
    }
}
