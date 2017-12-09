using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using MicroMvvm;

namespace MathStuff
{
    class PrimeFactorization : ObservableObject
    {
        public PrimeFactorization()
        {
            ResetSourceAndToken();
        }

        private String mStatus = String.Empty;
        public String Status
        {
            get { return mStatus; }
            set
            {
                mStatus = value;
                NotifyPropertyChanged("Status");
            }
        }

        private int mProgress;
        public int Progress
        {
            get { return mProgress; }
            set
            {
                if (mProgress != value)
                {
                    mProgress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        private List<BigInteger> mPrimeFactors = new List<BigInteger>();
        public String PrimeFactors
        {
            get
            {
                string res = String.Empty;
                if (mPrimeFactors.Count > 0)
                {
                    foreach (var p in mPrimeFactors)
                    {
                        res += $"{p,0:N0} • ";
                    }
                }
                int extraCharactersLen = 2;
                return res.Substring(0, res.Length - extraCharactersLen) + Environment.NewLine;
            }
        }

        public string InputStr
        {
            get { return mInput.ToString(); }
            set
            {
                if (mInput.ToString() != value)
                {
                    string toParse = value.Replace(",", "").Trim();
                    if (!BigInteger.TryParse(toParse, out mInput))
                    {
                        Status = $"Failed to parse '{value}'";
                    }
                    else
                    {
                        NotifyPropertyChanged("InputStr");
                    }
                }
            }
        }
        private BigInteger mInput;
        public BigInteger Input
        {
            get { return mInput; }
            set
            {
                if (mInput != value)
                {
                    mInput = value;
                    NotifyPropertyChanged("Input");
                }
            }
        }
        
        public CancellationTokenSource TokenSource { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        private void ResetSourceAndToken()
        {
            TokenSource = new CancellationTokenSource();
            CancellationToken = TokenSource.Token;
        }

        public int FindFactorsWithCancel(CancellationToken cancellationToken)
        {
            mPrimeFactors.Clear();
            var factored = mInput;
            while (factored % 2 == 0)
            {
                mPrimeFactors.Add(2);
                NotifyPropertyChanged("PrimeFactors");
                factored /= 2;
            }
            BigInteger factor = 3;
            int numberOfLoopIterations = 0;
            int progressUpateInterval = 1000000;
            while (factor * factor <= factored)
            {
                if (TokenSource.IsCancellationRequested)
                {
                    ResetSourceAndToken();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                if (numberOfLoopIterations > 0 && numberOfLoopIterations % progressUpateInterval == 0)
                {
                    Progress++;
                }
                numberOfLoopIterations++;
                if (factored % factor == 0)
                {
                    mPrimeFactors.Add(factor);
                    NotifyPropertyChanged("PrimeFactors");
                    factored /= factor;
                }
                else
                {
                    factor += 2;
                }
            }
            if (factored > 1) mPrimeFactors.Add(factored);
            Status = "... DONE";
            if (mPrimeFactors.Count == 1)
            {
                Status = $"{mInput,0:N0} IS PRIME!{Environment.NewLine}";
            }
            else
            {
                NotifyPropertyChanged("PrimeFactors");
            }
            return numberOfLoopIterations;
        }

        public async Task<int> FindFactorsWithCancelAsync(CancellationToken cancellationToken)
        {
            if (TokenSource == null || TokenSource.IsCancellationRequested)
            {
                ResetSourceAndToken();
            }
            int iterationCount = await Task.Run(() => FindFactorsWithCancel(cancellationToken));
            return iterationCount;
        }

        public async void FindFactorsAsync()
        {
            try
            {
                await FindFactorsWithCancelAsync(CancellationToken);
            }
            catch (OperationCanceledException cancelledException)
            {
                Status = "Operation Cancelled";
            }
            catch (Exception ex)
            {
                Status = "Exception in FindFactorsAsync: " + ex.Message;
            }
        }

        public void FindFactors()
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/async
            mPrimeFactors.Clear();
            while (mInput % 2 == 0)
            {
                mPrimeFactors.Add(2);
                NotifyPropertyChanged("PrimeFactors");
                mInput /= 2;
            }
            BigInteger factor = 3;
            while (factor * factor <= mInput)
            {
                if (mInput % factor == 0)
                {
                    mPrimeFactors.Add(factor);
                    NotifyPropertyChanged("PrimeFactors");
                    mInput /= factor;
                }
                else
                {
                    factor += 2;
                }

            }
            if (mInput > 1) mPrimeFactors.Add(mInput);
            NotifyPropertyChanged("PrimeFactors");
        }
    }
}
