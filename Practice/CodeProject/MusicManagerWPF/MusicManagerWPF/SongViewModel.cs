using System.ComponentModel;
using System.Windows.Input;
using MicroMvvm;

namespace MusicManagerWPF
{
    public class SongViewModel : ObservableObject
    {
        private int mCount = 0;

        public SongViewModel()
        {
            mSong = new Song {ArtistName = "Unknown", SongTitle = "Unknown"};
        }

        private Song mSong;
        public Song Song
        {
            get { return mSong; }
            set
            {
                if (mSong != value)
                {
                    mSong = value;
                }
            }
        }

        public string ArtistName
        {
            get { return mSong.ArtistName; }
            set
            {
                if (mSong.ArtistName != value)
                {
                    mSong.ArtistName = value;
                    NotifyPropertyChanged("ArtistName");
                }
            }
        }

        public string SongTitle
        {
            get { return mSong.SongTitle; }
            set
            {
                if (mSong.SongTitle != value)
                {
                    mSong.SongTitle = value;
                    NotifyPropertyChanged("SongTitle");
                }
            }
        }

        void UpdateArtistNameExecute()
        {
            ++mCount;
            ArtistName = string.Format("Elvis ({0})", mCount);
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute);} }
    }
}
