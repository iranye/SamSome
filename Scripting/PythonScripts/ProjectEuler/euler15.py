import sys
import math
import time

def usage():
    print """\nEuler15: 
    Count number or unique path in 20x20 box (or nxn)?....\nUsage:python %s n\n""" % sys.argv[0]
    
class Point():
    """ The pair (0, 0) is upper left  """
    def __init__(self, x, y):
        self.x = x
        self.y = y
    def to_string(self):
        return "[%d,%d]" % (self.x, self.y)class Edge():
    def __init__(self, a, b):
        """ Egde with enpoints a and b """
        self.a = a
        self.b = b
        self.trvld = False
    def to_string(self):
        return "'%s - %s'" % (self.a.to_string(), self.b.to_string())
def get_edges_arr(edges_arr, sq_size):
    """ populate supplied array with Edge object that form n x n square """
    for y in range(0, sq_size):
        """ work left-to-right: want: [0,1], [1,2], [2,3], .. [n-1, n]
        want: ([0,0], [0,1]), ([0,1],[0,2]), ([0,1],[0,1]), .. [n-1, n]
        
        ===> want: ([0,0], [1,0]), ([1,0],[2,0])        array dereferences: (aa[0][0], aa[0][1]), (aa[0][1], aa[0][2])
        1 1 1 1    <--- x axis --->
        1 1 1 1
        1 1 1 1 
        1 1 1 1
        """
        x = 0
        while x < sq_size - 1:
            p1 = Point(x, y)
            p2 = Point(p1.x + 1, p1.y)
            edges_arr.append(Edge(p1, p2))
            x += 1

    at_row = 0            
    while at_row < sq_size - 1:
        for x in range(1, sq_size): # start at 1 since there's symetry (only need to determine half the possible paths)
            """ work row n to row n+1
            want: ([1,0], [1,1]),  ([2,0], [2,1])

            """
            y = at_row
            p1 = Point(x, y)
            p2 = Point(x, y+1)
            edges_arr.append(Edge(p1, p2))
            # print "p1: %s, p2: %s" % (p1.to_string(), p2.to_string())
        at_row += 1
def get_path(paths_arr, edges_arr, n):
    """ 1 for right, 0 for down """
    paths_arr.append(42)
def mainBAK():
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)
    start = time.time()

    n = int(sys.argv[1])
    edges_arr = []
    get_edges_arr(edges_arr, n)

    if len(edges_arr) < 1:
        print "Error: no data saved"
        return
    
    print edges_arr[0].to_string()
    paths_arr = []
    get_path(paths_arr, edges_arr, n)
    end = time.time() - start
    print "found solution in %s seconds" % str(end)
def arr_to_str(s):
    return '0b' + s
def ones_count(s, max_n):
    total = 0
    for c in s:
        if total > max_n:
            return total
        if c is '1':
            total += 1
    return total
def main():
    """
    C:\Star\GNU_Compile\Euler>python euler15.py 20
    min_int: 1048575, min_bs: 0b11111111111111111111
    max_int: 1099510579200
    max_bs: 0b1111111111111111111100000000000000000000
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)
    start = time.time()

    n = int(sys.argv[1])
    moves_possible = 2 * n
    good_paths = []
    
    """ want to simply count the number of numbers within the specified range whose
    binary value contains the correct number of 1's (since we're working with nxn squares, look for n 1's)
    """
    arr = [1 for x in range(1,n+1)]  # string of 1's of length moves_possible
    min_bs = "".join("%s" % x for x in arr) # binary string, e.g., '0b1100100'
    min_bs = arr_to_str(min_bs) #
    min_int = int(min_bs, 2)

    print "min_int: %d, min_bs: %s" % (min_int, min_bs)
    max_int = min_int << n
    print "max_int: %d\nmax_bs: %s" % (max_int, bin(max_int))

    i, total = min_int, 0

    while i <= max_int:        # print "%d => %s" % (i, bin(i))
        bs = bin(i)
        # if ones_count(bs, n) == n:
        if bs.count('1') == n:
            # print bs
            # good_paths.append(bs)
            total += 1            
        i += 1
    print total
    # print len(good_paths)
    # print "\n".join(good_paths)
    
    end = time.time() - start
    print "found solution in %s seconds" % str(end)

    
if __name__ == '__main__':
    """ start program """

    main()

