"""ScanFiles_Gui.pyw

Show data for PDFs in a specified directory using the ScanFiles module

"""

import Tkinter
from Tkinter import *
import ScrolledText

class Application(Tkinter.Frame):
    """ This GUI class provides a UI
        
    """

    def __init__(self, parent=None):
        Frame.__init__(self, parent)
        self.pack()
        self.inputStr = Tkinter.StringVar()
        self.makeTopFrame()
        self.text = ScrolledText.ScrolledText(self, font="Courier")
        self.text.pack(side=TOP, expand=True)
        goButton = Tkinter.Button(self, text="P0001", command=self.P0001)
        goButton.pack()

    def makeTopFrame(self):
        """ Frame, textarea, and browse button instantiation """
        frame = Tkinter.Frame(self)
        frame.pack(side=TOP, fill=X)
        label = Label(frame, text="Input:  ")
        label.pack(side=LEFT)
        self.entry = Tkinter.Entry(frame, textvariable=self.inputStr)
        self.entry.pack(side=LEFT, fill=X, expand=True)

    def P0001(self):
        """ If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9. The sum of these multiples is 23.
            Find the sum of all the multiples of 3 or 5 below 1000. """
        self.text.delete(1.0, END)
        self.inputNumber = self.entry.get()
        self.text.insert(END, "You entered: ")
        self.text.insert(END, self.inputNumber)
        self.text.insert(END, "\n")
        self.sum = int(self.inputNumber) + 1
        self.text.insert(END, "Sum is: ")
        self.text.insert(END, self.sum)
        self.text.insert(END, "\n...Done\n")

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
root.title('Euler')
root.mainloop()


