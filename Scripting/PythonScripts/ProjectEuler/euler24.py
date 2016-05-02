import sys
import euler_base
import time
from itertools import permutations

def usage():
    print """Euler 24: A permutation is an ordered arrangement of objects. 
For example, 3124 is one possible permutation of the digits 1, 2, 3 and 4. 
If all of the permutations are listed numerically or alphabetically, we call it lexicographic order.
The lexicographic permutations of 0, 1 and 2 are:

012   021   102   120   201   210

What is the millionth lexicographic permutation of the digits:
0, 1, 2, 3, 4, 5, 6, 7, 8 and 9? (or 0, 1, 2, ..., n)
    
Usage:python %s n:\n""" % sys.argv[0]
def arr_to_str(arr):
    st = ""    for i in arr:
        st += "%s" % i
    return stdef main():    """ Answer = 2783915460
        
    C:\Star\GNU_Compile\Euler>python euler24.py 9
    2783915460
    found solution(s) in 0.641000032425 seconds
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    n = int(sys.argv[1])
    start = time.time()
    arr = range(0, n+1)    st = arr_to_str(arr)
    i = 1
    for tup in permutations(st, len(st)):
        if i == 1000000:
            print arr_to_str(tup)
        i += 1
# >>> for n in itertools.permutations('012', 3):# ... 	print n
# ... 	
# ('0', '1', '2')
# ('0', '2', '1')
# ('1', '0', '2')
# ('1', '2', '0')
# ('2', '0', '1')
# ('2', '1', '0')
# >>> 
    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)if __name__ == '__main__':
    main()