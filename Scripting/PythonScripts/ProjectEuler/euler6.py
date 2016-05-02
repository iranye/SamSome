import sys
import time

def usage():
	print """
    Euler 6:
    The sum of the squares of the first ten natural numbers is,
    1^(2) + 2^(2) + ... + 10^(2) = 385

    The square of the sum of the first ten natural numbers is,
    (1 + 2 + ... + 10)^(2) = 55^(2) = 3025

    Hence the difference between the sum of the squares of the first 10 natural numbers and the square of the sum is 3025 - 385 = 2640.

    Find the difference between the sum of the squares of the first 100 (or n) natural numbers and the square of the sum.

    Usage:python %s n
    """ % sys.argv[0]
    
if __name__ == '__main__':
    """ Answer = 25164150 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()

    n = int(sys.argv[1]) + 1
    print abs((sum([x for x in range (1, n)]))**2 - sum([x**2 for x in range (1, n)]))

    end = time.time() - start
    print "found solution in %s seconds" % str(end)
    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))
    