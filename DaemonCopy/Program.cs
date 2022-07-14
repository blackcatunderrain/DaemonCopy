using DaemonCopy.Services;
using System;
using System.IO;

namespace DaemonCopy
{
    internal class Program
    {
        private static string _argS;
        private static string _argD;

        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                 _argS = args[0];
                 _argD = args[1];
            }
            else
            {
                Console.WriteLine("Не верно указаны параметры запуска");
                Console.WriteLine("Использование: DaemonCopy.exe \"parametr 1\" \"parametr 2\"");
                Console.ReadLine();
            }

            var logger = new Log("Main");

            var fileLog = new LogRotation();
            fileLog.Rotation();

            var source = new DirectoryInfo(_argS);
            var destination = new DirectoryInfo(_argD);

            ICopyFiles mirrorCopyFiles = new MirrorCopyFiles(logger);
            mirrorCopyFiles.CopyFilesLefttoRight(source, destination);

            logger.Message(" Архивация закончена!");
        }
    }
}