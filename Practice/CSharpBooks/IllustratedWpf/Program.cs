using System;
using System.Windows;
using System.Windows.Controls;

namespace IllustratedWpf
{
    class MyWindow : Window
    {
        public MyWindow()
        {
            Width = 300;
            Height = 300;
            Title = "My Simple Window";
            WindowStyle = WindowStyle.ToolWindow;  // WindowStyle = WindowStyle.ToolWindow;

            StackPanel sp = new StackPanel();
            Button btn = new Button();
            TextBlock tb = new TextBlock();

            btn.Content = "Click Me";
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;

            tb.Text = "Illustrated WPF";

            sp.Children.Add(tb);
            sp.Children.Add(btn);

            Content = sp;
        }
    }

    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            var myWin = new MyWindow();
            myWin.Show();

            Application myApp = new System.Windows.Application();
            myApp.Run();
        }
    }
}
