using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.DataSources;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;
using DocxTemplater.WordProcessing.Parsers;

namespace DocxTemplater.WordProcessing.Processors
{
    internal sealed class TextContentControlProcessor : ContentControlProcessor
    {
        public TextContentControlProcessor(XElement sdt)
            : base(sdt)
        {
            
        }

        public override XElement Process(IXPathNavigable dataSource)
        {
            var parser = new TextContentControlParser(sdtContent);
            parser.Parse();

            if (string.IsNullOrWhiteSpace(parser.Path))
                throw new DocumentProcessingException("The data path is null or empty.", sdtContent);


            var dataProvider = CreateDataProvider(parser.Path, dataSource);

            var sourceParagraph = sdtContent.GetParagraph();
            if (sourceParagraph != null)
                return CreateParagraphWithData(sourceParagraph, dataProvider);

            var sourceRun = sdtContent.GetRun();
            if(sourceRun != null)
                return CreateRunWithData(sourceRun, dataProvider);

            throw new DocumentProcessingException("Malformed sdt content element. Must contain or paragraph or run.", sdtContent);
        }

        private XElement CreateParagraphWithData(XElement sourceParagraph, DataProvider dataProvider)
        {
            var resultParagraph = sourceParagraph.CreateParagraphWithEmptyRun();
            var run = resultParagraph.GetRun();
            new RunFiller().Fill(run, dataProvider);
            return resultParagraph;
        }

        private XElement CreateRunWithData(XElement sourceRun, DataProvider dataProvider)
        {
            var resultRun = sourceRun.CreateRunWithoutText();
            new RunFiller().Fill(resultRun, dataProvider);
            return resultRun;
        }

        private DataProvider CreateDataProvider(string dataPath, IXPathNavigable dataSource)
        {
            var navigator = dataSource.CreateNavigator().SelectSingleNode(dataPath);
            if (navigator == null)
                throw new DataSourceException($"Could not find data for data path '{dataPath}'.");

            return new DataProvider(navigator);
        }
    }
}
