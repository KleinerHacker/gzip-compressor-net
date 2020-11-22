using System;
using System.IO;
using System.IO.Compression;
using GZipCompressor.Types;
using Microsoft.VisualBasic;

namespace GZip
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("GZip Compressor");
            Console.WriteLine("Pfeiffer C Soft 2020 - Apache License 2.0");
            Console.WriteLine();

            if (args.Length < 3)
            {
                Console.Error.WriteLine("Missing arguments");
                Console.WriteLine();
                PrintHelp();

                return -1;
            }

            var options = ReadOptions(args);
            if (options == null)
                return -1;

            var direction = args[0];
            if (direction.Equals("-c", StringComparison.OrdinalIgnoreCase))
                return HandleCompress(args[1], args[2], options);

            if (direction.Equals("-d", StringComparison.OrdinalIgnoreCase))
                return HandleDecompress(args[1], args[2], options);

            Console.Error.WriteLine("First argument unknown: " + args[0]);
            Console.WriteLine();
            PrintHelp();

            return -1;
        }

        private static int HandleCompress(string sourceFileOrDirectory, string targetFile, Options options)
        {
            if (!File.Exists(sourceFileOrDirectory) && !Directory.Exists(sourceFileOrDirectory))
            {
                Console.Error.WriteLine("No such file or directory: " + sourceFileOrDirectory);
                Console.WriteLine();

                return -1;
            }
            
            if (File.Exists(targetFile) && !options.IsOverwriting)
            {
                Console.Error.WriteLine("Target file already exists: " + targetFile);
                Console.WriteLine();

                return -1;
            }

            if (File.Exists(targetFile) && File.GetAttributes(targetFile).HasFlag(FileAttributes.Directory))
            {
                Console.Error.WriteLine("Target file is a directory: " + targetFile);
                Console.WriteLine();

                return -1;
            }

            ISource source;
            var fileAttributes = File.GetAttributes(sourceFileOrDirectory);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                source = new DirectorySource(sourceFileOrDirectory, options.SearchPattern, options.IsRecursive);
            }
            else
            {
                source = new FileListSource(sourceFileOrDirectory);
            }

            try
            {
                GZipCompressor.GZip.Compress(source, new FileInfo(targetFile), options.CompressionLevel);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Compression failed!");
                Console.Error.WriteLine(e);
                
                return -1;
            }
            
            return 0;
        }

        private static int HandleDecompress(string sourceFile, string targetDirectory, Options options)
        {
            if (!File.Exists(sourceFile))
            {
                Console.Error.WriteLine("No such file: " + sourceFile);
                Console.WriteLine();

                return -1;
            }

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            if (!File.GetAttributes(targetDirectory).HasFlag(FileAttributes.Directory))
            {
                Console.Error.WriteLine("Target directory is a file: " + targetDirectory);
                Console.WriteLine();

                return -1;
            }

            try
            {
                GZipCompressor.GZip.Decompress(new DirectoryTarget(targetDirectory), new FileInfo(sourceFile), options.IsOverwriting);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Decompression failed!");
                Console.Error.WriteLine(e);
                
                return -1;
            }
            
            return 0;
        }

        private static Options ReadOptions(string[] args)
        {
            var options = new Options();
            
            if (args.Length < 4)
                return options;

            for (var i = 3; i < args.Length; i++)
            {
                if (args[i].Equals("-r", StringComparison.OrdinalIgnoreCase) || args[i].Equals("--recursive", StringComparison.OrdinalIgnoreCase))
                {
                    options.IsRecursive = true;
                }
                else if (args[i].Equals("-o", StringComparison.OrdinalIgnoreCase) || args[i].Equals("--overwrite", StringComparison.OrdinalIgnoreCase))
                {
                    options.IsOverwriting = true;
                }
                else if (args[i].Equals("-p", StringComparison.OrdinalIgnoreCase) || args[i].Equals("--pattern", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ArgsLengthCheck(args[i], ++i))
                        return null;
                    
                    options.SearchPattern = args[i];
                }
                else if (args[i].Equals("-l", StringComparison.OrdinalIgnoreCase) || args[i].Equals("--level", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ArgsLengthCheck(args[i], ++i))
                        return null;

                    if (args[i].Equals("no", StringComparison.OrdinalIgnoreCase))
                    {
                        options.CompressionLevel = CompressionLevel.NoCompression;
                    }
                    else if (args[i].Equals("fast", StringComparison.OrdinalIgnoreCase))
                    {
                        options.CompressionLevel = CompressionLevel.Fastest;
                    }
                    else if (args[i].Equals("maximum", StringComparison.OrdinalIgnoreCase))
                    {
                        options.CompressionLevel = CompressionLevel.Optimal;
                    }
                    else
                    {
                        Console.Error.WriteLine("Unknown compression level: " + args[i]);
                        Console.WriteLine();

                        return null;
                    }
                }
                else
                {
                    Console.Error.WriteLine("Unknown argument: " + args[i]);
                    Console.WriteLine();

                    return null;
                }
            }

            return options;

            #region Inner Functions

            bool ArgsLengthCheck(string arg, int index)
            {
                if (index >= args.Length)
                {
                    Console.Error.WriteLine("Missing argument value for " + arg);
                    Console.WriteLine();

                    return false;
                }

                return true;
            }

            #endregion
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Syntax & Usage");
            Console.WriteLine("Compress  : gzip -c <source directory or file> <target file> [options]");
            Console.WriteLine("Decompress: gzip -d <source file> <target directory> [options]");
            Console.WriteLine();
            Console.WriteLine("Options");
            Console.WriteLine("-r, --recursive                       Recursive file search");
            Console.WriteLine("-p, --pattern <search pattern>        File / Directory search pattern");
            Console.WriteLine("-l, --level <tag>                     Compression level: <no>, <fast>, <maximum> (default)");
            Console.WriteLine("-o, --overwrite                       Overwrite any existing files");
            Console.WriteLine();
            Console.WriteLine("Examples");
            Console.WriteLine("Compress  : gzip -c my_dir my_file.gzip");
            Console.WriteLine("Compress  : gzip -c my_dir my_file.gzip -r -p *.txt,*.dat -c fast");
            Console.WriteLine("Decompress: gzip -d my_gzip_file my_dir");
            Console.WriteLine("Decompress: gzip -d my_gzip_file my_dir -o");
            Console.WriteLine();
        }
    }

    public sealed class Options
    {
        public bool IsOverwriting { get; set; } = false;
        public bool IsRecursive { get; set; } = false;
        public string SearchPattern { get; set; } = "*";
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;
    }
}