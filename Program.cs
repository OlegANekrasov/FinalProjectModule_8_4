using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace FinalProjectModule_8_4
{
    class Program
    {
        [Serializable]
        class Student
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Student(string name, string group, DateTime dateOfBirth)
            {
                Name = name;
                Group = group;
                DateOfBirth = dateOfBirth;
            }
        }

        // Написать программу-загрузчик данных из бинарного формата в текст.
        // На вход программа получает бинарный файл, предположительно, это база данных студентов.
        // Свойства сущности Student:
        // Имя — Name(string);
        // Группа — Group(string);
        // Дата рождения — DateOfBirth(DateTime).

        //Ваша программа должна:
        // 1.	Создать на рабочем столе директорию Students.
        // 2.	Внутри раскидать всех студентов из файла по группам (каждая группа-отдельный текстовый файл),
        // в файле группы студенты перечислены построчно в формате "Имя, дата рождения".

        static void Main(string[] args)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryInfo dirInfo = new DirectoryInfo(desktop);
            if (!dirInfo.Exists)
            {
                Console.WriteLine("Ошибка доступа к рабочему столу!");
                return;
            }

            var dirStudents = dirInfo.GetDirectories().FirstOrDefault(d => d.Name == "Students");
            if (dirStudents != null)
                dirStudents.Delete(true);

            dirStudents = dirInfo.CreateSubdirectory("Students");

            BinaryFormatter formatter = new BinaryFormatter();
            
            // десериализация
            using (var fs = new FileStream(@"Students.dat", FileMode.OpenOrCreate))
            {
                Student[] students = (Student[])formatter.Deserialize(fs);
                foreach (var gr in students.GroupBy(g => g.Group))
                {
                    using (StreamWriter sw = File.CreateText(dirStudents.FullName + @"\" + gr.Key + ".txt"))
                    {
                        foreach (Student st in gr)
                        {
                            var dateOfBirth = st.DateOfBirth.ToString("dd.MM.yyyy");
                            sw.WriteLine($"{st.Name}, {dateOfBirth}");
                        }
                    }
                }
                Console.WriteLine("Программа завершена!");
           }

            Console.ReadLine();
        }
    }
}
