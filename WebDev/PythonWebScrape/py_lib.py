from urllib.request import urlopen
from urllib.error import HTTPError
from urllib.error import URLError
from bs4 import BeautifulSoup
import json

def get_bs_obj(url):
    try:
        html = urlopen(url)
    except URLError as e:
        print("failed to get %s:\r\n%s" % (url, e))
        return None
    except HTTPError as e:
        print(e)
        return None
    else:
        return BeautifulSoup(html.read(), "html.parser")

def get_json_response(url):
    try:
        response = urlopen(url).read().decode('utf-8')
    except URLError as e:
        print("failed to get %s:\r\n%s" % (url, e))
        return None
    except HTTPError as e:
        print(e)
        return None
    else:
        return json.loads(response)