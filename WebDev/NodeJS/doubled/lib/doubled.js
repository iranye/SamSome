exports.calculate = function (num) {
    if (typeof num === 'number') {
        return num * 2;
    }
    else {
        throw new Error("Expecting a number");
    }
}
exports.read = function() {
    var stdin = process.openStdin();
    
    stdin.on('data', function (chunk) {
        process.exit();
    });
};