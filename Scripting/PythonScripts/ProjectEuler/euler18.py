import sys
import os
import time

def usage():
	print """\nEuler 18: Max path down the Tree?....\nUsage:python %s <input_file>:\n""" % sys.argv[0]
def factor(primes, factors, n):
    """ determine the prime factor(s) that make up n (e.g., the prime factors of 13195 are 5, 7, 13 and 29 """

    if n == 1: return
    f = n
    for p in primes:
        if f % p == 0:
            factors.append(p)
            factor(primes, factors, f/p)
            return
            # f = f/p
        elif p == primes[-1] and f == n: # none of the possible primes divide n, so n must be prime
            factors.append(n)
            return
class Tree():
    def __init__(self, n):
        self.n = n
        self.left = None
        self.right = None
        self.current_max = n
    def node_count(self):
        left_count, right_count = 0, 0
        if self.left:
            left_count = self.left.node_count()
        if self.right:
            right_count = self.right.node_count()
        return 1 + left_count + right_count
            
    def add_nodes(self, m, n):
        # add children 2 at a time to lowest depth child tree
        if self.left is None:
            assert(self.right is None)
            if m > n:
                self.current_max += m
            else:
                self.current_max += n
            self.left = Tree(m)
            self.right = Tree(n)
        else:
            left_count = self.left.node_count()
            right_count = self.right.node_count()
            if left_count > right_count:
                self.right.add_nodes(m, n)
            else:
                self.left.add_nodes(m, n)
    def dump(self):
        print "n: %d, cm: %d" % (self.n, self.current_max)
        if self.left:
            print "Left---"
            self.left.dump()
            assert(self.right)
            print "Right---"
            self.right.dump()

    def adjust_max(self):
        print self.n
        if self.left:
            print "Left---"
            self.left.dump()
            assert(self.right)
            print "Right---"
            self.right.dump()

def populate_tree(input):
    """ Read input file and return useful data structure
    """
    fullpath = os.path.abspath(input)
    if os.path.exists(fullpath) is False:
        print "Error: cannot find: '%s'" % input
        return None

    root = None
    for numbs in file(fullpath):
        temparr = []
        if ' ' in numbs: # create array from each line after first line (e.g., 12 24 => [12, 34]
            while ' ' in numbs:
                ind = numbs.index(' ')
                n = int(numbs[0:ind])
                temparr.append(n)
                numbs = numbs[ind+1:]
            assert len(numbs) > 0
            n = int(numbs)
            temparr.append(n)
        else: # starting line with single number goes into root node
            n = int(numbs)
            root = Tree(n)
            continue
        # assert len(temparr) % 2 == 0 # expecting each line after first to have 2**n distinct numbers
        print temparr
        while len(temparr) > 0:
            m, n = temparr[:2]
            # print "m: %d, n: %d" % (m, n)
            root.add_nodes(m, n)
            temparr = temparr[2:]
    return rootdef read_input(input):
    """ Read input file and return useful data structure
    """
    fullpath = os.path.abspath(input)
    if os.path.exists(fullpath) is False:
        print "Error: cannot find: '%s'" % input
        return None

    arrarr = []
    for numbs in file(fullpath):
        temparr = []
        if ' ' in numbs: # create array from each line after first line (e.g., 12 24 => [12, 34]
            while ' ' in numbs:
                ind = numbs.index(' ')
                n = int(numbs[0:ind])
                temparr.append(n)
                numbs = numbs[ind+1:]
            assert len(numbs) > 0
            n = int(numbs)
            temparr.append(n)
        else: # starting line with single number goes into root node
            arrarr.append([int(numbs)])
            continue
        assert len(temparr) == len(arrarr[-1]) + 1 # expecting each line after first to have specific count
        arrarr.append(temparr)

    return arrarr
def btm_up(arrarr):
    for i in range(len(arrarr)-1, 0, -1):
        btm = arrarr[i]
        for j in range(0, len(btm)-1):
            parent = arrarr[i-1][j]
            arrarr[i-1][j] = max(parent + btm[j], parent + btm[j+1])
    print arrarr[0]def main(input):    data = read_input(input)
    if len(data) == 0:
        print "Error: nothing saved"
        return
    # print data
    btm_up(data)

if __name__ == '__main__':
    """ 
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    main(sys.argv[1])

