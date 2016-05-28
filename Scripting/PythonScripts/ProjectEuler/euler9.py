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
    
def is_pyth_triplet(a, b, c):
    if a >= b or b >= c or a >= c:
        return False
    if a**2 + b**2 != c**2:
        return False
    return True
if __name__ == '__main__':
    """ 200 * 375 * 425 = 31875000 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()
    init_a = 1
    init_b = 2
    init_c = 997
    a, b, c = init_a, init_b, init_c
    while is_pyth_triplet(a, b, c) is False:
        if b >= c:
            a = init_a
            init_b += 1
            b = init_b
            c = (1000 - a - b)
        elif a >= b:
            a = init_a
            init_b += 1
            b = init_b
            c = (1000 - a) - b
        else:
            a += 1
            c = (1000 - a) - b
            if c in range(420, 430): print "a: %s, b: %s, c: %s" % (a, b, c)
            
    print "a: %s, b: %s, c: %s => %d" % (a, b, c, a * b * c)
    print is_pyth_triplet(3, 4, 5)
    end = time.time() - start
    print "found solution in %s seconds" % str(end)
    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))

    