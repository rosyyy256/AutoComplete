using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoComplete
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var fileName = "100000";
            var sr = new StreamReader($"{fileName}.txt");
            var autoCompleter = new AutoCompleter();
            var fullNameList = new List<FullName>();
            var sw = new Stopwatch();
            var rand = new Random();
            var namesCount = int.Parse(fileName);
            Console.WriteLine("Creating full names list");
            var str = sr.ReadToEnd().Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < namesCount; i++)
            {
                var currentStr = str[i].Split(' ');
                var currentFN = new FullName
                {
                    Surname = currentStr[0],
                    Name = currentStr[1],
                    Patronymic = currentStr[2]
                };
                fullNameList.Add(currentFN);
            }
            //fullNameList = GetFullNamesList(50000);

            Console.WriteLine($"List created for {sw.Elapsed}");
            Console.WriteLine("Adding names");
            sw.Restart();
            autoCompleter.AddToSearch(fullNameList);
            Console.WriteLine($"Add to search time - {sw.Elapsed}");
            var randomNames = GetRandomNames(fullNameList, 10);
            foreach (var fullName in randomNames)
            {
                sw.Restart();
                var result = autoCompleter.Search(fullName);
                Console.WriteLine($"{sw.ElapsedTicks} - {result.Count}");
            }
            Console.ReadLine();
        }

        private static List<FullName> GetFullNamesList(int count)
        {
            var random = new Random();
            var result = new List<FullName>();
            for (var i = 0; i < count; i++)
            {
                const int maxWordLength = 100;
                var str = new string[3];
                for (var j = 0; j < 3; j++)
                {
                    var singleStr = new byte[random.Next(34, maxWordLength)];
                    for (var l = 0; l < singleStr.Length; l++)
                    {
                        singleStr[l] = (byte) random.Next(97, 122);
                    }

                    str[j] = Encoding.ASCII.GetString(singleStr);
                }

                result.Add(new FullName
                {
                    Name = str[0],
                    Surname = str[1],
                    Patronymic = str[2]
                });
            }

            return result;
        }

        private static List<string> GetRandomNames(List<FullName> fullNames, int count)
        {
            var rand = new Random();
            var result = new List<string>();
            var length = fullNames.Count;
            for (var i = 0; i < count; i++)
            {
                result.Add(fullNames[rand.Next(length - 1)].ToString().Substring(0, 1));
            }

            return result;
        }
    }
}