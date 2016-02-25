import re
from py_lib import get_bs_obj

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
    
    # use a regex to find images
    images = bsObj.findAll("img", {"src":re.compile("\.\.\/img\/gifts/img.*\.jpg")})
    for image in images:
        print(image["src"])

def get_image():
    try:
        response = urlopen("http://www.pythonscraping.com/img/gifts/img1.jpg")
        content = response.read()
        pic = open("img1.jpg", 'wb')
        pic.write(content)
        pic.close()
    except URLError as e:
        print(e)
     
def read_using_lambda():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/page3.html")
    #bsObj = get_bs_obj("file:///C:/Users/ira.nye/source/WebDev/PythonWebScrape/page3.html") # this causes encoding exception
    if bsObj == None:
        return
    
    # use a regex to find images
    twoAttribTags = bsObj.html.body.findAll(lambda tag: len(tag.attrs) == 2)
    for tag in twoAttribTags:
        print(tag.get_text())
        
def read_using_parent():
    bsObj = get_bs_obj("http://www.pythonscraping.com/pages/page3.html")
    if bsObj == None:
        return
    
    # get the price of the image starting from the img
    #print(bsObj.find("img",{"src":"../img/gifts/img1.jpg"}).parent.previous_sibling.get_text()) # SHORT VERSION
    price_el = bsObj.find("img",{"src":"../img/gifts/img1.jpg"}).parent.previous_sibling
    print(price_el.get_text())

if __name__ == "__main__":
    read_html_h1()