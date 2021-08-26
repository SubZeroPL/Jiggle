# Jiggle
Simple mouse jiggle app (prevents your computer from going to sleep/starting screensaver).

Requires .NET Core 3.1

## Building
- `git clone https://github.com/SubZeroPL/Jiggle`
- `dotnet build -c Release`
- `dotnet .\Jiggle\bin\Release\netcoreapp3.1\Jiggle.dll`
- or use `publish-winXX.cmd` in root directory to create single file app (Windows only)

Program configuration is saved in _alternate data stream_, so it is recommended to use on systems with NTFS file system. It should work with other filesystems (FAT, FAT32) but will not be able to persist configuration.

On first run you'll get an error that configuration can't be loaded - that's to be expected as ADS that's containing it doesn't exist yet.

Works on Windows only ATM because of no GUI implementation in .NET for Linux.
