using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using System;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; private set; }

        public MainViewModel(INavigationViewModel navigationViewModel)
        {
            NavigationViewModel = navigationViewModel;
            //NavigationViewModel = new NavigationViewModel(new NavigationDataProvider(() => new FileDataService()));

        }
        
        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
