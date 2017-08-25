function myFunction(x:number, y:number):number {
    return x + y;
}
console.log(myFunction(18, 2));

function optionalParams(x:number, y:number=5):number {
    return x*y;
}
console.log(optionalParams(2));

function arraryParamter(x:Array<number>):number {
    let sum:number=0;
    for (let e of x) {
        sum += e;
    }
    return sum;
}
let arr:Array<number> = new Array<number>();
arr.push(1);
arr.push(2);
console.log(arraryParamter(arr));

function restArraryParamter(...x:Array<number>):number {
    let sum:number=0;
    for (let e of x) {
        sum += e;
    }
    return sum;
}
console.log(restArraryParamter(1, 3, 5));

// Arrow functions - first pass returns false
class Customer {
    IsSaved:boolean=false;
    Save(f):void {
        setTimeout(function() {
            this.IsSaved = true; // 'this' refers to anonymous function, not class
            f();
        }, 100);
    }
}
let e:Customer = new Customer();
e.Save(function() {
    console.log(e.IsSaved);
});

// Arrow functions - 2nd pass returns true
class CustomerWorks {
    IsSaved:boolean=false;
    // Save(f):void {
    Save(f:()=>void):void { // this is a more type-safe declaration than previous line
            setTimeout(() => {
            this.IsSaved = true; // 'this' refers to class
            f();
        }, 100);
    }
}
let e2:CustomerWorks = new CustomerWorks();
e2.Save(function() {
    console.log(e2.IsSaved);
});