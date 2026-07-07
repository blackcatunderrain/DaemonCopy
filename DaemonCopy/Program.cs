using DaemonCopy.Services;

namespace DaemonCopy
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Неверно указаны параметры запуска.");
                Console.WriteLine("Использование: DaemonCopy \"source path\" \"destination path\"");
                return 1;
            }

            var logger = new Log("Main");

            var fileLog = new LogRotation();
            fileLog.Rotation();

            var source = new DirectoryInfo(args[0]);
            var destination = new DirectoryInfo(args[1]);

            if (!source.Exists)
            {
                logger.Message($"Папка-источник не найдена: {source.FullName}");
                Console.WriteLine($"Папка-источник не найдена: {source.FullName}");
                return 2;
            }

            ICopyFiles mirrorCopyFiles = new MirrorCopyFiles(logger);
            mirrorCopyFiles.CopyFilesLefttoRight(source, destination);

            logger.Message(" Архивация закончена!");
            return 0;
        }
    }
}
