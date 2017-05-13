const fs = require('fs');
const util = require('util');

const tasks = [];
const wordCounts = {};
const filesDir = './text';
let completedTasks = 0;

function checkIfComplete() {
    completedTasks++;
    if (completedTasks === tasks.length) {
        for (let index in wordCounts) {
            console.log("%s:%s", index, wordCounts[index]);
        }
    }
}

function addWordCount(word) {
    wordCounts[word] = (wordCounts[word]) ? wordCounts[word] + 1 : 1;
}

function countWordsInText(text) {
    const words = text
        .toString()
        .toLowerCase()
        .split(/\W+/)
        .sort();

    words
        .filter(word => word)
        .forEach(word => addWordCount(word));
}

fs.readdir(filesDir, (err, files) => {
    if (err) throw Error
    files.forEach(file => {
        const task = (file => {
            return () => {
                fs.readFile(file, (err, text) => {
                    if (err) throw Error;
                    countWordsInText(text);
                    checkIfComplete();
                });
            };
        })(util.format("%s/%s", filesDir, file));
        tasks.push(task);
    })
    tasks.forEach(task => task());
});
