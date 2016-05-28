"""ScanFiles_Gui.pyw

Show data for PDFs in a specified directory using the ScanFiles module

"""

import Tkinter
from Tkconstants import *
import ScrolledText
import tkFileDialog
import os
from ScanFiles import scan_pdf_files

class FileInfoGui(Tkinter.Frame):
    """ This GUI class provides a UI to the sub-module (currently ScanFiles) 
        and dumps output to a text area 
        
    """

    def __init__(self, parent=None):
        Tkinter.Frame.__init__(self, parent)
        self.pack()
        self.dirName = Tkinter.StringVar()
        self.makeTopFrame()
        self.text = ScrolledText.ScrolledText(self, font="Courier")
        self.text.pack(side=TOP, expand=True)
        goButton = Tkinter.Button(self, text="Go", command=self.onGo)
        goButton.pack()

    def makeTopFrame(self):
        """ Frame, textarea, and browse button instantiation """
        frame = Tkinter.Frame(self)
        frame.pack(side=TOP, fill=X)
        label = Tkinter.Label(frame, text="Target Directory:  ")
        label.pack(side=LEFT)
        self.entry = Tkinter.Entry(frame, textvariable=self.dirName)
        self.entry.pack(side=LEFT, fill=X, expand=True)
        browseBtn = Tkinter.Button(frame, text="Browse...", command=self.onBrowse)
        browseBtn.pack()

    def onBrowse(self):
        """ set the directory to scan """
        self.dirName.set(os.path.normpath(tkFileDialog.askdirectory()))

    def onGo(self):
        """ invoke the scan on the directory and put search hits in the textarea """
        self.text.delete(1.0, END)

        if os.path.exists(self.dirName.get()) == False:
            self.text.insert(END, "Error: Invalid Directory: '%s'\n" % self.dirName.get())
            return

        self.text.insert(END, "\t".join(["filename", "PDF Creator", "PDF Producer", "PageCount"]))
        self.text.insert(END, "\n")

        for fname, creator, producer, pagecount in scan_pdf_files(self.dirName.get()):
            # filter out results
            search_word = "Crystal"
            if search_word in str(creator):
                self.text.insert(END, "\t".join([fname, creator, producer, pagecount]))
                self.text.insert(END, "\n")
        self.text.insert(END, "\n...Done\n")

""" Invoke the GUI class """
root = Tkinter.Tk()
app = FileInfoGui(root)
root.title('File Data Gui')
root.mainloop()


