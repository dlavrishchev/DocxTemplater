using System;
using System.IO;
using System.IO.Compression;

namespace DocxTemplater
{
    internal sealed class ZipWorker : IDisposable
    {
        private ZipArchive _zipFile;
        private bool _disposed;

        public ZipWorker(Stream stream)
        {
            _zipFile = new ZipArchive(stream, ZipArchiveMode.Update, true);
        }

        public Stream GetEntryStream(string entryName)
        {
            var entry = GetEntry(entryName);
            return entry?.Open();
        }

        public Stream PrepareEntryForUpdate(string entryName)
        {
            DeleteEntry(entryName);
            var entry = CreateEntry(entryName);
            return entry.Open();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_zipFile != null)
                {
                    _zipFile.Dispose();
                    _zipFile = null;
                    _disposed = true;
                }
            }
        }

        private ZipArchiveEntry CreateEntry(string entryName)
        {
            return _zipFile.CreateEntry(entryName);
        }

        private ZipArchiveEntry GetEntry(string entryName)
        {
            return _zipFile.GetEntry(entryName);
        }

        private void DeleteEntry(string entryName)
        {
            var entry = GetEntry(entryName);
            entry?.Delete();
        }
    }
}
