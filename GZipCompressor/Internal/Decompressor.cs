using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipCompressor.Internal
{
    internal static class Decompressor
    {
        public static void Decompress(string targetDirectory, Stream stream, CompressionLevel level)
        {
            using var zipStream = new GZipStream(stream, level);
            using var zipReader = new BinaryReader(zipStream, Encoding.UTF8);

            var length = zipReader.ReadInt64();
            for (var i = 0; i < length; i++)
            {
                var file = zipReader.ReadString();
            }
        }
    }
}