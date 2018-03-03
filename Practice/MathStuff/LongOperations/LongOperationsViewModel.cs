using System.Threading.Tasks;
using System.Windows.Input;
using MicroMvvm;
using System.Net;
using System.Net.Http;
using System;

namespace MathStuff.LongOperations
{
    class LongOperationsViewModel : ObservableObject
    {

        public PrimeFactorization PrimeFactorization { get; set; }
        public StatusViewModel StatusViewModelLongOps { get; set; }

        public LongOperationsViewModel()
        {
            StatusViewModelLongOps = new StatusViewModel();
        }

        private async Task<int> LongOperation()
        {
            StatusViewModelLongOps.AddLogMessage("LongOperation() entered.");

            StatusViewModelLongOps.AddLogMessage("Starting the long (3 second) process in LongOperation()...");
            await Task.Delay(4000);
            StatusViewModelLongOps.AddLogMessage("Completed the long (3 second) process in LongOperation()...");
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
            StatusViewModelLongOps.AddLogMessage("WithAwaitAtCallAsync() entered.");

            StatusViewModelLongOps.AddLogMessage("Awaiting when I call LongOperation().");
            int res = await LongOperation();
            StatusViewModelLongOps.AddLogMessage($"res = {res}");
            StatusViewModelLongOps.AddLogMessage("Pretending to do other work in WithAwaitAtCallAsync().");
        }

        private async Task WithoutAwaitAtCallAsync()
        {
            StatusViewModelLongOps.AddLogMessage("WithoutAwaitAtCallAsync() entered.");

            StatusViewModelLongOps.AddLogMessage("Call made to LongOperation() with NO await.");
            Task<int> task = LongOperation();

            StatusViewModelLongOps.AddLogMessage("Do some other work in WithoutAwaitAtCallAsync() after calling LongOperation().");
            int res = await task;
            StatusViewModelLongOps.AddLogMessage($"res = {res}");
        }

        void RunTestOneExecute()
        {
            StatusViewModelLongOps.AddLogMessage("Demo 1: Awaiting call to long operation:");
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
            StatusViewModelLongOps.AddLogMessage("Demo 2: NOT awaiting call to long operation:");
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
        void Test3Execute()
        {
            ClearLogMessagesExecute();
            StatusViewModelLongOps.AddLogMessage("Fetching...");

            string url = "http://csharpindepth.com";
            string url2 = "http://msn.com";
            GetHttpTextNoAsync(url2);
            StatusViewModelLongOps.AddLogMessage("Done!");
            //IPAddress[] ipAddresses = null;
            //Task<IPAddress[]> ipAddressesPromise = Dns.GetHostAddressesAsync("oreilly.com");
            //ipAddressesPromise.ContinueWith(_ =>
            //{
            //    ipAddresses = ipAddressesPromise.Result;
            //});
            //if (ipAddresses == null)
            //{
            //    StatusViewModelLongOps.AddLogMessage("ipAddresses is null");
            //    return;
            //}
            //foreach (var el in ipAddresses)
            //{
            //    StatusViewModelLongOps.AddLogMessage(el.ToString());
            //}
        }

        private void GetHttpTextNoAsync(string url)
        {
            //string httpText = String.Empty;
            Task<string> task = new HttpClient().GetStringAsync(url);
            task.ContinueWith(_ =>
            {
                StatusViewModelLongOps.AddLogMessage(String.IsNullOrWhiteSpace(task.Result) ? "No HTTP Text" : task.Result);
            });
        }
        
        bool CanTest3Execute()
        {
            return true;
        }

        public ICommand Test3
        {
            get { return new RelayCommand(Test3Execute, CanTest3Execute); }
        }

        async void Test4Execute()
        {
            StatusViewModelLongOps.AddLogMessage("Fetching...");

            string url = "http://csharpindepth.com";
            string url2 = "http://msn.com";
            await GetHttpText(url2);
            StatusViewModelLongOps.AddLogMessage("Done!");
            //IPAddress[] ipAddresses = null;
            //Task<IPAddress[]> ipAddressesPromise = Dns.GetHostAddressesAsync("oreilly.com");
            //ipAddressesPromise.ContinueWith(_ =>
            //{
            //    ipAddresses = ipAddressesPromise.Result;
            //});
            //if (ipAddresses == null)
            //{
            //    StatusViewModelLongOps.AddLogMessage("ipAddresses is null");
            //    return;
            //}
            //foreach (var el in ipAddresses)
            //{
            //    StatusViewModelLongOps.AddLogMessage(el.ToString());
            //}
        }

        private async Task GetHttpText(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> task = client.GetStringAsync(url);
                StatusViewModelLongOps.AddLogMessage(await task);
            }
        }

        bool CanTest4Execute()
        {
            return true;
        }

        public ICommand Test4
        {
            get { return new RelayCommand(Test4Execute, CanTest4Execute); }
        }

        #endregion

        #region ClearLogMessages

        void ClearLogMessagesExecute()
        {
            StatusViewModelLongOps.ClearLog();
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
        #endregion
    }
}
