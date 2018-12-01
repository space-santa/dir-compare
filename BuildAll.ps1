$array = ("win10-x64", "osx-x64", "linux-x64")
$array |foreach {
    dotnet build DirCompare/DirCompare.csproj -r $_
    dotnet publish DirCompare/DirCompare.csproj -c release -r $_
}
