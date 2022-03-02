using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoComplete;
using NUnit.Framework;

namespace AutoCompleteTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Search_Test1()
        {
            var addToSearchList = new List<FullName>
            {
                new FullName
                {
                    Name = "Анастасия"
                },
                new FullName
                {
                    Name = "Анна"
                },
                new FullName
                {
                    Name = "Афанасья"
                },
                new FullName
                {
                    Name = "Богдан"
                },
                new FullName
                {
                    Name = "Боря"
                }
            };
            var autoCompleter = new AutoCompleter();
            autoCompleter.AddToSearch(addToSearchList);
            var actual = autoCompleter.Search("   А   ").OrderBy(n => n).ToList();
            var expected = new List<string>
            {
                "Анастасия",
                "Анна",
                "Афанасья"
            }.OrderBy(n => n).ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
        
        [Test]
        public void Search_Test2()
        {
            var addToSearchList = new List<FullName>
            {
                new FullName
                {
                    Surname = "Хордин",
                    Name = "Анатолий",
                    Patronymic = "Михайлович"
                },
                new FullName
                {
                    Surname = "   Ходайкин   ",
                    Name = "    Миша     ",
                    Patronymic = "   Петрович"
                },
                new FullName
                {
                    Surname = "Арбузова",
                    Name = "Анна",
                    Patronymic = "Павловна"
                },
                new FullName
                {
                    Surname = "Яшин",
                    Name = "Олег",
                    Patronymic = "Анатольевич"
                }
            };
            var autoCompleter = new AutoCompleter();
            autoCompleter.AddToSearch(addToSearchList);
            var actual = autoCompleter.Search("   Хо   ").OrderBy(n => n).ToList();
            var expected = new List<string>
            {
                "Хордин Анатолий Михайлович",
                "Ходайкин Миша Петрович"
            }.OrderBy(n => n).ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestCase("  a  ", "a")]
        [TestCase("a  a   ", "a a")]
        [TestCase("   a    a   ", "a a")]
        [TestCase("   a     a   a      ", "a a a")]
        public void Trim_Test(string str, string expected)
        {
            var actual = AutoCompleter.TrimPrefix(str);
            Assert.AreEqual(expected, actual);
        }
    }
}