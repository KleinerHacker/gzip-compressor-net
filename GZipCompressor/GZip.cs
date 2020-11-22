using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using GZipCompressor.Internal;
using GZipCompressor.Types;

namespace GZipCompressor
{
    public static class GZip
    {
        public static void Compress(ISource source, FileInfo file, CompressionLevel level = CompressionLevel.Optimal)
        {
            using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            Compress(source, stream, level);
        }
        
        public static void Compress(ISource source, byte[] data, CompressionLevel level = CompressionLevel.Optimal)
        {
            using var stream = new MemoryStream(data);
            Compress(source, stream, level);
        }
        
        public static void Compress(ISource source, Stream stream, CompressionLevel level = CompressionLevel.Optimal)
        {
            Compressor.Compress(source.RootDirectory, source.Files, stream, level);
        }

        public static void Decompress(ITarget target, FileInfo file, CompressionLevel level = CompressionLevel.Optimal, bool overwrite = true)
        {
            using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            Decompress(target, stream, level, overwrite);
        }

        public static void Decompress(ITarget target, byte[] data, CompressionLevel level = CompressionLevel.Optimal, bool overwrite = true)
        {
            using var stream = new MemoryStream(data);
            Decompress(target, stream, level, overwrite);
        }

        public static void Decompress(ITarget target, Stream stream, CompressionLevel level = CompressionLevel.Optimal, bool overwrite = true)
        {
            Decompressor.Decompress(target.TargetDirectory, stream, level, overwrite);
        }
    }
}