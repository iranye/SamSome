using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            FibStuff();
            ListStuff();
        }

        private static void ListStuff()
        {
            Console.WriteLine("ListStuff()");
            var list = GetList();
            var squares = list.map(n => n * n);
            int i = 0;
            foreach (var el in squares)
            {
                Console.WriteLine(el);
                if (i++ > 20)
                    break;
            }
        }
        #region Fib

        private static void FibStuff()
        {
            Console.WriteLine("FibStuff()");
            var lst = GetList();
            int i = 0;
            foreach (var el in lst)
            {
                if (i++ > 10)
                    break;
                Console.WriteLine("el: {0}, Fib(el): {1}", el, Fib(el));
            }
        }

        static int Fib(int n)
        {
            if (n == 0) return 0;
            if (n < 2) return 1;
            int f0 = 0;
            int f1 = 1;
            for (int i = 0; i < n; i++)
            {
                int temp = f0;
                f0 = f1;
                f1 = temp + f1;
            }
            return f0;
        } 
        #endregion

        public static IEnumerable<int> GetList()
        {
            int i = 0;
            while (true)
                yield return i++;
        }
    }

    public static class Functions
    {
        public static IEnumerable<R> map<T, R>(this IEnumerable<T> lst, Func<T, R> mapping)
        {
            foreach (var el in lst)
            {
                yield return mapping(el);
            }
        }
    }
}
