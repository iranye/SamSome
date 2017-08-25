using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpBooks
{
    class AsyncForm : Form
    {
        Label label;
        Button buttonWebSiteLength;
        Button buttonCopyHugeFile;

        public AsyncForm()
        {
            label = new Label {Location = new Point(10, 20), Text = "Length"};
            buttonWebSiteLength = new Button { Location = new Point(10, 50), Text = "Get Website Length" };
            buttonWebSiteLength.Click += DisplayWebSiteLength;

            buttonCopyHugeFile = new Button { Location = new Point(10, 80), Text = "Copy file(s)" };
            buttonCopyHugeFile.Click += CopyFileAsync;

            // Source= C:\Users\imnyex\Downloads\Infragistics\Infragistics_Professional_20151_WithSamplesAndHelp.zip
            // Dest= C:\tmp\_TBD_\Infragistics_Professional_20151_WithSamplesAndHelp.zip
            AutoSize = true;
            Controls.Add(label);
            Controls.Add(buttonWebSiteLength);
            Controls.Add(buttonCopyHugeFile);
        }

        async void DisplayWebSiteLength(object sender, EventArgs e)
        {
            //            PrintPageLength();
            label.Text = "Fetching...";
            using (HttpClient client = new HttpClient())
            {
                Task<string> task = client.GetStringAsync("http://csharpindepth.com");
                string text = await task;
                label.Text = text.Length.ToString();
            }
        }

        public async void CopyFileAsync(object sender, EventArgs e)
        {
            // code from: https://stackoverflow.com/questions/882686/asynchronous-file-copy-move-in-c-sharp
            label.Text = "Copying...";
            //            string sourcePath = @"C:\Users\imnyex\Downloads\Infragistics\Infragistics_Professional_20151_WithSamplesAndHelp.zip";
            string sourcePath = @"C:\tmp\_TBD_\src\Infragistics_ASPNET_20151.msi";
            string destinationPath = @"C:\tmp\_TBD_\Infragistics_ASPNET_20151.msi";
            using (Stream source = File.Open(sourcePath, FileMode.Open))
            {
                using (Stream destination = File.Create(destinationPath))
                {
                    await source.CopyToAsync(destination);
                    label.Text = "File(s) copied";
                }
            }
        }

//        async void CopyFile(object sender, EventArgs e)
//        {
//            //            PrintPageLength();
//            label.Text = "Copying...";
//            FileInfo fileInfo = new FileInfo(@"C:\tmp\_TBD_\snapshotId=1800.json");
//            fileInfo.CopyTo()
//            using (HttpClient client = new HttpClient())
//            {
//                Task<string> task = client.GetStringAsync("http://csharpindepth.com");
//                string text = await task;
//                label.Text = text.Length.ToString();
//            }
//        }

        static async Task<int> GetPageLengthAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> fetchTextTask = client.GetStringAsync(url);
                int length = (await fetchTextTask).Length;
                return length;
            }
        }

        void PrintPageLength()
        {
            label.Text = "Fetching...";
            Task<int> lengthTask = GetPageLengthAsync("http://csharpindepth.com");
            label.Text = lengthTask.Result.ToString();
        }
    }
}
