using System;
using System.IO;

namespace OC_LAB01_Pyrshiev_BBBO10
{
        class Program
        {
            public static void Main(string[] args)
            {
            Console.WriteLine("Введите номер задания: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        First();
                        break;
                    case "2":
                        Second();
                        //FileInf.Delete(@"D:\Documents\zxc.txt");
                        break;
                    default:
                        break;

                }
                static void First()
                {
                    DriveInfo[] drives = DriveInfo.GetDrives();

                    foreach (DriveInfo drive in drives)
                    {
                        Console.WriteLine($"Название: {drive.Name}");
                        if (drive.IsReady)
                        {
                            Console.WriteLine($"Объем диска: {drive.TotalSize}");
                            Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                            Console.WriteLine($"Метка диска: {drive.VolumeLabel}");
                            Console.WriteLine($"Тип диска: {drive.DriveType}");
                        }
                        Console.WriteLine();
                    }
                }
                static void Second()
                {

                    string path = @"C:\Documents";
                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                    if (!dirInfo.Exists)
                    {
                        dirInfo.Create();
                    }
                    Console.WriteLine("Введите строку для записи в файл:");
                    string text = Console.ReadLine();

                    // запись в файл
                    using (FileStream fstream = new FileStream($"{path}\\note.txt", FileMode.OpenOrCreate))
                    {
                        // преобразуем строку в байты
                        byte[] array = System.Text.Encoding.Default.GetBytes(text);
                        // запись массива байтов в файл
                        fstream.Write(array, 0, array.Length);
                        Console.WriteLine("Текст записан в файл");
                    }

                    // чтение из файла
                    using (FileStream fstream = File.OpenRead($"{path}\\note.txt"))
                    {
                        // преобразуем строку в байты
                        byte[] array = new byte[fstream.Length];
                        // считываем данные
                        fstream.Read(array, 0, array.Length);
                        // декодируем байты в строку
                        string textFromFile = System.Text.Encoding.Default.GetString(array);
                        Console.WriteLine($"Текст из файла: {textFromFile}");
                    }

                    // удаление файла
                    File.Delete(@"C:\Documents\note.txt");
                    Console.WriteLine("Файл удалён");
                    Console.ReadLine();

                }
            }
        }
    }
