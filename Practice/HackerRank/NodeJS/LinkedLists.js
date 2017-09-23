
var Node = function(data) {
    this.data = data;
    this.next = null;
}

function main() {
    var list = generateLinkedList();
    var anotherList = generateAnotherLinkedList();
    printLinkedList(list);
    printLinkedList(anotherList);
    console.log();
    // var reversed = reverseLinkedList(list);
    // var listWithNodeRemoved = deleteNode(list, 0);
    // printLinkedList(listWithNodeRemoved);
    printLinkedList(mergeLinkedLists(list, anotherList));
}
main();

function mergeLinkedLists(headA, headB) {
    if (headA == null) {
        return headB;
    }
    if (headB == null) {
        return headA;
    }

    function compareDatas(node1, node2) {
        if (node1 == null) { return 1; }
        if (node2 == null) { return -1; }
        if (node1.data < node2.data) { return -1; }
        if (node1.data > node2.data) { return 1; }
        return 0;
    }

    var newHead = null;
    var current = newHead;
    var nextNodeToAppend = null;
    do {
        var retVal = compareDatas(headA, headB);
        if (retVal <= 0) {
            nextNodeToAppend = headA;
            headA = headA.next;
        }
        else {
            nextNodeToAppend = headB;
            headB = headB.next;
        }
        if (newHead == null) {
            newHead = nextNodeToAppend;
            current = newHead;
        }
        else {
            current.next = nextNodeToAppend;
            current = current.next;
        }
    } while (nextNodeToAppend != null && (headA != null || headB != null));

    return newHead;
}

function printLinkedList(head) {
    // console.log("foobar");
    if (head == null) {
        return;
    }
    var current = head;
    while (current != null) {
        console.log(current.data);
        current = current.next;
    }
}

function deleteNode(head, position) {
    if (head == null) {
        return null;
    }
    if (position == 0) {
        return head.next;
    }
    var previous = head;
    for(var i = 0, current = head; i <= position && current != null; i++, current = current.next) {
        if (i < position) {
            previous = current;
        }
    }
    previous.next = current;
    return head;
}

function reverseLinkedList(head) {
    if (head == null) {
        return null;
    }
    if (head.next == null) {
        return head;
    }
    var current = head;
    var previous = null;
    while (current != null) {
        var temp = current.next;
        if (previous == null) {
            previous = head;
            previous.next = null;
        }
        else {
            current.next = previous;
        }
        previous = current;
        current = temp;
        if (current != null) {
            temp = current.next;
            current.next = previous;
            previous = current;
            current = temp;
        }
    }
    return previous;
}

function generateLinkedList() {
    var arr = [2, 3, 5, 7, 11];
    var head = null;
    var current = null;
    for(var i = 0; i < arr.length; i++) {
        // console.log("arr[i]" + arr[i]);
        if (head == null) {
            head = new Node(arr[i]);
            // console.log("head.data: " + head.data);
        }
        else {
            if (current == null) {
                current = new Node(arr[i]);
                head.next = current;
                // console.log(head.next.data);
            }
            else {
                current.next = new Node(arr[i]);
                current = current.next;
            }
        }
    }
    return head;
}

function generateAnotherLinkedList() {
    var arr = [4, 6, 8, 12];
    var head = null;
    var current = null;
    for(var i = 0; i < arr.length; i++) {
        // console.log("arr[i]" + arr[i]);
        if (head == null) {
            head = new Node(arr[i]);
            // console.log("head.data: " + head.data);
        }
        else {
            if (current == null) {
                current = new Node(arr[i]);
                head.next = current;
                // console.log(head.next.data);
            }
            else {
                current.next = new Node(arr[i]);
                current = current.next;
            }
        }
    }
    return head;
}
