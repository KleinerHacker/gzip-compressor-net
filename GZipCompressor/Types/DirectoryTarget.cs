using System;
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
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }
            if (!File.GetAttributes(targetDirectory).HasFlag(FileAttributes.Directory))
                throw new InvalidOperationException("Target directory is a file: " + targetDirectory);
        }

        public DirectoryTarget(DirectoryInfo targetDirectory)
            :this(targetDirectory.FullName)
        {
        }
    }
}