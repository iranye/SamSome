using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class TestCamelCase
    {
        [TestMethod]
        public void Test_GetLongestChain_SmallLists()
        {
            List<string> listOfStrings = new List<string>
            {
                "blat blit boo",
                "aces bar ventura",
                "bar foo",
            };
            listOfStrings = listOfStrings.OrderBy(s => s).ToList();

            string expectedLongestStringChain = "aces bar foo ventura";
            var actualLongestStringChain = CamelCase.Program.GetLongestStringChain(listOfStrings);
            Assert.AreEqual(expectedLongestStringChain, actualLongestStringChain);
        }

        [TestMethod]
        public void Test_GetLongestChain_LongLists()
        {
            List<string> listOfStrings = new List<string>
            {
                "aces bar ventura",
                "blat blit boo bing bong",
                "bar foo",
            };
            listOfStrings = listOfStrings.OrderBy(s => s).ToList();

            string expectedLongestStringChain = "blat blit boo bing bong";
            var actualLongestStringChain = CamelCase.Program.GetLongestStringChain(listOfStrings);
            Assert.AreEqual(expectedLongestStringChain, actualLongestStringChain);
        }

        [TestMethod]
        public void Test_CheckSosTransmission_ShortStr()
        {
            string input = "SOSSRSBBB";
            int expectedAlteredCharCount = 4;
            var actualLongestStringChain = CamelCase.Program.CheckSosTransmission(new[] { input });
            Assert.AreEqual(expectedAlteredCharCount, actualLongestStringChain);
        }

        [TestMethod]
        public void Test_LookForStringExistence_ShortStrings()
        {
            //Tuple<string, bool> inputs = new Tuple<string, bool>("hereiamstackerrank", true);
            Dictionary<string, bool> inputs = new Dictionary<string, bool>();
            inputs.Add("4", false);
            inputs.Add("hereiamstackerrank", true);
            inputs.Add("hackerworld", false);
            inputs.Add("hackeranklespankr", false);
            inputs.Add("hackerankhackerank", true);

            //inputs.Add("hereiamstackerrank", true);

            string[] keys = inputs.Keys.ToArray();
            bool[] actualResults = CamelCase.Program.LookForStringExistence(keys);
            for (int i = 1, j = 0; i < keys.Length && j < actualResults.Length; i++, j++)
            {
                Assert.AreEqual(inputs[keys[i]], actualResults[j], $"Failed for {keys[i]}");
            }
        }

        [TestMethod]
        public void Test_WeightedUniformStrings_ShortStrings()
        {
            Dictionary<string, string> inputs = new Dictionary<string, string>();

            inputs.Add("1", "Yes");
            inputs.Add("3", "Yes");
            inputs.Add("12", "Yes");
            inputs.Add("5", "Yes");
            inputs.Add("9", "No");
            inputs.Add("10", "No");
            int queryCount = inputs.Count;

            string inputStr = "abccddde";
            List<string> inputsList = new List<string>
            {
                inputStr, queryCount.ToString()
            };

            string[] keys = inputs.Keys.ToArray();
            inputsList.AddRange(keys);

            string[] actualResults = CamelCase.Program.WeightedUniformStrings(inputsList.ToArray());
            for (int i = 0; i < keys.Length; i++)
            {
                Assert.AreEqual(inputs[keys[i]], actualResults[i], $"Failed for {keys[i]}");
            }
        }

        [TestMethod]
        public void Test_WeightedUniformStrings_LongString()
        {
            Dictionary<string, string> inputs = new Dictionary<string, string>();

            inputs.Add("1", "Yes");
            inputs.Add("3", "Yes");
            inputs.Add("12", "Yes");
            inputs.Add("5", "Yes");
            inputs.Add("9", "No");
            inputs.Add("10", "No");
            int queryCount = inputs.Count;

            string inputStr = "kppppppppppppppppppppppppppppppppp";
            List<string> inputsList = new List<string>
            {
                inputStr, queryCount.ToString()
            };

            string[] keys = inputs.Keys.ToArray();
            inputsList.AddRange(keys);

            string[] actualResults = CamelCase.Program.WeightedUniformStrings(inputsList.ToArray());
            for (int i = 0; i < keys.Length; i++)
            {
                Assert.AreEqual(inputs[keys[i]], actualResults[i], $"Failed for {keys[i]}");
            }
        }
        
        [TestMethod]
        public void Test_WeightedUniformStrings_FromFile()
        {
            var filenameExpectedOutputs =
                @"C: \Users\imnyex\source_git\SamSome\Practice\HackerRank\TestInputOutputs\WeightedUniformStrings\output06.txt";
            string line;
            var fileReader = new StreamReader(filenameExpectedOutputs);
            var queries = new List<string>();
            while ((line = fileReader.ReadLine()) != null)
            {
                queries.Add(line.Trim());
            }
            fileReader.Close();

            List<Tuple<string, string>> inputs = new List<Tuple<string, string>>();
            string inputStr = "";

            var filenameInputs =
                @"C: \Users\imnyex\source_git\SamSome\Practice\HackerRank\TestInputOutputs\WeightedUniformStrings\input06.txt";

            int counter = 0;
            fileReader = new StreamReader(filenameInputs);
            int inputSegment = 0;
            //while ((line = fileReader.ReadLine()) != null)
            for (int i = 0; (line = fileReader.ReadLine()) != null; i++)
            {
                switch (inputSegment++)
                {
                    case 0:
                        inputStr = line;
                        break;
                    case 1:
                        string queryCountStr = line;
                        break;
                    default:
                        inputs.Add(new Tuple<string, string>(line, queries[i-2]));
                        break;
                }
            }
            fileReader.Close();
            int queryCount = inputs.Count;
            List<string> inputsList = new List<string>
            {
                inputStr, queryCount.ToString()
            };

            string[] queryStrings = inputs.Select(tup => tup.Item1).ToArray();
            inputsList.AddRange(queryStrings);
            string[] actualResults = CamelCase.Program.WeightedUniformStrings(inputsList.ToArray());
            for (int i = 0; i < queryStrings.Length; i++)
            {
                Assert.AreEqual(queryStrings[i], actualResults[i], $"Failed for query {i}: {queryStrings[i]}");
            }
        }
    }
}
