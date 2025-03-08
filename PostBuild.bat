set xcp=xcopy /y

%xcp% "%1..\RD\IbInputSimulator\VS\x64\%3\I*.dll" "%1\Simulator\%3\*.*"
%xcp% "%1..\RD\IbInputSimulator\VS\x64\%3\I*.pdb" "%1\Simulator\%3\*.*"

%xcp% "%1\Simulator\%3\*.*" "%2"
%xcp% "%1\Sounds\*.*" "%2"

"%1LicenseMaker\bin\Release\net9.0-windows\LicenseMaker.exe" "%2license.lic"