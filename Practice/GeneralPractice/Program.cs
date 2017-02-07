using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneralPractice
{
    public class MyClass
    {
        public void MagicMethod()
        {
            Console.WriteLine("MyClass");
        }
    }

    public class MyOtherClass : MyClass
    {
        public new void MagicMethod()
        {
            Console.WriteLine("MyOtherClass");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            // ExtensionMethod();
            // DailyLinq();
            // MoreLinq();
            NutshellLinq();
            // QueryVsLinqMethod();
            // SimpleLinq();
            // FuzzyStrCompare();
            // RefsOfCollections();
            // TypeVariance();
            Console.ReadLine();
        }

        private static void ExtensionMethod()
        {
            int myInt = 55;
            Console.WriteLine(55.Negate());
        }

        #region Fuzzy String Compare
        public static void FuzzyStrCompare()
        {
            //string original = "21-01_The Rolling Stones_Miss You.mp3";
            //string wantToExclude = "39_03_The Rolling Stones_Miss You.mp3";
            string original = "abdac";
            string wantToExclude = "abcd";

            const float threshold = .80F;
            bool fuzzyEqual = AreFuzzyEqual(original, wantToExclude, threshold);
            Console.WriteLine("{0} is{1}fuzzy equal to {2}", original, fuzzyEqual ? " " : " not ", wantToExclude);
        }

        public static bool AreFuzzyEqual(string original, string compareStr, float matchingThreshold)
        {
            original = MassageString(original);
            Console.WriteLine(original);
            compareStr = MassageString(compareStr);
            Console.WriteLine(compareStr);

            //if (original.Contains(compareStr) || compareStr.Contains(original))
            //{
            //    return true;
            //}

            char[] origArr = original.ToCharArray();

            IEnumerable<char> origQuery =
                from c in origArr
                orderby c descending
                select c;

            char[] compareStrArr = compareStr.ToCharArray();
            IEnumerable<char> compQuery =
                from c in compareStrArr
                orderby c descending
                select c;

            int origLen = origArr.Length;
            int compLen = compareStrArr.Length;

            Stack<char> matchesStack = new Stack<char>();
            Stack<char> origStack = new Stack<char>();
            Stack<char> compStack = new Stack<char>();

            foreach (char origChar in origQuery)
            {
                origStack.Push(origChar);
            }

            foreach (char compChar in compQuery)
            {
                compStack.Push(compChar);
            }

            // aabcd vs abcd
            while (origStack.Count > 0 && compStack.Count > 0)
            {
                char origPopped = origStack.Pop();
                char compPopped = compStack.Pop();
                while (origPopped < compPopped && origStack.Count > 0)
                {
                    origPopped = origStack.Pop();
                }
                while (compPopped < origPopped && compStack.Count > 0)
                {
                    compPopped = compStack.Pop();
                }
                if (origPopped == compPopped)
                {
                    matchesStack.Push(origPopped);
                }
            }

            float comparisonQuotient = (float)matchesStack.Count / origLen;
            Console.WriteLine("comparisonQuotient: {0}", comparisonQuotient);
            return !(comparisonQuotient < matchingThreshold);
        }

        public static string MassageString(string original)
        {
            string newString = original;
            newString = newString.Replace(" ", "");
            newString = newString.Replace("_", "");
            newString = newString.Replace("-", "");

            Regex pat = new Regex(@"[\d]*(.*)");
            Match match = pat.Match(newString);
            if (match.Success)
            {
                newString = match.Groups[1].Value;
            }
            return newString.ToLower();
        }

        #endregion
        
        #region Linq

        public static void DailyLinq()
        {
            List<int> numbers = Enumerable.Range(1, 200).ToList();
            List<int> oddNumbers = numbers.Where(n => n%2 == 1).ToList();
            List<int> odds = numbers.FindAll(n => n%2 == 1);
            bool test = numbers.TrueForAll(n => n < 50);

            oddNumbers.ForEach(n => Console.Write(n));
            //numbers.ForEach();
            return;
            int start = 0;
            const int limit = 12;

            // using infinite list method, print even numbers

            //int[] numbers = new int[7] { 7, 13, 2, 33, 14, 55, 6 };
            //Console.WriteLine(numbers.Any(n => n == 14));

            //Console.WriteLine(numbers.Min());

            // using infinite list method, print even numbers with 10 added to each

            // define and use Map extension method

            // define and use query syntax (instead of method syntax)
        }

        public static void MoreLinq()
        {
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            List<Foo> fooList = new List<Foo>();
            List<Bar> barList = new List<Bar>();
            foreach (var n in numbers)
            {
                fooList.Add(new Foo { X = n });
                if ((n % 2) == 0)
                {
                    barList.Add(new Bar { X = n });
                }
            }
            Console.WriteLine("Join Query");
            var joinQuery =
                from f in fooList
                join b in barList on f.X equals b.X
                select new { X = b.X };
            foreach (var el in joinQuery)
            {
                Console.WriteLine(el);
            }
        }
        
        private static void NutshellLinq()
        {   // http://www.albahari.com/nutshell/linqquiz.aspx
            string[] colors = {"green", "brown", "blue", "red"};

            // Output the max stringlength of the colors
            Console.WriteLine("colors.Max(c => c.Length): " + colors.Max(c => c.Length));

            // Output the color with the min stringlength
            var ambig = colors.OrderBy(c => c.Length).First();
            Console.WriteLine(ambig);

            // What type is queryA?
            var queryA = from c in colors where c.Length > 3 orderby c.Length select c;
            Console.WriteLine("A");
            foreach (var el in queryA)
            {
                Console.WriteLine(el);
            }

            // What's the output?
            var queryB = from c in colors where c.Length == colors.Max(c2 => c2.Length) select c;
            Console.WriteLine("B");
            foreach (var el in queryB)
            {
                Console.WriteLine(el);
            }

            // How can queryB be made more efficient?
            var maxLengh = colors.Max(c => c.Length);
            var queryC = from c in colors where c.Length == maxLengh select c;
            Console.WriteLine("C");
            foreach (var el in queryC)
            {
                Console.WriteLine(el);
            }

            // What is the output?
            var list = new List<string>(colors);
            var queryD = list.Where(c => c.Length == 3);
            Console.WriteLine("D");
            Console.WriteLine(queryD.Count());
            list.Remove("red");
            Console.WriteLine(queryD.Count());
        }

        public static void QueryVsLinqMethod()
        {
            // Data source
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            // Query definition w/ Query syntax
            IEnumerable<int> numQuery =
                from n in numbers
                where (n % 2) == 0
                orderby n descending
                select n;

            // Query definition w/ Method syntax
            IEnumerable<int> mQuery = numbers.Where(n => n % 2 == 0).OrderByDescending(n => n);

            // Query 1 execution
            Console.WriteLine("Query 1 execution");
            foreach (var el in numQuery)
            {
                Console.WriteLine(el);
            }
            // Query 2 execution
            Console.WriteLine("Query 2 execution");
            foreach (var el in mQuery)
            {
                Console.WriteLine(el);
            }
        }

        private static void SimpleLinq()
        {
            int max = 50;
            IEnumerable<int> lst = LongSeq().Where(i => i % 2 == 0);
            IEnumerable<int> squares = lst.Map<int, int>((int i) => { return i * i; });
            foreach (var el in squares)
            {
                if (el > max)
                    break;
                Console.WriteLine(el);
            }
        }

        public static IEnumerable<int> LongSeq()
        {   // infinite list method that starts at 0 and increments by 1
            int i = 0;
            while (true)
                yield return i++;
        }
        #endregion

        public static void RefsOfCollections()
        {
            List<Vector2> vectors = new List<Vector2>
            {
                new Vector2(0.0f, 0.008f),
                new Vector2(0.0f, 0.008f),
                new Vector2(0.0f, 0.006f)
            };

            // Skip() returns an IEnumerable<T> that doesn't support List methods like Clear() and Add()
            // Even if the user guesses the type, she still can't cast and run the methods
            // see: http://stackoverflow.com/questions/491375/readonlycollection-or-ienumerable-for-exposing-member-collections
            try
            {
                List<Vector2> lst = (List<Vector2>)vectors.Skip(0); // throws InvalidCastException at runtime
                lst.Clear();
            }
            catch (InvalidCastException e)
            {
                Console.Error.WriteLine(e.Message);
            }
            try
            {
                List<Vector2> lst2 = vectors.Skip(0) as List<Vector2>; // lst2 gets set to null
                lst2.Clear();
            }
            catch (NullReferenceException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        #region TypeVariance
        public static void TypeVariance()
        {
            List<Foo> foos = new List<Foo> { new Foo() { X = 1 }, new Foo() { X = 2 }, };
            List<Bar> bars = new List<Bar> { new Bar() { X = 17 }, new Bar() { X = 22 }, };

            PrintX(foos);
            PrintX(bars);
        }

        // http://blogs.msdn.com/b/csharpfaq/archive/2010/02/16/covariance-and-contravariance-faq.aspx
        public static void PrintX(IEnumerable<IXFieldPrintable> objsWithX)
        {
            foreach (var objWithX in objsWithX)
            {
                Console.Write("{0}, ", objWithX.GetX());
            }
            Console.WriteLine();
        }
    }

    public interface IXFieldPrintable
    {
        int GetX();
    }

    public class Foo : IXFieldPrintable
    {
        public int X { get; set; }
        public int GetX()
        {
            return X;
        }
    }

    public class Bar : IXFieldPrintable
    {
        public int X { get; set; }
        public int GetX()
        {
            return X;
        }
    }
    #endregion

    public static class ExtensionMethods
    {
        public static int Negate(this int value)
        {
            return -value;
        }

        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> lst, Func<T, R> mapping)
        {
            foreach (T element in lst)
                yield return mapping(element);
        }
    }
}
