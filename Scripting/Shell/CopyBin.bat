if not exist c:\temp\bin mkdir c:\temp\bin

copy /Y "\\soong\utepccommon\IMN\bin\*.*" c:\temp\bin

set path=%PATH%;C:\temp\bin