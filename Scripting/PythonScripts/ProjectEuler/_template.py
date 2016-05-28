import sys
import euler_base
import time

def usage():
    print """Euler 2: Find the sum of all the even-valued terms in the Fibonacci sequence which do not exceed four million (or n).....
    
    Usage:python %s n:\n""" % sys.argv[0]
def main():    """ Answer = 4613732 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    n = int(sys.argv[1])
    start = time.time()
    arr = euler_base.fibo_by_valuelimit(n)
    print "%d-limit of Fibonacci sequence: %s" % (n, arr)

    print filter(lambda x: x % 2 == 0 , arr)
    print sum(filter(lambda x: x % 2 == 0 , arr))
    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)if __name__ == '__main__':
    main()