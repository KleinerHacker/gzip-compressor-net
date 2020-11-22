using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using GZipCompressor.Types;

namespace GZipCompressor.Internal
{
    internal static class Decompressor
    {
        public static void Decompress(string targetDirectory, Stream stream, CompressionLevel level, bool overwrite)
        {
            using var zipStream = new GZipStream(stream, level);
            using var zipReader = new BinaryReader(zipStream, Encoding.UTF8);

            var length = zipReader.ReadInt64();
            for (var i = 0; i < length; i++)
            {
                var relativeFile = zipReader.ReadString();
                var file = targetDirectory + "/" + relativeFile;
                var fileDirectory = new FileInfo(file).Directory;
                if (fileDirectory == null)
                    throw new InvalidOperationException("File with no directory: " + file);
                if (!fileDirectory.Exists)
                {
                    fileDirectory.Create();
                }

                var fileSize = zipReader.ReadInt64();
                using var fileStream = new FileStream(file, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
                CopyStream(zipStream, fileStream, fileSize);
            }
        }
        
        private static void CopyStream(Stream input, Stream output, long bytes)
        {
            var buffer = new byte[81920];
            
            int read;
            while (bytes > 0 && (read = input.Read(buffer, 0, (int) Math.Min(buffer.Length, bytes))) > 0)
            {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }
    }
}