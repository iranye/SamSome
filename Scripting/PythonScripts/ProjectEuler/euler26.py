import sys
import euler_base
import time

def usage():
    print """Euler 26: Find the value of d < 1000 (or n) for which 1/d contains the longest recurring cycle in its decimal fraction part.
    
    Usage:python %s n:\n""" % sys.argv[0]

def substr_countBAK(dict, denom, st, substr, rec):
    """ input:
            string st to search in
            string substr to search for
            number rec for expected number of recurences
    '0869565217391304347826086956521739130434782608695'
    """
    subdict = {}
    dict[denom] = subdict
    for n in st.split(substr):
        count = st.count(n)
        if count > rec:
            key = substr + n
            if len(key) > 1:
                subdict[key] = count
        # print "n: %s, count: %d" % (n, fp2.count(n))
def get_numerator(numerator, denom):
    numerator *= 10
    zeros = ''
    while numerator < denom:
        numerator *= 10
        zeros += '0'
    return zeros, numerator
def unique_digit_count(st):
    nd = {}
    for c in st:
        nd[c] = 1
    return len(nd)
def get_max_substr(st):
    """ input:

    >>> st
    '0869565217391304347826086956521739130434782608695'
    >>> for i in range(len(st)):
    ... 	ts = st[:i]
    ... 	print "ts: %s, count: %d" % (ts, st.count(ts))
    ... 
    ts: , count: 50
    ts: 0, count: 5
    ts: 08, count: 3
    ts: 086, count: 3
    ts: 0869, count: 3
    ts: 08695, count: 3
    ts: 086956, count: 2
    ts: 0869565, count: 2
    ts: 08695652, count: 2
    ts: 086956521, count: 2
    ts: 0869565217, count: 2
    ts: 08695652173, count: 2
    ts: 086956521739, count: 2
    ts: 0869565217391, count: 2
    ts: 08695652173913, count: 2
    ts: 086956521739130, count: 2
    ts: 0869565217391304, count: 2
    ts: 08695652173913043, count: 2
    ts: 086956521739130434, count: 2
    ts: 0869565217391304347, count: 2
    ts: 08695652173913043478, count: 2
    ts: 086956521739130434782, count: 2
    ts: 0869565217391304347826, count: 2
    ts: 08695652173913043478260, count: 1
    """
    str_len = len(st)
    max_tup = ('', 0) # tuple storing (longest_string with count > 1)
    # dict_arr = []
    min_len = 7
    max_found = '' # longest_string with count > 1
    start_ind = 0
    while start_ind * 2 < str_len: # only go halfway through the string
        for i in range(str_len):
            ts = st[:i] # e.g., if st = '1234', st[:2] gives '12'
            occ_count = st.count(ts)
            if len(ts) < min_len or occ_count < 2 or ts != st[len(ts):len(ts) * 2]:
                # re-loop for small substrings or if substring not immediately followed by same string
                continue
            if len(max_tup[0]) < len(ts) and max_tup[1] < occ_count:
                max_tup = (ts, occ_count)
            st = st[1:start_ind]
        start_ind += 1

    return max_tup
        # print "n: %s, count: %d" % (n, fp2.count(n))

def main():    """ Answer =  """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    n = int(sys.argv[1])
    start = time.time()
    init = 10
    rc = 5 # recurrence count
    max = 2000 # max str len
    dict = {}
    
    max_str = ''
    max_dict = {}
    
    for i in range(2, n+1):
        fp = ''   # fractional part as string]
        zeros, nmrt = get_numerator(1, i)
        q, r = divmod(nmrt, i)
    
        while r > 0 and len(fp) < max:            
            ts = "%s" % q
            fp += ts
            zeros, nmrt = get_numerator(r, i)
            fp += zeros
            if len(fp) > 20:
                if fp[-11:-1].count(ts) == 10 or unique_digit_count(fp) < 4:
                    # break out if simply repeating digits (e.g., 1/6 => 0.1666) or (1/22 => 0.0454545454)
                    break
            q, r = divmod(nmrt, i)            if len(fp) == max:
                max_tup = get_max_substr(fp)
                if len(max_str) < len(max_tup[0]):
                    max_str = max_tup[0]
                    max_dict.clear()
                    max_dict[i] = (max_tup[0], max_tup[1], fp)
                # print "i: %d, fp: %s, max: %s, nmrt: %d" % (i, fp, max_str, nmrt)
                # print "i: %d, fp: %s, max: %s" % (i, fp, max_tup)    
    print max_dict
    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)if __name__ == '__main__':
    main()