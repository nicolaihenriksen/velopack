| [docs](..)  / [using](.) / visual-studio-packaging.md
|:---|

# Visual Studio Build Packaging

Squirrel packaging can be easily integrated directly into your build process using only NuGet and Squirrel. 

## Define Build Target

The first step is to define a build target in your `.csproj` file.

```xml
<Target Name="AfterBuild" Condition=" '$(Configuration)' == 'Release'">
```

This will generate a NuGet package from .nuspec file setting version from AssemblyInfo.cs and place it in OutDir (by default bin\Release). Then it will generate release files from it.

## Example .nuspec file for MyApp

Here is an example `MyApp.nuspec` file for the above build target example.

```xml
<?xml version="1.0" encoding="utf-8"?>
```

## Additional Notes

Please be aware of the following when using this solution:

* Solution needs to have nuget.exe available which can be accomplished by installing `NuGet.CommandLine` package in your solution.  

  ~~~pm
PM>  Install-Package NuGet.CommandLine
  ~~~
* It suffers from a bug when sometimes NuGet packages are not loaded properly and throws nuget/squirrel is not recogized (9009) errors.  
 **Tip:** In this case you may simply need to restart Visual Studio so the Package Manager Console will have loaded all the package tools
* If you get the following error you may need add the full path to squirrel.exe in the build target `Exec Command` call. `'squirrel' is not recognized as an internal or external command`

**Source:** [Issue #630](https://github.com/Squirrel/Squirrel.Windows/issues/630)

---
| Return: [Packaging Tools](packaging-tools.md) |
|----|


