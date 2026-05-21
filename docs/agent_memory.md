# Memoria operativa del agente

## Nombre del producto/proyecto

AppWindows. El nombre queda tomado del repositorio y de la aplicacion inicial WinUI 3.

## Objetivo principal

Crear una aplicacion de escritorio para Windows con WinUI 3 y Windows App SDK. El primer hito es una ventana "Hello world".

## Decisiones activas

- Usar `AGENTS.md` como indice operativo principal del repositorio.
- Leer siempre la documentacion persistente en este orden antes de desarrollar:
  1. `docs/agent_memory.md`
  2. `docs/app_spec.md`
  3. `docs/roadmap.md`
- Mantener `agent_memory.md`, `app_spec.md` y `roadmap.md` actualizados al cierre de cada sesion.
- No inventar decisiones de producto importantes; marcar como pendiente todo contexto no confirmado.
- Usar WinUI 3 con Windows App SDK para la interfaz de escritorio.
- Iniciar el proyecto como una aplicacion unpackaged (`WindowsPackageType=None`) para mantener el arranque simple.
- Referenciar `Microsoft.WindowsAppSDK` version `2.0.1`.
- Target framework inicial: `net8.0-windows10.0.19041.0`.

## Contexto funcional

- La aplicacion inicial muestra una ventana con el texto "Hello world".
- El alcance funcional posterior esta pendiente de especificacion.

## Restricciones y notas

- Repositorio inspeccionado el 2026-05-21.
- El arbol de trabajo inicial no contiene archivos de aplicacion rastreables.
- El repositorio Git existe y apunta al remoto `origin`.
- La rama local actual es `main`.
- El remoto configurado es `https://github.com/ElDiablo45/AppWindows.git`.
- El estado inicial indicaba `origin/main [gone]`, por lo que el remoto puede no tener todavia esa rama o puede requerir sincronizacion.
- Visual Studio Build Tools 2022 esta instalado.
- Se instalo la carga `Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools`.
- `dotnet.exe` esta disponible en `C:\Program Files\dotnet\dotnet.exe`.
- `C:\Program Files\dotnet` esta en el PATH de maquina y tambien se agrego al PATH de usuario el 2026-05-21.
- Se emitio una notificacion de cambio de entorno (`WM_SETTINGCHANGE`) para que nuevos procesos detecten el PATH actualizado.
- Las terminales abiertas antes del cambio pueden requerir refrescar PATH manualmente o reiniciarse.

## Estado del entorno

- Sistema operativo del entorno de trabajo: Windows/PowerShell.
- Directorio de trabajo declarado por el usuario: `d:\AppWindows`.
- Sandbox de ejecucion activo con permisos de escritura en el workspace.
- Fecha de referencia de la sesion: 2026-05-21.
- MSBuild detectado en `C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe`.
- .NET SDKs instalados: `9.0.314` y `10.0.300`.
- SDK predeterminado actual: `10.0.300`.
- Runtimes instalados: .NET 8.0.27, .NET 9.0.16 y .NET 10.0.8.

## Implementado hasta ahora

- Sistema de memoria operativa persistente creado en documentacion:
  - `AGENTS.md`
  - `docs/agent_memory.md`
  - `docs/app_spec.md`
  - `docs/roadmap.md`
- Aplicacion WinUI 3 Hello World creada:
  - `AppWindows.sln`
  - `src/AppWindows/AppWindows.csproj`
  - `src/AppWindows/App.xaml`
  - `src/AppWindows/MainWindow.xaml`
  - `src/AppWindows/app.manifest`
  - `.gitignore`
  - `README.md`
- Configuracion local de NuGet creada en `NuGet.Config` para restaurar desde `nuget.org` sin depender del perfil de usuario.
- Build Debug x64 validado correctamente el 2026-05-21.

## Notas de build/deploy

- Stack inicial: WinUI 3 + Windows App SDK + .NET 8 para Windows.
- Comandos previstos cuando `dotnet` este disponible:
  - `dotnet restore`
  - `dotnet build -c Debug -p:Platform=x64`
  - `dotnet run --project src/AppWindows/AppWindows.csproj`
- Validacion intentada con MSBuild el 2026-05-21:
  - Comando: `MSBuild.exe AppWindows.sln /restore /p:Configuration=Debug /p:Platform=x64`
  - Resultado: fallo porque no se pudo resolver `Microsoft.NET.Sdk`.
- Validacion posterior con .NET CLI el 2026-05-21:
  - Restore: correcto usando `NuGet.Config`.
  - Build: correcto con `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore`.
- Validacion posterior con SDK predeterminado .NET `10.0.300` el 2026-05-21:
  - Build Debug x64 correcto con 0 errores y 0 advertencias.

## Riesgos activos

- Falta definicion del producto, publico, alcance funcional posterior y criterios de aceptacion mas alla del Hello World.
- Las terminales abiertas antes de configurar PATH pueden no encontrar `dotnet`; solucion temporal:
  - `$env:Path = [Environment]::GetEnvironmentVariable('Path','Machine') + ';' + [Environment]::GetEnvironmentVariable('Path','User')`
- El remoto `origin/main` aparece como inexistente o no sincronizado en el estado local inicial.
