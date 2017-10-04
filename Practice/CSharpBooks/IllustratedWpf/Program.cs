using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            Color color = new Color();
            color.A = 255;
            color.R = 100;
            color.G = 150;
            color.B = 200;

            SolidColorBrush scb = new SolidColorBrush(color);
            Background = scb;

            tb.Text = "Illustrated WPF";

            sp.Children.Add(tb);
            sp.Children.Add(btn);

            Content = sp;
        }
    }

    class Program
    {
        static void App_Startup(object sender, StartupEventArgs args)
        {
            MessageBox.Show("Application is starting.", "Starting Message");
        }

        [STAThread]
        static void Main(string[] args)
        {
            var myWin = new MyWindow();
            myWin.Show();

            Application myApp = new System.Windows.Application();
            myApp.Startup += App_Startup;
            myApp.Run();
        }
    }
}
