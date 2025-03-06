::xcopy /s /e /y /q /i "%1\..\Blackbone\Blackbone\build\x64\Release(DLL)\*.DLL" "%2"
xcopy /s /e /y /q /i "%1\Simulator\*.DLL" "%2"
xcopy /s /e /y /q /i "%1\Sounds\*.*" "%2"