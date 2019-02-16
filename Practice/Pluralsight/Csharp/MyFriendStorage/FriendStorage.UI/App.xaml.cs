using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FriendStorage.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var navigationViewModel = new NavigationViewModel(new NavigationDataProvider(() => new FileDataService()));
            var mainViewModel = new MainViewModel(navigationViewModel);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
