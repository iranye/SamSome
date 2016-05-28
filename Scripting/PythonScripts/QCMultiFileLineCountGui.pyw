"""QCMultiFileLineCountGui

Count lines in all files in a specified directory"""

import Tkinter
from Tkconstants import *
import ScrolledText
import tkFileDialog
import os

def countLinesInAllFiles(targetdir):
    for fname in os.listdir(targetdir):
        fullpath = os.path.join(targetdir, fname)
        linecount = sum(1 for line in file(fullpath))
        yield fname, linecount

class MultiFileLineCountGui(Tkinter.Frame):
    def makeTopFrame(self):
        frame = Tkinter.Frame(self)
        frame.pack(side=TOP, fill=X)
        label = Tkinter.Label(frame, text="Target Directory:  ")
        label.pack(side=LEFT)
        self.entry = Tkinter.Entry(frame, textvariable=self.dirName)
        self.entry.pack(side=LEFT, fill=X, expand=True)
        browseBtn = Tkinter.Button(frame, text="Browse...", command=self.onBrowse)
        browseBtn.pack()
    def __init__(self, parent=None):
        Tkinter.Frame.__init__(self, parent)
        self.pack()
        self.dirName = Tkinter.StringVar()
        self.makeTopFrame()
        self.text = ScrolledText.ScrolledText(self, font="Courier")
        self.text.pack(side=TOP, expand=True)
        goButton = Tkinter.Button(self, text="Go", command=self.onGo)
        goButton.pack()
    def onBrowse(self):
        self.dirName.set(tkFileDialog.askdirectory().replace('/','\\'))
    def onGo(self):
        self.text.delete(1.0, END)
        filelist = os.listdir(self.dirName.get())
        textformat = "%%%ds\t%%s\n" % (max(len(x) for x in filelist))
        for fname, linecount in countLinesInAllFiles(self.dirName.get()):
            self.text.insert(END,  textformat % (fname, linecount))

root = Tkinter.Tk()
app = MultiFileLineCountGui(root)
root.title('QC Multi File Line Counter')
root.mainloop()