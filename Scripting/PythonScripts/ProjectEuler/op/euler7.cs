class Program
    {
        static void Main(string[] args)
        {
            List<int> primes = new List<int>();
            primes.Add(2);
 
            int n = 3;
            while (primes.Count < 10001)
            {
                if (IsPrime(primes, n))
                {
                    primes.Add(n);
                }
                n+=2;
            }
 
            Console.WriteLine(primes[10000]);
            Console.ReadLine();
        }
 
        static bool IsPrime(List<int> primes, int n)
        {
            foreach (int prime in primes)
            {
                if (n % prime == 0) return false;
            }
            return true;
        }
    }
