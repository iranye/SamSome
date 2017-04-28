"""ScanFiles_Gui.pyw

Show data for PDFs in a specified directory using the ScanFiles module

"""

import tkinter
from tkinter import *
import os

class Application(tkinter.Frame):
    """ This GUI class provides a UI
        
    """

    def __init__(self, parent=None):
        Frame.__init__(self, parent)
        self.pack()
        self.createWidgets()

    def createWidgets(self):
        """ Frame, textarea, and browse button instantiation """
        self.QUIT = Button(self)
        self.QUIT["text"] = "QUIT"
        self.QUIT["fg"] = "red"
        self.QUIT["command"] = self.quit
        self.QUIT.pack({"side": "left"})
        
        self.hi_there = Button(self)
        self.hi_there["text"] = "Hello",
        self.hi_there["command"] = self.say_hi
        self.hi_there.pack({"side": "left"})
        
    def say_hi(self):
        print("hi everyone")
        
""" Invoke the GUI class """
root = Tk()
app = Application(root)
root.title('Hi Gui')
root.mainloop()


