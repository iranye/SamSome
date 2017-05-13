// See article at: https://caolan.org/posts/unit_testing_in_node_js.html
var doubled = require('../lib/doubled');

exports['calculate'] = function (test) {
    test.equal(doubled.calculate(2), 4);
    test.equal(doubled.calculate(5), 10);
    test.throws(function () { doubled.calculate(); });
    test.throws(function () { doubled.calculate(null); });
    test.throws(function () { doubled.calculate(true); });
    test.throws(function () { doubled.calculate([]); });
    test.throws(function () { doubled.calculate({}); });
    test.throws(function () { doubled.calculate('asdf'); });
    test.throws(function () { doubled.calculate('123'); });
    test.done();
};

exports['read a number'] = function (test) {
    // This fails:
    // var ev = new events.EventEmitter();
    
    // process.openStdin = function () { return ev; };
    // process.exit = test.done;
    
    // doubled.read();
    // ev.emit('data', '12');
};