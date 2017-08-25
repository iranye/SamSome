var MathsNameSpace;
(function (p) {
    function add(x, y) {
        alert(x+y);
    }
    function sub(x, y) {
        alert(x - y);
    }
    p.add = add;
})(MathsNameSpace || (MathsNameSpace = {}));