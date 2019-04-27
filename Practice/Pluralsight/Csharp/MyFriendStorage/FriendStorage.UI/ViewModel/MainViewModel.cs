using System;
using System.Collections.ObjectModel;
using System.Linq;
using FriendStorage.UI.Events;
using Prism.Events;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; private set; }
        public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; private set; }

        private Func<IFriendEditViewModel> _friendEditVmCreator;

        private IFriendEditViewModel _selectedFriendEditViewModel;
        public IFriendEditViewModel SelectedFriendEditViewModel
        {
            get { return _selectedFriendEditViewModel; }
            set
            {
                _selectedFriendEditViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IFriendEditViewModel> friendEditVmCreator, IEventAggregator eventAggregator)
        {
            NavigationViewModel = navigationViewModel;
            FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();
            _friendEditVmCreator = friendEditVmCreator;
            eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
        }

        private void OnOpenFriendEditView(int friendId)
        {
            var friendEditVm = FriendEditViewModels.SingleOrDefault(vm => vm.Friend.Id == friendId);
            if (friendEditVm == null)
            {
                friendEditVm = _friendEditVmCreator();
                FriendEditViewModels.Add(friendEditVm);
                friendEditVm.Load(friendId);
            }
            SelectedFriendEditViewModel = friendEditVm;
        }

        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
