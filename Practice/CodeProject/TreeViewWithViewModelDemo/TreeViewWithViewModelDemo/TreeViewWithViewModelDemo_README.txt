https://www.codeproject.com/Articles/26288/Simplifying-the-WPF-TreeView-by-Using-the-ViewMode

* Things to learn about *
Composition of xaml code using separate files vs everything-in-one-xaml approach.
Defining DataContext in code-behind.
Passing parameters to ICommand properties.
Code structuring into separate projects & folders.
INotifyPropertyChanged is a UI-specific interface, which is why the PersonViewModel class implements it, not the Person class.
Hitting <Enter> into a textbox to effect a change vs having to "tab out".

* Do these in this Order *
Create TreeViewWithViewModelDemo Sln & Project
Create BusinessLib project and classes
Add Folders and ViewModelClasses in TreeViewWithViewModelDemo
Add UserControl: TextSearchDemoControl and write code-behind & XAML

Rename MainWindow to DemoWindow and update ref in App.xaml
Add to DemoWindow: xlmns and <TabControl>
