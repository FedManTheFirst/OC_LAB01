using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.IO.Compression;

namespace OC_LAB01_Pyrshiev_BBBO10
{
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Введите номер задания(1-5): ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    First();
                    break;
                case "2":
                    Second();
                    break;
                case "3":
                await Third(args);
                    break;
                case "4":
                    Fourth();
                    break;
                case "5":
                    Fifth();
                    break;
                default:
                    break;

            }
            //Первое задание
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
            //Второе задание
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
            //Третье задание
              static async Task Third(string[] args)
              {
                  string path = @"C:\Documents";

                  // сохранение данных
                  using (FileStream fs = new FileStream($"{path}\\user.json", FileMode.OpenOrCreate))
                  {
                    User tom = new User() { Name = "Tom", Age = 35 };
                      await JsonSerializer.SerializeAsync<User>(fs, tom);
                      Console.WriteLine("Data has been saved to file");
                  }

                  // чтение данных
                  using (FileStream fs = new FileStream($"{path}\\user.json", FileMode.OpenOrCreate))
                  {
                    User restoredPerson = await JsonSerializer.DeserializeAsync<User>(fs);
                      Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
                  }

                  // удаление файла
                  File.Delete(@"C:\Documents\user.json");
                  Console.WriteLine("Файл удалён");
                  Console.ReadLine();
              }
            //Четвёртое задание
            /*Перед выполнением создать файл вида:
<?xml version="1.0" encoding="utf-8"?>
<users>
</users>
            */
            static void Fourth()
            {
                string path = @"C:\Documents";

                List<User> users = new List<User>();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load($"{path}\\users.xml");
                XmlElement xRoot = xDoc.DocumentElement;

                // создаем новый элемент user
                XmlElement userElem = xDoc.CreateElement("user");
                // создаем атрибут name
                XmlAttribute nameAttr = xDoc.CreateAttribute("name");
                // создаем элементы company и age
                XmlElement ageElem = xDoc.CreateElement("age");
                // создаем текстовые значения для элементов и атрибута
                Console.WriteLine("Создание нового узла");
                Console.WriteLine("Введите имя пользователя: ");
                string name = Console.ReadLine();
                Console.WriteLine("Введите возраст пользователя: ");
                string age = Console.ReadLine();
                XmlText nameText = xDoc.CreateTextNode(name);
                XmlText ageText = xDoc.CreateTextNode(age);

                //добавляем узлы
                nameAttr.AppendChild(nameText);
                ageElem.AppendChild(ageText);
                userElem.Attributes.Append(nameAttr);
                userElem.AppendChild(ageElem);
                xRoot.AppendChild(userElem);
                xDoc.Save($"{path}\\users.xml");
                Console.WriteLine("Данные сохранены");
                //Вывод данных
                Console.WriteLine("Вывод данных");
                foreach (XmlElement xnode in xRoot)
                {
                    User user = new User();
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        user.Name = attr.Value;

                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "age")
                            user.Age = Int32.Parse(childnode.InnerText);
                    }
                    users.Add(user);
                }
                foreach (User u in users)
                    Console.WriteLine($"Имя: {u.Name} Возраст: {u.Age}");

                // удаление файла
                File.Delete(@"C:\Documents\users.xml");
                Console.WriteLine("Файл удалён");
                Console.ReadLine();
            }
            //Пятое задание
            static void Fifth()
            {
                string sourceFile = @"C:\Documents\user.txt"; // исходный файл
                string compressedFile = @"C:\Documents\user.gz"; // сжатый файл
                string targetFile = @"C:\Documents\user_new.txt"; // восстановленный файл

                // создание сжатого файла
                Compress(sourceFile, compressedFile);
                // чтение из сжатого файла
                Decompress(compressedFile, targetFile);

                Console.ReadLine();
            }
            //Архивирование
            static void Compress(string sourceFile, string compressedFile)
            {
                // поток для чтения исходного файла
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                                sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                        }
                    }
                }
            }
            ///Разархивирование
            static void Decompress(string compressedFile, string targetFile)
            {
                // поток для чтения из сжатого файла
                using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                {
                    // поток для записи восстановленного файла
                    using (FileStream targetStream = File.Create(targetFile))
                    {
                        // поток разархивации
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                            Console.WriteLine("Восстановлен файл: {0}", targetFile);
                        }
                    }
                }
            }
        }
    }
}


