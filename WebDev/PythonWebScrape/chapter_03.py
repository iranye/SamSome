import re
from py_lib import get_bs_obj

def six_degrees_kevin_bacon():
    bsObj = get_bs_obj("https://en.wikipedia.org/wiki/Kevin_Bacon")
    if bsObj == None:
        return

    for link in bsObj.find("div", {"id":"bodyContent"}).findAll("a", href=re.compile("^(/wiki/)((?!:).)*$")):
        if 'href' in link.attrs:
            print(link.attrs['href'])