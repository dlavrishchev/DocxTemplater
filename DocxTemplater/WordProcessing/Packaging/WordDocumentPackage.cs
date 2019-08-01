using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace DocxTemplater.WordProcessing.Packaging
{
    internal class WordDocumentPackage : ZipArchive
    {
        private const string MainDocumentEntryName = "word/document.xml";
        private const string ContentTypesEntryName = "[Content_Types].xml";

        private XDocument _mainDocumentPart;
        private readonly Dictionary<string, XDocument> _headerParts;
        private readonly Dictionary<string, XDocument> _footerParts;
        private XDocument _contentTypesPart;

        private bool _mainDocumentPartLoaded;
        private bool _headerPartsLoaded;
        private bool _footerPartsLoaded;
        private bool _contentTypesPartLoaded;

        public XDocument MainDocument
        {
            get
            {
                if (!_mainDocumentPartLoaded)
                    LoadMainDocumentPart();
                return _mainDocumentPart;
            }
        }

        public IEnumerable<XDocument> Headers
        {
            get
            {
                if(!_contentTypesPartLoaded)
                    LoadContentTypesPart();

                if(!_headerPartsLoaded)
                    LoadHeaderParts();

                return _headerParts.Values.ToArray();
            }
        }

        public IEnumerable<XDocument> Footers
        {
            get
            {
                if (!_contentTypesPartLoaded)
                    LoadContentTypesPart();

                if (!_footerPartsLoaded)
                    LoadFooterParts();

                return _footerParts.Values.ToArray();
            }
        }

        public WordDocumentPackage(Stream stream) : base(stream, mode: ZipArchiveMode.Update, leaveOpen: true)
        {
            _headerParts = new Dictionary<string, XDocument>();
            _footerParts = new Dictionary<string, XDocument>();
        }

        public void Save()
        {
            if(_mainDocumentPartLoaded)
                SavePart(_mainDocumentPart, MainDocumentEntryName);

            if (_headerPartsLoaded)
            {
                foreach (var p in _headerParts)
                {
                    SavePart(p.Value, p.Key);
                }
            }

            if (_footerPartsLoaded)
            {
                foreach (var p in _footerParts)
                {
                    SavePart(p.Value, p.Key);
                }
            }
        }

        private XDocument LoadPart(string entryName)
        {
            using (var stream = GetEntry(entryName).Open())
                return XDocument.Load(stream);
        }

        private void LoadMainDocumentPart()
        {
            _mainDocumentPart = LoadPart(MainDocumentEntryName);
            _mainDocumentPartLoaded = true;
        }

        private void LoadContentTypesPart()
        {
            _contentTypesPart = LoadPart(ContentTypesEntryName);
            _contentTypesPartLoaded = true;
        }

        private void LoadHeaderParts()
        {
            var entryNames = GetEntryNamesForContentType("application/vnd.openxmlformats-officedocument.wordprocessingml.header+xml");

            foreach (var entryName in entryNames)
            {
                _headerParts.Add(entryName, LoadPart(entryName));
            }
            _headerPartsLoaded = true;
        }

        private void LoadFooterParts()
        {
            var entryNames = GetEntryNamesForContentType("application/vnd.openxmlformats-officedocument.wordprocessingml.footer+xml");
            foreach (var entryName in entryNames)
            {
                _footerParts.Add(entryName, LoadPart(entryName));
            }
            _footerPartsLoaded = true;
        }

        private void SavePart(XDocument part, string entryName)
        {
            var entry = RecreateEntry(entryName);
            using (var stream = entry.Open())
                part.Save(stream, SaveOptions.DisableFormatting);
        }

        private ZipArchiveEntry RecreateEntry(string entryName)
        {
            GetEntry(entryName).Delete();
            return CreateEntry(entryName);
        }

        private IEnumerable<string> GetEntryNamesForContentType(string contentType)
        {
            var @namespace = _contentTypesPart.Root.Name.Namespace;
            return _contentTypesPart.Root.Elements(@namespace + "Override").
                Where(e => e.Attribute("ContentType").Value.Equals(contentType, StringComparison.OrdinalIgnoreCase)).
                Select(e =>
                {
                    var entryName = e.Attribute("PartName").Value;
                    return entryName[0].Equals('/') ? entryName.Substring(1) : entryName;
                });
        }
    }
}
