namespace DaemonCopy.Services
{
    class LogRotation
    {
        private const string LogDirectory = "log";
        private const string ErrorLog = "error.log";

        public LogRotation()
        {

        }

        public void Rotation()
        {
            var moveFileName = $"error_{DateTime.Now:ddMMyyyyHHmm}.log";

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            if (File.Exists(ErrorLog))
            {
                try
                {
                    File.Move(ErrorLog, Path.Combine(LogDirectory, moveFileName));
                }
                catch(Exception ex)
                {
                    Console.WriteLine("DEBUG: " + ex.Message);
                }
            }

            var files = Directory.GetFiles(LogDirectory);

            foreach (var file in files)
            {
                if (DateTime.Now - File.GetCreationTime(file) > TimeSpan.FromDays(7d))
                    File.Delete(file);
            }
        }
    }
}
