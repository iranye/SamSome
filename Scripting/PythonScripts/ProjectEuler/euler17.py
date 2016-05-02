import sys
import re
import time

def usage():
	print """\nEuler 17: How many letter are needed to write out numbers to 1000 (or n)?....
    Usage:python %s n:\n""" % sys.argv[0]

def main(n):
    """ Answer for 1000 = 21124
    """
    import time
    start = time.clock()
    ndict = {1:'one', 2:'two', 3:'three', 4:'four', 5:'five', 6:'six', 7:'seven', 8:'eight', 9:'nine', 10:'ten', 11:'eleven', 12:'twelve', 13:'thirteen', 14:'fourteen', 15:'fifteen', 16:'sixteen', 17:'seventeen', 18:'eighteen', 19:'nineteen', 20:'twenty', 20:'twenty', 30:'thirty', 40:'forty', 50:'fifty', 60:'sixty', 70:'seventy', 80:'eighty', 90:'ninety', 100:'one hundred'}

    ltr_count = 0
    if n == 1000:
        ltr_count += len('onethousand')
        n -= 1
    print n
    if n > 999:
        print "%s too high, try 100 or lower" % n
        sys.exit(0)
    while n > 0:
        numb = n
        nasword = ''
        if numb > 100:
            q, r = divmod(n, 100)
            nasword += ndict[q] + ' hundred '
            numb = r
            if numb > 0:
                nasword += 'and '
        if numb > 19:
            q, r = divmod(numb, 10)
            nasword += ndict[q * 10]
            if r > 0:
                nasword += ' ' + ndict.get(r)
        elif numb > 0:
            nasword += ndict[numb]
        # print nasword
        # print re.sub("\s+", "", nasword)
        ltr_count += len(re.sub("\s+", "", nasword))

        n -= 1
    print "%d (%.2g seconds)" % (ltr_count, time.clock() - start)

if __name__ == '__main__':
    """ 
    """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    main(int(sys.argv[1]))
 
    
