'use strict';

const electron = require('electron');
const app = electron.app;
const BrowserWindow = electron.BrowserWindow;

let mainWindow = null;
console.log("foobar");
console.log(require('osenv').home());

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') app.quit();
});

app.on('ready', () => {
    mainWindow = new BrowserWindow();
    mainWindow.loadURL(`file://${app.getAppPath()}/index.html`);
    mainWindow.on('closed', () => {
        mainWindow = null;
    });
});