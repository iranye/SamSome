function Add(...values:Array<number>):number {
    let sum:number = 0;
    values.forEach(element => {
        sum += element;
    });
    return sum;
}

export function AddTwoNumbers(x:number, y:number) {
    return Add(x, y);
}