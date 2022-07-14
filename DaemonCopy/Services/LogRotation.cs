using System;
using System.IO;

namespace DaemonCopy.Services
{
    class LogRotation
    {
        public LogRotation()
        {

        }

        public void Rotation()
        {
            var moveFileName = $"error_{DateTime.Now:ddMMyyyyHHmm}.log";
            var errorLog = "error.log";

            if (File.Exists(errorLog))
            {
                if (!Directory.Exists("log"))
                    Directory.CreateDirectory("log");
                try
                {
                    File.Move(errorLog, "log\\" + moveFileName);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("DEBUG: " + ex.Message);
                }
            }

            var filesl = Directory.GetFiles("log");

            foreach (var file in filesl)
            {
                if (DateTime.Now - File.GetCreationTime(file) > TimeSpan.FromDays(7d))
                    File.Delete(file);
            }
        }
    }
}
