import sys
import euler_base
import time

def usage():
    print """
    Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n).
    If d(a) = b and d(b) = a, where a != b, then a and b are an amicable pair and each of a and b are called amicable numbers.

    For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110;
    therefore d(220) = 284. The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220.

    Euler 21: Evaluate the sum of all the amicable numbers under 10000 (or n).....
    
    Usage:python %s n:\n""" % sys.argv[0]
def get_sum_of_divisors(primes, n):    prime_factors = []
    euler_base.get_prime_factors(primes, prime_factors, n)
    divisors = euler_base.get_divisors(prime_factors, n, include_n=False)
    divisors.sort()
    return sum(divisors)
def main():    """ Answer = 4613732 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()
    n = int(sys.argv[1])
    amic_pairs = set()    primes = euler_base.get_primes_to_sqrt_n(n)
    for i in range(1, n+1):
        sum_i = get_sum_of_divisors(primes, i) # e.g., 220 => 284
        if sum_i == i:
            continue
        pap = get_sum_of_divisors(primes, sum_i) # possible amicable pair to i
        if i == pap:
            print "%d and %d are amicable pairs" % (i, sum_i)
            amic_pairs.add(i)
            amic_pairs.add(sum_i)
    print "sum: %d, amic_pairs: %s" % (sum(amic_pairs), amic_pairs)
    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)if __name__ == '__main__':
    main()