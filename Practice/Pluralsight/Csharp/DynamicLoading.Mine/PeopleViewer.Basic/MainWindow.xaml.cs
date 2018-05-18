using System.Windows;

namespace PeopleViewer.Basic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FetchData();
            ShowRepositoryType();
        }
        
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ClearData();
        }

        private void ShowRepositoryType()
        {
            MessageBox.Show(string.Format("Repository Type:\n{0}",
                _viewModel.RepositoryType));
        }
    }
}
