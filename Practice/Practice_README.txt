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
                    
                    