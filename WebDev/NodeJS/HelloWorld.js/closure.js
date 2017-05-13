console.log("Hello")

function getColorAsync(callback) {
    setTimeout(callback, 2000);
}

// getColorAsync outputs "green"
let color = "blue";
getColorAsync(() => {
    console.log("The color is %s", color);
});
color = "green";

color = "purple";
// use a JavaScript closure to "freeze" the assigned value of color:
(color => {
    getColorAsync(() => {
        console.log("The color is %s", color);
    });
})(color);

color = "blue";

