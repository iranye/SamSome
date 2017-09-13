﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CamelCase
{
    public class Node
    {
        public int Data { get; set; }
        public Node Next { get; set; }
    }

    // This is a "method-only" submission.
    // You only need to complete this method.

    public class Program
    {
        static void Main(string[] args)
        {
            //AmazonScoringProblem();
            //CamelCaseProblem();
            //CompareTheTriplets(args);
            //CheckSosTransmission(args);
            //LookForStringExistence(args);
            //WeightedUniformStrings(args);
            LinkedListStuff();
        }

        public static Node Insert(Node head, int x)
        {
            if (head == null)
            {
                head = new Node { Data = x, Next = null };
            }
            else
            {
                Node current = head.Next;
                Node previous = head.Next;
                while (current != null)
                {
                    current = current.Next;
                    if (current != null)
                    {
                        previous = current;
                    }
                }

                //bool dataSaved = false;
                //do
                //{
                //    Node current = head.Next;
                //    while (current != null)
                //    {
                //        current = current.Next;
                //    }
                //    if (head.Next == null)
                //    {
                //        head.Next = new Node { Data = x, Next = null };
                //        dataSaved = true;
                //    }
                //    else
                //    {
                        
                //    }
                //} while (!dataSaved);
            }
            return head;
        }

        private static void LinkedListStuff()
        {

    }

    public static string[] WeightedUniformStrings(string[] args)
        {
            string s = args.Length > 0 ? args[0] : Console.ReadLine();
            if (String.IsNullOrWhiteSpace(s))
            {
                Console.Error.WriteLine("Invalid input string");
                return null;
            }
            string queryCountStr = args.Length > 1 ? args[1] : Console.ReadLine();
            if (String.IsNullOrWhiteSpace(queryCountStr))
            {
                Console.Error.WriteLine("Invalid query count string");
                return null;
            }
            int n = Convert.ToInt32(queryCountStr);

            Tuple<int, int, string>[] queryTuples = new Tuple<int, int, string>[n];
            int i = 0;
            for (i = 2; i < args.Length; i++)
            {
                queryTuples[i-2] = new Tuple<int, int, string>(i-2, Convert.ToInt32(args[i]), "No");
            }

            List<Tuple<int, int, string>> tupleArrToList = queryTuples.ToList().OrderBy(tup => tup.Item2).ToList();
            List<Tuple<char, int>> charCounts = new List<Tuple<char, int>>();
            List<char> currentChars = new List<char>();

            char[] charArr = s.ToCharArray();

            char currentChar = '0';
            int currentCharWeight = 1;
            for (i = 0; i < charArr.Length; i++)
            {
                if (currentChar == charArr[i])
                {
                    currentCharWeight++;
                }
                else
                {
                    charCounts.Add(new Tuple<char, int>(currentChar, currentCharWeight));
                    currentChar = charArr[i];
                    currentCharWeight = 1;
                }
            }
            if (currentChar == charArr[i-2]) // Test with really short charArr
            {
                currentCharWeight++;
                charCounts.Add(new Tuple<char, int>(currentChar, currentCharWeight));
            }
            else
            {
                charCounts.Add(new Tuple<char, int>(currentChar, currentCharWeight));
            }
            charCounts.RemoveAt(0);
            int[] totalWeights = new int[charCounts.Count];
            for(i = 0; i < charCounts.Count; i++) // TODO: Get only Distinct elements in the list
            {
                totalWeights[i] = (Convert.ToInt32(charCounts[i].Item1) - '`') * charCounts[i].Item2;
                Console.WriteLine($"Found {charCounts[i].Item2} of {charCounts[i].Item1}. totalWeight[{i}]={totalWeights[i]}");
            }

            string[] results = new string[n];
            for (int j = 0; j < n; j++)
            {
                bool queryPass = false;
                for (i = 0; i < totalWeights.Length; i++)
                {
                    int quotient = totalWeights[i] % queryTuples[j].Item2;
                    //int charVal = Convert.ToInt32(
                    // use formula: queryTuples[j].Item2 % 'p' int value == 0 && totalWeights[i] > queryTuples[j].Item2
                    // e.g., 429744 % 16 == 0 && 735920 > 429744
                    if (quotient == 0)
                    {
                        results[j] = "Yes";
                        Console.WriteLine("Yes");
                        queryPass = true;
                        break;
                    }
                }
                if (!queryPass)
                {
                    results[j] = "No";
                    Console.WriteLine("No");
                }
            }
            return results;
        }

        public static bool[] LookForStringExistence(string[] args)
        {
            string queryCount = String.Empty;
            queryCount = args.Length > 0 ? args[0] : Console.ReadLine();
            if (String.IsNullOrWhiteSpace(queryCount))
            {
                Console.Error.WriteLine("Invalid query count string");
                return null;
            }
            bool[] results = new bool[args.Length - 1];
            string searchForString = "hackerrank";
            Stack<char> searchStack = new Stack<char>(searchForString.Length);
            int q = Convert.ToInt32(queryCount);

            for (int a0 = 0; a0 < q; a0++)
            {
                foreach (var el in searchForString.Reverse().ToArray())
                {
                    searchStack.Push(el);
                }
                string s = args[a0+1];
                foreach (var subChar in s.ToCharArray())
                {
                    if (searchStack.Count > 0 && subChar == searchStack.Peek())
                    {
                        searchStack.Pop();
                    }
                }
                Console.WriteLine(searchStack.Count == 0 ? "YES" : "NO");
                results[a0] = searchStack.Count == 0;
                searchStack.Clear();
            }
            return results;
        }

        public static int CheckSosTransmission(string[] args)
        {
            string trans = String.Empty;
            trans = args.Length > 0 ? args[0] : Console.ReadLine();
            if (String.IsNullOrWhiteSpace(trans) || trans.Length % 3 != 0)
            {
                Console.Error.WriteLine("Invalid transmission string");
                return 0;
            }
            int startInd = 0;
            int subStrLen = 3;
            int alteredMessageCount = 0;
            do
            {
                string subStr = trans.Substring(startInd, subStrLen);
                if (subStr != "SOS")
                {
                    char[] charArr = subStr.ToCharArray();
                    for(int i = 0; i < charArr.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                            case 2:
                                if (charArr[i] != 'S')
                                {
                                    alteredMessageCount++;
                                }
                            break;
                            case 1:
                                if (charArr[i] != 'O')
                                {
                                    alteredMessageCount++;
                                }
                            break;
                        }
                    }
                }
                startInd += subStrLen;
            } while (trans.Length > startInd);
            
            Console.WriteLine(alteredMessageCount);
            return alteredMessageCount;
        }

        private static void CompareTheTriplets(string[] args)
        {
            int a0, a1, a2, b0, b1, b2 = 0;
            
            if (args.Length != 6)
            {
                string[] tokens_a0 = Console.ReadLine().Split(' ');
                a0 = Convert.ToInt32(tokens_a0[0]);
                a1 = Convert.ToInt32(tokens_a0[1]);
                a2 = Convert.ToInt32(tokens_a0[2]);
                string[] tokens_b0 = Console.ReadLine().Split(' ');
                b0 = Convert.ToInt32(tokens_b0[0]);
                b1 = Convert.ToInt32(tokens_b0[1]);
                b2 = Convert.ToInt32(tokens_b0[2]);
            }
            else
            {
                int i = 0;
                a0 = Convert.ToInt32(args[i++]);
                a1 = Convert.ToInt32(args[i++]);
                a2 = Convert.ToInt32(args[i++]);
                b0 = Convert.ToInt32(args[i++]);
                b1 = Convert.ToInt32(args[i++]);
                b2 = Convert.ToInt32(args[i++]);
            }
            int[] result = Solve(a0, a1, a2, b0, b1, b2);
            Console.WriteLine(String.Join(" ", result));
        }

        private static int[] Solve(int a0, int a1, int a2, int b0, int b1, int b2)
        {
            Console.WriteLine($"Solving for {a0}, {a1}, {a2}, {b0}, {b1}, {b2}");
            int aliceScore = 0;
            int bobScore = 0;
            if (a0 > b0)
            {
                aliceScore++;
            }
            else if (b0 > a0)
            {
                bobScore++;
            }
            if (a1 > b1)
            {
                aliceScore++;
            }
            else if (b1 > a1)
            {
                bobScore++;
            }
            if (a2 > b2)
            {
                aliceScore++;
            }
            else if (b2 > a2)
            {
                bobScore++;
            }
            return new[] {aliceScore, bobScore};
        }

        #region AmazonScoringProblem
        private static void AmazonScoringProblem()
        {
            /// [[Item1, Item2], [Item3, Item4]] => expecting Item1, Item2
            /// [[Item1, Item2], [Item3, Item4], [Item4, Item5]] => expecting Item3, Item4, Item5
            /// 
            /// [a, b, c], [d, e, f, g] => [d, e, f, g]
            List<string> sortedLists = GetSortedInputs();
            if (sortedLists.Count == 0)
            {
                Console.Error.WriteLine("No strings entered");
                return;
            }
            var longestStringChain = GetLongestStringChain(sortedLists);
            Console.WriteLine($"longestStringChain: {longestStringChain}");
        }

        public static string GetLongestStringChain(List<string> sortedLists)
        {
            string longestStringChain = "";
            foreach (var el in sortedLists)
            {
                if (String.IsNullOrWhiteSpace(longestStringChain))
                {
                    longestStringChain = el;
                    continue;
                }

                List<string> currentLongest = longestStringChain.Split(' ').ToList();
                List<string> split = el.Split(' ').ToList();
                bool stringsMerged = false;
                foreach (var str in split)
                {
                    if (currentLongest.Contains(str))
                    {
                        currentLongest = new List<string>(currentLongest);
                        currentLongest.AddRange(split);
                        currentLongest = currentLongest.Distinct().OrderBy(s => s).ToList();
                        longestStringChain = String.Join(" ", currentLongest);
                        stringsMerged = true;
                        break;
                    }
                }
                if (!stringsMerged && (split.Count > currentLongest.Count))
                {
                    longestStringChain = el;
                }
            }

            return longestStringChain;
        }

        private static List<string> GetSortedInputs()
        {
            Console.WriteLine("Enter a series of sets of strings separated by a single space, then enter when done.");

            List<string> inputStrings = new List<string>();
            string inputStr = "";
            do
            {
                inputStr = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(inputStr))
                {
                    string[] strArr = inputStr.Split(' ');
                    IOrderedEnumerable<string> strArrSorted = strArr.ToList().OrderBy(s => s);
                    inputStrings.Add(String.Join(" ", strArrSorted));
                    inputStrings = inputStrings.OrderBy(s => s).ToList();
                }

            } while (!String.IsNullOrWhiteSpace(inputStr));
            foreach (var list in inputStrings)
            {
                Console.WriteLine(list);
            }
            return inputStrings;
        }

        #endregion

        private static void CamelCaseProblem()
        {
            // https://www.hackerrank.com/challenges/camelcase
            string s = Console.ReadLine();
            if (String.IsNullOrEmpty(s))
            {
                Console.Error.WriteLine("No string supplied");
                return;
            }

            const int MIN_UPPERCASE_VALUE = 65;
            const int MAX_UPPERCASE_VALUE = 90;

            char[] chars = s.ToCharArray();
            int wordCount = 1;
            for (int i = 0; i < chars.Length; i++)
            {
                int charToInt = (int)chars[i];
                //                Console.Write($"{chars[i]} => {charToInt} ");
                if (charToInt >= MIN_UPPERCASE_VALUE && charToInt <= MAX_UPPERCASE_VALUE)
                {
                    wordCount++;
                }
            }
            Console.WriteLine(wordCount);
        }
    }
}
