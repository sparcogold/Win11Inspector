# Win11 Inspector

Windows 11 compatibility diagnostic tool for Windows 10/11.

## Features

- RAM detection
- system disk capacity
- UEFI detection
- Secure Boot detection
- TPM detection
- CPU detection
- GPU detection
- Windows version
- Apple/Mac detection
- Apple/Boot Camp awareness
- Apple-style WPF interface
- GitHub Actions build

## Build locally

Install .NET 8 SDK, then:

dotnet restore

dotnet build -c Release

dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o publish

## GitHub build

Push the project to a GitHub repository.

Open:

Actions â†’ Build Win11 Inspector â†’ Run workflow

The executable is uploaded as:

Win11Inspector-win-x64

## Safety

This version is diagnostic.

It does not automatically modify:

- TPM configuration
- Secure Boot
- boot configuration
- registry compatibility settings

Any future installation-preparation functionality should show a preview, create a backup and require explicit confirmation.