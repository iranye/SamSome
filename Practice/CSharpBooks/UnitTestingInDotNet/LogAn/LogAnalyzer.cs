using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class LogAnalyzer
    {
        public bool WasLastFileNameValid { get; private set; }

        public bool IsValidLogFileName(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("fileName");
            }
            bool ret = fileName.ToLower().EndsWith(".slf");
            WasLastFileNameValid = ret;
            
            return ret;
        }
    }
}
