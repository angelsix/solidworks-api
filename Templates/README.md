# Project Templates
To see how to make a VSIX project template check my video here https://www.youtube.com/watch?v=Jhi1WFp47Qk

Basically, edit the templates in this folder such as `SolidDna.Blank` and `SolidDna.WPF.Blank` and then for each project do `Project > Export Template` 

- Template Name: `SolidDNA Blank Add-In` or similar
- Description: `A blank SolidWorks add-in that displays a message box to the user` or similar
- Icon Image: An `.ico` file
- Preview Image: A `.png` or similar image
- Uncheck Automatically Import

Take that zip, extract it, and edit the `MyTemplate.vstemplate` file, changing the `<DefaultName>` back to something you would want to see in the `New Project Name` text field of a new project, such as `SolidDna.Blank`

Re-zip the folder, and put it into `VSIX Installer\SolidDNA.Templates\ProjectTemplates\SolidDNA`

Open up `SolidDNA.Templates.sln` 

If you added new zips that didn't already exist in the project template folder, open up `source.extension.vsixmanifest` then go to `Assets` and click `New`. Select the type `ProjectTemplate` and the source as `File` and select the zip file from the drop-down.

In the `source.extension.vsixmanifest` update the Version number. 

Once done just select `Release` as your configuration and build the project. The project `.vsix` installer is then in the `bin\Release` folder ready to distribute.

Install locally by double-clicking and checking it works correctly first.

To upload it to the Visual Studio Marketplace, go to https://marketplace.visualstudio.com/ and create an account/login, then click `Publish Extensions` 