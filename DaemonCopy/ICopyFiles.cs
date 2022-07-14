using System.IO;

namespace DaemonCopy
{
    interface ICopyFiles
    {
        void CopyFilesLefttoRight(DirectoryInfo dirPath, DirectoryInfo destination);
    }
}