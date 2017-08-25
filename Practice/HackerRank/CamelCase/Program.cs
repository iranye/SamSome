using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelCase
{
    public class Program
    {
        static void Main(string[] args)
        {
            //CamelCaseProblem();
            AmazonScoringProblem();
        }

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
