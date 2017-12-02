"use strict";

function increment_counter_0() {
    return ++counter;
}

var counter = 0;
function increment_counter_stuff() {
    counter = increment_counter_0();
    console.log("counter: " + counter);

    var increment = counter => counter + 1;
    console.log("counter: " + increment(counter));
}

function calculate_average_stuff() {
    var sum = (total, current) => total + current;
    var total = arr => arr.reduce(sum);
    var size = arr => arr.length;
    var divide = (a, b) => a / b;
    var average = arr => divide(total(arr), size(arr));
    console.log(average([5, 6, 7, 8]));
}

function square_all_numbers_0(array) {
    // var array = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
    for (let i = 0; i < array.length; i++) {
        array[i] = Math.pow(array[i], 2);
    }
    console.log(array);
}

function square_all_numbers_1(array) {
    var new_array = array.map(function(num) {return Math.pow(num, 2)});
    console.log(new_array);
}

function square_all_numbers_2(array) {
    var new_array = array.map(num => Math.pow(num, 2));
    console.log(new_array);
}

function square_all_numbers_stuff() {
    // square_all_numbers_0([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
    // square_all_numbers_1([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
    square_all_numbers_2([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
}

function get_student(ssnKey) {
    // ssn, firstname, lastname
    return {ssn: ssnKey, firstname: "Barb", lastname: "Logger"};
}

function show_student_info_0() {
    let student = get_student(123);
    if (student != null) {
        console.log(student.ssn + " " + student.firstname + " " + student.lastname);
    }
    else {
        throw new Error('Student not found w/ ssn=' + ssnKey);
    }
}

function sort_array_stuff() {
    var sortDesc = arr => {
        return arr.sort((a, b) => b - a);
    }
    var arr = [3, 1, 4, 1, 5, 9];
    var newArr = sortDesc(arr);
    console.log("arr: " + arr);
    console.log("newArr: " + newArr);
}

function main() {
    // increment_counter_stuff();
    // calculate_average_stuff();
    // square_all_numbers_stuff();
    // show_student_info_0();
    sort_array_stuff();
}

main();