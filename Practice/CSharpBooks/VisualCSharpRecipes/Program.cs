using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VisualCSharpRecipes
{
    class Program
    {
        public static EventHandler MyEventHandler;
        static void Main(string[] args)
        {
            //EventHandlerStuff();
            BigIntegerStuff();

            Console.WriteLine("\nMain method done");
            Console.ReadLine();
        }

        private static void BigIntegerStuff()
        {
            BigInteger myBigInt = BigInteger.Multiply(Int64.MaxValue, 2);
            Console.WriteLine($"{Int64.MaxValue} * 2 = {myBigInt}");
        }

        private static void EventHandlerStuff()
        {
            MyEventHandler += new EventHandler(EventHandlerMethod);

            MyEventHandler += new EventHandler(delegate (object sender, EventArgs eventArgs)
            {
                Console.WriteLine("Anonymous method called");
            });

            MyEventHandler += new EventHandler((sender, eventArgs) =>
            {
                Console.WriteLine("Lambda expression called");
            });

            Console.WriteLine("Raising the event");
            MyEventHandler.Invoke(new object(), new EventArgs());
        }

        private static void EventHandlerMethod(object sender, EventArgs e)
        {
            Console.WriteLine("Named method called");
        }
    }
}
