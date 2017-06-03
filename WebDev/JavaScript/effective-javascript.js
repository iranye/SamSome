// 'use strict';

// Item 5
function item5() {
    var result = "1.0e0" == { valueOf: function() { return true; } };
    console.log(result);
    result = "1.0e0" === { valueOf: function() { return true; } };
    console.log(result);
}

// Item 9
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

// Item 11
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

// Main
function main() {
    item11();
}

main();