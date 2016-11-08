SSIS-GDAL
=========

GDAL components for SQL Server Integration Services

![alt tag](http://download-codeplex.sec.s-msft.com/Download?ProjectName=GDALSSIS&DownloadId=602441)


Requirements
------------

* Visual Studio 2015
* SQL Server Data Tools
* GDAL >= 2.0.1


Build
-----

Before building you must create a strong-name key file to sign your assemblies:

	sn.exe -k SSISGDAL.snk
    
Build:

    msbuild.exe build.targets /t:build
    
Install:

    msbuild.exe build.targets /t:install

Uninstall:

    msbuild.exe build.targets /t:uninstall
    
If you have issues check build.targets and make sure the properties are set correctly for your environment  


Installer Project
-----------------


* Requires InstallShield Limited Edition


Use this to install SSIS-GDAL DLL's into the GAC and pre-build GDAL (2.0.1) and CSharp interop DLL's into the Windows folder.
