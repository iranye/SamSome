using System;
using System.Windows;

namespace IllustratedWpf
{
    class MyWindow : Window
    {
        public MyWindow()
        {
            Width = 300;
            Height = 300;
            Title = "My Simple Window";
            Content = "Hi";
        }
    }
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            var myWin = new MyWindow();
            myWin.Show();

            Application myApp = new Application();
            myApp.Run();
        }
    }
}
