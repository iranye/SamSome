
function FieldObj(field_name, type, aux) {
  this.field_name = field_name;
  this.type       = type;
  this.aux        = aux;
}

FieldObj.prototype.equals = function(name) {
  return (this.field_name == name);
}

//CREATE TABLE Boats([bid] INTEGER PRIMARY KEY, [bname] CHAR(20), [color] CHAR(20));
FieldObj.prototype.toString = function() {
  var temp_aux = (this.aux) ? this.aux : "";
  var listing = this.field_name + " " + this.type + "(" + temp_aux + ")";
  return listing;
}

//ListNode Object
function ListNode(data_obj) {
  this.data = data_obj;
  this.next = null;
}

function InsertSorted(head, data_obj) {
    if (head === null) {
        head = new ListNode(data_obj);
        return head;
    }

    var current = head;
    
    if (data_obj < head.data) { // insert at head   
        head = new ListNode(data_obj);
        head.next = current;
        return head;
    }

    var previous = current;
    while(current != null && current.data < data_obj) {
        previous = current;
        current = current.next;
    }
    var tmp = new ListNode(data_obj);
    previous.next = tmp;
    tmp.next = current;

    return head;
}

//Table Object keeps FieldObjs as Linked List
function TableObj(name) {
  this.name = name;
  this.list = null;
}

TableObj.prototype.addField = function(new_obj) {
  if (!this.list) {
    this.list = new ListNode(new_obj);
    return;
  }
  var current = this.list;
  if (current.data.equals(new_obj.field_name)) { return -1; }

  //traverse to the end or bail out if Field is already in the TableObj
  while (current.next) {
    current = current.next;
    if (current && current.data.equals(new_obj.field_name)) { return -1; }
  }
  var new_node = new ListNode(new_obj);
  current.next = new_node;

  return 1;
}

TableObj.prototype.equals = function(name) {
  return (this.name == name);
}

TableObj.prototype.toString = function() {
  var listing = this.name;
  if (this.list) {
    //listing += "\n" + this.list.data.toString();
    var current = this.list;
    while (current) {
      listing += "\n" + current.data.toString();
      current = current.next;
    }
    listing += "\n";
  }

  else {
    listing += "\nno fields\n";
  }
  return listing;
}

//Type Object
function TypeObj(name, aux, default_aux) {
  this.type_name    = name;
  this.expected_aux = aux;
  this.default_aux  = default_aux;
}

TypeObj.prototype.equals = function(name) {
  return (this.type_name == name);
}

TypeObj.prototype.toString = function() {
  return  "TypeObj.type_name: " + this.type_name + "\n"
        + "TypeObj.expected_aux: " + this.expected_aux + "\n"
        + "TypeObj.default_aux: " + this.default_aux + "\n";
}


//Table Object keeps FieldObjs as Linked List
function DBObj(name) {
  this.db_name = name;
  this.list = null;
}

DBObj.prototype.addType = function(new_obj) {
  if (!this.list) {
    this.list = new ListNode(new_obj);
    return;
  }
  var current = this.list;
  if (current.data.equals(new_obj.type_name)) { return -1; }

  //traverse to the end or bail out if Field is already in the TableObj
  while (current.next) {
    current = current.next;
    if (current && current.data.equals(new_obj.type_name)) { return -1; }
  }
  var new_node = new ListNode(new_obj);
  current.next = new_node;

  return 1;
}

DBObj.prototype.get_default_aux = function(type) {
  print_to_out("looking up aux for: " + type + "\n");
  print_to_out("this.list.data.default_aux: " + this.list.data.default_aux + "\n");

  if (!this.list) {
    return null;
  }
  var current = this.list;
  if (current.data.equals(type)) { return current.data.default_aux; }

  while (current.next) {
    current = current.next;
    if (current && current.data.equals(type)) { return current.data.default_aux; }
  }
  return null;
}

DBObj.prototype.equals = function(name) {
  return (this.db_name == name);
}

DBObj.prototype.typeNames = function() {
  var typesList = new Array();
  if (this.list) {
    var current = this.list;
    while (current) {
      //print_to_out("current.data.name: " + current.data.name + "\n");

      typesList.push(current.data.type_name);
      current = current.next;
    }
  }

  return typesList;
}

DBObj.prototype.toString = function() {
  var listing = this.name;
  if (this.list) {
    var current = this.list;
    while (current) {
      listing += "\n" + current.data.toString();
      current = current.next;
    }
    listing += "\n";
  }

  else {
    listing += "\nno fields\n";
  }
  return listing;
}

//The List of Database Type objects (each is a list of Common Field Types)
var DBArray = new Array();

/*
** The lookup function returns the index if its in the array, or -1 if not
*/
Array.prototype.lookup_db_type = function(name) {
  for (var i = 0; i < DBArray.length; i++) {
    if (this[i].equals(name)) {
      return i;
    }
  }
  return -1;
}

//The array of TypeObj objects
var TypesArray = new Array();

/*
** The lookup function returns the index if its in the array, or -1 if not
*/
Array.prototype.lookup_type = function(type_name) {
  for (var i = 0; i < TypesArray.length; i++) {
    if (this[i].equals(type_name)) {
      return i;
    }
  }
  return -1;
}

//The array of TableObj objects: Array of linked lists
var TablesArray = new Array();

/*
** The lookup function returns the index of the JobObj if its in the array, or -1 if not
*/
Array.prototype.lookup_table = function(table_name) {
  for (var i = 0; i < TablesArray.length; i++) {
    if (this[i].equals(table_name)) {
      return i;
    }
  }
  return -1;
}

/// Regex Definition //////////////////////////////////////////////////////
function RegexObj(radio_name, sample_string) {
  this['radio_name']    = radio_name;
  this['sample_string'] = sample_string;
}

RegexObj.prototype.toString = function() {
  var listing = this['radio_name'] + ":" + this['sample_string'];
  return listing;
}

var RegexList = new Array();
/*
** The lookup function returns the index of the JobObj if its in the array, or -1 if not
*/
Array.prototype.lookup_regex = function(name) {
  for (var i = 0; i < RegexList.length; i++) {
    if (this[i].radio_name == name) {
      return i;
    }
  }
  return -1;
}
