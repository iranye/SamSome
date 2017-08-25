using System.ComponentModel;

namespace ControlsInWpf
{
    public class SalesPeriod
    {
        private int month, year;

        public int Year
        {
            get { return year; }
            set
            {
                if (year != value)
                {
                    year = value;
//                    NotifyPropertyChanged("Year");
                }
            }
        }

        public int Month
        {
            get { return month; }
            set
            {
                if (month != value)
                {
                    month = value;
//                    NotifyPropertyChanged("Month");
                }
            }
        }

        public override string ToString()
        {
            return $"{Month:D2}.{Year}";
        }

        //public virtual event PropertyChangedEventHandler PropertyChanged;
        //protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        foreach (string propertyName in propertyNames)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //        }
        //        PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
        //    }
        //}
    }
}
