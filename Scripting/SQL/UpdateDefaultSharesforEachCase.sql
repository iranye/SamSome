-- You only need to leave the share/s that you want to update in, otherwise just remove it from the script.
sp_msForEachDb
	'
	use [?]
	if exists ( select * from sysobjects where name = ''tblSettings'')
	begin
		if exists (select * from tblSettings)
		Begin
		select ''[?]''

		Update tblSettings
		Set
		InventoryShare = ''\\pctrtitan\i1\'',
		ArchiveShare = ''\\pctrtitan\a2\'',
		LibraryShare = ''\\vxbluearc\library04\'',
		WorkingShare = ''\\pctrtitan\w1\'',
		ConversionShare = ''\\pctrtitan\c1\''
		End
	end
	'