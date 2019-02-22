using FriendStorage.UI.ViewModel;
using Moq;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class MainViewModelTests
    {
       [Fact]
       public void ShouldCallLoadMethodOfNavigationViewModel()
        {
            var navigationViewModelMock = new Mock<INavigationViewModel>();
            var viewModel = new MainViewModel(navigationViewModelMock.Object);
            viewModel.Load();
            navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }
    }
}
