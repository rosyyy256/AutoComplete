using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete
{
    public struct FullName
    {
        public string Name;
        public string Surname;
        public string Patronymic;

        public override string ToString() => $"{Surname} {Name} {Patronymic}".Trim();

        public void Trim()
        {
            Name = Name?.Trim();
            Surname = Surname?.Trim();
            Patronymic = Patronymic?.Trim();
        }
    }

    public class AutoCompleter
    {
        private readonly Dictionary<string, List<string>> _fullNameSet;

        public AutoCompleter()
        {
            _fullNameSet = new Dictionary<string, List<string>>();
        }

        public void AddToSearch(List<FullName> fullNames)
        {
            foreach (var fullName in fullNames)
            {
                fullName.Trim();
                var fullNameStr = fullName.ToString();
                var length = fullNameStr.Length;
                
                for (var i = length; i > 0; i--)
                {
                    var possiblePrefix = fullNameStr.Substring(0, i);
                    if (!_fullNameSet.ContainsKey(possiblePrefix)) _fullNameSet.Add(possiblePrefix, new List<string>());
                    _fullNameSet[possiblePrefix].Add(fullNameStr);
                }
            }
        }

        public List<string> Search(string prefix)
        {
            prefix = TrimPrefix(prefix);

            return _fullNameSet.TryGetValue(prefix, out var result) ? result : new List<string>();
        }

        public static string TrimPrefix(string str)
        {
            var sb = new StringBuilder();
            var strLength = str.Length;
            var i = 0;
            
            while (i < strLength && str[i] == ' ') i++;
            for (; i < strLength && str[i] != ' '; i++)
                sb.Append(str[i]);

            while (i < strLength && str[i] == ' ') i++;
            if (i < strLength) sb.Append(' ');
            for (; i < strLength && str[i] != ' '; i++)
                sb.Append(str[i]);

            while (i < strLength && str[i] == ' ') i++;
            if (i < strLength) sb.Append(' ');
            for (; i < strLength && str[i] != ' '; i++)
                sb.Append(str[i]);
            
            if (sb.Length == 0 || sb.Length > 100)
                throw new ArgumentException(
                    "Query parameter cannot be NULL or an empty string or an white-space string");
            if (sb.Length > 100)
                throw new ArgumentException(
                    "Query parameter cannot be more than 100 characters");
            
            return sb.ToString();
        }
    }
}