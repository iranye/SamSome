import sys
import time
from datetime import datetime
from datetime import date
import re

def usage():
	print """\nEuler 19: How many Sundays fell on the first of the month during the twentieth century (or 1901 to <date>)?....
    >>> d = date.fromordinal(693961)
    >>> d.ctime()
    'Tue Jan  1 00:00:00 1901'

    >>> d = date.fromordinal(730485)
    >>> d.ctime()
    'Sun Dec 31 00:00:00 2000'

    Usage:python %s <datebegin> <dateend>:\n""" % sys.argv[0]

def main(start_ord, end_ord):
    print "start_ord: %d, end_ord: %d" % (start_ord, end_ord)
    first_sunday_ord = 0
    datearr = []
    for day_ord in range(start_ord, start_ord + 7):
        # find the first Sunday within the range
        dt = date.fromordinal(day_ord)
        if 'Sun' in dt.ctime():
            first_sunday_ord = day_ord
            # print dt.ctime()
            break

    st = date.fromordinal(first_sunday_ord)
    print st.ctime()
    # return    pat = re.compile(r'Sun [\w]{3}  1 .*')
    # st = 'Sun Jan  1 00:00:00 1901'
    # if pat.match(st):
        # print "OK"
    sundays_counted = 0
    for day_ord in range(first_sunday_ord, end_ord, 7):
        # count the Sundays that are on the first
        # print "day_ord: %d, first_sunday_ord: %d, ctime: %s" % (day_ord. first_sunday_ord, dt.ctime())
        dt = date.fromordinal(day_ord)
        if pat.match(dt.ctime()):
            print dt.ctime()
            sundays_counted += 1
            # print first_sunday_ord = day_ord

    print "sundays_counted: %d" % sundays_counted

    return
if __name__ == '__main__':
    """ 
    """
    if len(sys.argv) < 3:
        usage()
        sys.exit(0)

    main(int(sys.argv[1]), int(sys.argv[2]))
 
    