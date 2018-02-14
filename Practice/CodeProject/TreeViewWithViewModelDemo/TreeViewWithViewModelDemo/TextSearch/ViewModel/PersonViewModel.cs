using System;
using System.ComponentModel;
using System.Linq;
using BusinessLib;
using System.Collections.ObjectModel;

namespace TreeViewWithViewModelDemo.TextSearch
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        readonly Person _person;
        public string Name
        {
            get { return _person.Name; }
        }

        readonly PersonViewModel _parent;
        public PersonViewModel Parent
        {
            get { return _parent; }
        }

        readonly ReadOnlyCollection<PersonViewModel> _children;
        public ReadOnlyCollection<PersonViewModel> Children
        {
            get { return _children; }
        }

        bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public PersonViewModel(Person person)
            : this(person, null)
        { }

        private PersonViewModel(Person person, PersonViewModel parent)
        {
            _person = person;
            _parent = parent;

            _children = new ReadOnlyCollection<PersonViewModel>(
                (from child in _person.Children
                 select new PersonViewModel(child, this))
                 .ToList<PersonViewModel>());
        }

        internal bool NameContainsText(string searchText)
        {
            if (String.IsNullOrWhiteSpace(searchText) || String.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return Name.IndexOf(searchText, StringComparison.InvariantCulture) > -1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
