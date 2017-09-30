function myFunction(x, y) {
    return x + y;
}
function varStuff() {
    var myVar = "ira";
    var n = 5;
    var b = true;
    console.log(myVar);
    var dyn = 5;
    console.log(dyn + 1);
    dyn = "A";
    console.log(dyn + 1);
    console.log(myFunction(1, 2));
}
var node = (function () {
    function node(numb) {
        if (numb === void 0) { numb = 1; }
        this.next = null;
        this._data = numb;
    }
    Object.defineProperty(node.prototype, "data", {
        get: function () {
            return this._data;
        },
        enumerable: true,
        configurable: true
    });
    return node;
}());
function printLinkedList(head) {
    if (head == null) {
        console.log("null");
        return;
    }
    var current = head;
    while (current != null) {
        console.log(current.data);
        current = current.next;
    }
}
function linkedListStuff() {
    var head = new node(22);
    var newNode = new node();
    head.next = newNode;
    // console.log(head.data);
    printLinkedList(head);
}
function main() {
    // varStuff();
    linkedListStuff();
}
main();
