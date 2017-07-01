const readFile = require('fs').readFile;
const yargs = require('yargs');
const argv = yargs
    .demand('f')
    .nargs('f', 1)
    .describe('f', 'JSON file to parse')
    .argv;
const file = argv.f;


function checkJsonObj(rdl) {
    console.log("rdl.registerFile.registers.length: " + rdl.registerFile.registers.length);
}

function main() {
  console.log({ f: argv.f });
  readFile(file, (err, dataBuffer) => {
      var rdl = JSON.parse(dataBuffer.toString());
      checkJsonObj(rdl);
      // console.log(JSON.stringify(value));
  });
}

main();
