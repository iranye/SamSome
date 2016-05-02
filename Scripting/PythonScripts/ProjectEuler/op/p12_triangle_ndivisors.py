#!/usr/bin/env python
"""Solution for project euler problem #12.
 
Based on:
http://stackoverflow.com/questions/571488/project-euler-12-in-python/571526#571526
"""
import sys
def ndiv(n, prime_factors):
    """Return number of divisors of `n`.
 
prime_factors: prime factors of `n`.
 
>>> from itertools import starmap
>>> list(starmap(ndiv, ((1, []), (12, [2, 3]))))
[1, 6]
"""
    assert n > 0
    phi = 1
    for prime in prime_factors:
        alpha = 0 # multiplicity of `prime` in `n`
        q, r = divmod(n, prime)
        while r == 0: # `prime` is a factor of `n`
            n = q
            alpha += 1
            q, r = divmod(n, prime)
        phi *= alpha + 1 # see http://en.wikipedia.org/wiki/Divisor_function
    return phi
 
def prime_factors_gen():
    """Yield prime factors for each natural number.
Based on
http://stackoverflow.com/questions/567222/simple-prime-generator-in-python/568618#568618

>>> from itertools import islice
>>> list(islice(prime_factors_gen(), 20)) #doctest:+NORMALIZE_WHITESPACE
[(1, []), (2, [2]), (3, [3]), (4, [2]), (5, [5]), (6, [3, 2]),
(7, [7]), (8, [2]), (9, [3]), (10, [5, 2]), (11, [11]), (12, [3, 2]),
(13, [13]), (14, [7, 2]), (15, [5, 3]), (16, [2]), (17, [17]),
(18, [3, 2]), (19, [19]), (20, [5, 2])]
"""
    D = {1:[]} # nonprime -> prime factors of `nonprime`
    #NOTE: dictionary could be replaced by priority queue
    q = 1
    while True: # Sieve of Eratosthenes algorithm
        if q not in D: # `q` is a prime number
            D[q + q] = [q]
            yield q, [q]
        else: # q is a composite
            for p in D[q]: # `p` is a factor of `q`
                # therefore `p` is a factor of `p + q` too
                D.setdefault(p + q, []).append(p)
                # print D
            yield q, D[q]
            del D[q]
        q += 1
 
def highly_composite_triangular(max_ndivisors):
    """

    """
    ndivs = {0: 0} # n -> number of divisors
    for n, pfs in prime_factors_gen():
        # save number of divisor of `n`
        ndivs[n] = ndiv(n, pfs)
        # decompose `(n-1)`th triangular number: `n * (n - 1) // 2`
        half, odd = (n//2, n-1) if n % 2 == 0 else ((n - 1)//2, n)
        # n and (n-1) do not have common factors, therefore
        # ndiv(n * (n - 1)) == ndiv(n) * ndiv(n-1)
        #NOTE: we already cached ndiv therefore there is no need to
        # to save ndivs[half], ndivs[odd] for further use
        if ndivs[half] * ndivs[odd] > max_ndivisors:
            print "OK: n: %d, (n-1)th tn: %d, ndivs[half] * ndivs[odd]: %d" % (n, half * odd, ndivs[half] * ndivs[odd])
            return half * odd # `(n-1)`th triangular number
            
        # print "n: %d, (n-1)th tn: %d, ndivs[half] * ndivs[odd]: %d" % (n, half * odd, ndivs[half] * ndivs[odd])
 
def main(n):
    import time
    start = time.clock()
    h = highly_composite_triangular(n)
    print "%d (%.2g seconds)" % (h, time.clock() - start)
 
if __name__ == '__main__':
    """ 
    10001st prime number is 104743
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    main(int(sys.argv[1]))
    


