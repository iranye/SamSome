using System;
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

        void UseMaxValueExecute()
        {
            UInt64 maxValue = UInt64.MaxValue;
            StatusViewModel.AddLogMessage($"Use Maximum Value for Prime factorization: {maxValue}");
            PrimeFactorization.Input = maxValue;
        }

        bool CanUseMaxValueExecute()
        {
            return true;
        }

        public ICommand UseMaxValue
        {
            get { return new RelayCommand(UseMaxValueExecute, CanUseMaxValueExecute); }
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
