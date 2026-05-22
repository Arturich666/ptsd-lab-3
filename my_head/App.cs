using System;
using System.IO;

namespace my_head
{
    public class App
    {
        public static int Run(string[] args, TextReader inReader, TextWriter outWriter, TextWriter errorWriter)
        {
            int linesToPrint = 10; // Стандартна кількість рядків для утиліти head
            string filePath = null;

            // Парсинг аргументів
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-n")
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int n) && n >= 0)
                    {
                        linesToPrint = n;
                        i++; // Пропускаємо значення числа
                    }
                    else
                    {
                        errorWriter.WriteLine("my_head: invalid number of lines");
                        return 2; // Exit code 2 — неправильні аргументи
                    }
                }
                else if (args[i].StartsWith("-"))
                {
                    errorWriter.WriteLine($"my_head: unknown option '{args[i]}'");
                    return 2; // Exit code 2 — неправильні аргументи
                }
                else
                {
                    filePath = args[i];
                }
            }

            try
            {
                if (filePath != null)
                {
                    // Читання з файлу
                    if (!File.Exists(filePath))
                    {
                        errorWriter.WriteLine($"my_head: cannot open '{filePath}' for reading: No such file or directory");
                        return 1; // Exit code 1 — часткова помилка (файл не знайдено)
                    }

                    using (var reader = new StreamReader(filePath))
                    {
                        PrintLines(reader, outWriter, linesToPrint);
                    }
                }
                else
                {
                    // Читання зі stdin (якщо файл не вказано)
                    PrintLines(inReader, outWriter, linesToPrint);
                }

                return 0; // Успішне виконання
            }
            catch (Exception ex)
            {
                errorWriter.WriteLine($"my_head: error: {ex.Message}");
                return 1;
            }
        }

        private static void PrintLines(TextReader reader, TextWriter outWriter, int linesCount)
        {
            for (int i = 0; i < linesCount; i++)
            {
                string line = reader.ReadLine();
                if (line == null) break; // Кінець файлу/потоку
                outWriter.WriteLine(line);
            }
        }
    }
}