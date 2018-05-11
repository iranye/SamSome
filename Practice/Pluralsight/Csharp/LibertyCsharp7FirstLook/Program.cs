using System;
using System.Collections.Generic;
using static System.Console;
using static System.Math;

namespace LibertyCsharp7FirstLook
{
    // https://app.pluralsight.com/player?course=csharp-7-first-look&author=jesse-liberty&name=csharp-7-first-look-m3&clip=3&mode=live
    class Program
    {
        private static Runner _runner = new Runner();
        static void Main(string[] args)
        {
            //MathStuff();
            //OutParameterStuff();
            //PatternsStuff();
            //PatternsAndSwitchStatmentStuff();
            CreateAndDeconstructTuplesStuff();
            //LocalFunctionsStuff();
            Csharp7EnhancementsStuff();
        }

        private static void Csharp7EnhancementsStuff()
        {
            int GetBigNumber()
            {
                return 1_234_567;
            }
            WriteLine(GetBigNumber());
            
            // return by reference and update the array at position 4
            int[] numbers = { 2, 7, 1, 9, 12, 8, 15 };
            ref int position = ref _runner.Substitute(12, numbers);
            position = -12;
            WriteLine(numbers[4]);

            BallPlayer joe = new BallPlayer("Forward");
            WriteLine(joe.Position);

            // Throw exception as an expression
            BallPlayer mary = new BallPlayer(null);
            WriteLine(mary.Position);
        }

        private static void LocalFunctionsStuff()
        {
            WriteLine(_runner.Fibonacci(6));
        }

        private static void CreateAndDeconstructTuplesStuff()
        {
            var time = _runner.GetTime();
            WriteLine($"Time: {time.Item1}:{time.Item2}:{time.Item3}");

            var time2 = _runner.GetTime2();
            WriteLine($"Time2: {time2.hour}:{time2.minutes}:{time2.seconds}");

            (int hour, int minutes, int seconds) = _runner.GetTime2(); // deconstruct the returned tuple
            WriteLine($"{hour}:{minutes}:{seconds}");

            var tupleDictionary = new Dictionary<(int, int), string>();
            tupleDictionary.Add((100, 20), "Your room is #20 on the 100th floor");
            tupleDictionary.Add((50, 10), "Your room is on the 50th floor, room 10");

            var result = tupleDictionary[(50, 10)];
            WriteLine(result);
        }

        private static void PatternsAndSwitchStatmentStuff()
        {
            Employee theEmployee = new VicePresident();
            theEmployee.Salary = 175000;
            theEmployee.Years = 7;
            (theEmployee as VicePresident).NumberManaged = 200;
            (theEmployee as VicePresident).StockShares = 6000;

            switch (theEmployee) // order matters (more specific types above less specific types)
            {
                case VicePresident vp when (vp.StockShares < 5000):
                    WriteLine($"Junior VP with {vp.StockShares} shares");
                    break;
                case VicePresident vp when (vp.StockShares >= 5000):
                    WriteLine($"Senior VP with {vp.StockShares} shares");
                    break;
                case Manager m:
                    WriteLine($"Manager with {m.NumberManaged} reporting");
                    break;
                case Employee e:
                    WriteLine($"Employee with salary: {e.Salary}");
                    break;
            }
        }

        private static void PatternsStuff()
        {
            _runner.PrintSum(7);
            _runner.PrintSum2("6");
        }

        private static void OutParameterStuff()
        {
            _runner.UseOutTheOldWay();
            _runner.UseOutTheNewWay();
        }

        private static void MathStuff()
        {
            WriteLine(Sqrt(3 * 3 + 4 * 4));
        }
    }

    public class Runner
    {
        #region new "out" functionality
        public void UseOutTheOldWay()
        {
            int hour;
            int minutes;
            int seconds;
            GetTime(out hour, out minutes, out seconds);
            WriteLine($"{hour}:{minutes}:{seconds}");
        }

        public void UseOutTheNewWay()
        {
            GetTime(out int hour, out int minutes, out int seconds);
            WriteLine($"{hour}:{minutes}:{seconds}");
        }

        public void GetTime(out int hour, out int minutes, out int seconds)
        {
            hour = 1;
            minutes = 30;
            seconds = 20;
        }
        #endregion

        #region Tuple Deconstruction
        public (int, int, int) GetTime()
        {
            return (1, 30, 40); // tuple literal
        }

        public (int hour, int minutes, int seconds) GetTime2()
        {
            return (1, 35, 40); // tuple literal
        }
        #endregion

        #region pattern matching [constant,type,var]
        public void PrintSum(object o)
        {
            if (o is null) return;
            if (!(o is int i)) return; // type pattern (int)

            int sum = 0;
            for (int j = 0; j <= i; j++)
            {
                sum += j;
            }

            WriteLine($"The sum of 1 to {i} is {sum}");
        }

        public void PrintSum2(object o)
        {
            if (o is int i || o is string s && int.TryParse(s, out i))
            {
                int sum = 0;
                for (int j = 0; j <= i; j++)
                {
                    sum += j;
                }

                WriteLine($"The sum of 1 to {i} is {sum}");
            }
        }
        #endregion

        public int Fibonacci(int x)
        {
            if (x < 0) throw new ArgumentException("Must be at least 0", nameof(x));

            return Fib(x).current;
            (int current, int previous) Fib(int i)
            {
                if (i == 0) return (1, 0);
                var (current, previous) = Fib(i - 1);
                return (current + previous, current);
            }
        }

        public ref int Substitute(int value, int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == value)
                {
                    return ref numbers[i];
                }
            }
            throw new IndexOutOfRangeException("Not found!");
        }
    }

    public class BallPlayer : Employee
    {
        public BallPlayer(string position) => Position = position ?? throw new ArgumentNullException();
    }

    public class Employee
    {
        public int Salary { get; set; }
        public int Years { get; set; }
        public string Position { get; set; }
    }

    public class Manager : Employee
    {
        public int NumberManaged { get; set; }
    }

    public class VicePresident : Manager
    {
        public int StockShares { get; set; }
    }
}
