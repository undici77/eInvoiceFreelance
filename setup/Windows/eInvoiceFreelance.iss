#define ApplicationName 'eInvoiceFreelance'
#define ApplicationFile 'eInvoiceFreelance.exe'
#define ApplicationVersion GetStringFileInfo('..\..\eInvoiceFreelance\bin\Release\eInvoiceFreelance.exe', PRODUCT_VERSION)

[Files]
; Applicazione
Source: "..\..\eInvoiceFreelance\bin\Release\{#ApplicationFile}"; DestDir: {app}; Flags: ignoreversion replacesameversion replacesameversion restartreplace; Permissions: everyone-full
Source: "..\..\eInvoiceFreelance\bin\Release\itextsharp.dll"; DestDir: {app}; Flags: ignoreversion replacesameversion replacesameversion restartreplace; Permissions: everyone-full
Source: "..\..\eInvoiceFreelance\bin\Release\Template.xml"; DestDir: {app}; Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist; Permissions: everyone-full

[Dirs]
Name: "{app}"; Permissions: everyone-full

[Setup]
AppName={#ApplicationName}
AppVersion={#ApplicationVersion}
AppVerName={#ApplicationName} {#ApplicationVersion}
OutputBaseFilename={#ApplicationName}_Win_Setup_{#ApplicationVersion}
AppPublisher=Undici77
AppPublisherURL=https://github.com/undici77/eInvoiceFreelance
DefaultDirName={pf}\{#ApplicationName}
DefaultGroupName={#ApplicationName}
Compression=lzma
SolidCompression=yes
MinVersion=6.1.7600
PrivilegesRequired=admin
AppCopyright=Copyright (C) 2019 Undici77
SetupIconFile=..\..\eInvoiceFreelance\res\eInvoiceFreelance.ico
UninstallDisplayIcon=yes
OutputDir=output\{#ApplicationVersion}
LicenseFile=license\license.rtf

[Icons]
Name: "{group}\{#ApplicationName}"; Filename: "{app}\{#ApplicationFile}"
Name: "{group}\Uninstall {#ApplicationName}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#ApplicationName}"; Filename: "{app}\{#ApplicationFile}";
