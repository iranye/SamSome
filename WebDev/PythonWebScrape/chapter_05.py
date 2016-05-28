from py_lib import *
from urllib.request import urlretrieve
import csv

def get_web_scraping_logo():
    bsObj = get_bs_obj("http://www.pythonscraping.com")
    if bsObj == None:
        return

    img_fn = "logo"      
      
    imageLocation = bsObj.find("a",{"id":img_fn}).find("img")
    if (imageLocation == None):
        print("find failed in fn:get_web_scraping_logo")
        return

    imageLocation = imageLocation["src"]
    img_fn_ext = img_fn + ".jpg"
    urlretrieve (imageLocation, img_fn_ext)
    print("saved image: %s" % img_fn_ext)

def get_gatherer_image():
    bsObj = get_bs_obj("http://gatherer.wizards.com/Pages/Card/Details.aspx?multiverseid=5680")
    if bsObj == None:
        return

    img_fn = "5680"

    imageLocation = bsObj.find("div",{"class":"cardImage"}).find("img")
    if (imageLocation == None):
        print("failed to find image in fn:get_gatherer_image")
        return
    
    imageLocation = imageLocation["src"]
    baseUrl = "http://gatherer.wizards.com/Pages/"
    imageLocation = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=5680&type=card"
    print(imageLocation)

    img_fn_ext = img_fn + ".jpg"
    urlretrieve (imageLocation, img_fn_ext)
    print("saved image: %s" % img_fn_ext)

def create_csv():
    csvFile = open("test.csv","w+", newline="\n", encoding="utf-8")
    try:
        writer = csv.writer(csvFile)
        writer.writerow(('number', 'number plus 2', 'number times 2'))
        for i in range(10):
            writer.writerow( (i, i+2, i*2) )
    finally:
        csvFile.close()