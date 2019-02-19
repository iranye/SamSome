namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationViewModel NavigationViewModel { get; private set; }

        public MainViewModel(INavigationViewModel navigationViewModel)
        {
            NavigationViewModel = navigationViewModel;
        }
        
        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
