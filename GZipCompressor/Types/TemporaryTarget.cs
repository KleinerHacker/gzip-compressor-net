using System;
using System.IO;
using System.Text;

namespace GZipCompressor.Types
{
    /// <summary>
    /// Represent a temporary target in temp directory.<br/>
    /// This target deletes structure after process exits.
    /// </summary>
    public sealed class TemporaryTarget : ITarget
    {
        public string TargetDirectory { get; }

        public TemporaryTarget(string name)
        {
            TargetDirectory = new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory(name).FullName;
            DeleteOnExit();
        }

        public TemporaryTarget(string prefix = null, string postfix = null)
        {
            var name = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                name.Append(prefix);
            }

            name.Append(Guid.NewGuid().ToString());

            if (!string.IsNullOrWhiteSpace(postfix))
            {
                name.Append(postfix);
            }
            
            TargetDirectory = new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory(name.ToString()).FullName;
            DeleteOnExit();
        }

        private void DeleteOnExit()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
        }

        private void CurrentDomainOnProcessExit(object sender, EventArgs e)
        {
            Directory.Delete(TargetDirectory, true);
        }
    }
}