using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendStorage.UI.Events;
using Prism.Commands;
using Prism.Events;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationItemViewModel
    {
        private IEventAggregator _eventAggregator;
        public int Id { get; private set; }
        public string DisplayMember { get; private set; }

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenFriendEditViewCommand = new DelegateCommand(OnFriendEditViewExecute);
            _eventAggregator = eventAggregator;
        }

        private void OnFriendEditViewExecute()
        {
            _eventAggregator.GetEvent<OpenFriendEditViewEvent>().Publish(Id);
        }

        public ICommand OpenFriendEditViewCommand { get; private set; }
    }
}
