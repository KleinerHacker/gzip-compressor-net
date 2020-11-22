using System.IO;

namespace GZipCompressor.Types
{
    /// <summary>
    /// Basic interface for sources<br/>
    /// <br/>
    /// Use one of:
    /// <ul>
    /// <li><see cref="FileListSource"/></li>
    /// </ul>
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// Source Files
        /// </summary>
        string[] Files { get; }
    }
}