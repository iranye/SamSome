using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPractice
{
    // see: https://msdn.microsoft.com/en-us/library/aa288464(v=vs.71).aspx
    public class Document
    {
        public class WordCollection
        {
            readonly Document document;

            internal WordCollection(Document d)
            {
                document = d;
            }

            private bool GetWord(char[] text, int begin, int wordCount, out int start, out int length)
            {
                int end = text.Length;
                int count = 0;
                int inWord = -1;
                start = length = 0;

                for (int i = begin; i <= end; ++i)
                {
                    bool isLetter = i < end && Char.IsLetterOrDigit(text[i]);
                    if (inWord >= 0)
                    {
                        if (!isLetter)
                        {
                            if (count++ == wordCount)
                            {
                                start = inWord;
                                length = i - inWord;
                                return true;
                            }
                            inWord = -1;
                        }
                    }
                }
                    return true;
            }
        }
    }
}
