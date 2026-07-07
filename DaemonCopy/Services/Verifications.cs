namespace DaemonCopy.Services
{
    class Verifications
    {
        private readonly Log _logger;

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
            return File.Exists(Path.Combine(path.FullName, file.Name));
        }

        public bool IsFileNew (DirectoryInfo path, FileInfo file)
        {
            var lastWriteLeft = File.GetLastWriteTime(file.FullName);
            var lastWriteRight = File.GetLastWriteTime(Path.Combine(path.FullName, file.Name));

            return lastWriteLeft > lastWriteRight;
        }
    }
}
