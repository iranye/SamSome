
def bubble(badList):
    badList[1] = 42

    length = len(badList) - 1
    sorted = False
    
    # return
    while not sorted:
        sorted = True
        for element in range(0, length):
            if badList[element] > badList[element + 1]:
                sorted = False
                hold = badList[element + 1]
                badList[element + 1] = badList[element]
                badList[element] = hold
#     return badList

def bubbleBAK(badList):
    badList[1] = 42

    length = len(badList) - 1
    unsorted = True
    
    # return
    while unsorted:
        for element in range(0, length):
            unsorted = False
            if badList[element] > badList[element + 1]:
                hold = badList[element + 1]
                badList[element + 1] = badList[element]
                badList[element] = hold
            else:
                unsorted = True
#     return badList

def main():
    mylist = [12, 5, 13, 8, 9, 65]
    bubble(mylist)
    print mylist

main()

# [12, 42, 13, 8, 9, 65]

