using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanString
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = " ?&^$#@SUPERSTAR!()+-,:;<>’\'-_*";
            if (args.Length > 0)
            {
                input = args[0];
            }
            Console.WriteLine(input);
            Console.WriteLine(UseStringBuilderWithHashSet(input));
        }

        // by Paolo Tedesco, but using a HashSet
        public static String UseStringBuilderWithHashSet(string strIn)
        {
            var hashSet = new HashSet<char>(" ?&^$#@!()+-,:;<>’\'-_*");
            // specify capacity of StringBuilder to avoid resizing
            StringBuilder sb = new StringBuilder(strIn.Length);
            foreach (char x in strIn.Where(c => !hashSet.Contains(c)))
            {
                sb.Append(x);
            }
            return sb.ToString();
        }

    }
}
