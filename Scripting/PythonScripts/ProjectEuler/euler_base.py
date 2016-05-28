import sys
import math
import time

def usage():
	print """\nEuler Base: 
    What is the 10001st prime number? (or nth)?....\nUsage:python %s n\n""" % sys.argv[0]

def factorial(n):
    """Return the factorial of n, an exact integer >= 0.

    If the result is small enough to fit in an int, return an int.
    Else return a long.

    >>> [factorial(n) for n in range(6)]
    [1, 1, 2, 6, 24, 120]
    >>> [factorial(long(n)) for n in range(6)]
    [1, 1, 2, 6, 24, 120]
    >>> factorial(30)
    265252859812191058636308480000000L
    >>> factorial(30L)
    265252859812191058636308480000000L
    >>> factorial(-1)
    Traceback (most recent call last):
        ...
    ValueError: n must be >= 0

    Factorials of floats are OK, but the float must be an exact integer:
    >>> factorial(30.1)
    Traceback (most recent call last):
        ...
    ValueError: n must be exact integer
    >>> factorial(30.0)
    265252859812191058636308480000000L

    It must also not be ridiculously large:
    >>> factorial(1e100)
    Traceback (most recent call last):
        ...
    OverflowError: n too large
    """

    if not n >= 0:
        raise ValueError("n must be >= 0")
    if math.floor(n) != n:
        raise ValueError("n must be exact integer")
    if n+1 == n:  # catch a value like 1e300
        raise OverflowError("n too large")
    result = 1
    factor = 2
    while factor <= n:
        result *= factor
        # print result
        factor += 1
    return result
    
# Fibonacci numbers ####################################################################################

def fibo_gen():
    """ generate the Fibonacci numbers

    """
    fibo_0 = 0
    fibo_1 = 1
    i = 0
    while True:
        if i == 0:
            fibo_next = fibo_0
        elif i == 1:
            fibo_next = fibo_1
        else:
            fibo_next = fibo_0 + fibo_1
            fibo_0, fibo_1 = fibo_1, fibo_next
            
        # print fibo_next
        yield i, fibo_next
        i += 1
def fibos_to_nth_index(arr, n):
    """ set arr to an n-length array of the Fibonacci numbers """
    l = len(arr)
    if l < 3:

        arr.append(0)
        arr.append(1)
        arr.append(1)

    while (l < n):
        arr.append(sum([arr[-1], arr[-2]]))
        l = len(arr)


def fibo_recursive(arr, n):
    l = len(arr)
    if l < 3:
        arr.append(0)
        arr.append(1)
        arr.append(1)

    if l >= n:
        return
    
    arr.append(sum([arr[l-1], arr[l-2]]))

    fibo(arr, n)

# Primes and factorization ####################################################################################
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
 
def is_prime(arr, n):
    """ checks n against prime numbers stored in arr (pre-populated) """
    for prime in arr:
        if n % prime == 0: return False

    return True    
def get_primes_to_sqrt_n(n):
    """ get prime numbers that are <= sqrt(n) (these are the ONLY possible divisors) """

    max_prime = int(math.sqrt(n))
    pp = 3 # possible prime

    arr = [2, 3]

    while pp < max_prime:
        pp += 2
        if is_prime(arr, pp): arr.append(pp)
    
    return arr

def get_nth_prime(arr, n):
    """ append prime numbers to arr as they're found 
    10001th prime = 104743
    """
    
    pp = 3 # possible prime
    if n is 1: return 2

    if len(arr) < 2:
        arr.append(2)
        arr.append(3)
    else:
        pp = arr[-1]

    i = len(arr)
    while i < n:
        pp += 2
        if is_prime(arr, pp):
            arr.append(pp)
            i += 1
    return arr[-1]

def get_primes_upto_n(arr, n):
    """ append prime numbers to arr that are <= n (these are the ONLY possible divisors) """

    if len(arr) < 2:
        arr.append(2)
        arr.append(3)

    pp = 5 # possible prime

    while pp < n:
        if is_prime(arr, pp): arr.append(pp)
        pp += 2

def sum_primes_upto_n(arr, n):
    """ sum prime numbers as they're found and return sum
    """
    
    if len(arr) < 2:
        arr.append(2)
        arr.append(3)

    sum_of_primes = sum(arr)
    pp = 5 # possible prime

    while pp < n:
        if is_prime(arr, pp):
            arr.append(pp)
            sum_of_primes += pp
        pp += 2

    return sum_of_primes

def get_prime_factors(primes, factors, n):
    """ determine the prime factor(s) that make up n (e.g., the prime factors of 13195 are 5, 7, 13 and 29 """

    if n == 1: return
    f = n
    for p in primes:
        if f % p == 0:
            factors.append(p)
            get_prime_factors(primes, factors, f/p)
            return
            # f = f/p
        elif p == primes[-1] and f == n: # none of the possible primes divide n, so n must be prime
            factors.append(n)
            return

def get_divisors(prime_factors, n, include_n=True):
    """ return a set of numbers that are the divisors of n
    To get only proper divisors (divisors that are less than n), pass in include_n=False
    """
    
    divisors = set(prime_factors)

    if include_n:
        divisors.add(n)
    temp_set = set('')

    while True:
        # add divisors first pass (e.g., 220 => [1, 2, 5, 11, 44, 110, 20]
        temp_set.clear()
        for m in divisors:
            q, r = divmod(n, m)
            if r == 0 and q not in divisors:
                temp_set.add(q)
        if len(temp_set) is 0: break
        divisors = divisors.union(temp_set)
    divisors = divisors.union(set([1]))  

    while True:
        # add divisors second pass (e.g., 220 => [1, 2, 4, 5, 10, 11, 20, 22, 44, 55, 110]
        temp_set.clear()
        for m in divisors:
            for o in divisors:
                pd = m*o
                q, r = divmod(n, pd)
                if r == 0:
                    if q not in divisors and q != n: temp_set.add(q)
                    if pd not in divisors and pd != n: temp_set.add(pd)
        if len(temp_set) is 0: break
        divisors = divisors.union(temp_set)
    if n in divisors:
        divisors.remove(n)
    divisors = divisors.union(set([1]))  

    return list(divisors)

def get_divisorsNEW(n, include_n=True):
    """ return a set of numbers that are the divisors of n
    To get only proper divisors, pass in include_n=False
    """
    divisors = [1]
    if include_n:
        divisors.add(n)
    temp_set = set('')

    # while True:
        # temp_set.clear()
        # for m in divisors:
            # pd = n / m  # possible divisors
            # if pd not in divisors:
                # temp_set.add(pd)
        # if len(temp_set) is 0: break
        # divisors = divisors.union(temp_set)

    while True:
        temp_set.clear()
        for m in divisors:
            q, r = divmod(n, m)
            if r == 0 and q not in divisors:
                temp_set.add(q)
        if len(temp_set) is 0: break
        divisors = divisors.union(temp_set)
    return divisors

def main():
    """
    C:\Star\GNU_Compile\Euler>python euler21.py 10000
    220 and 284 are amicable pairs
    284 and 220 are amicable pairs
    1184 and 1210 are amicable pairs
    1210 and 1184 are amicable pairs
    2620 and 2924 are amicable pairs
    2924 and 2620 are amicable pairs
    5020 and 5564 are amicable pairs
    5564 and 5020 are amicable pairs
    6232 and 6368 are amicable pairs
    6368 and 6232 are amicable pairs
    sum: 31626, amic_pairs: set([1184, 6368, 220, 5020, 2924, 6232, 1210, 5564, 284, 2620])
    found solution(s) in 2.34399986267 seconds
    """    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    n = int(sys.argv[1])
    start = time.time()

    print "Fibonacci sequence up to %d is %s" % (n, fibo_by_valuelimit(n))
    print "%d! is %d" % (n, factorial(n))
    primes = []
    print "%dth prime is %d" % (n, get_nth_prime(primes, n))

    end = time.time() - start
    print "found solution(s) in %s seconds" % str(end)
        
if __name__ == '__main__':
    """ 
    this script intended to be imported into other scripts
    """
    # main()
    # fibo_gen()
    for i, fibo in fibo_gen():
        print "the %dth Fibonacci number is: %d" % (i, fibo)
        if fibo > 33:
            break

    import doctest
    # doctest.testmod()
