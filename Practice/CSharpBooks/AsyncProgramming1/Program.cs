using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgramming1
{
    class Program
    {
        public class DataImporter
        {
            public void Import(string directory)
            {
                Console.WriteLine($"Importing {directory}");
            }
        }

        static void Main(string[] args)
        {
            #region Chapter 3
            //RunTask1();
            //RunTask2();
            //RunTask3();
            //GetThreadType();
            //RunTask4();
            //RunTaskWithInputs();
            //RunLoopWithClosure1();
            //RunLoopWithClosure2();
            //RunLoopWithClosure3();
            CrunchNumbersInParallel1();
            Console.ReadLine();

            #endregion
        }

        private static void CrunchNumbersInParallel1()
        {
            // ACTUAL:
            //ulong n = 49000;
            //ulong r = 600;

            ulong n = 58;
            ulong r = 8;

            // synchronous way
            //ulong part1 = Factorial(n);
            //ulong part2 = Factorial(n - r);
            //ulong part3 = Factorial(r);

            // asynchronous way
            Task<ulong> part1 = Task.Factory.StartNew<ulong>(() => Factorial(n));
            Task<ulong> part2 = Task.Factory.StartNew<ulong>(() => Factorial(n - r));
            Task<ulong> part3 = Task.Factory.StartNew<ulong>(() => Factorial(r));

            float chances = 0;
            try
            {
                chances = part1.Result / (part2.Result * part3.Result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR " + Environment.NewLine + ex.Message);
                return;
            }
            Console.WriteLine(chances);
        }

        private static void RunLoopWithClosure3()
        {
            foreach (var i in new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            {
                Task.Factory.StartNew(() => Console.WriteLine(i)); // prints out 0 to 9 but (probably) not in order
            }
        }

        private static void RunLoopWithClosure2()
        {
            for (int i = 0; i < 10; i++)
            {
                int toCapture = i;
                Task.Factory.StartNew(() => Console.WriteLine(toCapture)); // prints out 0 to 9 but (probably) not in order
            }
        }

        private static void RunLoopWithClosure1()
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() => Console.WriteLine(i)); // prints out 10 10s
            }
        }

        private static void RunTaskWithInputs()
        {
            var importer = new DataImporter();
            string importDirectory = @"C:\Data";
            Task.Factory.StartNew(() => importer.Import(importDirectory));
        }

        private static void RunTask4()
        {
            Task.Factory
                .StartNew(WhatTypeOfThreadAmI, TaskCreationOptions.LongRunning)
                .Wait();
        }

        private static void GetThreadType()
        {
            Task.Factory.StartNew(WhatTypeOfThreadAmI).Wait();
        }

        private static void WhatTypeOfThreadAmI()
        {
            Console.WriteLine("I'm a {0} thread", Thread.CurrentThread.IsThreadPoolThread ? "Thread Pool" : "Custom");
        }

        private static void RunTask3()
        {
            //Task t = Task.Run(Speak); // DOES NOT COMPILE
        }

        private static void RunTask2()
        {
            Task t = Task.Factory.StartNew(Speak);
        }

        private static void RunTask1()
        {
            Task t = new Task(Speak);
            t.Start();
            Console.WriteLine("Waiting for completion");
            t.Wait();
        }

        private static void Speak()
        {
            Console.WriteLine("Hello World");
        }

        static ulong Factorial(ulong n)
        {
            ulong fact = 1;
            for(ulong i = n; i > 1; i--)
            {
                fact *= i;
            }
            return fact;
        }
    }
}
