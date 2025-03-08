echo "Loading driver"
::"d:\MyDev\Devel\Misc\MxTools\AutoCheck\Loader\kdu.exe" -prv 12 -map "D:\MyDev\Devel\Misc\MxTools\AutoCheck\MxDriver\x64\Release\MxDriver.sys"

::"d:\MyDev\Devel\Misc\MxTools\RD\KDMapper\kdmapper\x64\Release\kdmapper_Release.exe" "D:\MyDev\Devel\Misc\MxTools\AutoCheck\MxDriver\x64\Release\MxDriver.sys"

"d:\MyDev\Devel\Misc\MxTools\RD\TDL\TDL\Source\Furutaka\output\x64\Release\Furutaka.exe" "D:\MyDev\Devel\Misc\MxTools\AutoCheck\MxDriver\x64\Release\MxDriver.sys"

echo Press Y to continue...
choice /c YN /n