using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using DocxTemplater.Exceptions;
using DocxTemplater.Extensions;
using DocxTemplater.WordProcessing.Packaging;
using DocxTemplater.WordProcessing.Processors;

namespace DocxTemplater
{
    public sealed class WordDocument : IDisposable
    {
        private WordDocumentPackage _package;
        private MemoryStream _memoryStream;
        private bool _disposed;

        public WordDocument(Stream stream)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            Initialize(stream);
        }

        public WordDocument(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                Initialize(fs);
        }

        public void ProcessMainDocumentContentControls(IXPathNavigable dataSource)
        {
            if (dataSource == null)
                throw new ArgumentNullException(nameof(dataSource));

            ProcessSdtElements(GetMainDocumentPartSdtElements(), dataSource, new ContentControlProcessorFactory());
        }

        public void ProcessHeadersContentControls(IXPathNavigable dataSource)
        {
            if (dataSource == null)
                throw new ArgumentNullException(nameof(dataSource));

            ProcessSdtElements(GetHeaderPartsSdtElements(), dataSource, new ContentControlProcessorFactory());
        }

        public void ProcessFootersContentControls(IXPathNavigable dataSource)
        {
            if (dataSource == null)
                throw new ArgumentNullException(nameof(dataSource));

            ProcessSdtElements(GetFooterPartsSdtElements(), dataSource, new ContentControlProcessorFactory());
        }

        public void ProcessAllContentControls(IXPathNavigable dataSource)
        {
            ProcessMainDocumentContentControls(dataSource);
            ProcessHeadersContentControls(dataSource);
            ProcessFootersContentControls(dataSource);
        }

        public void Save(string filePath)
        {
            _package.Save();
            _package.Dispose(); // Flush

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                _memoryStream.Position = 0;
                _memoryStream.CopyTo(fs);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _package.Dispose();
                _disposed = true;
            }
        }

        private void Initialize(Stream stream)
        {
            PrepareMemoryStream(stream);
            _package = new WordDocumentPackage(_memoryStream);
        }

        private void PrepareMemoryStream(Stream sourceStream)
        {
            _memoryStream = new MemoryStream();
            sourceStream.CopyTo(_memoryStream);
            _memoryStream.Position = 0;
        }

        private IEnumerable<XElement> GetMainDocumentPartSdtElements()
        {
            var body = _package.MainDocument.GetBody();
            var bodySdtElements = body.GetSdtElements();
            var paragraphSdtElements = body.GetParagraphs().SelectMany(p => p.GetSdtElements());
            return bodySdtElements.Concat(paragraphSdtElements).ToArray();
        }

        private IEnumerable<XElement> GetHeaderPartsSdtElements()
        {
            return _package.Headers.SelectMany(header =>
                header.GetSdtElementsFromRoot().Concat(header.GetSdtElementsFromParagraphs()).Concat(header.GetSdtElementsFromRuns())).ToArray();
        }

        private IEnumerable<XElement> GetFooterPartsSdtElements()
        {
            return _package.Footers.SelectMany(footer =>
                footer.GetSdtElementsFromRoot().Concat(footer.GetSdtElementsFromParagraphs()).Concat(footer.GetSdtElementsFromRuns())).ToArray();
        }

        private void ProcessSdtElements(IEnumerable<XElement> sdtElements, IXPathNavigable dataSource, ContentControlProcessorFactory processorFactory)
        {
            foreach (var sdt in sdtElements)
            {
                ProcessSdtElement(sdt, dataSource, processorFactory);
            }
        }

        private void ProcessSdtElement(XElement sdtElement, IXPathNavigable dataSource, ContentControlProcessorFactory processorFactory)
        {
            var tagValue = sdtElement.GetSdtTagValue();
            if (string.IsNullOrWhiteSpace(tagValue))
                throw new DocumentProcessingException("Sdt (content control) tag value is null or empty.", sdtElement);

            var processor = processorFactory.Create(tagValue, sdtElement);
            if (processor == null)
                throw new DocumentProcessingException($"Unknown sdt (content control) tag: '{tagValue}'.", sdtElement);

            var result = processor.Process(dataSource);
            sdtElement.AddAfterSelf(result);
            sdtElement.Remove();
        }
    }
}
