using System;
using System.Numerics;
using System.Reflection;
using System.Windows.Input;
using MicroMvvm;

namespace MathStuff
{
    class MathViewModel
    {
        public PrimeFactorization PrimeFactorization { get; set; }
        public StatusViewModel StatusViewModel { get; set; }

        public MathViewModel()
        {
            StatusViewModel = new StatusViewModel();
            PrimeFactorization = new PrimeFactorization();

            PrimeFactorization.PropertyChanged += PrimeFactorization_PropertyChanged;
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Version = version.ToString();
        }

        private void PrimeFactorization_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":
                    StatusViewModel.AddLogMessage(PrimeFactorization.Status);
                    break;
                case "PrimeFactors":
                    StatusViewModel.AddLogMessage(PrimeFactorization.PrimeFactors);
                    break;
            }
        }

        void GetPrimeFactorizationExecute()
        {
            StatusViewModel.AddLogMessage($"Getting Prime factorization for {PrimeFactorization.Input}");
            PrimeFactorization.FindFactors();
        }

        bool CanGetPrimeFactorizationExecute()
        {
            return PrimeFactorization.Input > 0;
        }

        public ICommand GetPrimeFactorization
        {
            get { return new RelayCommand(GetPrimeFactorizationExecute, CanGetPrimeFactorizationExecute); }
        }

        #region UseMaxValue
        void UseMaxValueExecute()
        {
            BigInteger maxValue = UInt64.MaxValue;
            StatusViewModel.AddLogMessage($"Use Maximum Value for Prime factorization: {maxValue}");
            PrimeFactorization.InputStr = maxValue.ToString();
        }

        bool CanUseMaxValueExecute()
        {
            return true;
        }

        public ICommand UseMaxValue
        {
            get { return new RelayCommand(UseMaxValueExecute, CanUseMaxValueExecute); }
        } 
        #endregion

        void RunTestExecute()
        {
            // https://msdn.microsoft.com/en-us/library/system.numerics.biginteger.max(v=vs.110).aspx
            var format = "{0,0:N0}";
            BigInteger[] numbers = { Int64.MaxValue * BigInteger.MinusOne,
                               BigInteger.MinusOne,
                               10359321239000,
                               BigInteger.Pow(103988, 2),
                               BigInteger.Multiply(Int32.MaxValue, Int16.MaxValue),
                               BigInteger.Add(BigInteger.Pow(Int64.MaxValue, 2),
                               BigInteger.Pow(Int32.MaxValue, 2)) };
            if (numbers.Length < 2)
            {
                StatusViewModel.AddLogMessage($"Cannot determine which is the larger of {numbers.Length} numbers.");
                return;
            }
            BigInteger largest = numbers[numbers.GetLowerBound(0)];

            for (int ctr = numbers.GetLowerBound(0) + 1; ctr <= numbers.GetUpperBound(0); ctr++)
            {
                largest = BigInteger.Max(largest, numbers[ctr]);
            }
            StatusViewModel.AddLogMessage("The values:");
            foreach (BigInteger number in numbers)
            {
                StatusViewModel.AddLogMessage(string.Format(format, number));
            }
            StatusViewModel.AddLogMessage("The largest number is: " + string.Format(format, largest));
        }

        bool CanRunTestExecute()
        {
            return true;
        }

        public ICommand RunTest
        {
            get { return new RelayCommand(RunTestExecute, CanRunTestExecute);}
        }

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

        public string Version { get; set; }
    }
}
