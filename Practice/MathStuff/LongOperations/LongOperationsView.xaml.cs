using System.Windows.Controls;
using System.Windows.Input;

namespace MathStuff.LongOperations
{
    /// <summary>
    /// Interaction logic for AsyncAwait.xaml
    /// </summary>
    public partial class LongOperationsView : UserControl
    {
        public LongOperationsView()
        {
            InitializeComponent();
            DataContext = new LongOperationsViewModel();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var tb = sender as TextBox;
                if (tb != null)
                {
                    var vm = DataContext as LongOperationsViewModel;

                    if (vm != null)
                    {
                        switch (tb.Name)
                        {
                            case "Url":
                                vm.UrlInputStr = tb.Text;
                                break;
                            case "FilePath":
                                vm.FilePathInputStr = tb.Text;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
