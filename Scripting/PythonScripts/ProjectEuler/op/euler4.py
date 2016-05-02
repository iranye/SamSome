def palindrome(n):
 
	n = str(n)
 
	return n == n[::-1]
 
 
print max(a*b for b in xrange(a, 1000) for a in xrange(110, 1000, 11) if palindrome(a*b))
