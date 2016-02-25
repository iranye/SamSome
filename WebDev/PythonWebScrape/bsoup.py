from urllib.request import urlopen
from urllib.error import HTTPError
from urllib.error import URLError
from bs4 import BeautifulSoup
import re

def get_bs_obj(url):
    try:
        html = urlopen(url)
    except HTTPError as e:
        print(e)
        return None
    else:
        return BeautifulSoup(html.read(), "html.parser")

def read_html_h1():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/page1.html")
    if bsObj == None:
        return
    try:
        el = bsObj.h1
    except AttributeError as e:
        print("Tag was not found")
    else:
        if el == None:
            print("Tag was not found")
        else:
            print(el)

def read_html_span_groups():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/warandpeace.html")
    if bsObj == None:
        return
        
    # findAll(tag, attributes, recursive, text, limit, keywords)
    # find(tag, attributes, recursive, text, keywords)
    try:
        namesList = bsObj.findAll("span", {"class":"green"})
        princeOccurances = bsObj.findAll(text="the prince")
        princeOccuranceCount = len(princeOccurances)
    except AttributeError as e:
        print("Tag(s) not found")

    if (namesList == None):
        return
    for name in namesList:
        print(name.get_text())

    if (princeOccurances == None):
        return
    print("")
    print("princeOccuranceCount: ", princeOccuranceCount)
    
def read_products_page_children():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/page3.html")
    if bsObj == None:
        return
    
    # select all rows including the title row
    for child in bsObj.find("table",{"id":"giftList"}).children:
        print(child)
        
def read_products_page_siblings():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/page3.html")
    if bsObj == None:
        return
    
    # select all rows after the title row
    for sibling in bsObj.find("table",{"id":"giftList"}).tr.next_siblings:
        print(sibling)
        
def read_products_page_images():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/page3.html")
    if bsObj == None:
        return
    
    # select all rows after the title row
    images = bsObj.findAll("img", {"src":re.compile("\.\.\/img\/gifts/img.*\.jpg")})
    for image in images:
        print(image["src"])
        
if __name__ == "__main__":
    # read_html_h1()
    # read_html_span_groups()
    # read_products_page_children()
    # read_products_page_siblings()
    read_products_page_images()
    
    