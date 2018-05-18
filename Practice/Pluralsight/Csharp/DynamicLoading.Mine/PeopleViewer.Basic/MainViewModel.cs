using PersonRepository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleViewer.Basic
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IPersonRepository _repository;

        private IEnumerable<Person> _people;

        public IEnumerable<Person> People
        {
            get { return _people; }
            set
            {
                if (_people != value)
                {
                    _people = value;
                    RaisePropertyChanged("People");
                }
            }
        }

        public string RepositoryType { get { return _repository.GetType().ToString(); } }

        public MainViewModel()
        {
            _repository = RepositoryFactory.GetRepository();
        }

        public void FetchData()
        {
            People = _repository.GetPeople();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ClearData()
        {
            People = new List<Person>();
        }
        #endregion
    }
}
