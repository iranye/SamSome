using FriendStorage.UI.Command;
using System;
using System.Windows.Input;
using Prism.Events;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationItemViewModel
    {
        public string DisplayMember { get; set; }
        public int Id { get; set; }
        public ICommand OpenFriendEditViewCommand { get; private set; }

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenFriendEditViewCommand = new DelegateCommand(OnFriendEditViewExecute);
        }

        private void OnFriendEditViewExecute(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
