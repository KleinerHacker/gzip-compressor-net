namespace GZipCompressor.Types
{
    /// <summary>
    /// Basic interface for targets<br/>
    /// <br/>
    /// Use one of:
    /// <ul>
    /// <li><see cref="DirectoryTarget"/></li>
    /// <li><see cref="TemporaryTarget"/></li>
    /// </ul>
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// Target directory
        /// </summary>
        string TargetDirectory { get; }
    }
}