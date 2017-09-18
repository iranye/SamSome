using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamelCase
{
    class Node
    {
        public int Data { get; set; }
        public Node Next { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder($"Data={Data}");
            Node current = Next;
            while (current != null)
            {
                sb.Append($", Data={current.Data}");
                current = current.Next;
            }
            return sb.ToString();
        }
    }

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

        private static void LinkedListStuff()
        {
            var head = GenerateLinkedList();
            Console.WriteLine(head);
            //	Node list = InsertAtHead(head, 5);
            //	Node list = InsertAtEnd(head, 5);
            //	Node list = InsertNth(head, 42, 11);
            //	Node list = DeleteNth(head, 0);
            Node list = ReverseList(head);

            Console.WriteLine(list);
            Console.ReadLine();
        }

        static Node ReverseList(Node head)
        {
            if (head == null || head.Next == null)
            {
                return head;
            }
            if (head.Next == null)
            {
                Console.WriteLine(head.Data);
                return head;
            }

            Node current = head;
            Node previous = null;

            while (current != null)
            {
                var temp = current.Next;
                if (previous == null)
                {
                    previous = head;
                    previous.Next = null;
                }
                else
                {
                    current.Next = previous;
                }
                previous = current;
                current = temp;
                if (current != null)
                {
                    temp = current.Next;
                    current.Next = previous;
                    previous = current;
                    current = temp;
                }

            }
            return previous;
        }

        static Node GenerateLinkedList()
        {
            Node head = null;
            Node current = null;
            int[] arr = { 2, 3, 5, 7, 11 };
            foreach (var el in arr)
            {
                if (head == null)
                {
                    head = new Node { Data = el };
                }
                else
                {
                    if (current == null)
                    {
                        current = new Node { Data = el };
                        head.Next = current;
                    }
                    else
                    {
                        current.Next = new Node { Data = el };
                        current = current.Next;
                    }
                }
            }
            return head;
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

            int[] queries = new int[n];
            int i = 0;
            for (i = 2; i < args.Length; i++)
            {
                queries[i - 2] = Convert.ToInt32(args[i]);
            }
            
            List<Tuple<char, int, int>> charCounts = new List<Tuple<char, int, int>>();

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
                    charCounts.Add(new Tuple<char, int, int>(currentChar, currentCharWeight, Convert.ToInt32(currentChar) - '`'));
                    currentChar = charArr[i];
                    currentCharWeight = 1;
                }
            }

            if (charArr.Length == 1 || currentChar == charArr[i-2])
            {
                currentCharWeight++;
                charCounts.Add(new Tuple<char, int, int>(currentChar, currentCharWeight, Convert.ToInt32(currentChar) - '`'));
            }
            else
            {
                charCounts.Add(new Tuple<char, int, int>(currentChar, currentCharWeight, Convert.ToInt32(currentChar) - '`'));
            }

            charCounts.RemoveAt(0);
            
            string[] results = new string[n];
            bool queryPass;
            for (int j = 0; j < n; j++)
            {
                queryPass = false;
                for (i = 0; i < charCounts.Count; i++) // TODO: Get only Distinct elements in the list
                {
                    int totalCharWeight = charCounts[i].Item2 * charCounts[i].Item3;
                    //Console.WriteLine($"Found {charCounts[i].Item2} of '{charCounts[i].Item1}' (totalWeight={totalCharWeight})");

                    if ((queries[j] % charCounts[i].Item3 == 0) && queries[j] <= totalCharWeight)
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
