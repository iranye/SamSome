const util = require('util');

const greeting = "Hello";
console.log("${greeting}"); // interpolate fails
console.log("%s", greeting);

const yourName = "you";
const fullGreeting = util.format("%s %s", greeting, yourName);
console.log(fullGreeting);