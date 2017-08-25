// Throws an exception
var CustomerWithProperties = (function () {
    function CustomerWithProperties() {
    }
    Object.defineProperty(CustomerWithProperties.prototype, "age", {
        get: function () {
            return this._age;
        },
        set: function (theAge) {
            console.log('new age received ' + theAge);
            if (theAge < 0 || theAge > 100) {
                throw "Invalid Age";
            }
            this._age = theAge;
            console.log('new age is set to ' + theAge);
        },
        enumerable: true,
        configurable: true
    });
    return CustomerWithProperties;
}());
var c2 = new CustomerWithProperties();
c2.age = 55;
c2.age = -65;
