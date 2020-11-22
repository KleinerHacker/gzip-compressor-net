using System;
using System.IO;
using System.Linq;
using GZipCompressor.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZip.Test
{
    [TestClass]
    public class CompressorTest
    {
        [TestMethod]
        public void TestCompress()
        {
            using var stream = new MemoryStream();
            GZipCompressor.GZip.Compress(new DirectorySource(Environment.CurrentDirectory + "/Data", "*.txt"), stream);

            var expected = File.ReadAllBytes(Environment.CurrentDirectory + "/Data/test.gzip");
            var actual = stream.ToArray();
            
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
