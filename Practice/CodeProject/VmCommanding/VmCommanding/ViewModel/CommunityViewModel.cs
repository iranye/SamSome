using MicroMvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VmCommanding.Model;

namespace VmCommanding.ViewModel
{
    public class CommunityViewModel : CommandSink
    {
        public ReadOnlyCollection<PersonViewModel> People { get; private set; }

        public CommunityViewModel()
        {
            Person[] people = Person.GetPeople();
            IEnumerable<PersonViewModel> peopleView = people.Select(p => new PersonViewModel(p));
            People = new ReadOnlyCollection<PersonViewModel>(peopleView.ToArray());

            RegisterCommand(
                KillAllMembersCommand,
                param => CanKillAllMembers,
                param => KillAllMembers()
                );
        }

        public static readonly RoutedCommand KillAllMembersCommand = new RoutedCommand();
        public bool CanKillAllMembers
        {
            get { return People.Any(p => p.CanDie); }
        }

        public void KillAllMembers()
        {
            foreach (PersonViewModel personView in People)
            {
                if (personView.CanDie)
                    personView.Die();
            }
        }

        void KillAllExecute()
        {
            foreach (PersonViewModel personView in People)
            {
                if (personView.CanDie)
                    personView.Die();
            }
        }

        bool CanKillAllExecute()
        {
            return People.Any(p => p.CanDie);
        }

        public ICommand KillAll
        {
            get { return new RelayCommand(KillAllExecute, CanKillAllExecute); }
        }
    }
}
