import sys
import time

def usage():
	print """
    Euler 5:
    2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.
    What is the smallest number that is evenly divisible by all of the numbers from 1 to 20 (n)?....Usage:python %s n
    """ % sys.argv[0]

def try_dividing(arr, numerand):
    for i in arr:
        # print "numerand: %d, divisor: %d" % (numerand, i)
        if numerand % i != 0:
            return False    return True
    
if __name__ == '__main__':
    """  Answer = 232792560 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()

    n = int(sys.argv[1])
    init = n
    max_divisor = 999999999
    
    while try_dividing(range(2, init + 1), n) is False and n < max_divisor:
        n += init
    print n
    end = time.time() - start
    print "found solution in %s seconds" % str(end)
    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))
    