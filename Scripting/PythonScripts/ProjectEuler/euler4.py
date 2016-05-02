import sys
import time

def usage():
	print """
    Euler 4:
    A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 x 99.
    Find the largest palindrome made from the product of two 3-digit numbers (or n-digit)?....Usage:python %s n
    """ % sys.argv[0]

def is_palindrome(n):
    p = str(n)
    rev = p[::-1]
    return p == rev

class threeple:
    def __init__(self, f1, f2, product):
        self.f1, self.f2, self.product = (f1, f2, product)
        
    def get_product(self):
        return self.product

if __name__ == '__main__':
    """     """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()

    n = int(sys.argv[1])
    strarr = []
    while n > 0:
        strarr.append('9')
        n -= 1;
    s = "".join(strarr)
    max_init = int(s)
    min_init = (max_init + 1) / 10
    (n1, n2) = (max_init, max_init)
    max_found = 0
    
    palarr = []
    
    while n2 > min_init:
        if is_palindrome(n1 * n2):
            product = n1 * n2
            palarr.append(threeple(n1, n2, product))
            if max_found < product: max_found = product

        if n1 == min_init:
            n2 -= 1
            n1 = max_init
        else:
            n1 -= 1
     # want 2 => 99, 3 => 999, etc
    
    if len(palarr) is 0:
        print "No solutions found"
        
    else:
        for tup in palarr:
            if tup.get_product() == max_found:
                print "%d * %d = %d" % (tup.f1, tup.f2, tup.product)
                break
        # print max_found # max(palarr)
    end = time.time() - start
    print "found solution in %s seconds" % str(end)
    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))
    