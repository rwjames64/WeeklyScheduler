using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Export
{
    static class HTMLBuilder
    {
        public static string GenerateHTML(string name, DateTime date)
        {
            StringBuilder html = new StringBuilder();

            html.Append(GenerateCSS());
            html.Append(GenerateHeader(name, date));
            html.Append(GenerateContent());
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

        private static string GenerateContent()
        {
            StringBuilder content = new StringBuilder();

            content.Append("<div id=\"Content\">");
            content.Append(GenerateDay("Sunday"));
            content.Append(GenerateDay("Monday"));
            content.Append(GenerateDay("Tuesday"));
            content.Append(GenerateDay("Wednesday"));
            content.Append(GenerateDay("Thursday"));
            content.Append(GenerateDay("Friday"));
            content.Append(GenerateDay("Saturday"));
            content.Append("</div>");

            return content.ToString();
        }

        private static string GenerateDay(string name)
        {
            StringBuilder day = new StringBuilder();

            day.Append("<div class=\"day\">");
            day.Append(name);
            day.Append("<div class=\"border\">");
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
