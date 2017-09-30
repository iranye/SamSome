using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp_in_Depth_Third_Edition
{
    class Program
    {
        static void Main(string[] args)
        {
            CsharpInDepthThirdEd();
            Console.Read();
        }

        #region CsharpInDepthThirdEd

        private static void PrintList<T>(List<T> list)
        {
            foreach (var el in list)
            {
                Console.WriteLine(string.Format(el.ToString()));
            }
        }

        #region Chp1
        private static void Chp1_ListSorting()
        {
            Console.WriteLine("** Chp1_ListSorting (starts at Listing1_3) **");
            PrintList(Product.GetSampleProducts());
            List<Product> products = Product.GetSampleProducts();
            Console.WriteLine();

            // Sort via anonymous method
            products.Sort(delegate (Product x, Product y) { return x.Name.CompareTo(y.Name); });
            PrintList(products);
            Console.WriteLine();

            // Sort via lambda
            products = Product.GetSampleProducts();
            products.Sort((x, y) => x.Name.CompareTo(y.Name));
            PrintList(products);
            Console.WriteLine();

            // Sort w/o modifying original list via extension method
            PrintList(Product.GetSampleProducts().OrderBy(p => p.Name).ToList());
        }

        private static void Chp1_ListQueryForHighPricedProducts() // Show Product where Price > $10
        {
            Console.WriteLine("** Chp1_ListQueryForHighPricedProducts (starts at Listing1_11) **");

            // Query via anonymous method
            List<Product> products = Product.GetSampleProducts();
            Predicate<Product> test = delegate (Product p) { return p.Price > 10m; };
            List<Product> matches = products.FindAll(test);
            Action<Product> print = Console.WriteLine;
            matches.ForEach(print);
            Console.WriteLine();

            // Query via inline anonymous method
            products.FindAll(delegate (Product p) { return p.Price > 10; }).ForEach(Console.WriteLine);
            Console.WriteLine();

            // Query via inline lambda method
            foreach (Product product in products.Where(p => p.Price > 10))
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();

            // Query via LINQ query expression
            var filtered = from Product p in products
                           where p.Price > 10
                           select p;
            filtered.ToList().ForEach(print);
            Console.WriteLine();
        }
        #endregion

        delegate void StringProcessor(string input);

        static void PrintString(string x)
        {
            Console.WriteLine(x);
        }

        private static void DelegateStringProcessor()
        {
            StringProcessor proc1;
            proc1 = new StringProcessor(Program.PrintString);
            proc1("Foobar");
        }

        private static void Chp2_Delegates()
        {
            Console.WriteLine("** Chp2_Delegates **");
            DelegateStringProcessor();
        }

        #region Chp3
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

        static bool AreReferencesEqual<T>(T first, T second) where T : class
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
        #endregion

        private static void CsharpInDepthThirdEd()
        {
            // Chp1
            // Chp1_ListSorting();
            //Chp1_ListQueryForHighPricedProducts();

            // Chp2
            //Chp2_Delegates();

            // Chp15
            Chp15_AsynchronousForm();
            //            Chp15_AsyncTaskCancel();

        }

        /// Things to try:
        ///   leap-froggin await calls with several sets of data <employee, employee.Id>
        ///     Task<decimal> hourlyRateTask = employee.GetHourlyRateAsync();
        ///     decimal hourlyRate = await hourlyRateTask;
        ///     Task<int> hoursWorkedTask = timeSheet.GetHoursWorkedAsync(employee.Id);
        ///     int hoursWorked = await hoursWorkedTask;
        ///     AddPayment(hourlyRate * hoursWorked);
        /// This expansion reveals a potential inefficiency in the original statement—you could
        /// introduce parallelism into this code by starting both tasks(calling both Get...Async
        /// methods) before awaiting either of them.
        private static void Chp15_AsynchronousForm()
        {
            Application.Run(new AsyncForm());
        }

        static async Task DelayFor30Seconds(CancellationToken token)
        {
            Console.WriteLine("Waiting 30 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(30), token);
        }

        private static void Chp15_AsyncTaskCancel()
        {
            var source = new CancellationTokenSource();
            var task = DelayFor30Seconds(source.Token);
            source.CancelAfter(TimeSpan.FromSeconds(1));
            Console.WriteLine("Initial status: {0}", task.Status);
            try
            {
                task.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Caught {0}", e.InnerExceptions[0]);
            }
            Console.WriteLine("Final status: {0}", task.Status);
        }

        #endregion
    }

}
