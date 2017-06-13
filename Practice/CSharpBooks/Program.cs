using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            CsharpInDepthThirdEd();
            Console.Read();
        }

        #region CsharpInDepthThirdEd

        static double TakeSqareRoot(int x)
        {
            return Math.Sqrt(x);
        }

        private static void Listing3_2()
        {
            Console.WriteLine("** Listing3_2 **");
            List<int> integers = new List<int>();
            integers.Add(1);
            integers.Add(2);
            integers.Add(3);
            integers.Add(4);
            Converter<int, double> converter = TakeSqareRoot;
            List<double> doubles = integers.ConvertAll<double>(converter);
            foreach (var d in doubles)
            {
                Console.WriteLine(d);
            }
        }

        public static T CreateInstance<T>() where T : new()
        {
            return new T();
        }

        public static void TypeConstraints()
        {
            Console.WriteLine("** TypeConstraints **");
            var newInt = CreateInstance<int>();
            Console.WriteLine(newInt.ToString());
            //var newStr = CreateInstance<string>(); // illegal
        }

        static int CompareToDefault<T>(T value) where T : IComparable<T>
        {
            return value.CompareTo(default(T));
        }

        private static void Listing3_4()
        {
            Console.WriteLine("** Listing3_4 **");
            Console.WriteLine("CompareToDefault('x'): " + CompareToDefault("x"));
            Console.WriteLine("CompareToDefault(10): " + CompareToDefault(10));
            Console.WriteLine("CompareToDefault(0): " + CompareToDefault(0));
            Console.WriteLine("CompareToDefault(-10): " + CompareToDefault(-10));
            Console.WriteLine("CompareToDefault(DateTime.MinValue): " + CompareToDefault(DateTime.MinValue));
        }

        static bool AreReferencesEqual<T>(T first, T second) where T: class
        {
            return first == second;
        }
        private static void Listing3_5()
        {
            Console.WriteLine("** Listing3_5 **");
            string name = "Ira";
            string intro1 = "My name is " + name;
            string intro2 = "My name is " + name;
            Console.WriteLine("intro1 == intro2: {0}", intro1 == intro2);
            Console.WriteLine("AreReferencesEqual(intro1, intro2): {0}", AreReferencesEqual(intro1, intro2));
        }

        private static void Listing3_6()
        {
            Console.WriteLine("** Listing3_6 **");
            Pair<int, string> pair = new Pair<int, string>(10, "value");
            Console.WriteLine("pair.Equals(new Pair<int, string>(11, 'value')): {0}", pair.Equals(new Pair<int, string>(11, "value")));
            Console.WriteLine("pair.Equals(new Pair<int, string>(10, 'value')): {0}", pair.Equals(new Pair<int, string>(10, "value")));

            Pair<int, string> newPair = Pair.Of(10, "value");  // use helper class
            Console.WriteLine("newPair = Pair.Of(10, 'value')");
            Console.WriteLine("pair.Equals(newPair)): {0}", pair.Equals(newPair));
        }

        private static void CsharpInDepthThirdEd()
        {
            Listing3_2();
            TypeConstraints();
            Listing3_4();
            Listing3_5();
            Listing3_6();
        } 
        #endregion
    }
}
