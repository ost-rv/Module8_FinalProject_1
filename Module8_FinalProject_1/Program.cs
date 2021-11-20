using System;
using System.IO;

namespace Module8_FinalProject_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = string.Empty;
            if (args.Length > 0)
            {
                directoryPath = args[0];
            }
            else
            {
                Console.WriteLine("Аргумент прогроммы directoryPath не задан.");
                return;
            }

            if (string.IsNullOrEmpty(directoryPath) || string.IsNullOrWhiteSpace(directoryPath))
            {
                Console.WriteLine("Путь не указан");
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            ClearDirectory(directoryInfo, true);

        }



        private static void ClearDirectory(DirectoryInfo directoryInfo, bool isDirectoryParant)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            if (!directoryInfo.Exists)
            {
                Console.WriteLine($"Папка по заданному ({directoryInfo.FullName}) пути не существует");
                return;
            }


            DirectoryInfo[] DirectoryInfos = directoryInfo.GetDirectories();

            foreach (DirectoryInfo di in DirectoryInfos)
            {
                ClearDirectory(di, false);
            }

            RemoveFiles(directoryInfo);

            if (isDirectoryParant)
            {
                return;
            }

            TimeSpan timePassed = DateTime.Now.Subtract(directoryInfo.LastAccessTime);

            if (timePassed.TotalMinutes > 30
                && directoryInfo.GetFiles().Length == 0
                && directoryInfo.GetDirectories().Length == 0)
            {
                try
                {
                    directoryInfo.Delete();
                    Console.WriteLine($"Папка {directoryInfo.FullName} удалена");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine($"Ошибка при удалении папки {directoryInfo.FullName}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("--------------------------------------");
                }
            }
        }

        private static void RemoveFiles(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                return;
            }

            foreach (FileInfo fi in directoryInfo.GetFiles())
            {
                TimeSpan timePassed = DateTime.Now.Subtract(fi.LastAccessTime);
                if (timePassed.TotalMinutes > 30)
                {
                    try
                    {
                        fi.Delete();
                        Console.WriteLine($"Файл {fi.FullName} удален");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("--------------------------------------");
                        Console.WriteLine($"Ошибка при удалении файла {fi.FullName}");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("--------------------------------------");
                    }
                }
            }
        }
    }
}
