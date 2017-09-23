function append(grades, newGrade) {
    var newGrades = grades.slice(0);
    newGrades.push(newGrade);
    return newGrades;
}

var incrementCounter = (counter) => counter + 1;

function main() {
    var myCounter = 0;
    var newCount = incrementCounter(myCounter);
    console.log(newCount);
    var grades = [3, 5 ,7];
    var newArr = append(grades, 17);
    console.log(newArr);
}

main();
