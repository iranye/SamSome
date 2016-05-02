import sys
import time
import euler_base

def usage():
	print """\nEuler 7: 
    What is the 10001st prime number? (or nth)?....\nUsage:python %s n\n""" % sys.argv[0]

if __name__ == '__main__':
    """ 
    10001st prime number is 104743
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()

    n = int(sys.argv[1])
    primes = []
    print euler_base.get_nth_prime(primes, n)
    end = time.time() - start
    print "found solution in %s seconds" % str(end)

    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))
    