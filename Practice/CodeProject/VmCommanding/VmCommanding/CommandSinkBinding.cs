using System;
using System.Windows;
using System.Windows.Input;

namespace VmCommanding
{
    public class CommandSinkBinding : CommandBinding
    {
        ICommandSink _commandSink;

        public ICommandSink CommandSink
        {
            get { return _commandSink; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Cannot set CommandSink to null");

                if (_commandSink != null)
                    throw new InvalidOperationException("Cannot set CommandSink more than once");

                _commandSink = value;
                CanExecute += (s, e) =>
                {
                    bool handled;
                    e.CanExecute = _commandSink.CanExecuteCommand(e.Command, e.Parameter, out handled);
                    e.Handled = handled;
                };
                Executed += (s, e) =>
                {
                    bool handled;
                    _commandSink.ExecuteCommand(e.Command, e.Parameter, out handled);
                    e.Handled = handled;
                };
            }
        }

        public static void SetCommandSink(DependencyObject obj, ICommandSink value)
        {
            obj.SetValue(CommandSinkProperty, value);
        }

        public static ICommandSink GetCommandSink(DependencyObject obj)
        {
            return (ICommandSink)obj.GetValue(CommandSinkProperty);
        }

        static void OnCommandSinkChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            ICommandSink commandSink = e.NewValue as ICommandSink;
            if (!ConfigureDelayedProcessing(depObj, commandSink))
            {
                ProcessCommandSinkChanged(depObj, commandSink);
            }
        }

        private static bool ConfigureDelayedProcessing(DependencyObject depObj, ICommandSink commandSink)
        {
            bool isDelayed = false;
            CommonElement elem = new CommonElement(depObj);
            if (elem.IsValid && !elem.IsLoaded)
            {
                RoutedEventHandler handler = null;
                handler = delegate
                {
                    elem.Loaded -= handler;
                    ProcessCommandSinkChanged(depObj, commandSink);
                };
                elem.Loaded += handler;
                isDelayed = true;
            }
            return isDelayed;
        }

        private static void ProcessCommandSinkChanged(DependencyObject depObj, ICommandSink commandSink)
        {
            CommandBindingCollection cmdBindings = GetCommandBindings(depObj);
            if (cmdBindings == null)
                throw new ArgumentException("The CommandSinkBinding.CommandSink attached property was set on an element that does not suppor CommandBindings");
            foreach (CommandBinding cmdBinding in cmdBindings)
            {
                CommandSinkBinding csb = cmdBinding as CommandSinkBinding;
                if (csb != null && csb.CommandSink == null)
                    csb.CommandSink = commandSink;
            }
        }

        public static readonly DependencyProperty CommandSinkProperty =
            DependencyProperty.RegisterAttached(
                "CommandSink",
                typeof(ICommandSink),
                typeof(CommandSinkBinding),
                new UIPropertyMetadata(null, OnCommandSinkChanged));

        static CommandBindingCollection GetCommandBindings(DependencyObject depObj)
        {
            var elem = new CommonElement(depObj);
            return elem.IsValid ? elem.CommandBindings : null;
        }

        private class CommonElement
        {
            readonly FrameworkElement _fe;
            readonly FrameworkContentElement _fce;

            public readonly bool IsValid;

            public CommonElement(DependencyObject depObj)
            {
                _fe = depObj as FrameworkElement;
                _fce = depObj as FrameworkContentElement;

                IsValid = _fe != null || _fce != null;
            }

            public CommandBindingCollection CommandBindings
            {
                get
                {
                    this.Verify();

                    if (_fe != null)
                        return _fe.CommandBindings;
                    else
                        return _fce.CommandBindings;
                }
            }

            public bool IsLoaded
            {
                get
                {
                    this.Verify();

                    if (_fe != null)
                        return _fe.IsLoaded;
                    else
                        return _fce.IsLoaded;
                }
            }

            public event RoutedEventHandler Loaded
            {
                add
                {
                    this.Verify();

                    if (_fe != null)
                        _fe.Loaded += value;
                    else
                        _fce.Loaded += value;
                }
                remove
                {
                    this.Verify();

                    if (_fe != null)
                        _fe.Loaded -= value;
                    else
                        _fce.Loaded -= value;
                }
            }

            void Verify()
            {
                if (!this.IsValid)
                    throw new InvalidOperationException("Cannot use an invalid CommmonElement.");
            }
        }
    }
}
