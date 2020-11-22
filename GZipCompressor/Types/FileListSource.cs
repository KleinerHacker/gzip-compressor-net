using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GZipCompressor.Types
{
    /// <summary>
    /// Represent a source based on a plain file list
    /// </summary>
    public sealed class FileListSource : ISource
    {
        public string[] Files { get; }

        public FileListSource(params FileInfo[] files)
        {
            Files = files.Select(x => x.FullName).ToArray();
        }

        public FileListSource(IEnumerable<FileInfo> files)
        {
            Files = files.Select(x => x.FullName).ToArray();
        }

        public FileListSource(params string[] files)
        {
            Files = files;
        }
        
        public FileListSource(IEnumerable<string> files)
        {
            Files = files.ToArray();
        }
    }
}