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
            var sr = new StreamReader($"100000.txt");
            var autoCompleter = new AutoCompleter();
            var fullNameList = new List<FullName>();
            var sw = new Stopwatch();
            var namesCount = 100000;
            
            Console.WriteLine("Reading full names file");
            var str = sr.ReadToEnd().Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < namesCount; i++)
            {
                var currentStr = str[i].Split(' ');
                var currentFullName = new FullName
                {
                    Surname = currentStr[0],
                    Name = currentStr[1],
                    Patronymic = currentStr[2]
                };
                fullNameList.Add(currentFullName);
            }
            Console.WriteLine($"File read for {sw.Elapsed}");
            
            Console.WriteLine("Adding names to base");
            sw.Restart();
            autoCompleter.AddToSearch(fullNameList);
            Console.WriteLine($"Adding to full names base time - {sw.Elapsed}");
            
            var randomNames = GetRandomCasesToSearch(fullNameList, 10);
            
            foreach (var searchCase in randomNames)
            {
                sw.Restart();
                var result = autoCompleter.Search(searchCase);
                var elapsedTicks = sw.ElapsedTicks;
                Console.WriteLine($"For case {searchCase} found {result.Count} results by {elapsedTicks} ticks");
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

        private static List<string> GetRandomCasesToSearch(List<FullName> fullNames, int count)
        {
            var rand = new Random();
            var result = new List<string>();
            var length = fullNames.Count;
            
            for (var i = 0; i < count; i++)
            {
                result.Add(fullNames[rand.Next(length - 1)].ToString().Substring(0, rand.Next(10, 20)));
            }

            return result;
        }
    }
}