import sys

def usage():
	print """\nEuler 1: Add all the natural numbers below one thousand (or n) that are multiples of 3 or 5....\nUsage:python %s n:\n""" % sys.argv[0]
if __name__ == '__main__':
    """  """
    if len(sys.argv) < 2:
        usage()
        sys.exit(0)

    n = int(sys.argv[1])
    print sum(filter(lambda x: x % 3 == 0 or x % 5 == 0, range(1, n)))