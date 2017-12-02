using System;
using System.Collections.Generic;
using System.Numerics;
using MicroMvvm;

namespace MathStuff
{
    class PrimeFactorization : ObservableObject
    {
        private String mStatus = String.Empty;
        public String Status
        {
            get { return mStatus; }
            set
            {
                if (mStatus != value)
                {
                    mStatus = value;
                    NotifyPropertyChanged("Status");
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
                        res += $"{p}, ";
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
