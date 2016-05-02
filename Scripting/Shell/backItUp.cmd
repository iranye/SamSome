set backupDir=D:\TOSYNC\DroidBackup
robocopy G:\Stuff\Documents %backupDir%\Documents /mir
robocopy "G:\Android\data\Amazon Apps" %backupDir%\AmazonApps /mir
robocopy F:\DCIM\Camera %backupDir%\Camera /e /s
robocopy F:\mtgtracker %backupDir%\mtgtracker /e /s
pause