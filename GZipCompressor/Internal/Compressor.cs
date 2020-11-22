using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipCompressor.Internal
{
    internal static class Compressor
    {
        public static void Compress(string[] files, Stream stream, CompressionLevel level)
        {
            using var zipStream = new GZipStream(stream, level);
            using var zipWriter = new BinaryWriter(zipStream, Encoding.UTF8);

            zipWriter.Write(files.LongLength);
            foreach (var file in files)
            {
                zipWriter.Write(file);
                
                using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                fileStream.CopyTo(zipStream);
            }
        }
    }
}