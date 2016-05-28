def divisors_sum(x):
    s = 1
    for k in xrange(2, int(x**0.5) + 1):
        if x % k == 0:
            s += k
            if x / k != k:
                s += x/k
    return s
 
def is_abundant(x):
    return divisors_sum(x) > x
 
 
data = [0] + [is_abundant(x) for x in xrange(1, 28123+1)]
print sum(x for x in xrange(0, len(data)) if not any(data[k] and data[x-k] for k in xrange(1, x/2+1)))
