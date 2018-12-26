using System;
using DocxTemplater.DataSources;

namespace DocxTemplater.Demo
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Processing...");

            try
            {
                var dataSource = new XmlDataSource();
                dataSource.Load(@".\files\data.xml");
                using (var tp = new TemplateProcessor(@".\files\template.docx"))
                {
                    tp.Process(dataSource);
                    tp.Save(@".\files\result.docx");
                }
                Console.WriteLine("Done.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
