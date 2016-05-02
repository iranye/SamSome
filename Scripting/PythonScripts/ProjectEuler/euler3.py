import sys
import euler_base
import time

def usage():
	print """\nEuler 3: The prime factors of 13195 are 5, 7, 13 and 29.\nWhat is the largest prime factor of the number 600851475143 (or n)?....\nUsage:python %s n:\n""" % sys.argv[0]
def main():
    """ 
    600851475143 = 71* 839* 1471* 6857
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()

    n = int(sys.argv[1])
    primes = euler_base.get_primes_to_sqrt_n(n)
    prime_factors = []
    euler_base.get_prime_factors(primes, prime_factors, n)
    print "%d has prime factors: %s" % (n, prime_factors)
    end = time.time() - start
    print "found solution in %s seconds" % str(end)

    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))
if __name__ == '__main__':
    main()