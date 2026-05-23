---
title: AppWindows Roadmap
type: roadmap
permalink: appwindows/projects/appwindows/app-windows-roadmap
migrated_from: D:/AppWindows/docs/roadmap.md
migration_date: '2026-05-23'
repository: D:/AppWindows
tags:
- appwindows
- roadmap
- work-history
- winui
- autoescuela
---

# AppWindows Roadmap

Roadmap operativo de AppWindows, migrado desde `docs/roadmap.md` el 2026-05-23 para centralizar el historial y los pendientes en Basic Memory.

## Historico De Trabajo

### 2026-05-21

- Se inspecciono el repositorio inicial.
- Se detecto que el arbol de trabajo no contiene archivos de aplicacion.
- Se creo el sistema anterior de memoria operativa persistente solicitado.
- Se creo una aplicacion inicial WinUI 3 + Windows App SDK con una ventana "Hello world".
- Se intento validar con MSBuild, pero el entorno no pudo resolver `Microsoft.NET.Sdk`.
- Se instalaron componentes de Visual Studio Build Tools para escritorio .NET.
- Se restauro NuGet y se compilo correctamente Debug x64 con .NET CLI.
- Se agrego `C:\Program Files\dotnet` al PATH de usuario para que PowerShell pueda resolver `dotnet`.
- Se emitio notificacion de cambio de entorno y se valido build Debug x64 con SDK predeterminado .NET `10.0.300`.

### 2026-05-22

- Se definio el primer modulo real de producto: `Alumnos` para autoescuela.
- Se decidio usar SQLite local desde v1.
- Se implemento esquema SQLite v1 con `Students`, `Tags`, `StudentTags` y `AppMetadata`.
- Se implemento listado, busqueda, filtro por tag, alta, detalle y edicion sin borrado.
- Se agrego seed de carnets predefinidos y creacion de tags personalizados.
- Se valido restore y build Debug x64 sin errores ni advertencias.
- Se adapto la interfaz a una direccion visual oscura tipo panel administrativo con menu lateral, cabecera, tabla y panel de detalle.
- Se valido build Debug x64 del rediseño sin errores ni advertencias.

### 2026-05-23

- Se localizo Python 3.14.5 instalado fuera del PATH resoluble por el sandbox, en `C:\Users\mdavi\AppData\Local\Programs\Python\Python314`.
- Se confirmo `winget` version `1.28.240` fuera del sandbox.
- Se creo el entorno virtual Python del repositorio en `D:\AppWindows\.venv`.
- Se valido que `.venv` usa Python `3.14.5` y `pip 26.1.1`.
- Se agrego `.venv/` a `.gitignore`.
- Se diagnostico `bm` no reconocido tras `uv tool install basic-memory`.
- Se confirmo `basic-memory v0.21.1` instalado con comandos `basic-memory` y `bm`.
- Se agrego `C:\Users\mdavi\.local\bin` al PATH de usuario para resolver ejecutables globales de `uv tool`.
- Se valido `bm --help` correctamente.
- Se migro la memoria operativa del repositorio a Basic Memory y se preparo el abandono del flujo basado en `docs/*.md`.
- Se creo el proyecto Basic Memory `appwindows` en `D:\AppWindows\memory` para versionar la memoria con Git.
- Se ejecuto `memory-defrag`, manteniendo la estructura central y creando un log diario.

## Estado Actual

Aplicacion WinUI 3 convertida en una primera herramienta de autoescuela con modulo `Alumnos` y persistencia SQLite local. Restore y build Debug x64 validados correctamente con .NET CLI. Desde el 2026-05-23 la memoria operativa principal vive en Basic Memory dentro del repositorio.

## Checklist Completado

- [x] Crear `AGENTS.md`.
- [x] Crear `docs/agent_memory.md`.
- [x] Crear `docs/app_spec.md`.
- [x] Crear `docs/roadmap.md`.
- [x] Documentar orden obligatorio de lectura.
- [x] Documentar reglas de cierre de sesion.
- [x] Documentar reglas de versionado.
- [x] Dejar placeholders accionables donde falta contexto.
- [x] Crear solucion `AppWindows.sln`.
- [x] Crear proyecto WinUI 3 en `src/AppWindows`.
- [x] Crear ventana principal con `Hello world`.
- [x] Documentar comandos previstos de restore/build/run en `README.md`.
- [x] Instalar/configurar soporte .NET de Visual Studio Build Tools.
- [x] Crear `NuGet.Config` local.
- [x] Restaurar dependencias NuGet.
- [x] Compilar Debug x64 sin errores.
- [x] Agregar `C:\Program Files\dotnet` al PATH de usuario.
- [x] Validar build con SDK predeterminado .NET `10.0.300`.
- [x] Definir primer alcance funcional de autoescuela.
- [x] Agregar `Microsoft.Data.Sqlite`.
- [x] Crear esquema SQLite v1 versionado.
- [x] Crear seed de carnets predefinidos.
- [x] Implementar alta de alumnos.
- [x] Implementar DNI/NIE obligatorio y unico.
- [x] Implementar listado con busqueda y filtro por tag.
- [x] Implementar detalle editable sin borrado.
- [x] Implementar tags personalizados.
- [x] Adaptar visualmente `Alumnos` a panel oscuro con sidebar y tabla.
- [x] Crear entorno virtual Python 3.14.5 en `.venv`.
- [x] Ignorar `.venv/` desde `.gitignore`.
- [x] Configurar PATH de usuario para ejecutar tools globales de `uv`.
- [x] Validar comando `bm` de Basic Memory.
- [x] Migrar memoria operativa, especificacion y roadmap a Basic Memory.
- [x] Crear proyecto Basic Memory `appwindows` dentro del repositorio en `memory/`.
- [x] Ejecutar defrag de memoria y registrar log diario.

## Checklist En Curso

- [ ] Reiniciar terminales abiertas antes del cambio de PATH o refrescar PATH manualmente.
- [ ] Realizar prueba manual completa de persistencia abriendo la app interactiva.
- [ ] Revisar visualmente la nueva UI en ventana real con el usuario.

## Checklist Pendiente

- [ ] Definir modulo de facturas/cobros.
- [ ] Definir gestion de practicas y examenes.
- [ ] Definir importacion/exportacion compatible entre versiones.
- [ ] Definir pruebas automatizadas.
- [ ] Configurar CI/CD si aplica.

## Riesgos Abiertos

- No hay pruebas automatizadas todavia.
- Las terminales ya abiertas antes del cambio pueden no tener `dotnet` en PATH hasta reiniciarse o refrescar `$env:Path`.
- Falta validar manualmente el flujo completo en una ventana interactiva.
- Falta validar visualmente proporciones, legibilidad y ergonomia en ventana real.
- La estrategia de importacion/exportacion entre versiones esta pendiente.

## Observations

- [history] El 2026-05-21 se creo la aplicacion inicial WinUI 3 y se valido build Debug x64 #timeline
- [history] El 2026-05-22 se implemento el modulo `Alumnos` con SQLite local #timeline
- [history] El 2026-05-22 se adapto la interfaz a un panel oscuro administrativo #timeline
- [history] El 2026-05-23 se valido Python, uv y Basic Memory #timeline
- [history] El 2026-05-23 se migro la memoria operativa del repositorio a Basic Memory #memory
- [history] El 2026-05-23 se creo el proyecto Basic Memory `appwindows` en `D:\AppWindows\memory` para versionar la memoria con Git #memory
- [history] Commit local `323bfa9` creado para migrar la memoria del proyecto a Basic Memory #git
- [history] Memory defrag ran on 2026-05-23 and refreshed Basic Memory relation sections #memory
- [status] La app actual tiene modulo `Alumnos` funcional con persistencia SQLite local #status
- [todo] Realizar prueba manual completa de persistencia abriendo la app interactiva #qa
- [todo] Revisar visualmente la nueva UI en ventana real con el usuario #ux
- [todo] Definir modulo de facturas y cobros #roadmap
- [todo] Definir gestion de practicas y examenes #roadmap
- [todo] Definir importacion/exportacion compatible entre versiones #roadmap
- [todo] Definir pruebas automatizadas #testing
- [risk] Falta validar manualmente el flujo completo #qa
- [risk] Falta definir alcance funcional posterior a alumnos #product
- [blocked] `git push origin main` requiere aprobacion explicita del usuario por publicar en el remoto GitHub #git
- [history] Installed Microsoft WinUI development skills from `microsoft/win-dev-skills` on 2026-05-23 #skills #winui

## Relations

- tracks [[AppWindows Operational Memory]]
- tracks [[AppWindows Product Specification]]
- migrated_by [[AppWindows Memory Migration - 2026-05-23]]

## Update 2026-05-23 - WinUI Setup

- [history] Ran `winui-setup` and verified `.NET SDK >= 8`; installed `Microsoft.WinAppCli` version `0.3.1` and WinUI 3 .NET templates #winui #setup
- [status] `winapp` and WinUI templates are available outside the sandbox after PATH refresh; sandbox shell may still need refreshed alias/PATH context #winui #environment
- [todo] Enable Developer Mode if the user approves the required UAC/admin elevation #winui #setup
