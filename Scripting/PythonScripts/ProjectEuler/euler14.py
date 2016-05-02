import sys
import time

def usage():
	print """Euler 14: Which starting number, under one million (or n), produces the longest chain?....
    Usage:python %s n:\n""" % sys.argv[0]

def get_next_in_sequence(n):
    if n % 2 == 0:
        return n / 2
    return 3*n + 1

def main(n):
    import time
    start = time.clock()
    dict = {'max_len' : 0}
    arr = []

    while n > 1: # iterate down from input number        arr = []
        next = n
        arr.append(next)
        while next > 1:
            next = get_next_in_sequence(next)
            arr.append(next)
        arrlen = len(arr)
        if dict['max_len'] < arrlen:
            dict['n'] = n
            dict['max_len'] = arrlen
            dict['arr'] = arr
        n -= 1
    # print dict

    print "n: %d, chain length: %d (%.2g seconds)" % (dict['n'], dict['max_len'], time.clock() - start)
if __name__ == '__main__':
    """ 
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    main(int(sys.argv[1]))
 
    