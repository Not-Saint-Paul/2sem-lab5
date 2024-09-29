using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";
            List<string> dictionaryForWrongWords = new List<string>();
            List<string> dictionaryForCorrectedWords = new List<string>();
            bool isSelected = false;

            do
            {
                Console.WriteLine("Введите способ заполнения словаря с ошибочными словами (В - вручную; Ф - из файла):");
                switch (Console.ReadLine())
                {
                    case "В":
                        isSelected = true;
                        string newWord;

                        Console.WriteLine("Введите ошибочные слова (от 1 до 100). '/Останов' - для прекращения ввода");
                        for (int word = 0; word < 100; ++word)
                        {
                            newWord = Console.ReadLine();
                            dictionaryForWrongWords.Add(newWord);

                            if (newWord == "/Останов")
                            {
                                break;
                            }
                        }

                        break;
                    case "Ф":
                        isSelected = true;

                        Console.WriteLine("Укажите путь к файлу с ошибочными словами");
                        path = Console.ReadLine();

                        using (FileStream fstream = File.OpenRead(path))
                        {
                            byte[] buffer = new byte[fstream.Length];
                            fstream.Read(buffer, 0, buffer.Length);
                            string textFromFile = Encoding.Default.GetString(buffer);
                            string[] words = textFromFile.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var word in words)
                            {
                                dictionaryForWrongWords.Add(word);
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Неверный символ. Повторите ввод");
                        break;
                }
            } while (!isSelected);

            isSelected = false; // почему бы и не использовать одну переменную дважды для той же цели?

            do
            {
                Console.WriteLine("Введите способ заполнения словаря с правильными словами (В - вручную; Ф - из файла):");
                switch (Console.ReadLine())
                {
                    case "В":
                        isSelected = true;
                        string newWord;

                        Console.WriteLine("Введите правильные слова (от 1 до 100). '/Останов' - для прекращения ввода");
                        for (int word = 0; word < 100; ++word)
                        {
                            newWord = Console.ReadLine();
                            dictionaryForCorrectedWords.Add(newWord);

                            if (newWord == "/Останов")
                            {
                                break;
                            }
                        }

                        break;
                    case "Ф":
                        isSelected = true;

                        Console.WriteLine("Укажите путь к файлу с правильными словами");
                        path = Console.ReadLine();

                        using (FileStream fstream = File.OpenRead(path))
                        {
                            byte[] buffer = new byte[fstream.Length];
                            fstream.Read(buffer, 0, buffer.Length);
                            string textFromFile = Encoding.Default.GetString(buffer);
                            string[] words = textFromFile.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var word in words)
                            {
                                dictionaryForCorrectedWords.Add(word);
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Неверный символ. Повторите ввод");
                        break;
                }
            } while (!isSelected);

            Console.WriteLine("Укажите путь к файлу со словами, которые нужно заменить");
            path = Console.ReadLine();

            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                string[] words = textFromFile.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string symbolsForReplacement = "[()-]";
                Regex regexForNumbers = new Regex(symbolsForReplacement);
                string target = " ";

                textFromFile = regexForNumbers.Replace(textFromFile, target);

                foreach(var word in words)
                {
                    for(int numberOfWords = 0; numberOfWords < dictionaryForWrongWords.Count; ++numberOfWords)
                    {
                        if (word == dictionaryForWrongWords[numberOfWords])
                        {
                            textFromFile = textFromFile.Replace(word, dictionaryForCorrectedWords[numberOfWords]);
                        }
                    }
                }

                buffer = Encoding.Default.GetBytes(textFromFile);
                fstream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("Текст исправлен!");
            }
        }
    }
}
