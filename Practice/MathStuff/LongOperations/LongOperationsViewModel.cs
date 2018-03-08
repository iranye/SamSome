using System.Threading.Tasks;
using System.Windows.Input;
using MicroMvvm;
using System.Net.Http;
using System;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MathStuff.LongOperations
{
    class LongOperationsViewModel : ObservableObject
    {
        public PrimeFactorization PrimeFactorization { get; set; }
        public StatusViewModel StatusViewModel { get; set; }

        const string _defaultUrlInputStr = "http://news.google.com"; // "http://csharpindepth.com";
        const string _defaultPathToBigFile = @"C:\Users\imnyex\source_git\WorkingProjects\FileManagers\FileXmlSerializer\FileItems.cs"; // @"C:\Users\imnyex\Downloads\Dev\MS-SQL\2014SqlServerExpress\SQLEXPRADV_x64_ENU.exe";

        private string _urlInputStr = _defaultUrlInputStr;
        public string UrlInputStr
        {
            get { return _urlInputStr; }
            set
            {
                if (_urlInputStr != value)
                {
                    _urlInputStr = value;
                    NotifyPropertyChanged("UrlInputStr");
                }
                HttpTextAsyncExecute();
            }
        }
        private string _filePathInputStr = _defaultPathToBigFile;
        public string FilePathInputStr
        {
            get { return _filePathInputStr; }
            set
            {
                if (_filePathInputStr != value)
                {
                    _filePathInputStr = value;
                    NotifyPropertyChanged("FilePathInputStr");
                }
                CalculateHashExecuteNoAync();
            }
        }

        public LongOperationsViewModel()
        {
            StatusViewModel = new StatusViewModel();
        }

        private async Task<int> LongOperation()
        {
            StatusViewModel.AddLogMessage("LongOperation() entered.");

            StatusViewModel.AddLogMessage("Starting the long (3 second) process in LongOperation()...");
            await Task.Delay(4000);
            StatusViewModel.AddLogMessage("Completed the long (3 second) process in LongOperation()...");
            return 42;
        }

        #region Commands
        #region With or Without Await Examples
        /* https://www.codeproject.com/Articles/1229574/Addressing-a-Simple-Yet-Common-Csharp-Async-Await
        *
        * Per Sacha Barber:
        * Calling .Wait() is blocking so that is certainly influencing the printed statements
            Not awaiting a method that returns a Task is just plain nuts and will result in unpredictable behaviour as your demo shows, just don't do it
            What you should be doing is creating another task using task.run and then using await Task.All where you pass it an array task to wait for

            Not using awaits for things that return Task is not good
        * 
        * 
        */

        private async Task WithAwaitAtCallAsync()
        {
            StatusViewModel.AddLogMessage("WithAwaitAtCallAsync() entered.");

            StatusViewModel.AddLogMessage("Awaiting when I call LongOperation().");
            int res = await LongOperation();
            StatusViewModel.AddLogMessage($"res = {res}");
            StatusViewModel.AddLogMessage("Pretending to do other work in WithAwaitAtCallAsync().");
        }

        private async Task WithoutAwaitAtCallAsync()
        {
            StatusViewModel.AddLogMessage("WithoutAwaitAtCallAsync() entered.");

            StatusViewModel.AddLogMessage("Call made to LongOperation() with NO await.");
            Task<int> task = LongOperation();

            StatusViewModel.AddLogMessage("Do some other work in WithoutAwaitAtCallAsync() after calling LongOperation().");
            int res = await task;
            StatusViewModel.AddLogMessage($"res = {res}");
        }

        void RunTestOneExecute()
        {
            StatusViewModel.AddLogMessage("Demo 1: Awaiting call to long operation:");
            Task withAwaitAtCallTask = WithAwaitAtCallAsync();
            Task.Run(() => WithAwaitAtCallAsync());
        }

        bool CanRunTestOneExecute()
        {
            return true;
        }

        public ICommand RunTestOne
        {
            get { return new RelayCommand(RunTestOneExecute, CanRunTestOneExecute); }
        }

        void RunTestTwoExecute() // CAUSES THE CLIENT TO HANG
        {
            StatusViewModel.AddLogMessage("Demo 2: NOT awaiting call to long operation:");
            Task withoutAwaitAtCallTask = WithoutAwaitAtCallAsync();
            withoutAwaitAtCallTask.Wait();
        }

        bool CanRunTestTwoExecute()
        {
            return true;
        }

        public ICommand RunTestTwo
        {
            get { return new RelayCommand(RunTestTwoExecute, CanRunTestTwoExecute); }
        }
        #endregion

        #region Async in C#
        void UseExampleValueExecute()
        {
            UrlInputStr = _defaultUrlInputStr;
        }

        bool CanUseExampleValueExecute()
        {
            return true;
        }

        public ICommand UseExampleValue
        {
            get { return new RelayCommand(UseExampleValueExecute, CanUseExampleValueExecute); }
        }

        private void GetHttpTextNoAsync(string url)
        {
            //string httpText = String.Empty;
            Task<string> task = new HttpClient().GetStringAsync(url);
            task.ContinueWith(_ =>
            {
                StatusViewModel.AddLogMessage("Done!");
                StatusViewModel.AddLogMessage($"{task.Result.Length} chars in {url}");
            });
        }

        void HttpTextAsyncExecute()
        {
            StatusViewModel.AddLogMessage("HttpTextAsync");

            string url = String.IsNullOrWhiteSpace(UrlInputStr) ? _defaultUrlInputStr : UrlInputStr;
            StatusViewModel.AddLogMessage("Fetching HTML from " + url);
            GetHttpTextNoAsync(url);
        }

        bool CanHttpTextAsyncExecute()
        {
            return true;
        }

        public ICommand HttpTextAsync
        {
            get { return new RelayCommand(HttpTextAsyncExecute, CanHttpTextAsyncExecute); }
        }
        #endregion

        #region Analysis
        // https://www.codeproject.com/Articles/660482/Insides-Of-Async-Await
        async void AnalysisExecute()
        {
            ClearLogMessagesExecute();
            StatusViewModel.AddLogMessage("Please wait while analysing the population.");
            try
            {
                int result = await new AnalysisEngine().AnalyzePopulationAsync();
                StatusViewModel.AddLogMessage(result.ToString());
            }
            catch (Exception ex)
            {
                StatusViewModel.AddLogMessage($"Analyze Fail:{Environment.NewLine}{ex.Message}");
            }
        }

        bool CanAnalysisExecute()
        {
            return true;
        }

        public ICommand Analysis
        {
            get { return new RelayCommand(AnalysisExecute, CanAnalysisExecute); }
        }

        static bool VerifyFilePath(string filePath)
        {
            return !String.IsNullOrWhiteSpace(filePath) && File.Exists(filePath);
        }

        void CalculateHashExecuteNoAync()
        {
            ClearLogMessagesExecute();
            if (!VerifyFilePath(FilePathInputStr))
            {
                StatusViewModel.AddLogMessage($"Invalid or empty path: '{FilePathInputStr}'");
                return;
            }
            StatusViewModel.AddLogMessage($"Please wait while calculating hashcode (no async) for '{FilePathInputStr}'");
            Task<string> task = new AnalysisEngine().GetChecksumAsync(FilePathInputStr);
            task.ContinueWith(_ =>
                {
                    StatusViewModel.AddLogMessage("Done!");
                    StatusViewModel.AddLogMessage($"{task.Result}");
                });
        }

        async void CalculateHashExecuteAync()
        {
            ClearLogMessagesExecute();
            if (!VerifyFilePath(FilePathInputStr))
            {
                StatusViewModel.AddLogMessage($"Invalid or empty path: '{FilePathInputStr}'");
                return;
            }
            StatusViewModel.AddLogMessage($"Please wait while calculating hashcode (async) for '{FilePathInputStr}'");
            try
            {
                var analysisEngine = new AnalysisEngine { PathToFile = FilePathInputStr };
                string result = await analysisEngine.GetChecksumPartTwoAsync();
                StatusViewModel.AddLogMessage(result);
            }
            catch (Exception ex)
            {
                StatusViewModel.AddLogMessage($"Calculate Hashcode Fail:{Environment.NewLine}{ex.Message}");
            }
        }

        bool CanCalculateHashExecute()
        {
            return !String.IsNullOrWhiteSpace(FilePathInputStr);
        }

        public ICommand CalculateHash
        {
            get { return new RelayCommand(CalculateHashExecuteAync, CanCalculateHashExecute); }
        }

        void CalculateHashSyncExecute()
        {
            AnalysisEngine analysisEngine = new AnalysisEngine { PathToFile = FilePathInputStr };
            //StatusViewModel.AddLogMessage($"The Checksum for {FilePathInputStr} is {analysisEngine.GetChecksum()} (using analysisEngine.GetChecksum)");
            StatusViewModel.AddLogMessage($"The Checksum for {FilePathInputStr} is {analysisEngine.GetChecksumAsyncStopsHere()} (using analysisEngine.GetChecksumAsyncStopsHere)");            
        }

        bool CanCalculateHashSyncExecute()
        {
            return true;
        }

        public ICommand CalculateHashSync
        {
            get { return new RelayCommand(CalculateHashSyncExecute, CanCalculateHashSyncExecute); }
        }

        void CalculateHashPartTwoExecute()
        {
            AnalysisEngine analysisEngine = new AnalysisEngine { PathToFile = FilePathInputStr };

        }

        bool CanCalculateHashPartTwo()
        {
            return true;
        }

        public ICommand CalculateHashPartTwo
        {
            get { return new RelayCommand(CalculateHashPartTwoExecute, CanCalculateHashPartTwo); }
        }

        private class AnalysisEngine
        {
            internal string PathToFile { get; set; } = String.Empty;
            internal CancellationTokenSource TokenSource { get; private set; }
            internal CancellationToken CancellationToken { get; private set; }

            private string _status;
            public string Status
            {
                get { return _status; }
                set
                {
                    if (_status != value)
                    {
                        _status = value;
                        //NotifyPropertyChanged("Status");
                    }
                }
            }

            public AnalysisEngine()
            {
                ResetSourceAndToken();
            }

            private void ResetSourceAndToken()
            {
                TokenSource = new CancellationTokenSource();
                CancellationToken = TokenSource.Token;
            }

            public Task<int> AnalyzePopulationAsync()
            {
                var task = new Task<int>(AnalyzePopulation);
                task.Start();
                return task;
            }

            public int AnalyzePopulation()
            {
                //Sleep is used to simulate the time consuming activity.
                Thread.Sleep(3000);
                return new Random().Next(1, 5000);
            }

            public string GetChecksumPartTwoWithCancel(CancellationToken cancellationToken)
            {
                byte[] buffer;
                byte[] oldBuffer;
                int bytesRead;
                int oldBytesRead;
                long size;
                long totalBytesRead = 0;
                StringBuilder sb = new StringBuilder("0x");

                using (Stream stream = File.OpenRead(PathToFile))
                {
                    using (HashAlgorithm hashAlgorithm = MD5.Create())
                    {
                        size = stream.Length;
                        buffer = new byte[4096];
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        totalBytesRead += bytesRead;

                        do
                        {
                            if (TokenSource.IsCancellationRequested)
                            {
                                ResetSourceAndToken();
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                            oldBytesRead = bytesRead;
                            oldBuffer = buffer;
                            buffer = new byte[4096];
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            totalBytesRead += bytesRead;
                            if (bytesRead == 0)
                            {
                                hashAlgorithm.TransformFinalBlock(oldBuffer, 0, oldBytesRead);
                            }
                            else
                            {
                                hashAlgorithm.TransformBlock(oldBuffer, 0, oldBytesRead, oldBuffer, 0);
                            }
                        } while (bytesRead != 0);

                        foreach (var b in hashAlgorithm.Hash)
                        {
                            sb.Append(string.Format("{0:X2}", b));
                        }
                    }
                }
                return sb.ToString();
            }

            public async Task<string> GetChecksumPartTwoWithCancelAsync(CancellationToken cancellationToken)
            {
                if (TokenSource == null || TokenSource.IsCancellationRequested)
                {
                    ResetSourceAndToken();
                }
                string checksum = await Task.Run(() => GetChecksumPartTwoWithCancel(CancellationToken));
                return checksum;
            }

            public async Task<string> GetChecksumPartTwoAsync()
            {
                string checksum = string.Empty;
                try
                {
                    Task<string> task = GetChecksumPartTwoWithCancelAsync(CancellationToken);
                    checksum = await task;
                }
                catch (OperationCanceledException cancelledException)
                {
                    Status = "Operation Cancelled";
                }
                catch (Exception ex)
                {
                    Status = "Exception in FindFactorsAsync: " + ex.Message;
                }
                return checksum;
            }

            public string GetChecksumAsyncStopsHere()
            {
                byte[] buffer;
                byte[] oldBuffer;
                int bytesRead;
                int oldBytesRead;
                long size;
                long totalBytesRead = 0;
                StringBuilder sb = new StringBuilder("0x");

                using (Stream stream = File.OpenRead(PathToFile))
                {
                    using (HashAlgorithm hashAlgorithm = MD5.Create())
                    {
                        size = stream.Length;
                        buffer = new byte[4096];
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        totalBytesRead += bytesRead;

                        do
                        {
                            oldBytesRead = bytesRead;
                            oldBuffer = buffer;
                            buffer = new byte[4096];
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            totalBytesRead += bytesRead;
                            if (bytesRead == 0)
                            {
                                hashAlgorithm.TransformFinalBlock(oldBuffer, 0, oldBytesRead);
                            }
                            else
                            {
                                hashAlgorithm.TransformBlock(oldBuffer, 0, oldBytesRead, oldBuffer, 0);
                            }
                        } while (bytesRead != 0);

                        foreach (var b in hashAlgorithm.Hash)
                        {
                            sb.Append(string.Format("{0:X2}", b));
                        }
                    }
                }
                return sb.ToString();
            }

            public Task<string> GetChecksumAsync(string fullPath)
            {
                PathToFile = fullPath;
                var task = new Task<string>(GetChecksum);
                return task;
            }

            public string GetChecksum()
            {
                byte[] checkSum;
                using (var md5 = MD5.Create())
                {
                    using (var file = File.OpenRead(PathToFile))
                    {
                        checkSum = md5.ComputeHash(file);
                    }
                }
                StringBuilder sb = new StringBuilder("0x");
                foreach (var b in checkSum)
                {
                    sb.Append(string.Format("{0:X2}", b));
                }
                return sb.ToString();
            }
        }

        #endregion

        #endregion

        /* NEW STUFF HERE */

        #region ClearLogMessages

        void ClearLogMessagesExecute()
        {
            StatusViewModel.ClearLog();
        }

        bool CanClearLogMessagesExecute()
        {
            return true;
        }

        public ICommand ClearLogMessages
        {
            get { return new RelayCommand(ClearLogMessagesExecute, CanClearLogMessagesExecute); }
        }
        #endregion
    }
}
