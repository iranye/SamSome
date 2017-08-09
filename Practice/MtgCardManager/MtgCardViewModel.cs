using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MtgCardManager
{
    class MtgCardViewModel : INotifyPropertyChanged
    {
        private MtgCard mMtgCard;
        private int mCount;
        
        public string CardName
        {
            get
            {
                return mMtgCard.Name;
            }
            set
            {
                if (mMtgCard.Name != value)
                {
                    mMtgCard.Name = value;
                    RaisePropertyChanged("CardName");
                }
            }
        }

        public string ArtistName
        {
            get
            {
                return mMtgCard.ArtistName;
            }
            set
            {
                if (mMtgCard.ArtistName != value)
                {
                    mMtgCard.ArtistName = value;
                    RaisePropertyChanged("ArtistName");
                }
            }
        }

        public MtgCardViewModel()
        {
            mCount = 0;
            mMtgCard = new MtgCard { Name = "Unknown Card", ArtistName = "Unknown Artist" };
            //mMtgCard = new MtgCard { Name = "Cranial Archive", ArtistName = "Volkan Baga" };
        }


        void UpdateArtistNameExecute()
        {
            ArtistName = string.Format("Elvis {0}", mCount++);
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute); } }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
