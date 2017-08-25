// Simple class
class Customer {
    private CustomerName:String;
    public Address:String;
    Save():void {
        console.log("Customer Saved: " + this.CustomerName + "-" + this.Address);
    }
}

let c1:Customer = new Customer();
c1.CustomerName = "Customer1";
c1.Address = "Address1"
c1.Save();

// Class w/ Constructor
class CustomerWithConstructor{
    CustomerName:String;
    Address:String;
    constructor(){
        console.log("Customer new object created");
    }
}

let c3:CustomerWithConstructor = new CustomerWithConstructor();

// Class w/ Constructor & Params
class CusomterWithConstructorAndParams{
    CustomerName:String;
    Address:String;
    constructor(name, address){
        this.CustomerName = name;
        this.Address = address;
    }
}

let c4:CusomterWithConstructorAndParams = new CusomterWithConstructorAndParams("Steve Tyler", "Conshohocken PA");
console.log(c4.CustomerName + " at " + c4.Address);

// Class w/ Constructor & Shorthand properties
class CusomterWithConstructorAndShorthandProperties{
    constructor(public CustomerName:String, public Address:String){
    }
}

let c5:CusomterWithConstructorAndShorthandProperties = new CusomterWithConstructorAndShorthandProperties("Jaw Bone", "Miami FLA");
console.log(c5.CustomerName + " at " + c5.Address);

// Base Class and Sub-class
class Animal{
    Name:string;
    protected Habitat:string;
    ToString():string {
        return "The " + this.Name + " lives in the " + this.Habitat;
    }
}

class EndangeredAnimal extends Animal {
    Population:number;
    constructor(address:string) {
        super();
        this.Habitat = address;
    }
    ToString():string {
        return "The " + this.Name + " lives in the " + this.Habitat + " (" + this.Population + ") left";
    }
}

let zebra:EndangeredAnimal = new EndangeredAnimal("Tundra");
zebra.Name = "Zebra";
zebra.Population = 300000;
console.log(zebra.ToString());