using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

namespace GZipCompressor.Types
{
    /// <summary>
    /// Represent a source based on a plain file list
    /// </summary>
    public sealed class FileListSource : ISource
    {
        private string[] _files;
        
        public string RootDirectory { get; }

        public string[] Files
        {
            get => _files;
            private set
            {
                _files = value;
                CheckFiles();
            }
        }

        public FileListSource(DirectoryInfo rootDirectory, params FileInfo[] files)
        {
            RootDirectory = rootDirectory.FullName;
            Files = files.Select(x => x.FullName).ToArray();
        }

        public FileListSource(DirectoryInfo rootDirectory, IEnumerable<FileInfo> files)
        {
            RootDirectory = rootDirectory.FullName;
            Files = files.Select(x => x.FullName).ToArray();
        }

        public FileListSource(string rootDirectory, params string[] files)
        {
            RootDirectory = rootDirectory;
            Files = files;
        }
        
        public FileListSource(string rootDirectory, IEnumerable<string> files)
        {
            RootDirectory = rootDirectory;
            Files = files.ToArray();
        }

        private void CheckFiles()
        {
            foreach (var file in Files)
            {
                if (!File.Exists(file))
                    throw new InvalidOperationException("File not found: " + file);
                if (File.GetAttributes(file).HasFlag(FileAttribute.Directory))
                    throw new InvalidOperationException("File is a directory: " + file);
            }
        }
    }
}