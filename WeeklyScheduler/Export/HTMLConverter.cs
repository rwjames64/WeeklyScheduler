using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPdf;

namespace WeeklyScheduler.Export
{
    static class HTMLConverter
    {
        public static void convertHTML(string pdfFile, string html)
        {
            HtmlToPdf htmlToPdf = new HtmlToPdf();
            PdfResource PDF = htmlToPdf.RenderHtmlAsPdf(html);
            PDF.SaveAs(pdfFile);
        }
    }
}
