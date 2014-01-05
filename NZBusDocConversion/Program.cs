using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;

namespace NZBusDocConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            var startNum = int.Parse(args[0]);
            var endNum = int.Parse(args[1]);
            var infos = new List<ShiftInfo>();
            for (int i = startNum; i < endNum; i++)
            {
                if(File.Exists("W" + i + ".pdf"))
                    infos.Add(ExtractPDFText("W" + i + ".pdf"));
            }
            Console.ReadLine();
            var exporter = new ExcelExporter();
            exporter.ExportToCSV(infos);
        }

        public static ShiftInfo ExtractPDFText(string fileName)
        {
            PdfReader reader = new PdfReader(File.ReadAllBytes(fileName));
            PdfDictionary resources = reader.GetPageResources(1);
            var listener = new ShiftCardRenderListener();
            PdfContentStreamProcessor processor = new PdfContentStreamProcessor(listener);
            processor.ProcessContent(ContentByteUtils.GetContentBytesForPage(reader, 1), resources);
            listener.PrintShiftInfo();
            return listener.GetShiftInfo();
        }
    }
}
