using MicroMvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VmCommanding.Model;

namespace VmCommanding.ViewModel
{
    public class PersonViewModel : ICommandSink, INotifyPropertyChanged
    {
        readonly CommandSink _commandSink;
        readonly Person _person;

        public int Age
        {
            get { return _person.Age; }
        }

        public string Name
        {
            get { return _person.Name; }
        }
        
        public static readonly RoutedCommand SpeakCommand = new RoutedCommand();

        public bool CanSpeak
        {
            get { return _person.IsAlive; }
        }

        public void Speak(string whatToSay)
        {
            string msg = whatToSay ?? String.Empty;
            string title = _person.Name + " says...";
            MessageBox.Show(whatToSay, title);
        }

        void SpeakExecute(string whatToSay)
        {
            string msg = whatToSay ?? String.Empty;
            string title = _person.Name + " says...";
            MessageBox.Show(whatToSay, title);
        }

        bool CanSpeakExecute(string parameter)
        {
            return _person.IsAlive;
        }

        public ICommand SpeakRelay
        {
            get { return new RelayCommand<string>(SpeakExecute, CanSpeakExecute); }
        }

        public static readonly RoutedCommand DieCommand = new RoutedCommand();

        public bool CanDie
        {
            get { return _person.IsAlive; }
        }

        public void Die()
        {
            _person.IsAlive = false;
            OnPropertyChanged("CanDie");
            OnPropertyChanged("CanSpeak");
        }

        void DieExecute()
        {
            _person.IsAlive = false;
            OnPropertyChanged("CanDie");
            OnPropertyChanged("CanSpeak");
        }

        bool CanDieExecute()
        {
            return _person.IsAlive;
        }

        public ICommand DieCommandRelay
        {
            get { return new RelayCommand(DieExecute, CanDieExecute); }
        }

        public PersonViewModel(Person person)
        {
            _person = person;
            _commandSink = new CommandSink();
            _commandSink.RegisterCommand(
                DieCommand,
                param => this.CanDie,
                parame => this.Die()
                );
            _commandSink.RegisterCommand(
                SpeakCommand,
                param => CanSpeak,
                param => Speak(param as string));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecuteCommand(ICommand command, object parameter, out bool handled)
        {
            return _commandSink.CanExecuteCommand(command, parameter, out handled);
        }

        public void ExecuteCommand(ICommand command, object parameter, out bool handled)
        {
            _commandSink.ExecuteCommand(command, parameter, out handled);
        }

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
