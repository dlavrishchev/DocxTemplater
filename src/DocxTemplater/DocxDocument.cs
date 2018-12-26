using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocxTemplater.ContentControls;

namespace DocxTemplater
{
    internal sealed class DocxDocument : IDisposable
    {
        private const string MainDocumentEntryName = "word/document.xml";
        private const string ContentTypesEntryName = "[Content_Types].xml";

        private readonly ZipWorker _zipWorker;
        private bool _disposed;

        private MainDocumentPart _mainPart;
        private IEnumerable<HeaderFooterPart> _headerParts;
        private IEnumerable<HeaderFooterPart> _footerParts;
        private ContentTypesPart _contentTypesPart;

        public DocxDocument(Stream documentStream)
        {
            _zipWorker = new ZipWorker(documentStream);
            ReadMainPart();
            ReadContentTypesPart();
            ReadHeaderParts();
            ReadFooterParts();
        }

        public IEnumerable<ContentControl> GetContentControls()
        {
            var sdtElements = _mainPart.GetSdtElements().
                Concat(_headerParts.SelectMany(h => h.GetSdtElements())).
                Concat(_footerParts.SelectMany(f => f.GetSdtElements())).ToArray();

            var creator = new ContentControlCreator();
            return sdtElements.Select(e => creator.Create(e));
        }

        public void Save()
        {
            SaveMainPart();
            SaveHeaderParts();
            SaveFooterParts();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _zipWorker.Dispose();
                _disposed = true;
            }
        }

        private void SaveMainPart()
        {
            var stream = _zipWorker.PrepareEntryForUpdate(MainDocumentEntryName);
            _mainPart.SaveToStream(stream);
        }

        private void SaveHeaderParts()
        {
            foreach (var part in _headerParts)
            {
                var stream = _zipWorker.PrepareEntryForUpdate(part.PartName);
                part.SaveToStream(stream);
            }
        }

        private void SaveFooterParts()
        {
            foreach (var part in _footerParts)
            {
                var stream = _zipWorker.PrepareEntryForUpdate(part.PartName);
                part.SaveToStream(stream);
            }
        }

        private void ReadMainPart()
        {
            using (var stream = _zipWorker.GetEntryStream(MainDocumentEntryName))
                _mainPart = new MainDocumentPart(stream);
        }

        private void ReadContentTypesPart()
        {
            using (var stream = _zipWorker.GetEntryStream(ContentTypesEntryName))
                _contentTypesPart = new ContentTypesPart(stream);
        }

        private void ReadHeaderParts()
        {
            _headerParts = ReadHeaderFooterParts(_contentTypesPart.HeaderPartNames).ToArray();
        }

        private void ReadFooterParts()
        {
            _footerParts = ReadHeaderFooterParts(_contentTypesPart.FooterPartNames).ToArray();
        }

        private IEnumerable<HeaderFooterPart> ReadHeaderFooterParts(IEnumerable<string> partNames)
        {
            return partNames.Select(partName =>
            {
                using (var stream = _zipWorker.GetEntryStream(partName))
                    return new HeaderFooterPart(stream) { PartName = partName };
            });
        }
    }
}
