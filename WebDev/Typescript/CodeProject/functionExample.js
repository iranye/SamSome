function myFunction(x, y) {
    return x + y;
}
console.log(myFunction(18, 2));
function optionalParams(x, y) {
    if (y === void 0) { y = 5; }
    return x * y;
}
console.log(optionalParams(2));
function arraryParamter(x) {
    var sum = 0;
    for (var _i = 0, x_1 = x; _i < x_1.length; _i++) {
        var e_1 = x_1[_i];
        sum += e_1;
    }
    return sum;
}
var arr = new Array();
arr.push(1);
arr.push(2);
console.log(arraryParamter(arr));
function restArraryParamter() {
    var x = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        x[_i] = arguments[_i];
    }
    var sum = 0;
    for (var _a = 0, x_2 = x; _a < x_2.length; _a++) {
        var e_2 = x_2[_a];
        sum += e_2;
    }
    return sum;
}
console.log(restArraryParamter(1, 3, 5));
// Arrow functions - first pass returns false
var Customer = (function () {
    function Customer() {
        this.IsSaved = false;
    }
    Customer.prototype.Save = function (f) {
        setTimeout(function () {
            this.IsSaved = true; // 'this' refers to anonymous function, not class
            f();
        }, 100);
    };
    return Customer;
}());
var e = new Customer();
e.Save(function () {
    console.log(e.IsSaved);
});
// Arrow functions - 2nd pass returns true
var CustomerWorks = (function () {
    function CustomerWorks() {
        this.IsSaved = false;
    }
    // Save(f):void {
    CustomerWorks.prototype.Save = function (f) {
        var _this = this;
        setTimeout(function () {
            _this.IsSaved = true; // 'this' refers to class
            f();
        }, 100);
    };
    return CustomerWorks;
}());
var e2 = new CustomerWorks();
e2.Save(function () {
    // console.log(e2.IsSaved);
    return "foobar";
});
