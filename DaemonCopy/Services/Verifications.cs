using System;
using System.IO;

namespace DaemonCopy.Services
{
    class Verifications
    {
        private Log _logger;
        private bool _isExistFile = false;
        private bool _isFileNew = false;

        public Verifications(Log logger)
        {
            _logger = logger;
        }

        public void CheckPathExist(DirectoryInfo path)
        {
            if (!path.Exists)
            {
                try
                {
                    path.Create();
                }
                catch(Exception ex)
                {
                    _logger.Fatal(ex);
                }
            }
        }

        public bool IsFileExist(DirectoryInfo path, FileInfo file)
        {
            if (File.Exists(path.FullName + "\\" + file.Name))
                _isExistFile = true;
            return _isExistFile;
        }

        public bool IsFileNew (DirectoryInfo path, FileInfo file)
        {
            var lastWriteLeft = File.GetLastWriteTime(file.FullName);
            var lastWriteRight = File.GetLastWriteTime(path.FullName + "\\" + file.Name);
            if (lastWriteLeft > lastWriteRight)
                _isFileNew = true;
            return _isFileNew;
        }
    }
}