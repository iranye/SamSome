*** Stuff to do ***
* Refer to WPF TreeViewWithViewModelDemo *
Hitting <Enter> into a textbox to effect a change vs having to "tab out".
Setting FocusedElement (e.g. for textbox)
LinearGradient Border wrapping a stackpanel for a text box.

* Refer to WPF HighlightableTextBlock *
Passing parameters to ICommand properties.

* Refer to WPF AReusableWPFAutocompleteTextBox *
Define exotic stuff in Window.Resources (beyond Style tags)
Themes in WPF
Explicitly defining Bindings to source (i.e., no DataContext)
Implementing TextBox in cs file then including as visual control in xaml.


Work through HighlightableTextBlock in NewStuff and put in SamSome\Practice.
Create a .Net Core API to support a CRUD interface for playlists.
Add Shadow effect to buttons

*** Be able to do this ***
** WPF **
* Show or Hide items in a ListBox/ListView *
	<ListBox ItemsSource="{Binding Rdls}"
		 SelectedItem="{Binding SelectedRdl}"
		 MinWidth="260"
		 Height="280"
		 Margin="0,0,6,0"
		 HorizontalAlignment="Stretch" >
		<ListBox.ItemContainerStyle>
			<Style TargetType="{x:Type ListBoxItem}">
				<Style.Triggers>
					<DataTrigger Binding="{Binding ShownInList}" Value="False">
						<Setter Property="Visibility" Value="Collapsed" />
					</DataTrigger>
				</Style.Triggers>
			</Style>
		</ListBox.ItemContainerStyle>
	</ListBox>
* Use DockPanel elements to fill width *
* Implement a file or directory picker *
	<DockPanel>
		<Label Content="CRIF File"
			   DockPanel.Dock="Left"
			   Style="{StaticResource Labels}"/>
		<Button Content="..."
				Width="20"
				Margin="0,0,6,0"
				DockPanel.Dock="Right"
				Height="20"
				Command="{Binding PickFile}" />
		<TextBox Text="{Binding LoadConfig.CrifFileName}"
				 DockPanel.Dock="Left"
				 HorizontalAlignment="Stretch"
				 Margin="6,3" />
	</DockPanel>
* Tabs *
	<ScrollViewer VerticalScrollBarVisibility="Auto" >
        <Grid>
			...
            <StackPanel Grid.Row="0" Orientation="Horizontal">
                <Label Content="One Source Uploader"
                       FontSize="14"
                       HorizontalAlignment="Left"/>
                <Label  Content="{Binding Version}" FontSize="14" />
            </StackPanel>
            <TabControl Grid.Row="1">
                <TabItem Header="Curl">
                    <StackPanel>
						...
				</TabItem>
				<TabItem Header="Read RDLs">
				</TabItem>
			</TabControl>
		</Grid>
	</ScrollViewer>
                    
                    