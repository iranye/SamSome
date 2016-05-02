import sys
import time

def usage():
	print """
    Euler 6:
    The sum of the squares of the first ten natural numbers is,
    1^(2) + 2^(2) + ... + 10^(2) = 385

    The square of the sum of the first ten natural numbers is,
    (1 + 2 + ... + 10)^(2) = 55^(2) = 3025

    Hence the difference between the sum of the squares of the first 10 natural numbers and the square of the sum is 3025 - 385 = 2640.

    Find the difference between the sum of the squares of the first 100 (or n) natural numbers and the square of the sum.

    Usage:python %s n
    """ % sys.argv[0]
    
def get_product(string_of_digits):
    p = 1
    for n in string_of_digits:
        p *= int(n)
    return p
if __name__ == '__main__':
    """ 99879 => 40824 """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    start = time.time()
    
    str_len_const = 5
    lower_ind = 0
    upper_ind = str_len_const
    max_found = 0
    max_digits = ''
    n = '7316717653133062491922511967442657474235534919493496983520312774506326239578318016984801869478851843858615607891129494954595017379583319528532088055111254069874715852386305071569329096329522744304355766896648950445244523161731856403098711121722383113622298934233803081353362766142828064444866452387493035890729629049156044077239071381051585930796086670172427121883998797908792274921901699720888093776657273330010533678812202354218097512545405947522435258490771167055601360483958644670632441572215539753697817977846174064955149290862569321978468622482839722413756570560574902614079729686524145351004748216637048440319989000889524345065854122758866688116427171479924442928230863465674813919123162824586178664583591245665294765456828489128831426076900422421902267105562632111110937054421750694165896040807198403850962455444362981230987879927244284909188845801561660979191338754992005240636899125607176060588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450'
    # print n[lower_ind:upper_ind]

    while len(n[lower_ind:upper_ind]) == str_len_const:
        product = get_product(n[lower_ind:upper_ind])
        if max_found < product:
            max_found = product
            max_digits = n[lower_ind:upper_ind]
        lower_ind += 1
        upper_ind += 1

    print "%s => %d" % (max_digits, max_found)    
    end = time.time() - start
    print "found solution in %s seconds" % str(end)
    # print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))

    