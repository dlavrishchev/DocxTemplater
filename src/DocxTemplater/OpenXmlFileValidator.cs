using System.IO;

namespace DocxTemplater
{
    internal sealed class OpenXmlFileValidator
    {
        public bool IsOpenXmlFile(Stream stream)
        {
            var probe = GetProbe(stream);
            return probe[0] == 0x50 && probe[1] == 0x4B;
        }

        private byte[] GetProbe(Stream stream)
        {
            var origPosition = stream.Position;
            var probe = new byte[2];

            stream.Position = 0;
            stream.Read(probe, 0, probe.Length);
            stream.Position = origPosition;

            return probe;
        }
    }
}
