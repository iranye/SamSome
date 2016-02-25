import re
from py_lib import *
import json

def parse_literal_Json():
    jsonStr = '{"arrayOfNums":[{"number":0},{"number":1},{"number":2}],"arrayOfFruits":[{"fruit":"apple"},{"fruit":"banana"},{"fruit":"pear"}]}'
    jsonObj = json.loads(jsonStr);
    print(jsonObj.get("arrayOfNums"))
    print(jsonObj.get("arrayOfNums")[1])
    print(jsonObj.get("arrayOfNums")[1].get("number") + jsonObj.get("arrayOfNums")[2].get("number"))
    print(jsonObj.get("arrayOfFruits")[2].get("fruit"))

def parse_gmap_Json():
    jsonObj = get_json_response("https://maps.googleapis.com/maps/api/geocode/json?address=1+Science+Park+Boston+MA+02114")
    if jsonObj == None:
        return
    print(jsonObj.get('results')[0].get('address_components')[0])

#{
#   "results" : [
#      {
#         "address_components" : [
#            {
#               "long_name" : "Museum Of Science Driveway",
#               "short_name" : "Museum Of Science Driveway",
#               "types" : [ "route" ]
#            },
#            {
#               "long_name" : "Boston",
#               "short_name" : "Boston",
#               "types" : [ "locality", "political" ]
#            },
#            {
#               "long_name" : "Massachusetts",
#               "short_name" : "MA",
#               "types" : [ "administrative_area_level_1", "political" ]
#            },
#            {
#               "long_name" : "United States",
#               "short_name" : "US",
#               "types" : [ "country", "political" ]
#            },
#            {
#               "long_name" : "02114",
#               "short_name" : "02114",
#               "types" : [ "postal_code" ]
#            }
#         ],
#         "formatted_address" : "Museum Of Science Driveway, Boston, MA 02114, USA",
#         "geometry" : {
#            "bounds" : {
#               "northeast" : {
#                  "lat" : 42.3687854,
#                  "lng" : -71.06961339999999
#               },
#               "southwest" : {
#                  "lat" : 42.3666775,
#                  "lng" : -71.07326490000001
#               }
#            },
#            "location" : {
#               "lat" : 42.3677994,
#               "lng" : -71.0708078
#            },
#            "location_type" : "GEOMETRIC_CENTER",
#            "viewport" : {
#               "northeast" : {
#                  "lat" : 42.3690804302915,
#                  "lng" : -71.06961339999999
#               },
#               "southwest" : {
#                  "lat" : 42.3663824697085,
#                  "lng" : -71.07326490000001
#               }
#            }
#         },
#         "place_id" : "ChIJ3YE7YpZw44kRQKJTFGx_8V0",
#         "types" : [ "route" ]
#      }
#   ],
#   "status" : "OK"
#}