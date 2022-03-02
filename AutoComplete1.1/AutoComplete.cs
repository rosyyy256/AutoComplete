using System;
using System.Collections.Generic;
using System.Linq;
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
        private class Trie
        {
            private class Node
            {
                private bool _isValue;
                private string _value;
                private readonly Dictionary<char, Node> _children;

                public Node()
                {
                    _children = new Dictionary<char, Node>();
                    _isValue = false;
                }

                public void SetValue(string value)
                {
                    _isValue = true;
                    _value = value;
                }

                public bool Contains(char ch) => _children.ContainsKey(ch);

                public Node GetChild(char ch) => _children[ch];

                public void Add(char ch)
                {
                    _children.Add(ch, new Node());
                }
                
                /// <summary>
                /// Bypass in breadth to get all downstream values 
                /// </summary>
                public static List<string> GetDownstreamValues(Node node)
                {
                    var result = new List<string>();
                    var queue = new Queue<Node>();
                    queue.Enqueue(node);

                    while (queue.Count > 0)
                    {
                        var currentNode = queue.Dequeue();
                        if (currentNode._isValue) result.Add(currentNode._value);

                        foreach (var child in currentNode._children.Values)
                        {
                            queue.Enqueue(child);
                        }
                    }

                    return result;
                }
            }

            private readonly Node _head;

            public Trie()
            {
                _head = new Node();
            }

            /// <summary>
            /// Inserts the value at the correct location in the trie 
            /// </summary>
            public void Insert(string value)
            {
                var currentNode = _head;
                var valueLength = value.Length;

                for (var i = 0; i < valueLength; i++)
                {
                    if (!currentNode.Contains(value[i])) currentNode.Add(value[i]);
                    currentNode = currentNode.GetChild(value[i]);
                }
                
                currentNode.SetValue(value);
            }

            public List<string> Search(string desiredStr)
            {
                var currentNode = _head;
                var strLength = desiredStr.Length;

                for (var i = 0; i < strLength; i++)
                {
                    if (!currentNode.Contains(desiredStr[i])) return new List<string>();
                    currentNode = currentNode.GetChild(desiredStr[i]);
                }

                return Node.GetDownstreamValues(currentNode);
            }
        }
        
        private readonly Trie _trie;

        public AutoCompleter()
        {
            _trie = new Trie();
        }

        /// <summary>
        /// Add person's full names list to abstract data base 
        /// </summary>
        public void AddToSearch(List<FullName> fullNames)
        {
            foreach (var fullName in fullNames)
            {
                fullName.Trim();
                _trie.Insert(fullName.ToString());
            }
        }

        /// <summary>
        /// Search all matching full names by prefix
        /// </summary>
        /// <returns>List of matching full names</returns>
        public List<string> Search(string prefix)
        {
            prefix = TrimPrefix(prefix);
            return _trie.Search(prefix);
        }

        private static string TrimPrefix(string str)
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