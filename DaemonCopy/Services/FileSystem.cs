using System;
using System.IO;

namespace DaemonCopy.Services
{
    class FileSystem
    {
        private Log _logger;

        public FileSystem(Log logger)
        {
            _logger = logger;
        }

        public void Delete(DirectoryInfo path, FileInfo file)
        {
            try
            {
                if (path != null) File.Delete(path.FullName + "\\" + file.Name);
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex);
            }
        }

        public void Copy(DirectoryInfo path, FileInfo file)
        {
            try
            {
                File.Copy(sourceFileName: file.FullName, destFileName: path.FullName + "\\" + file.Name);
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex);
            }
        }
    }
}