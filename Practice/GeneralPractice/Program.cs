using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneralPractice
{
    public class Program
    {
        static void Main(string[] args)
        {
            //TypeVarianceStuff();
            //RefsOfCollectionsStuff();
            //DailyLinq();
            //LinqStuff();
            //MoreLinqStuff();
            FuzzyStrCompare();
        }

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
            
        public static void RefsOfCollectionsStuff()
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

        public static void DailyLinq()
        {
            int start = 0;
            const int limit = 12;

            // write and use infinite list method

            int[] numbers = new int[7] { 7, 13, 2, 33, 14, 55, 6 };
            Console.WriteLine(numbers.Any(n => n == 14));

            //Console.WriteLine(numbers.Min());


            // linq method query to: (e.g., get even numbers, add 10 to each)

            // define and use Map extension method

            // define and use query syntax (instead of method syntax)
        }

        public static void MoreLinqStuff()
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
                select new {X = b.X};
            foreach (var el in joinQuery)
            {
                Console.WriteLine(el);
            }
        }

        public static void LinqStuff()
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

        #region TypeVarianceStuff
        public static void TypeVarianceStuff()
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

    public static class Extensions
    {
    }
}
