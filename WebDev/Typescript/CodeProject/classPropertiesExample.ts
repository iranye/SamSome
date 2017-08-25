// Throws an exception
// Transpile with: tsc classPropertiesExample.ts --target es5
class CustomerWithProperties {
    private _age:number;
    get age():number {
        return this._age;
    }
    set age(theAge:number) {
        console.log('new age received ' + theAge);
        if (theAge < 0 || theAge > 100) {
            throw "Invalid Age";
        }
        this._age = theAge;
        console.log('new age is set to ' + theAge);
    }
}

let c2:CustomerWithProperties = new CustomerWithProperties();
c2.age = 55;
c2.age = -65;

