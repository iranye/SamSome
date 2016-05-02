import sys
import euler_base
import time
import os

def usage():
    print """Euler 22: What is the total of all the name scores in the file <filename?
    
For example, when the list is sorted into alphabetical order, COLIN,
which is worth 3 + 15 + 12 + 9 + 14 = 53,
is the 938th name in the list. So, COLIN would obtain a score of 938 * 53 = 49714.

Usage:python %s <filename>:\n""" % sys.argv[0]
def name_score(name):
    total = 0
    for c in name:
        if ord(c) == 34: # c is a double quote char
            continue
        char_val = ord(c)
        assert(char_val > 64 and char_val < 91)
        char_val -= 64
        total += char_val
    return totaldef main():    """ Answer = 4613732 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)
    fn = sys.argv[1]
    if os.path.exists(fn) is False:
        print "Error: Cannot find file: '%s'" % fn
        return
    start = time.time()

    arr = []    for line in file(fn):
        st = line
        arr.extend(st.split(','))
    arr.sort()
    total = 0
    for name in arr:
        # if name == '"COLIN"':
            # print name
            # print arr.index('"COLIN"')
        total += name_score(name) * (arr.index(name) + 1)

    print total
    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)if __name__ == '__main__':
    main()