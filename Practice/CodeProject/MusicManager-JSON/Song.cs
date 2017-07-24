namespace MusicManager
{
    public class Song
    {
        public string ArtistName { get; set; }
        public string SongTitle { get; set; }

        public Song Copy()
        {
            return new Song {ArtistName = ArtistName, SongTitle = SongTitle};
        }
    }
}
