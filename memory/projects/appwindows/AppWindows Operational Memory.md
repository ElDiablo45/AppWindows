---
title: AppWindows Operational Memory
type: project_memory
permalink: appwindows/projects/appwindows/app-windows-operational-memory
migrated_from: D:/AppWindows/docs/agent_memory.md
migration_date: '2026-05-23'
repository: D:/AppWindows
tags:
- appwindows
- project-memory
- winui
- autoescuela
---

# AppWindows Operational Memory

Memoria viva del proyecto AppWindows, migrada desde `docs/agent_memory.md` el 2026-05-23 para abandonar el flujo de memoria basado en archivos locales del repositorio y usar Basic Memory como fuente persistente principal.

AppWindows es una aplicacion de escritorio para Windows con WinUI 3 y Windows App SDK orientada a la gestion de una autoescuela. El primer modulo funcional es la gestion local de alumnos.

## Decisiones Activas

- Usar Basic Memory como fuente persistente de memoria operativa del repositorio.
- Mantener las notas `AppWindows Operational Memory`, `AppWindows Product Specification` y `AppWindows Roadmap` como contexto base antes de desarrollar.
- Versionar la memoria de Basic Memory dentro del repositorio en `memory/`.
- No inventar decisiones de producto importantes; marcar como pendiente todo contexto no confirmado.
- Usar WinUI 3 con Windows App SDK para la interfaz de escritorio.
- Iniciar el proyecto como una aplicacion unpackaged (`WindowsPackageType=None`) para mantener el arranque simple.
- Referenciar `Microsoft.WindowsAppSDK` version `2.0.1`.
- Target framework inicial: `net8.0-windows10.0.19041.0`.
- Usar SQLite local para persistencia inicial.
- Referenciar `Microsoft.Data.Sqlite` version `10.0.6`.
- Guardar la base de datos en `%LocalAppData%\AppWindows\appwindows.db`.
- El primer modulo se llama `Alumnos`.
- En v1 no hay borrado de alumnos; se permite crear, listar, buscar, filtrar, ver detalle y editar.
- La direccion visual acordada para la app es un panel oscuro de gestion: menu lateral izquierdo, cabecera superior, tabla/listado principal y ficha de detalle integrada.

## Contexto Funcional

La aplicacion gestiona alumnos de autoescuela. Los campos v1 de alumno son nombre, DNI/NIE, telefono, fecha de alta automatica, observaciones y tags/carnets. DNI/NIE es obligatorio y unico, sin validar checksum en v1.

Tags iniciales de carnet: `AM`, `A1`, `A2`, `A`, `B`, `B+E`, `C1`, `C1+E`, `C`, `C+E`, `D1`, `D1+E`, `D`, `D+E`. Se permiten tags personalizados.

## Entorno

- Sistema operativo del entorno de trabajo: Windows/PowerShell.
- Directorio de trabajo: `D:\AppWindows`.
- Sandbox con permisos de escritura en el workspace.
- Rama local actual: `main`.
- Remoto configurado: `https://github.com/ElDiablo45/AppWindows.git`.
- Visual Studio Build Tools 2022 instalado con carga `Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools`.
- `dotnet.exe` disponible en `C:\Program Files\dotnet\dotnet.exe`.
- MSBuild detectado en `C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe`.
- .NET SDKs instalados: `9.0.314` y `10.0.300`.
- SDK predeterminado actual: `10.0.300`.
- Runtimes instalados: .NET 8.0.27, .NET 9.0.16 y .NET 10.0.8.
- Python 3.14.5 instalado en `C:\Users\mdavi\AppData\Local\Programs\Python\Python314`.
- `uv` instalado dentro de `.venv` como `D:\AppWindows\.venv\Scripts\uv.exe`.
- `basic-memory` instalado con `uv tool`, comandos `basic-memory` y `bm` en `C:\Users\mdavi\.local\bin`.
- Basic Memory MCP disponible y configurado con proyecto local `appwindows`.
- La memoria de Basic Memory de este proyecto vive dentro del repositorio en `D:\AppWindows\memory` para versionarse con Git.

## Implementado Hasta Ahora

- Sistema anterior de memoria operativa persistente creado en documentacion: `AGENTS.md`, `docs/agent_memory.md`, `docs/app_spec.md`, `docs/roadmap.md`.
- Aplicacion WinUI 3 inicial creada: `AppWindows.sln`, `src/AppWindows/AppWindows.csproj`, `src/AppWindows/App.xaml`, `src/AppWindows/MainWindow.xaml`, `src/AppWindows/app.manifest`, `.gitignore`, `README.md`.
- Configuracion local de NuGet creada en `NuGet.Config` para restaurar desde `nuget.org` sin depender del perfil de usuario.
- Build Debug x64 validado correctamente el 2026-05-21.
- Modulo `Alumnos` implementado el 2026-05-22 con UI WinUI, listado, busqueda, filtro por tag, alta, detalle editable, persistencia SQLite, esquema v1 y seed automatico de carnets.
- Estilo visual de `Alumnos` adaptado el 2026-05-22 a una interfaz oscura inspirada en panel administrativo.
- Entorno virtual Python creado el 2026-05-23 en `D:\AppWindows\.venv` con Python `3.14.5` y `pip 26.1.1`.
- Basic Memory validado el 2026-05-23: `basic-memory v0.21.1`, `bm --help` correcto tras incluir `C:\Users\mdavi\.local\bin` en PATH.
- Proyecto Basic Memory `appwindows` creado el 2026-05-23 en `D:\AppWindows\memory` para versionar la memoria con Git.

## Notas De Build Y Deploy

Stack inicial: WinUI 3 + Windows App SDK + .NET 8 para Windows. Persistencia: SQLite local con `Microsoft.Data.Sqlite`.

Comandos previstos:

```powershell
dotnet restore
dotnet build -c Debug -p:Platform=x64
dotnet run --project src/AppWindows/AppWindows.csproj
```

Validaciones relevantes:

- 2026-05-21: MSBuild fallo al no resolver `Microsoft.NET.Sdk`.
- 2026-05-21: restore correcto usando `NuGet.Config`; build correcto con `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore`.
- 2026-05-21: build Debug x64 correcto con SDK predeterminado .NET `10.0.300`, 0 errores y 0 advertencias.
- 2026-05-22: modulo `Alumnos` validado con restore y build Debug x64, 0 errores y 0 advertencias.
- 2026-05-22: rediseño oscuro validado con build Debug x64, 0 errores y 0 advertencias.
- 2026-05-23: entorno Python y Basic Memory validados.

## Riesgos Activos

- Falta definir modulos posteriores: facturas, cobros, practicas, examenes, profesores e importaciones/exportaciones.
- Las terminales abiertas antes de configurar PATH pueden no encontrar `dotnet`; solucion temporal: `$env:Path = [Environment]::GetEnvironmentVariable('Path','Machine') + ';' + [Environment]::GetEnvironmentVariable('Path','User')`.
- No hay pruebas automatizadas todavia; la validacion actual es build y pruebas manuales previstas.
- El push a `origin/main` queda pendiente de aprobacion explicita del usuario.

## Observations

- [decision] Basic Memory reemplaza a `docs/agent_memory.md` como memoria operativa persistente del proyecto #memory
- [decision] El proyecto usa WinUI 3 con Windows App SDK y empaquetado unpackaged #architecture
- [decision] La persistencia inicial usa SQLite local en `%LocalAppData%\AppWindows\appwindows.db` #data
- [decision] El primer modulo funcional es `Alumnos` #product
- [decision] En v1 no existe borrado de alumnos #product
- [decision] La direccion visual acordada es un panel oscuro de gestion con sidebar, cabecera, tabla y detalle integrado #ux
- [decision] Versionar la memoria de Basic Memory dentro del repositorio en `memory/` #memory
- [environment] El repositorio local esta en `D:\AppWindows` #repo
- [environment] El remoto Git configurado es `https://github.com/ElDiablo45/AppWindows.git` #git
- [environment] Basic Memory v0.21.1 esta instalado y el comando `bm` fue validado el 2026-05-23 #memory
- [environment] El proyecto Basic Memory activo es `appwindows` y su ruta local es `D:\AppWindows\memory` #memory
- [risk] No hay pruebas automatizadas todavia #testing
- [risk] Falta validacion manual completa en ventana interactiva #qa
- [risk] El alcance posterior a alumnos sigue pendiente de definir #product
- [status] La migracion a Basic Memory quedo aplicada en el repositorio con commit local `323bfa9` #memory
- [blocked] El push a `origin/main` queda pendiente de aprobacion explicita del usuario #git
- [maintenance] Memory defrag ran on 2026-05-23 and kept the four-note core structure #memory
- [maintenance] Defrag log lives in [[AppWindows Memory Defrag - 2026-05-23]] #memory
- [tooling] Installed Microsoft `winui-*` skills from `microsoft/win-dev-skills` into `C:\Users\mdavi\.codex\skills` on 2026-05-23 #winui #skills
- [note] Restart Codex to pick up newly installed `winui-*` skills #skills

## Relations

- specifies [[AppWindows Product Specification]]
- tracks_work_in [[AppWindows Roadmap]]
- migrated_by [[AppWindows Memory Migration - 2026-05-23]]

## Update 2026-05-23 - WinUI Setup

- [tooling] Ran `winui-setup` checks on 2026-05-23 #winui #setup
- [tooling] `.NET SDK >= 8` requirement satisfied by installed SDKs `9.0.314` and `10.0.300` #dotnet #setup
- [tooling] `Microsoft.WinAppCli` installed via winget; published package id uses `Cli` casing rather than `CLI` #winui #setup
- [tooling] `winapp --version` works outside the sandbox and reports `0.3.1` from `C:\Users\mdavi\AppData\Local\Microsoft\WindowsApps\winapp.exe` #winui #setup
- [tooling] Installed `Microsoft.WindowsAppSDK.WinUI.CSharp.Templates`; `dotnet new list` shows WinUI app templates including `winui-mvvm` outside the sandbox #winui #templates
- [environment] Current sandbox shell may not resolve `winget` or `winapp` until PATH/App Execution Alias context is refreshed #windows #path
- [pending] Developer Mode is disabled and requires explicit user approval plus UAC/admin elevation to enable #winui #setup

## Update 2026-05-23 - Developer Mode Enabled

- [tooling] Developer Mode was enabled via elevated PowerShell on 2026-05-23; registry value `HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock\AllowDevelopmentWithoutDevLicense` is `1` #winui #setup
- [status] `winui-setup` prerequisites are now satisfied: .NET SDK >= 8, WinApp CLI, WinUI templates and Developer Mode #winui #setup

## Update 2026-05-23 - Inicio Dashboard

- [status] Implemented `Inicio` as the default first screen with a dark dashboard layout inspired by the provided reference image #inicio #ux
- [status] Sidebar navigation now switches between `Inicio` and `Alumnos`, keeping the existing students workflow intact #navigation #winui
- [status] `Inicio` shows real SQLite-backed student totals, tag totals and recent students; notes/exams remain prepared placeholders without new tables #data #inicio
- [validation] `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore` passed with 0 warnings and 0 errors after the dashboard implementation #qa
- [risk] Manual visual validation in a live window is still pending #qa #ux
