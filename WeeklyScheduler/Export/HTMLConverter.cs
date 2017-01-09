using HiQPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyScheduler.Export
{
    static class HTMLConverter
    {
        public static void convertHTML(string pdfFile, string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            if (File.Exists(pdfFile))
            {
                File.Delete(pdfFile);
            }
            converter.Document.PageOrientation = PdfPageOrientation.Landscape;
            converter.ConvertHtmlToFile(html, "", pdfFile);
        }
    }
}
