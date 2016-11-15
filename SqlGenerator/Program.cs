using System;
using System.IO;
using System.Linq;

namespace SqlGenerator
{
    class Program
    {
        private static string _fileName = "InstallScript.sql";
        private static string _rootFolderName;
        private static string _filePath;

        static void Main(string[] args)
        {
            _rootFolderName = AppDomain.CurrentDomain.BaseDirectory;
            var rootDir = new DirectoryInfo(_rootFolderName);

            Console.WriteLine("This script will combine all the sql scripts in the scripts folders");
            Console.WriteLine(rootDir.FullName);
            Console.WriteLine("to file: InstallScript.sql. Please enter new name or leave emtpy to confirm and press Enter.");

            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                _fileName = name.EndsWith(".sql") ? name : name + ".sql";
            }
            _filePath = _rootFolderName + _fileName;

            if (File.Exists(_fileName))
            {
                Console.WriteLine(string.Format("Deleting old file {0}", _fileName));
                File.Delete(_fileName);
            }

            foreach (var folderName in FolderNames.Names)
            {
                var subDirectory = rootDir.GetDirectories(folderName, SearchOption.TopDirectoryOnly);
                if (subDirectory != null && subDirectory.Length > 0)
                {
                    Console.WriteLine(string.Format("----- Current folder: {0} -----", subDirectory[0].Name));
                    File.AppendAllLines(_filePath, Directory.GetFiles(subDirectory[0].FullName).OrderBy(f => f).SelectMany(f => new[] { string.Format("/* script file: {0} */", f) }.Concat(File.ReadLines(f).Concat(new[] { Environment.NewLine }))));
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}