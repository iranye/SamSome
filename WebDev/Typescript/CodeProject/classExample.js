var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
// Simple class
var Customer = (function () {
    function Customer() {
    }
    Customer.prototype.Save = function () {
        console.log("Customer Saved: " + this.CustomerName + "-" + this.Address);
    };
    return Customer;
}());
var c1 = new Customer();
c1.CustomerName = "Customer1";
c1.Address = "Address1";
c1.Save();
// Class w/ Constructor
var CustomerWithConstructor = (function () {
    function CustomerWithConstructor() {
        console.log("Customer new object created");
    }
    return CustomerWithConstructor;
}());
var c3 = new CustomerWithConstructor();
// Class w/ Constructor & Params
var CusomterWithConstructorAndParams = (function () {
    function CusomterWithConstructorAndParams(name, address) {
        this.CustomerName = name;
        this.Address = address;
    }
    return CusomterWithConstructorAndParams;
}());
var c4 = new CusomterWithConstructorAndParams("Steve Tyler", "Conshohocken PA");
console.log(c4.CustomerName + " at " + c4.Address);
// Class w/ Constructor & Shorthand properties
var CusomterWithConstructorAndShorthandProperties = (function () {
    function CusomterWithConstructorAndShorthandProperties(CustomerName, Address) {
        this.CustomerName = CustomerName;
        this.Address = Address;
    }
    return CusomterWithConstructorAndShorthandProperties;
}());
var c5 = new CusomterWithConstructorAndShorthandProperties("Jaw Bone", "Miami FLA");
console.log(c5.CustomerName + " at " + c5.Address);
// Base Class and Sub-class
var Animal = (function () {
    function Animal() {
    }
    Animal.prototype.ToString = function () {
        return "The " + this.Name + " lives in the " + this.Habitat;
    };
    return Animal;
}());
var EndangeredAnimal = (function (_super) {
    __extends(EndangeredAnimal, _super);
    function EndangeredAnimal(address) {
        var _this = _super.call(this) || this;
        _this.Habitat = address;
        return _this;
    }
    EndangeredAnimal.prototype.ToString = function () {
        return "The " + this.Name + " lives in the " + this.Habitat + " (" + this.Population + ") left";
    };
    return EndangeredAnimal;
}(Animal));
var a1 = new EndangeredAnimal("Tundra");
a1.Name = "Zebra";
a1.Population = 300000;
console.log(a1.ToString());
