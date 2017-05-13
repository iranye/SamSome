const Currency = require('./currency.js');

const canadianDollar = 0.91;
const currency = new Currency(canadianDollar);

var valueToConvert = 50;
var converted = currency.canadianToUS(valueToConvert);
console.log(valueToConvert + " Canadian = " + converted + " US");

valueToConvert = 30;
converted = currency.USToCanadian(valueToConvert);
console.log(valueToConvert + " US = " + converted + " Canadian");

// TODO: Put Rectangle into separate file
class Rectangle {
    constructor(height, width) {
        this.height = height;
        this.width = width;
    }
    get area() {
        return this.calcArea();
    }
    calcArea() {
        return this.height * this.width;
    }
}

var p = new Rectangle(4, 2);
console.log("height: " + p.height + " width: " + p.width + " Area: " + p.area);

