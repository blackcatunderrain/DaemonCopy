namespace DaemonCopy.Services
{
    class Verifications
    {
        private readonly Log _logger;

        public Verifications(Log logger)
        {
            _logger = logger;
        }

        public bool TryEnsureDirectoryExists(DirectoryInfo path)
        {
            if (Directory.Exists(path.FullName))
                return true;

            try
            {
                Directory.CreateDirectory(path.FullName);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex);
                return false;
            }
        }

        public bool FileExists(DirectoryInfo path, FileInfo file)
        {
            return File.Exists(Path.Combine(path.FullName, file.Name));
        }

        public bool IsSourceNewer(DirectoryInfo path, FileInfo file)
        {
            var lastWriteLeft = File.GetLastWriteTime(file.FullName);
            var lastWriteRight = File.GetLastWriteTime(Path.Combine(path.FullName, file.Name));

            return lastWriteLeft > lastWriteRight;
        }
    }
}
