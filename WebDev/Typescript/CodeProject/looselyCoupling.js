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
var AbstractServiceInvoker = (function () {
    function AbstractServiceInvoker() {
    }
    AbstractServiceInvoker.prototype.Save = function () {
        var ServiceURL = this.GetServiceURL();
        var Credentials = "UserName:abcd,password:123";
        var data = this.getData();
        var Result = 5;
        this.setData(Result);
    };
    return AbstractServiceInvoker;
}());
var Logger = (function () {
    function Logger() {
    }
    Logger.prototype.LogError = function (e) {
        console.log("error " + e + " logged");
    };
    Logger.prototype.SendEmailLog = function (s) {
        console.log("message " + s + " sent via email");
        return true;
    };
    return Logger;
}());
var CustomerService = (function (_super) {
    __extends(CustomerService, _super);
    function CustomerService() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CustomerService.prototype.GetServiceURL = function () {
        return "http://someService.com/abcd";
    };
    CustomerService.prototype.getData = function () {
        return "{CustomerName:'a', Address:'b'}";
    };
    CustomerService.prototype.setData = function (n) {
        throw new Error("Method not implemented.");
    };
    return CustomerService;
}(AbstractServiceInvoker));
