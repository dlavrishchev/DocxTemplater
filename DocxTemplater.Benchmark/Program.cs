using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using BenchmarkDotNet.Running;
using DocxTemplater.DataSources;

namespace DocxTemplater.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProcessTableTemplate();
            //BenchmarkRunner.Run<TableTemplateBenchmark>();
        }

        private static void ProcessTableTemplate(int rowsCount = 1_000)
        {
            int cursorTop, cursorLeft;

            Console.WriteLine("Start process table template...");
            Console.Write("Generating dataset...");
            var ds = GenerateDataset(rowsCount:1000);
            Console.WriteLine("done");

            Console.Write("Processing template...");
            cursorTop = Console.CursorTop;
            cursorLeft = Console.CursorLeft;

            using (var doc = new WordDocument(@".\templates\table_template.docx"))
            {
                doc.ProcessMainDocumentContentControls(ds);

                Console.WriteLine("\nSave result to file? [y/n]");
                if(Console.ReadLine()?.ToLower() == "y")
                    doc.Save(@".\table_template_result.docx");
            }

            Console.CursorTop = cursorTop;
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("done");
        }

        private static IXPathNavigable GenerateDataset(int rowsCount)
        {
            var items = new XElement[rowsCount];
            for (var i = 0; i < rowsCount; i++)
            {
                var item = new XElement("item",
                    new XElement("c1", i, new XAttribute("textColor", "#FF0000")),
                    new XElement("c2", i, new XAttribute("highlightColor", "green")),
                    new XElement("c3", i, new XAttribute("bold", "true")),
                    new XElement("c4", i, new XAttribute("italic", "true")),
                    new XElement("c5", i, new XAttribute("underline", "true")),
                    new XElement("c6", i),
                    new XElement("c7", i),
                    new XElement("c8", i),
                    new XElement("c9", i),
                    new XElement("c10", i));
                items[i] = item;
            }

            using (var ms = new MemoryStream())
            {
                new XElement("items", items).Save(ms);
                ms.Position = 0;
                return new XPathDocument(ms);
            }
        }
    }
}
