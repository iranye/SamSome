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

        private String _status = String.Empty;
        public String Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        private List<BigInteger> _primeFactors = new List<BigInteger>();
        public String PrimeFactors
        {
            get
            {
                string res = String.Empty;
                if (_primeFactors.Count > 0)
                {
                    foreach (var p in _primeFactors)
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
            get { return _input.ToString(); }
            set
            {
                if (_input.ToString() != value)
                {
                    string toParse = value.Replace(",", "").Trim();
                    if (!BigInteger.TryParse(toParse, out _input))
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

        private BigInteger _input;
        public BigInteger Input
        {
            get { return _input; }
            set
            {
                if (_input != value)
                {
                    _input = value;
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
            _primeFactors.Clear();
            var factored = _input;
            while (factored % 2 == 0)
            {
                _primeFactors.Add(2);
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
                    _primeFactors.Add(factor);
                    NotifyPropertyChanged("PrimeFactors");
                    factored /= factor;
                }
                else
                {
                    factor += 2;
                }
            }
            if (factored > 1) _primeFactors.Add(factored);
            Status = "... DONE";
            if (_primeFactors.Count == 1)
            {
                Status = $"{_input,0:N0} IS PRIME!{Environment.NewLine}";
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
            _primeFactors.Clear();
            while (_input % 2 == 0)
            {
                _primeFactors.Add(2);
                NotifyPropertyChanged("PrimeFactors");
                _input /= 2;
            }
            BigInteger factor = 3;
            while (factor * factor <= _input)
            {
                if (_input % factor == 0)
                {
                    _primeFactors.Add(factor);
                    NotifyPropertyChanged("PrimeFactors");
                    _input /= factor;
                }
                else
                {
                    factor += 2;
                }

            }
            if (_input > 1) _primeFactors.Add(_input);
            NotifyPropertyChanged("PrimeFactors");
        }
    }
}
