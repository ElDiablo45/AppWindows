# AppWindows

Aplicacion Windows de escritorio creada con WinUI 3 y Windows App SDK.

## Requisitos

- Windows 10 version 1809 o superior.
- Visual Studio 2022 con la carga de trabajo de desarrollo de escritorio .NET y soporte para Windows App SDK.
- .NET SDK compatible con `net8.0-windows`.

## Ejecutar

Abrir `AppWindows.sln` en Visual Studio y ejecutar el proyecto `AppWindows`.

Desde terminal, cuando `dotnet` este disponible:

```powershell
dotnet restore
dotnet build
dotnet run --project src/AppWindows/AppWindows.csproj
```
