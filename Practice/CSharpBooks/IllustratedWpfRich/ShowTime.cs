using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace IllustratedWpfRich
{
    class ShowTime : MarkupExtension
    {
        private string mHeader = string.Empty;
        public string Header
        {
            get { return mHeader; }
            set { mHeader = value; }
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return $"{Header}: {DateTime.Now.ToLongDateString()}";
        }

        public ShowTime()
        {
        }

        public ShowTime(string input)
        {
            Header = input;
        }
    }
}
