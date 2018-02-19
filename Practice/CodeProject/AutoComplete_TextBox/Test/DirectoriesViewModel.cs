using MicroMvvm;

namespace Test
{
    class DirectoriesViewModel : ObservableObject
    {
        private string _blurb;
        public string Blurb
        {
            get { return _blurb; }
            set
            {
                if (_blurb != value)
                {
                    _blurb = value;
                    NotifyPropertyChanged("Blurb");
                }
            }
        }
    }
}
