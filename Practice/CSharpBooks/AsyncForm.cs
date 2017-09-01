using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpBooks
{
    class AsyncForm : Form
    {
        Label label;
        Button buttonWebSiteLength;
        Button buttonCopyHugeFile;
        Button buttonClearLogBox;
        Button buttonCancelTask;
        Button buttonGettingToGrips;
        Button buttonCancelAsyncOperation;
        
        private TextBox logBox;

        public AsyncForm()
        {
            Width = 600;
            Height = 500;
            int topControlPosition = 20;
            int positionShiftDown = 25;
            label = new Label {Location = new Point(10, topControlPosition), Text = "Length"};
            buttonWebSiteLength = new Button
            {
                Location = new Point(10, topControlPosition + positionShiftDown),
                Text = "Get Website Length"
            };
            buttonWebSiteLength.Click += DisplayWebSiteLength;

            buttonCopyHugeFile = new Button
            {
                Location = new Point(10, topControlPosition + positionShiftDown*2),
                Text = "Copy file(s)"
            };
            buttonCopyHugeFile.Click += CopyFileAsync;

            buttonClearLogBox = new Button
            {
                Location = new Point(10, topControlPosition + positionShiftDown*3),
                Text = "Clear Log Box"
            };
            buttonClearLogBox.Click += buttonClearLogBox_Click;

            buttonCancelTask = new Button
            {
                Location = new Point(10, topControlPosition + positionShiftDown*4),
                Text = "Cancel Task"
            };
            buttonCancelTask.Click += buttonCancelTask_Click;

            buttonGettingToGrips = new Button
            {
                Location = new Point(10, topControlPosition + positionShiftDown * 5),
                Text = "GetToGrips"
            };
            buttonGettingToGrips.Click += buttonGettingToGrips_Click;

            buttonCancelAsyncOperation = new Button
            {
                Location = new Point(300, topControlPosition),
                Text = "Cancel"
            };
            buttonCancelAsyncOperation.Click += buttonCancelAsyncOperation_Click;

            logBox = new TextBox
            {
                Location = new Point(10, 200),
                Multiline = true,
                Width = 500,
                Height= 200
            };

            // Source= C:\Users\imnyex\Downloads\Infragistics\Infragistics_Professional_20151_WithSamplesAndHelp.zip
            // Dest= C:\tmp\_TBD_\Infragistics_Professional_20151_WithSamplesAndHelp.zip
            AutoSize = true;
            Controls.Add(label);
            Controls.Add(buttonWebSiteLength);
            Controls.Add(buttonCopyHugeFile);
            Controls.Add(buttonClearLogBox);
            Controls.Add(buttonCancelTask);
            Controls.Add(buttonGettingToGrips);
            Controls.Add(buttonCancelAsyncOperation);
            Controls.Add(logBox);

            ResetSourceAndToken();
        }

        #region WebSiteLength
        //void DisplayWebSiteLength(object sender, EventArgs e)
        async void DisplayWebSiteLength(object sender, EventArgs e)
        {
            //PrintPageLength();
            label.Text = "Fetching...";
            using (HttpClient client = new HttpClient())
            {
                Task<string> task = client.GetStringAsync("http://csharpindepth.com");
                string text = await task;
                label.Text = text.Length.ToString();
            }
        }

        async Task<string> GetPageAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> fetchTextTask = client.GetStringAsync(url);
                label.Text = "Working...";
                return await fetchTextTask;
            }
        }

        void PrintPageLength()
        {
            label.Text = "Fetching...";
            var task = GetPageAsync("http://csharpindepth.com");
            label.Text = task.Result.Length.ToString();
        }
        #endregion

        #region CancelAsync
        async Task DelayFor30Seconds(CancellationToken token)
        {
            LogToLogBox("Waiting 30 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(5), token);
        }

        void buttonCancelTask_Click(object sender, EventArgs e)
        {
            // Want: one button to kick off a Task, another button to cancel it
            // Want: ability to display progress of async operation
            var task = CancelAsyncTask();
        }

        private async Task CancelAsyncTask()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            Task task = DelayFor30Seconds(source.Token);
            source.CancelAfter(TimeSpan.FromSeconds(1));
            LogToLogBox($"Initial status: {task.Status}");
            try
            {
                //                task.Wait();
                await task;
            }
            catch (AggregateException ex)
            {
                LogToLogBox($"Caught {ex.InnerExceptions[0]}");
            }
            LogToLogBox($"Final status: {task.Status}");
        }
        #endregion

        #region GettingToGripsWithAsync
        // https://www.codeproject.com/Articles/1097427/Asynchronous-Programming-Getting-to-grips-with-Asy

        private int ProveRiemannsHypothesis()
        {
            Thread.Sleep(2000);
            return 42;
        }

        private async Task<int> ProveRiemannsHypothesisAsync()
        {
            Task<int> taskReturnedFromTaskRun = Task.Run(() => ProveRiemannsHypothesis());
            int result = await taskReturnedFromTaskRun;
            return result;
        }

        //async void buttonGettingToGrips_Click(object sender, EventArgs e)
        //{
        //    label.Text = "Proving...";
        //    Task<int> result = ProveRiemannsHypothesisAsync();
        //    int value = await result;
        //    label.Text = value.ToString();
        //}

        int ProveRiemannsHypothesisWithCancel(CancellationToken cancellationToken)
        {
            Thread.Sleep(2000);
            if (tokenSource.IsCancellationRequested)
            {
                ResetSourceAndToken();
                cancellationToken.ThrowIfCancellationRequested();
            }
            return 42;
        }

        async Task<int> ProveRiemannsHypothesisWithCancelAsync(CancellationToken cancellationToken)
        {
            if (tokenSource.IsCancellationRequested)
            {
                ResetSourceAndToken();
            }
            int solution = await Task.Run(() => ProveRiemannsHypothesisWithCancel(cancellationToken));
            return solution;
        }

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        private void ResetSourceAndToken()
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
        }

        async void buttonGettingToGrips_Click(object sender, EventArgs e)
        {
            label.Text = "Proving...";
            ClearLogBox();
            try
            {
                Task<int> result = ProveRiemannsHypothesisWithCancelAsync(token);
                int value = await result;
                label.Text = value.ToString();

            }
            catch (Exception ex)
            {
                LogToLogBox("Exception in buttonGettingToGrips_Click: " + ex.Message);
                label.Text = ex.GetType().Name == "OperationCanceledException" ? "Cancelled" : "Failed";
            }
        }

        void buttonCancelAsyncOperation_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }
        #endregion

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

        #region Helpers
        private void LogToLogBox(string msg)
        {
            logBox.Text += $"{msg}{Environment.NewLine}";
        }

        void buttonClearLogBox_Click(object sender, EventArgs e)
        {
            ClearLogBox();
            LogToLogBox("Log box cleared");
        }

        private void ClearLogBox()
        {
            logBox.Text = "";
        } 
        #endregion
    }
}
