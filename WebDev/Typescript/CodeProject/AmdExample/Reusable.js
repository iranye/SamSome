// Define JS Module called "reusable" (Module name=File name).
define(["exports"], function (exports) {
    function getValue() {
        return getValue2();
    }
    exports.getValue = getValue;
    function getValue2() {
        return "Sukesh Marla";
    }
});