using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace AsycAwaitWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExecuteSync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            RunDownloadSync();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time (Sync): {elapsedMs}";
        }

        private void RunDownloadSync()
        {
            List<string> websites = PrepData();
            foreach (var site in websites)
            {
                var results = DownloadWebsite(site);
                ReportWebsiteInfo(results);
            }
        }

        private async void ExecuteAsync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await RunDownloadAsync(); // RunDownloadParallelAsync(); // RunDownloadAsync();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time (Sync): {elapsedMs}";

        }

        private async Task RunDownloadAsync()
        {
            List<string> websites = PrepData();
            foreach (var site in websites)
            {
                var results = await Task.Run(() => DownloadWebsite(site));
                ReportWebsiteInfo(results);
            }
        }

        private async Task RunDownloadParallelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            foreach (var site in websites)
            {
                tasks.Add(Task.Run(() => DownloadWebsite(site)));
            }

            WebsiteDataModel[] results = await Task.WhenAll(tasks);

            foreach (var item in results)
            {
                ReportWebsiteInfo(item);
            }
        }

        private void ReportWebsiteInfo(WebsiteDataModel data)
        {
            resultsWindow.Text += $"{data.WebsiteUrl} downloaded: { data.WebsiteData.Length} characters {Environment.NewLine}";
        }

        private WebsiteDataModel DownloadWebsite(string url)
        {
            var output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = url;
            output.WebsiteData = client.DownloadString(url);

            return output;
        }

        private List<string> PrepData()
        {
            var output = new List<string>();

            resultsWindow.Text = "";
            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");

            return output;
        }
    }
}
