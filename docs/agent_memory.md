# Memoria operativa del agente

## Nombre del producto/proyecto

AppWindows. El nombre queda tomado del repositorio y de la aplicacion inicial WinUI 3.

## Objetivo principal

Crear una aplicacion de escritorio para Windows con WinUI 3 y Windows App SDK orientada a la gestion de una autoescuela. El primer modulo funcional es la gestion local de alumnos.

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
- Usar SQLite local para persistencia inicial.
- Referenciar `Microsoft.Data.Sqlite` version `10.0.6`.
- Guardar la base de datos en `%LocalAppData%\AppWindows\appwindows.db`.
- El primer modulo se llama `Alumnos`.
- En v1 no hay borrado de alumnos; se permite crear, listar, buscar, filtrar, ver detalle y editar.
- La direccion visual acordada para la app es un panel oscuro de gestion: menu lateral izquierdo, cabecera superior, tabla/listado principal y ficha de detalle integrada.

## Contexto funcional

- La aplicacion gestiona alumnos de autoescuela.
- Campos v1 de alumno: nombre, DNI/NIE, telefono, fecha de alta automatica, observaciones y tags/carnets.
- DNI/NIE es obligatorio y unico. No se valida checksum en v1.
- Tags iniciales de carnet: `AM`, `A1`, `A2`, `A`, `B`, `B+E`, `C1`, `C1+E`, `C`, `C+E`, `D1`, `D1+E`, `D`, `D+E`.
- Se permiten tags personalizados.

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
- Python 3.14.5 instalado en `C:\Users\mdavi\AppData\Local\Programs\Python\Python314`.
- `winget` esta instalado y se resuelve fuera del sandbox como version `1.28.240`.
- El PATH activo incluye `C:\Users\mdavi\AppData\Local\Microsoft\WindowsApps`, pero el sandbox puede bloquear el acceso a aliases como `python.exe` y `winget.exe` con `Acceso denegado`; usar ejecucion escalada si hay que consultar esos aliases.

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
- Modulo `Alumnos` implementado el 2026-05-22:
  - UI WinUI con listado, busqueda, filtro por tag, alta y detalle editable.
  - Persistencia SQLite local con esquema v1.
  - Seed automatico de carnets predefinidos.
- Estilo visual del modulo `Alumnos` adaptado el 2026-05-22 a una interfaz oscura inspirada en panel administrativo:
  - Menu lateral con `Inicio` y `Alumnos`.
  - Cabecera superior con textura lineal sutil.
  - Tabla de alumnos con columnas de descripcion, telefono, tags y fecha de alta.
  - Panel derecho para alta/edicion.
- Entorno virtual Python creado el 2026-05-23:
  - Ruta: `D:\AppWindows\.venv`.
  - Version validada: Python `3.14.5`.
  - `pip` validado: `26.1.1`.
  - Base del venv: `C:\Users\mdavi\AppData\Local\Programs\Python\Python314`.

## Notas de build/deploy

- Stack inicial: WinUI 3 + Windows App SDK + .NET 8 para Windows.
- Persistencia: SQLite local con `Microsoft.Data.Sqlite`.
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
- Validacion del modulo `Alumnos` el 2026-05-22:
  - `dotnet restore AppWindows.sln --configfile D:\AppWindows\NuGet.Config`
  - `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore`
  - Resultado: 0 errores y 0 advertencias.
- Validacion visual del rediseño oscuro el 2026-05-22:
  - `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore`
  - Resultado: 0 errores y 0 advertencias.
- Validacion del entorno Python el 2026-05-23:
  - `.venv\Scripts\python.exe --version`
  - Resultado: `Python 3.14.5`.
  - `.venv\Scripts\python.exe -m pip --version`
  - Resultado: `pip 26.1.1` desde `D:\AppWindows\.venv\Lib\site-packages\pip`.

## Riesgos activos

- Falta definir modulos posteriores: facturas, cobros, practicas, examenes, profesores y posibles importaciones/exportaciones entre versiones.
- Las terminales abiertas antes de configurar PATH pueden no encontrar `dotnet`; solucion temporal:
  - `$env:Path = [Environment]::GetEnvironmentVariable('Path','Machine') + ';' + [Environment]::GetEnvironmentVariable('Path','User')`
- No hay pruebas automatizadas todavia; la validacion actual es build y pruebas manuales previstas.
