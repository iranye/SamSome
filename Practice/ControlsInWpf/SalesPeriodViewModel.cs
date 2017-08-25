using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlsInWpf
{
    public class SalesPeriodViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SalesPeriod> mSalesPeriods = new ObservableCollection<SalesPeriod>();
        public ObservableCollection<SalesPeriod> SalesPeriods
        {
            get { return mSalesPeriods; }
            set
            {
                mSalesPeriods = value;
                NotifyPropertyChanged("SalesPeriods");
            }
        }

        private SalesPeriod mSelectedItem = new SalesPeriod();
        public SalesPeriod SelectedItem
        {
            get { return mSelectedItem; }
            set
            {
                if (mSelectedItem != value)
                {
                    mSelectedItem = value;
                    NotifyPropertyChanged("SelectedItem");
                    NotifyPropertyChanged("SelectedItemTxt");
                }
            }
        }

        public string SelectedItemTxt
        {
            get
            {
                if (mSelectedItem.Month == 0 && mSelectedItem.Year == 0)
                {
                    return "";
                }
                return SelectedItem.ToString();
            }
            set { }
        }

        public SalesPeriodViewModel()
        {
            SalesPeriods.Add(new SalesPeriod { Month = 3, Year = 2013 });
            SalesPeriods.Add(new SalesPeriod { Month = 4, Year = 2013 });
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }
    }
}
