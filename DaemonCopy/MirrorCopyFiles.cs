using DaemonCopy.Services;
using System.IO;

namespace DaemonCopy
{
    class MirrorCopyFiles : ICopyFiles
    {

        private static Log _logger;

        public MirrorCopyFiles()
        {
            _logger = new Log();
        }

        public MirrorCopyFiles(Log logger)
        {
            _logger = logger;
        }

        public void CopyFilesLefttoRight(DirectoryInfo leftPath, DirectoryInfo rightPath)
        {
            var verify = new Verifications(_logger);
            var fileOperation = new FileSystem(_logger);

            verify.CheckPathExist(rightPath);

            var files = leftPath.GetFiles();

            foreach (var file in files)
            {
                if (verify.IsFileExist(rightPath, file))
                {
                    if (verify.IsFileNew(rightPath, file))
                    {
                        fileOperation.Delete(rightPath, file);
                        fileOperation.Copy(rightPath, file);
                    }
                }
                else
                {
                    fileOperation.Copy(rightPath, file);
                }
            }

            var dirs = leftPath.GetDirectories();

            foreach (var dir in dirs)
            {
                var destinationDir = Path.Combine(rightPath.FullName, dir.Name);
                
                CopyFilesLefttoRight(dir, new DirectoryInfo(destinationDir));
            }
        }
    }
}