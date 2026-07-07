namespace DaemonCopy.Services
{
    class FileSystem
    {
        private readonly Log _logger;

        public FileSystem(Log logger)
        {
            _logger = logger;
        }

        public void Delete(DirectoryInfo path, FileInfo file)
        {
            try
            {
                File.Delete(Path.Combine(path.FullName, file.Name));
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
                File.Copy(sourceFileName: file.FullName, destFileName: Path.Combine(path.FullName, file.Name));
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex);
            }
        }
    }
}
