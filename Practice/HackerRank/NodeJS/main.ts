function myFunction(x:number,y:number):number {
    return x+y;
}

function varStuff():void {
    let myVar:string = "ira";
    let n:number=5;
    let b:boolean=true;
    console.log(myVar);

    let dyn:any=5;
    console.log(dyn+1);
    dyn="A";
    console.log(dyn+1);
    console.log(myFunction(1,2));
}

class node {
    private _data:number;
    public next:node = null;
    constructor(numb:number=1) {
        this._data = numb;
    }
    get data():number {
        return this._data;
    }
    // print():string {
    //     return data.toString();
    // }
}

function printLinkedList(head:node):void {
    if (head == null) {
        console.log("null");
        return;
    }
    let current:node = head;
    while (current != null) {
        console.log(current.data);
        current = current.next;
    }
}

function linkedListStuff():void {
    let head:node = new node(22);
    let newNode = new node();
    head.next = newNode;
    // console.log(head.data);
    printLinkedList(head);
}

function main():void {
    // varStuff();
    linkedListStuff();

}

main();