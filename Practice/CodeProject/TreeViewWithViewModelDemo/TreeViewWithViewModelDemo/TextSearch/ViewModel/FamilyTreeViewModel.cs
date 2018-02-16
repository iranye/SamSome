using BusinessLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TreeViewWithViewModelDemo.TextSearch
{
    public class FamilyTreeViewModel
    {
        IEnumerator<PersonViewModel> _matchingPeopleEnumerator;
        readonly PersonViewModel _rootPerson;

        readonly ReadOnlyCollection<PersonViewModel> _firstGeneration;
        public ReadOnlyCollection<PersonViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
        }
        
        readonly ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand; }
        }

        string _searchText = String.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    _matchingPeopleEnumerator = null;
                }
            }
        }

        public FamilyTreeViewModel(Person rootPerson)
        {
            _rootPerson = new PersonViewModel(rootPerson);
            _firstGeneration = new ReadOnlyCollection<PersonViewModel>(
                new PersonViewModel[]
                {
                    _rootPerson
                });

            _searchCommand = new SearchFamilyTreeCommand(this);
        }

        private void PerformSearch()
        {
            if (_matchingPeopleEnumerator == null || !_matchingPeopleEnumerator.MoveNext())
                VerifyMatchingPeopleEnumerator();

            var person = _matchingPeopleEnumerator.Current;

            if (person == null)
                return;

            if (person.Parent != null)
                person.Parent.IsExpanded = true;

            person.IsSelected = true;
        }

        private void VerifyMatchingPeopleEnumerator()
        {
            var matches = FindMatches(_searchText, _rootPerson);
            _matchingPeopleEnumerator = matches.GetEnumerator();

            if (!_matchingPeopleEnumerator.MoveNext())
            {
                MessageBox.Show(
                    "No matching names found.",
                    "Try Again",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private IEnumerable<PersonViewModel> FindMatches(string searchText, PersonViewModel person)
        {
            if (person.NameContainsText(searchText))
                yield return person;

            foreach (PersonViewModel child in person.Children)
                foreach (PersonViewModel match in FindMatches(searchText, child))
                    yield return match;
        }

        private class SearchFamilyTreeCommand : ICommand
        {
            readonly FamilyTreeViewModel _familyTree;

            public SearchFamilyTreeCommand(FamilyTreeViewModel familyTreeViewModel)
            {
                this._familyTree = familyTreeViewModel;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                // I intentionally left these empty because
                // this command never raises the event, and
                // not using the WeakEvent pattern here can
                // cause memory leaks.  WeakEvent pattern is
                // not simple to implement, so why bother.
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _familyTree.PerformSearch();
            }
        }
    }
}
