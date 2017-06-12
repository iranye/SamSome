// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/Arrow_functions

const greeting = "Hello";
console.log("%s", greeting);

// IIFE (imm-invoked-func-expr
(() => console.log("fooobar"))();

var materials = [
  'Hydrogen',
  'Helium',
  'Lithium',
  'Beryllium'
];

var materialsLength1 = materials.map(function(material) {
 return material.length;
});

for (var i = 0; i < materialsLength1.length; i++) {
  console.log("materialsLength1[" + i + "] = " + materialsLength1[i]);
}
console.log("");

var materialsLength2 = materials.map((material) => {
 return material.length;
});

for (var i = 0; i < materialsLength1.length; i++) {
  console.log("materialsLength1[" + i + "] = " + materialsLength1[i]);
}
console.log("");

var materialsLength3 = materials.map(material => material.length);

for (var i = 0; i < materialsLength1.length; i++) {
  console.log("materialsLength1[" + i + "] = " + materialsLength1[i]);
}
console.log("");