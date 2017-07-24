using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MusicManager
{
    public class SongViewModel : INotifyPropertyChanged
    {
        public Song Song { get; set; }
        private int mCount = 0;

        public string ArtistName
        {
            get { return Song.ArtistName; }
            set
            {
                if (Song.ArtistName != value)
                {
                    Song.ArtistName = value;
                    OnPropertyChanged("ArtistName");
                }
            }
        }

        public SongViewModel()
        {
            Song = new Song {ArtistName = "Unknown", SongTitle = "Unknown"};
        }

        public string SongTitle
        {
            get { return Song.SongTitle; }
            set
            {
                if (Song.SongTitle != value)
                {
                    Song.SongTitle = value;
                    OnPropertyChanged("SongTitle");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void UpdateArtistNameExecute()
        {
            ArtistName = "Elvis" + mCount++;
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName
        {
            get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute); }
        }
    }
}
