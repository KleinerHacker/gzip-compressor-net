using System.IO;

namespace GZipCompressor.Types
{
    /// <summary>
    /// Represent a plain directory target
    /// </summary>
    public sealed class DirectoryTarget : ITarget
    {
        public string TargetDirectory { get; }

        public DirectoryTarget(string targetDirectory)
        {
            TargetDirectory = targetDirectory;
        }

        public DirectoryTarget(DirectoryInfo targetDirectory)
            :this(targetDirectory.FullName)
        {
        }
    }
}