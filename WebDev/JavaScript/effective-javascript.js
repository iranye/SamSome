// REGEXER: ([^;]+).*~~console.log("$1: ", $1);
// This file can be run using: node effective-javascript.js
// 'use strict';

// Item 2 floating point numbers
function item2() {
  var parsed = parseInt("1111", 2);
  console.log("parsed: " + parsed);

  var floatSum = 0.1 + 0.2;
  console.log("floatSum: " + floatSum);
}

// Item 3 implicit coercion
function item3() {  
  var obj = {
    tostring: function() {
      return "[object MyObject";
    },
    valueOf: function() {
      return 17;
    }
  };
  console.log("object: " + obj);  
}

// Item 5 Avoid using == with Mixed Types
function item5() {
    var result = "1.0e0" == { valueOf: function() { return true; } };
    console.log(result);
    result = "1.0e0" === { valueOf: function() { return true; } };
    console.log(result);
}

// Item 9 Always Declare Local Variables
function item9() {    
    var a = [10, 11, 12];
    console.log("a before: " + a);
    swap(a, 0, 1);
    console.log("a after: " + a);

    console.log("temp: " + temp);
}

function swap(a, i, j) {
    temp = a[i]; // global
    a[i] = a[j];
    a[j] = temp;
}

// Item 11 Closures
function makeSandwich() {
    var magicIngredient = "peanut butter";
    function make(filling) {
        return magicIngredient + " and " + filling;
    }
    return make("jelly");
}

function peanutButterSandwichMaker() {
    var magicIngredient = "peanut butter";
    
    // make is a closure that refers to two outer variables
    function make(filling) {        
        return magicIngredient + " and " + filling;
    }
    return make;
}

function sandwichMaker(mainIngredient) {
    // make is a closure that refers to two outer variables
    function make(filling) {
        return mainIngredient + " and " + filling;
    }
    return make;
}

function sandwichMaker(mainIngredient) {
    // make is a closure that refers to two outer variables
    function make(filling) {
        return mainIngredient + " and " + filling;
    }
    return make;
}

function sandwichMakerWithFunctionExpression(mainIngredient) {
    // anonymous closure that refers to two outer variables
    return function(filling) {
        return  mainIngredient + " and " + filling;
    }
}

function box() {
    var val = undefined;
    return {
        set: function(newVal) { val = newVal; },
        get: function() { return val; },
        type: function() { return typeof val; }
    }
}

function item11() {
    console.log("makeSandwich(): " + makeSandwich());
    var f = peanutButterSandwichMaker();
    console.log("f('jelly'): " + f('jelly'));
    console.log("f('bananas'): " + f('bananas'));
    
    var hamAnd = sandwichMaker("ham");
    console.log("hamAnd('swiss'): " + hamAnd('swiss'));
    
    var turkeyAnd = sandwichMakerWithFunctionExpression('turkey');
    console.log("turkeyAnd('ham'): " + turkeyAnd('ham'));
    
    var b = box();
    console.log("b.type(): " + b.type());
    console.log("b.set(98.6): " + b.set(98.6));
    console.log("b.get(): " + b.get());
    console.log("b.type(): " + b.type());
}

// Item12 variable hoisting
function isWinner(player, others) {
  var highest = 0;
  for (var i = 0, n = others.length; i < n; i++) {
    var player = others[i];
    if (player.score > highest) {
      highest = player.score;
    }
  }
  return player.score > highest;
}

function test() { // there is no hoisting from catch block
  var x = "var", result = [];
  result.push(x);
  try {
    throw "exception";
  } catch(x) {
    x = "catch";
  }
  result.push(x);
  return result;
}


function item12() {
  // TODO: Initialize a player and arrary of players
  // var player = 
        // set: function(newVal) { val = newVal; },
        // get: function() { return val; },
        // type: function() { return typeof val; }
    // }
  // var player = 
  
  console.log("test(): " + test());
}

// Item 13 use IIFEs to create local scope

// Item 14 Beware of Unportable Scoping of Named Function Expressions

// Item 15 Beware of Unportable Scoping of Block-Local Function Declarations
function f() { return "global"; }

function test1(x) {
  function f() { return "local"; }
  
  var result = [];
  if (x) {
    result.push(f());
  }
  result.push(f());
  return result;
}

function test2(x) {
  var result = [];
  if (x) {
    function f() { return "local"; }  
    result.push(f());
  }
  result.push(f());
  return result;
}

function test3(x) {
  var g = f, result = [];
  if (x) {
    g = function() { return "local"; }
    result.push(g());
  }
  result.push(g());
  return result;
}

function item15() {
  console.log("test(true): ", test(true));
  console.log("test(false): ", test(false));
  // console.log("test2(true): ", test2(true)); // ERROR
  // console.log("test2(false): ", test2(false)); // ERROR
  console.log("test3(true): ", test3(true));
  console.log("test3(false): ", test3(false));
}

// Item 16 Avoid Creating Local Variables with eval
// Item 17 Prefer Indirect eval to Direct eval
// Item 18 Understand the Difference between Function, Method, and Constructor Calls

// Item 19 Get Comfortable Using Higher-Order Functions (functions that take functions as parameters, or return functions)
function compareNumbers(x, y) {
  if (x < y) {
    return -1;
  }
  if (x > y) {
    return 1;
  }
  return 0;
}

function higherOrderSorting() {
  console.log("\n*** HIGHERORDERSORTING()")
  console.log("[3, 1, 4, 1, 5, 9].sort(compareNumbers): ", [3, 1, 4, 1, 5, 9].sort(compareNumbers));
  var sortedArr = [3, 1, 4, 1, 5, 9].sort(function(x, y) {
    if (x < y) {
      return -1;
    }
    if (x > y) {
      return 1;
    }
    return 0;    
  });
  console.log("sortedArr: ", sortedArr);
}

function stringMapping() {
  console.log("\n** STRINGMAPPING")
  var names = ["Fred", "Wilma", "Pebbles"];
  var upper = [];
  for (var i = 0, n = names.length; i < n; i++) {
    upper[i] = names[i].toUpperCase();
  }
  console.log("upper0: " + upper);
  
  upper = names.map(function(name) {
    return name.toUpperCase();
  });  
  console.log("upper1: " + upper);
}

function randomStringIteration0() {
  console.log("\n*** RANDOMSTRINGITERATION0")
  var aIndex = "a".charCodeAt(0); // 97
  var random = "";
  for (var i = 0; i < 8; i++) {
    random += String.fromCharCode(Math.floor(Math.random() * 26) + aIndex);
  }
  console.log("random0: ", random);
}
function item19() {
  higherOrderSorting();
  stringMapping();
  randomStringIteration0()
}


// Main
function main() {
    item19();
}

main();