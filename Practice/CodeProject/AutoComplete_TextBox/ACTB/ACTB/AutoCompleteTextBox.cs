/* To use this control, add an XmlNamespace attribute to the root element xaml file where it is to be used:
xmlns:actb="clr-namespace:ACTB;assembly=ACTB"

Then use it as follows in the XAML file:
        <actb:AutoCompleteTextBox Name="ActbInputPath"
        Text="{Binding Source={StaticResource vm}, Path=QueryText, UpdateSourceTrigger=PropertyChanged}"
        ItemsSource="{Binding Source={StaticResource prioritySources}}" 
        ItemTemplateSelector="{StaticResource TemplateSelector}"
        Binding="{Binding}" 
        MaxCompletions="1024"
        />
*/
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Aviad.WPF.Controls
{
    public class AutoCompleteTextBox : TextBox
    {
        #region fields
        
        public static readonly DependencyProperty ItemContainerStyleProperty =
            ItemsControl.ItemContainerStyleProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemContainerStyleChanged));

        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding",
                typeof(string),
                typeof(AutoCompleteTextBox), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ItemTemplateProperty =
            ItemsControl.ItemTemplateProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemTemplateChanged));
        
        public static readonly DependencyProperty MaxCompletionsProperty =
             DependencyProperty.Register("MaxCompletions",
                 typeof(int),
                 typeof(AutoCompleteTextBox), new UIPropertyMetadata(int.MaxValue));

        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            ItemsControl.ItemTemplateSelectorProperty.AddOwner(
                 typeof(AutoCompleteTextBox),
                 new UIPropertyMetadata(null, OnItemTemplateSelectorChanged));
        #endregion DependencyPropertyFields

        private Popup _popup;
        private ListBox _listBox;
        private string _textCache = string.Empty;
        private bool _suppressEvent = false;

        private Func<object, string, bool> _filter;
        public Func<object, string, bool> Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    if (_listBox != null)
                    {
                        if (_filter != null)
                            _listBox.Items.Filter = FilterFunc;
                        else
                            _listBox.Items.Filter = null;
                    }
                }
            }
        }
        
        private bool FilterFunc(object obj)
        {
            return _filter(obj, _textCache);
        }

        private FrameworkElement _dummy = new FrameworkElement();

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        #region properties
        public string Binding
        {
            get { return (string)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        public int MaxCompletions
        {
            get { return (int)GetValue(MaxCompletionsProperty); }
            set { SetValue(MaxCompletionsProperty, value); }
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        // ItemsSource Dependency Property
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion properties

        #region methods
        public void ShowPopup()
        {
            if (_listBox == null || _popup == null)
                InternalClosePopup();
            else if (_listBox.Items.Count == 0)
                InternalClosePopup();
            else
                InternalOpenPopup();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = Template.FindName("PART_Popup", this) as Popup;
            _listBox = Template.FindName("PART_ListBox", this) as ListBox;

            if (_listBox != null)
            {
                _listBox.PreviewMouseDown += new MouseButtonEventHandler(listBox_MouseUp);
                _listBox.KeyDown += new KeyEventHandler(listBox_KeyDown);

                OnItemsSourceChanged(ItemsSource);
                OnItemTemplateChanged(ItemTemplate);
                OnItemContainerStyleChanged(ItemContainerStyle);
                OnItemTemplateSelectorChanged(ItemTemplateSelector);

                if (_filter != null)
                    _listBox.Items.Filter = FilterFunc;
            }
        }

        // ItemsSource Dependency Property
        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (_listBox == null) return;
            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                _listBox.ItemsSource = new LimitedListCollectionView((IList)((ListCollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions };
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                _listBox.ItemsSource = new LimitedListCollectionView(((CollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions };
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                _listBox.ItemsSource = new LimitedListCollectionView((IList)itemsSource) { Limit = MaxCompletions };
                Debug.Print("Was IList");
            }
            else
            {
                _listBox.ItemsSource = new LimitedListCollectionView(itemsSource) { Limit = MaxCompletions };
                Debug.Print("Was IEnumerable");
            }

            if (_listBox.Items.Count == 0)
                InternalClosePopup();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (_suppressEvent) return;

            _textCache = Text ?? string.Empty;
            Debug.Print("Text: " + _textCache);

            if (_popup != null && _textCache == string.Empty)
            {
                InternalClosePopup();
            }
            else if (_listBox != null)
            {
                if (_filter != null)
                    _listBox.Items.Filter = FilterFunc;

                if (_popup != null)
                {
                    if (_listBox.Items.Count == 0)
                        InternalClosePopup();
                    else
                        InternalOpenPopup();
                }
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (_suppressEvent) return;

            if (_popup != null)
                InternalClosePopup();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var fs = FocusManager.GetFocusScope(this);
            var o = FocusManager.GetFocusedElement(fs);

            if (e.Key == Key.Escape)
            {
                InternalClosePopup();
                Focus();

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (_listBox != null && o == this)
                {
                    _suppressEvent = true;
                    _listBox.Focus();
                    _suppressEvent = false;

                    // Fix: Make sure the event is complete and not send to other controls in the window.
                    e.Handled = true;
                }
            }
        }

        // ItemsSource Dependency Property
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as IEnumerable);
        }

        // ItemTemplate Dependency Property
        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateChanged(e.NewValue as DataTemplate);
        }

        // ItemContainerStyle Dependency Property
        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemContainerStyleChanged(e.NewValue as Style);
        }

        // ItemTemplateSelector Dependency Property
        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnItemTemplateChanged(DataTemplate p)
        {
            if (_listBox == null)
                return;

            _listBox.ItemTemplate = p;
        }

        private void OnItemContainerStyleChanged(Style p)
        {
            if (_listBox == null) return;

            _listBox.ItemContainerStyle = p;
        }

        private void OnItemTemplateSelectorChanged(DataTemplateSelector p)
        {
            if (_listBox == null) return;

            _listBox.ItemTemplateSelector = p;
        }

        private void InternalClosePopup()
        {
            if (_popup != null)
                _popup.IsOpen = false;
        }

        private void InternalOpenPopup()
        {
            _popup.IsOpen = true;

            if (_listBox != null)
                _listBox.SelectedIndex = -1;
        }
        
        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (_popup != null)
            {
                InternalClosePopup();
                Dispatcher.Invoke(new Action(() =>
                {
                    Focus();
                    if (moveFocus)
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }), System.Windows.Threading.DispatcherPriority.Background);
            }

            // Retrieve the Binding object from the control.
            var originalBinding = BindingOperations.GetBinding(this, BindingProperty);
            if (originalBinding == null) return;

            // Set the dummy's DataContext to our selected object.
            _dummy.DataContext = obj;

            // Apply the binding to the dummy FrameworkElement.
            BindingOperations.SetBinding(_dummy, TextProperty, originalBinding);

            string listEntry = _dummy.GetValue(TextProperty).ToString();

            ////SelectAll();
            // Instead of selecting everything we simply go to the end of the displayed string
            // and attach a new directory seperator (if there is not one already)
            if (listEntry.Length > 0)
            {
                if (listEntry[listEntry.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                    Text = listEntry + System.IO.Path.DirectorySeparatorChar;
                else
                    Text = listEntry;
            }

            SelectionStart = CaretIndex = Text.Length;
            _listBox.SelectedIndex = -1;
        }

        private void listBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null) return;

            var item = _listBox.ItemContainerGenerator.ItemFromContainer(dep);

            if (item == null) return;

            SetTextValueBySelection(item, false);

            // Fix: Make sure the event is complete and not send to other controls in the window.
            e.Handled = true;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                SetTextValueBySelection(_listBox.SelectedItem, false);

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                // The original implementation used a tab key to select an entry and change the focus
                // to the next control in the window. I find this confusing and deactivate it to implement
                // the standard behaviour of an autocomplete control, which is to
                // close list of suggestions and tab away (via escape and tab keys).
                SetTextValueBySelection(_listBox.SelectedItem, false);

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
        }
        #endregion methods
    }
}
