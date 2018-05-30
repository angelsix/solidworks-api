# SolidDNA SDK
The new SolidDNA SDK, making the SolidWorks API easier, well behaved and modern

# Video
I will be making videos available on my YouTube channel that will be guides to everything contained in this repository

http://www.angelsix.com/youtube

# Publishing NuGet

To publish a new version of this project to the NuGet index, first edit the project properties `Package` tab and update the version number.

Next build the solution in `Release`. 

Finally, right-click the project and select `Pack`.

### Uploading new NuGet package

Your new package should be in `bin\Release\AngelSix.SolidDna.x.x.x.x.nupkg`. Go to that folder, type `cmd` in the address bar, then in that window type `dotnet nuget push AngelSix.SolidDna.x.x.x.x.nupkg -k myapikey -s https://api.nuget.org/v3/index.json`