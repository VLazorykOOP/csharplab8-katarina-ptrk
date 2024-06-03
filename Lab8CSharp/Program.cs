using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

class Program
{

    static string ShortenWord(string word)
    {
        if (word.Length <= 2)
        {
            return word; // Якщо слово складається з менше, ніж двох символів, то не міняємо його
        }

        StringBuilder result = new StringBuilder();
        result.Append(word[0]); // Додаємо першу літеру

        char prevChar = word[0];
        for (int i = 1; i < word.Length - 1; i++)
        {
            char currentChar = word[i];
            char nextChar = word[i + 1];

            if (currentChar + 1 == nextChar && nextChar != prevChar + 1)
            {
                // Якщо поточний символ наступний за попереднім і наступний за поточним є наступним за поточним,
                // але не наступним за попереднім, то ми знаємо, що це початок послідовності
                result.Append("-");
            }
            else if (prevChar + 1 != currentChar)
            {
                // Якщо поточний символ не наступний за попереднім, то він не належить до послідовності
                result.Append(currentChar);
            }

            prevChar = currentChar;
        }

        result.Append(word[word.Length - 1]); // Додаємо останню літеру

        return result.ToString();
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Lab #8");
        //task1
        Console.WriteLine("Task 1");
        string inputFilePath = "input8.1.txt";
        string outputFilePath = "output8.1.txt";

        string inputText = File.ReadAllText(inputFilePath);

        string pattern = @"\b[A-Za-z0-9._%+-]+@ukr\.net\b";

        MatchCollection matches = Regex.Matches(inputText, pattern);

        int count = matches.Count;

        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            writer.WriteLine($"Знайдено електронних адрес: {count}");
            foreach (Match match in matches)
            {
                writer.WriteLine(match.Value);
            }
        }

        Console.WriteLine("Завершено.");

        //task2
        Console.WriteLine("Task2");
        inputFilePath = "input8.2.txt";
        outputFilePath = "output8.2.txt";

        string text = File.ReadAllText(inputFilePath);

        string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        StringBuilder result = new StringBuilder();
        foreach (string word in words)
        {
            string shortenedWord = ShortenWord(word);
            result.Append(shortenedWord + " ");
        }

        File.WriteAllText(outputFilePath, result.ToString().Trim());

        Console.WriteLine($"Заміна завершена. Результат записано у файл {outputFilePath}.");

        //task3
        Console.WriteLine("Task3");
        inputFilePath = "input8.3.txt";
        outputFilePath = "output8.3.txt";

        text = File.ReadAllText(inputFilePath);
        words = text.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, int> wordCount = new Dictionary<string, int>();

        foreach (string word in words)
        {
            if (wordCount.ContainsKey(word))
            {
                wordCount[word]++;
            }
            else
            {
                wordCount[word] = 1;
            }
        }

        List<string> uniqueWords = wordCount.Where(pair => pair.Value == 1).Select(pair => pair.Key).ToList();

        File.WriteAllLines(outputFilePath, uniqueWords);
        Console.WriteLine($"Заміна завершена. Результат записано у файл {outputFilePath}.");

        //task 4
        Console.WriteLine("Task4");
        string filePath = "binary_numbers.bin";

        double[] numbers = { 3.14, 2.71, 1.618, 0.577, 1.414 };
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            foreach (double number in numbers)
            {
                writer.Write(number);
            }
        }
        Console.WriteLine("Numbers have been written to the file.");

        double threshold = 2.0;
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            int index = 0;
            double number;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                number = reader.ReadDouble();
                if (index % 2 == 0 && number < threshold)
                {
                    Console.WriteLine($"Component {index}: {number}");
                }
                index++;
            }
        }
        Console.WriteLine("Завершено.");

        //task5
        string studentSurname = "Shevchenko";
        string folderPath1 = $@"D:\\temp\\{studentSurname}1";
        string folderPath2 = $@"D:\\temp\\{studentSurname}2";
        string folderAllPath = @"D:\\temp\\ALL";

        Directory.CreateDirectory(folderPath1);
        Directory.CreateDirectory(folderPath2);

        string text1 = "<Шевченко Степан Іванович, 2001> року народження, місце проживання <м. Суми>";
        File.WriteAllText(Path.Combine(folderPath1, "t1.txt"), text1);

        string text2 = "<Комар Сергій Федорович, 2000 > року народження, місце проживання <м. Київ>";
        File.WriteAllText(Path.Combine(folderPath1, "t2.txt"), text2);

        File.Copy(Path.Combine(folderPath1, "t1.txt"), Path.Combine(folderPath2, "t3.txt"), true);
        File.AppendAllText(Path.Combine(folderPath2, "t3.txt"), File.ReadAllText(Path.Combine(folderPath1, "t2.txt")));


        Console.WriteLine("Files information:");
        string[] allFiles = Directory.GetFiles(folderAllPath);
        foreach (string file in allFiles)
        {
            Console.WriteLine($"File Name: {Path.GetFileName(file)}");
            Console.WriteLine($"File Path: {file}");
            Console.WriteLine($"File Content:");
            Console.WriteLine(File.ReadAllText(file));
            Console.WriteLine();
        }

        File.Move(Path.Combine(folderPath1, "t2.txt"), Path.Combine(folderPath2, "t2.txt"));

        File.Copy(Path.Combine(folderPath1, "t1.txt"), Path.Combine(folderPath2, "t1.txt"));

        Directory.Move(folderPath2, folderAllPath);

        Directory.Delete(folderPath1);

        Console.WriteLine("Full information about files in ALL folder:");
        string[] allFilesAfterChanges = Directory.GetFiles(folderAllPath);
        foreach (string file in allFilesAfterChanges)
        {
            Console.WriteLine($"File Name: {Path.GetFileName(file)}");
            Console.WriteLine($"File Path: {file}");
            Console.WriteLine($"File Content:");
            Console.WriteLine(File.ReadAllText(file));
            Console.WriteLine();
        }
    }
}

