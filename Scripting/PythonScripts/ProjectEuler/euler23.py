import sys
import euler_base
import time

def usage():
    print """Euler 23: Find the sum of all the even-valued terms in the Fibonacci sequence which do not exceed four million (or n).....
A perfect number is a number for which the sum of its proper divisors is exactly equal to the number.
For example, the sum of the proper divisors of 28 would be 1 + 2 + 4 + 7 + 14 = 28,
which means that 28 is a perfect number.

A number n is called deficient if the sum of its proper divisors is less than n and
it is called abundant if this sum exceeds n.

As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16,
the smallest number that can be written as the sum of two abundant numbers is 24.
By mathematical analysis, it can be shown that all integers greater than 28123
can be written as the sum of two abundant numbers. However, this upper limit cannot
be reduced any further by analysis even though it is known that the greatest number that
cannot be expressed as the sum of two abundant numbers is less than this limit.

Find the sum of all the positive integers which cannot be written as the sum of two abundant numbers.

    Usage:python %s n, where n is the upper limit:\n""" % sys.argv[0]

def get_sum_of_divisors(primes, n):
    prime_factors = []
    euler_base.get_prime_factors(primes, prime_factors, n)
    divisors = euler_base.get_divisors(prime_factors, n, include_n=False)
    divisors.sort()
    # print "n: %d, divisors: %s" % (n, divisors)
    return sum(divisors)
def find_non_sums_of_abundant_numbs(abundant_numbs, n):
    abnt_sums = set()
    for i in abundant_numbs:
        for j in abundant_numbs:
            if i + j > n:
                break
            abnt_sums.add(i + j)

    total = 0    for i in range(1, n+1):
        if i in abnt_sums:
            pass
        else:
            total += i
    # if abnt_sum in all_numbs:
        # print "i: %d, j: %d, abnt_sum: %d" % (i, j, abnt_sum)
        # all_numbs.remove(abnt_sum)
    return total
            
def main():
    """ Answer = 4179871, for n = 28123 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()
    n = int(sys.argv[1])
    abundant_numbs = []
    primes = euler_base.get_primes_to_sqrt_n(n)

    for i in range(1, n+1):
        sum_i = get_sum_of_divisors(primes, i) # e.g., 220 => 284
        if sum_i > i:
            abundant_numbs.append(i)

    print find_non_sums_of_abundant_numbs(abundant_numbs, n)

    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)if __name__ == '__main__':
    main()