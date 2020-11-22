using System;
using System.IO;
using System.IO.Compression;
using GZipCompressor.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZip.Test
{
    [TestClass]
    public class DecompressorTest
    {
        [TestMethod]
        public void TestDecompress()
        {
            using var stream = new FileStream(Environment.CurrentDirectory + "/Data/test.gzip", FileMode.Open, FileAccess.Read);
            GZipCompressor.GZip.Decompress(new DirectoryTarget(Environment.CurrentDirectory + "/Target"), stream, overwrite: true);
            
            Assert.IsTrue(File.Exists(Environment.CurrentDirectory + "/Target/test.txt"));
            Assert.AreEqual("This is a text", File.ReadAllText(Environment.CurrentDirectory + "/Target/test.txt"));
        }
    }
}