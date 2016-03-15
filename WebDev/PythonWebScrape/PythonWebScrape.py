from py_lib import get_bs_obj
from chapter_01_02 import *
from chapter_03 import *
from chapter_04 import *
from chapter_05 import *

def gathering():
    bsObj = get_bs_obj("http://gatherer.wizards.com/Pages/Search/Default.aspx?action=advanced&format=+[\"Standard\"]")
    if bsObj == None:
        return
    
    try:
        namesList = bsObj.findAll("span", {"class":"cardTitle"})
    except AttributeError as e:
        print("Tag(s) not found")

    if namesList == None:
        return

    for name in namesList:
        print(name.get_text())

def include(filename):
    if os.path.exists(filename):
        sys.path.append(os.path.abspath(filename))
    else:
        print("Error: could not find %s" % filename)        

if __name__ == "__main__":
    # chapter_01_02.py:
    #read_html_h1()
    #read_html_span_groups()
    #read_products_page_children()
    #read_products_page_siblings()
    #read_products_page_images()
    #get_image()
    #read_using_lambda()
    #read_using_parent()
    usd_read_products_page()

    # chapter_03.py:
    #six_degrees_kevin_bacon()
    #gathering()

    # chapter_04.py:
    #parse_literal_Json()
    #parse_gmap_Json()
    
    # chapter_05.py
    #get_web_scraping_logo()
    #get_gatherer_image() #does not work (file is corrupted)
    #create_csv()