using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicManager
{
    public class SongViewModel : ObservableObject
    {
        int _count = 0;

        public Song Song { get; set; }
        public string ArtistName
        {
            get{return Song.ArtistName;}
            set
            {
                if (Song.ArtistName != value)
                {
                    Song.ArtistName = value;
                    RaisePropertyChanged("ArtistName");
                }
            }
        }

        public SongViewModel()
        {
            Song = new Song { ArtistName = "Unknown", SongTitle = "Unknown" };
        }

        void UpdateArtistNameExecute()
        {
            ArtistName = string.Format("Elvis ({0})", ++_count);
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute); } }
    }
}
