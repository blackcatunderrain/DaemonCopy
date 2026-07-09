using System.IO;

namespace DaemonCopy
{
    public interface ICopyFiles
    {
        void CopyFilesLefttoRight(DirectoryInfo dirPath, DirectoryInfo destination);
    }
}
