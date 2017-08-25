var Customer = (function () {
    function Customer() {
    }
    Customer.prototype.Save = function () {
        console.log("Customer Saved: " + this.CustomerName + "-" + this.Address);
    };
    return Customer;
}());
// let c:Customer = new Customer();
// c.CustomerName = "Customer1";
// c.Address = "Address1"
// c.Save();
// let c1:Customer = new Customer();
// c1.CustomerName = "Customer1";
// c1.Address = "Address1"
// c1.Save(); 
