# AppWindows

Aplicacion Windows de escritorio para gestion de autoescuela, creada con WinUI 3 y Windows App SDK.

La version actual incluye un primer modulo de **Alumnos** con persistencia local en SQLite:

- Alta de alumnos con nombre, DNI/NIE, telefono, fecha de alta automatica y observaciones.
- DNI/NIE obligatorio y unico.
- Carnets/tags predefinidos y tags personalizados.
- Listado con busqueda por nombre, DNI/NIE o telefono.
- Filtro por carnet/tag.
- Detalle editable sin borrado.
- Base de datos local en `%LocalAppData%\AppWindows\appwindows.db`.

## Requisitos

- Windows 10 version 1809 o superior.
- Visual Studio 2022 con la carga de trabajo de desarrollo de escritorio .NET y soporte para Windows App SDK.
- .NET SDK compatible con `net8.0-windows`.

## Ejecutar

Abrir `AppWindows.sln` en Visual Studio y ejecutar el proyecto `AppWindows`.

Desde terminal, cuando `dotnet` este disponible:

```powershell
dotnet restore
dotnet build -c Debug -p:Platform=x64
dotnet run --project src/AppWindows/AppWindows.csproj
```

Si la terminal actual no ha refrescado el PATH tras instalar Visual Studio Build Tools, usa la ruta completa:

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' build AppWindows.sln -c Debug -p:Platform=x64
```
