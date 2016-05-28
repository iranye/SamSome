import sys
import euler_base
import time

def usage():
	print """\nEuler 10: The prime factors of 13195 are 5, 7, 13 and 29.\nWhat is the largest prime factor of the number 600851475143 (or n)?....\nUsage:python %s n:\n""" % sys.argv[0]

if __name__ == '__main__':
    """ 
    the sum of all the primes below two million = 142913828922
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()

    n = int(sys.argv[1])
    primes = []
    print euler_base.sum_primes_upto_n(primes, n)
    end = time.time() - start
    print "found solution in %s seconds" % str(end)

    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))
    