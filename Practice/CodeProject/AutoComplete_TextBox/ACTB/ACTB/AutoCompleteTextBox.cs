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

        private Popup popup;
        private ListBox listBox;
        private Func<object, string, bool> filter;
        private string textCache = string.Empty;
        private bool suppressEvent = false;

        private FrameworkElement dummy = new FrameworkElement();

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        #region properties
        public Func<object, string, bool> Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                if (this.filter != value)
                {
                    this.filter = value;
                    if (this.listBox != null)
                    {
                        if (this.filter != null)
                            this.listBox.Items.Filter = this.FilterFunc;
                        else
                            this.listBox.Items.Filter = null;
                    }
                }
            }
        }

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
            if (this.listBox == null || this.popup == null)
                this.InternalClosePopup();
            else if (this.listBox.Items.Count == 0)
                this.InternalClosePopup();
            else
                this.InternalOpenPopup();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.popup = Template.FindName("PART_Popup", this) as Popup;
            this.listBox = Template.FindName("PART_ListBox", this) as ListBox;

            if (this.listBox != null)
            {
                this.listBox.PreviewMouseDown += new MouseButtonEventHandler(this.listBox_MouseUp);
                this.listBox.KeyDown += new KeyEventHandler(this.listBox_KeyDown);

                OnItemsSourceChanged(this.ItemsSource);
                OnItemTemplateChanged(this.ItemTemplate);
                OnItemContainerStyleChanged(this.ItemContainerStyle);
                OnItemTemplateSelectorChanged(this.ItemTemplateSelector);

                if (this.filter != null)
                    this.listBox.Items.Filter = this.FilterFunc;
            }
        }

        // ItemsSource Dependency Property
        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (this.listBox == null) return;
            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                this.listBox.ItemsSource = new LimitedListCollectionView((IList)((ListCollectionView)itemsSource).SourceCollection) { Limit = this.MaxCompletions };
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                this.listBox.ItemsSource = new LimitedListCollectionView(((CollectionView)itemsSource).SourceCollection) { Limit = this.MaxCompletions };
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                this.listBox.ItemsSource = new LimitedListCollectionView((IList)itemsSource) { Limit = this.MaxCompletions };
                Debug.Print("Was IList");
            }
            else
            {
                this.listBox.ItemsSource = new LimitedListCollectionView(itemsSource) { Limit = this.MaxCompletions };
                Debug.Print("Was IEnumerable");
            }

            if (this.listBox.Items.Count == 0)
                this.InternalClosePopup();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (this.suppressEvent) return;

            this.textCache = Text ?? string.Empty;
            Debug.Print("Text: " + this.textCache);

            if (this.popup != null && this.textCache == string.Empty)
            {
                this.InternalClosePopup();
            }
            else if (this.listBox != null)
            {
                if (this.filter != null)
                    this.listBox.Items.Filter = this.FilterFunc;

                if (this.popup != null)
                {
                    if (this.listBox.Items.Count == 0)
                        this.InternalClosePopup();
                    else
                        this.InternalOpenPopup();
                }
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (this.suppressEvent) return;

            if (this.popup != null)
                this.InternalClosePopup();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var fs = FocusManager.GetFocusScope(this);
            var o = FocusManager.GetFocusedElement(fs);

            if (e.Key == Key.Escape)
            {
                this.InternalClosePopup();
                Focus();

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (this.listBox != null && o == this)
                {
                    this.suppressEvent = true;
                    this.listBox.Focus();
                    this.suppressEvent = false;

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
            if (this.listBox == null)
                return;

            this.listBox.ItemTemplate = p;
        }

        private void OnItemContainerStyleChanged(Style p)
        {
            if (this.listBox == null) return;

            this.listBox.ItemContainerStyle = p;
        }

        private void OnItemTemplateSelectorChanged(DataTemplateSelector p)
        {
            if (this.listBox == null) return;

            this.listBox.ItemTemplateSelector = p;
        }

        private void InternalClosePopup()
        {
            if (this.popup != null)
                this.popup.IsOpen = false;
        }

        private void InternalOpenPopup()
        {
            this.popup.IsOpen = true;

            if (this.listBox != null)
                this.listBox.SelectedIndex = -1;
        }

        /// <summary>
        /// The text in the textbox is changed by a selected item
        /// from the pop-up list of suggested items.
        /// </summary>
        /// <param name="obj">Item to set in Text portion of <see cref="AutoCompleteTextBox"/> control.
        /// <example>this.listBox.SelectedItem</example></param>
        /// <param name="moveFocus">The focus is moved to the next control if this true.
        /// Otherwise, the input focus stays in the <see cref="AutoCompleteTextBox"/> control.</param>
        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (this.popup != null)
            {
                this.InternalClosePopup();
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
            this.dummy.DataContext = obj;

            // Apply the binding to the dummy FrameworkElement.
            BindingOperations.SetBinding(this.dummy, TextProperty, originalBinding);

            string sPath = this.dummy.GetValue(TextProperty).ToString();

            ////SelectAll();
            // Instead of selecting everything we simply go to the end of the displayed string
            // and attach a new directory seperator (if there is not one already)
            if (sPath.Length > 0)
            {
                if (sPath[sPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                    this.Text = sPath + System.IO.Path.DirectorySeparatorChar;
                else
                    this.Text = sPath;
            }

            this.SelectionStart = this.CaretIndex = this.Text.Length;
            this.listBox.SelectedIndex = -1;
        }

        private bool FilterFunc(object obj)
        {
            return this.filter(obj, this.textCache);
        }

        private void listBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null) return;

            var item = this.listBox.ItemContainerGenerator.ItemFromContainer(dep);

            if (item == null) return;

            this.SetTextValueBySelection(item, false);

            // Fix: Make sure the event is complete and not send to other controls in the window.
            e.Handled = true;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                this.SetTextValueBySelection(this.listBox.SelectedItem, false);

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                // The original implementation used a tab key to select an entry and change the focus
                // to the next control in the window. I find this confusing and deactivate it to implement
                // the standard behaviour of an autocomplete control, which is to
                // close list of suggestions and tab away (via escape and tab keys).
                this.SetTextValueBySelection(this.listBox.SelectedItem, false);

                // Fix: Make sure the event is complete and not send to other controls in the window.
                e.Handled = true;
            }
        }
        #endregion methods
    }
}
