using System.IO;
using System.Linq;

namespace GZipCompressor.Types
{
    /// <summary>
    /// Represent a directory source.<br/>
    /// Supports search pattern and recursion.
    /// </summary>
    public sealed class DirectorySource : ISource
    {
        public string[] Files { get; }

        public DirectorySource(string directory, string pattern = "*", bool recursive = true)
        {
            Files = Directory.EnumerateFiles(directory, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToArray();
        }

        public DirectorySource(DirectoryInfo directory, string pattern = "*", bool recursive = true)
            : this(directory.FullName, pattern, recursive)
        {
        }
    }
}