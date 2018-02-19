using System.Windows;
using System.Windows.Controls;

namespace Test
{
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Window wnd = Application.Current.MainWindow;

            if (wnd == null) return null;

            // Return DataTemplate for WaitMesage or List of Suggestions based on the
            // type of item we are being asked to DataTemplate here...

            if (item is ViewModel.WaitMsg)
            {
                return wnd.FindResource("WaitTemplate") as DataTemplate;

            }
            return wnd.FindResource("TheItemTemplate") as DataTemplate;
        }
    }
}
