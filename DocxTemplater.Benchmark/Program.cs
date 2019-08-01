using System;
using System.Xml.Linq;
using BenchmarkDotNet.Running;
using DocxTemplater.DataSources;

namespace DocxTemplater.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProcessTableTemplate();
            BenchmarkRunner.Run<TableTemplateBenchmark>();
        }

        private static void ProcessTableTemplate(int rowsCount = 1_000)
        {
            int cursorTop, cursorLeft;

            Console.WriteLine("Start process table template...");
            var datasetPath = @".\table_dataset.xml";

            Console.Write("Generating dataset...");
            GenerateDataset();
            Console.WriteLine("done");

            Console.Write("Loading datasource...");
            var ds = XmlDataSource.Create(datasetPath);
            Console.WriteLine("done");

            Console.Write("Processing template...");
            cursorTop = Console.CursorTop;
            cursorLeft = Console.CursorLeft;

            using (var doc = new WordDocument(@".\templates\table_template.docx"))
            {
                doc.ProcessMainDocumentContentControls(ds);
                doc.ProcessFootersContentControls(ds);

                Console.WriteLine("\nSave result to file? [y/n]");
                if(Console.ReadLine()?.ToLower() == "y")
                    doc.Save(@".\table_template_result.docx");
            }

            Console.CursorTop = cursorTop;
            Console.CursorLeft = cursorLeft;
            Console.WriteLine("done");

            void GenerateDataset()
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
                var doc = new XDocument(
                    new XElement("items", items));
                doc.Save(datasetPath);
            }
        }
    }
}
