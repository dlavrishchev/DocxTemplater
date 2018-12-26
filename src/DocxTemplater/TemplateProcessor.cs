using System;
using System.IO;
using DocxTemplater.DataSources;

namespace DocxTemplater
{
    public sealed class TemplateProcessor : IDisposable
    {
        private MemoryStream _stream;
        private DocxDocument _document;
        private bool _disposed;

        /// <summary>
        /// Creates the template processor instance and loads the .docx template from a file.
        /// </summary>
        /// <remarks>Specified template not changed.</remarks>
        public TemplateProcessor(string filePath)
        {
            Guard.NotNullOrWhiteSpace(filePath, nameof(filePath));
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                Init(fs);
        }

        /// <summary>
        /// Creates the template processor instance and loads the .docx template from a stream.
        /// </summary>
        /// <remarks>
        /// The specified stream is used only for reading and is not used after the method execution.
        /// </remarks>
        public TemplateProcessor(Stream stream)
        {
            Guard.NotNull(stream, nameof(stream));
            Init(stream);
        }

        /// <summary>
        /// Process template.
        /// </summary>
        public void Process(IXmlDataSource dataSource)
        {
            ThrowIfDisposed();
            Guard.NotNull(dataSource, nameof(dataSource));

            var contentControls = _document.GetContentControls();
            foreach (var contentControl in contentControls)
                contentControl.Replace(dataSource);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _document.Dispose();
                _stream.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Save the document to a file.
        /// </summary>
        /// <remarks>The existing file will be overwritten.</remarks>
        public void Save(string filePath)
        {
            ThrowIfDisposed();
            Guard.NotNullOrWhiteSpace(filePath, nameof(filePath));

            _document.Save();
            _document.Dispose();

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                _stream.Position = 0;
                _stream.CopyTo(fs);
            }
        }

        private void Init(Stream stream)
        {
            var validator = new OpenXmlFileValidator();
            if (!validator.IsOpenXmlFile(stream))
                throw new ArgumentOutOfRangeException(nameof(stream), "Invalid file signature. File must be a valid .docx file.");

            _stream = new MemoryStream();
            CopyToMemoryStream(stream);
            CreateDocxDocument();
        }

        private void CopyToMemoryStream(Stream stream)
        {
            stream.CopyTo(_stream);
            _stream.Position = 0;
        }

        private void CreateDocxDocument()
        {
            _document = new DocxDocument(_stream);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(null);
        }
    }
}
