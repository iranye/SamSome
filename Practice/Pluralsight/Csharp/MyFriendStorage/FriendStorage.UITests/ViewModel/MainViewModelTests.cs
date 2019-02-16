using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class MainViewModelTests
    {
       [Fact]
       public void ShouldCallLoadMethodOfNavigationViewModel()
        {
            NavigationViewModelMock navigationViewModel = new NavigationViewModelMock();
            var viewModel = new MainViewModel(navigationViewModel);

            viewModel.Load();
            Assert.True(navigationViewModel.LoadHasBeenCalled);
        }
    }

    public class NavigationViewModelMock : INavigationViewModel
    {
        public bool LoadHasBeenCalled { get; set; }
        public void Load()
        {
            LoadHasBeenCalled = true;
        }
    }
}
