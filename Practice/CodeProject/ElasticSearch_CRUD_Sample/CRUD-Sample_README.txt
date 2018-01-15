ToDo *
Log box
Fix "new Customer fails to get added using GUI"
Fetch Ids of all existing documents and auto-gen the next one for new adds.

https://www.elastic.co/guide/en/elasticsearch/client/net-api/1.x/nest-quick-start.html

https://www.codeproject.com/Articles/1029482/A-Beginners-Tutorial-for-Understanding-and-Imple

* Create "database" & "table" *
PUT http://localhost:9200/crud_sample
{
	"mappings": {
		"Customer_Info": {
			"properties": {
				"id": {
					"type": "long"
				},
				"name": {
					"type": "string",
					"index": "not_analyzed"
				},
				"age": {
					"type": "integer"
				},
				"birthday": {
					"type": "date",
					"format": "basic_date"
				},
				"hasChildren": {
					"type": "boolean"
				},
				"enrollmentFee": {
					"type": "double"
				}
			}
		}
	}
}

* Check new mapping *
GET http://localhost:9200/crud_sample/_mapping

* Update schema *
PUT http://localhost:9200/crud_sample/_mapping/Customer_Info
{
  "properties" : {
    "opinion" : {
     "type" : "string",
	 "index" : "not_analyzed"
    }
  }
}

* insert row(s) *
PUT http://localhost:9200/crud_sample/Customer_Info/1
{
  "age" : 32,
  "birthday": "19830120",
  "enrollmentFee": 175.25,
  "hasChildren": false,
  "name": "PH",
  "opinion": "It's Ok, I guess..."
}

PUT http://localhost:9200/crud_sample/Customer_Info/10
{
  "age" : 37,
  "birthday": "19930101",
  "enrollmentFee": 42.25,
  "hasChildren": true,
  "name": "Cezar",
  "opinion": "Lucky Dog..."
}

* insert rows in bulk *
POST http://localhost:9200/crud_sample/Customer_Info/_bulk
{"index": { "_id": 1 }}
{"age" : 32, "birthday": "19830120", "enrollmentFee": 175.25, "hasChildren": false, "name": "PH", "opinion": "It's cool, I guess..." }
{"index": { "_id": 2 }}
{"age" : 32, "birthday": "19830215", "enrollmentFee": 175.25, "hasChildren": true, "name": "Marcel", "opinion": "It's very nice!" }
{"index": { "_id": 3 }}
{"age" : 62, "birthday": "19530215", "enrollmentFee": 205.25, "hasChildren": false, "name": "Mayra", "opinion": "I'm too old for that!" }
{"index": { "_id": 4 }}
{"age" : 32, "birthday": "19830101", "enrollmentFee": 100.10, "hasChildren": false, "name": "Juan", "opinion": "¿Qué tal estás?" }
{"index": { "_id": 5 }}
{"age" : 30, "birthday": "19850101", "enrollmentFee": 100.10, "hasChildren": true, "name": "Cezar", "opinion": "Just came for the food..." }
{"index": { "_id": 6 }}
{"age" : 42, "birthday": "19730101", "enrollmentFee": 50.00, "hasChildren": true, "name": "Vanda", "opinion": "Where am I again?" }
{"index": { "_id": 7 }}
{"age" : 42, "birthday": "19730101", "enrollmentFee": 65.00, "hasChildren": false, "name": "Nice", "opinion": "What were u saying again?" }
{"index": { "_id": 8 }}
{"age" : 22, "birthday": "19930101", "enrollmentFee": 150.10, "hasChildren": false, "name": "Telks", "opinion": "Can we go out now?" }
{"index": { "_id": 9 }}
{"age" : 32, "birthday": "19830120", "enrollmentFee": 175.25, "hasChildren": false, "name": "Rafael", "opinion": "Should be fine..." }

* select all *
GET http://localhost:9200/crud_sample/Customer_Info/_search

* deleting *
Delete the whole storage:
delete crud_sample

Delete a specific customer:
delete crud_sample/Customer_Info/1

*** Queries ***
{
    "query" : {
        "filtered" : { 
            "query" : {
                "match_all" : {} 
            },
            "filter" : {
                "term" : { 
                    "opinion" : "It's cool, I guess..."
                }
            }
        }
    }
}

{
   "query" : {
      "filtered" : { 
         "filter" : {
            "bool" : {
              "must" : {
                 "term" : {"hasChildren" : false} 
              },
              "must_not": [ 
                { "term": { "name": "PH"  }},
                { "term": { "name": "Felix"  }}
              ],
              "should" : [
                 { "term" : {"age" : 30}}, 
                 { "term" : {"age" : 31}}, 
                 { "term" : {"age" : 32}} 
              ]
           }
         }
      }
   }
}

{
    "query" : {
        "filtered" : {
            "filter" : {
                "terms" : { 
                    "age" : [22, 62]
                }
            }
        }
    }
}

** aggregation **
GET /crud_sample/Customer_Info/_search?search_type=count

* Count names *
{
  "aggregations": {
    "my_agg": {
      "terms": {
        "field": "name",
         "size": 1000
      }
    }
  }
}

* Get AVG Age *
{
    "aggs" : {
        "avg_grade" : { "avg" : { "field" : "age" } }
    }
}

* Get AVG Age, group by hasChildren *
{
   "aggs": {
      "colors": {
         "terms": {
            "field": "hasChildren"
         },
         "aggs": { 
            "avg_age": { 
               "avg": {
                  "field": "age" 
               }
            }
         }
      }
   }
}

