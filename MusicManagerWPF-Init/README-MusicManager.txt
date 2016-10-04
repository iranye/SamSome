http://www.codeproject.com/Articles/165368/WPF-MVVM-Quick-Start-Tutorial

Stage 1: Broken in the example
Stage 2:
- Song: {ArtistName, SongTitle} and SongViewModel: constructor setting things to "Unknown" and RaisePropertyChanged defined and tied to ArtistName
- MainWindow.xaml.cs has SongViewModel instance and ButtonClick event to update its ArtistName
- MainWindow.xaml has
* Window.DataContext that defines local
* 4 rows, 2 cols as:
<Title>
Artist:		Binding
			Button

Stage 3:
- Define new type: RelayCommand w/:
* Constructor: public RelayCommand(Action execute, Func<Boolean> canExecute) (just updates backing fields)
* public event EventHandler CanExecuteChanged
{
    add
    {
        if (mCanExecute != null)
            CommandManager.RequerySuggested += value;
    }
    // similar code for remove { } block
* defines Boolean CanExecute(Object parameter) and void Execute(Object paramter)
- Refactor SongViewModel
* Define method: ActionExecute
* Define method: CanExecute
* Add property: ICommand UpdateArtistName (to use in XAML {Binding:Action}) as get return new RelayCommand(ActionExecute, CanExecute)
- Clean up MainWindow.xaml.cs to only have default code definition
- MainWindow.xaml:
* Button changes from Click=.. to Command=
* Re-use UpdateArtistName command in menu
        <Menu Grid.Row="0" Grid.ColumnSpan="3">
            <MenuItem Header="Test">
                <MenuItem Header="Update Artist" Command="{Binding UpdateArtistName}" />
            </MenuItem>
        </Menu>

Stage 4:
- New MicroMvvm project:
  * Add PresentationCore
  * Move RelayCommand to here
  * Add new type: abstract ObservableObject
- SongViewModel subclasses new type and uses PropertyChange Event Handlers in superclass
