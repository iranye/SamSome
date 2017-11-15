using System;
using System.Collections.Generic;
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

        private List<UInt64> mPrimeFactors = new List<UInt64>();
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

        private UInt64 mInput;
        public UInt64 Input
        {
            get { return mInput; }
            set
            {
                if (mInput != value)
                {
                    if (value < 0 || value > UInt64.MaxValue)
                    {
                        Status = $"Invalid input: '{value}'";
                    }
                    else
                    {
                        mInput = value;
                        NotifyPropertyChanged("Input");
                    }
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
            UInt64 factor = 3;
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
