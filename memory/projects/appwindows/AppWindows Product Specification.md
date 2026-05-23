---
title: AppWindows Product Specification
type: product_spec
permalink: appwindows/projects/appwindows/app-windows-product-specification
migrated_from: D:/AppWindows/docs/app_spec.md
migration_date: '2026-05-23'
repository: D:/AppWindows
tags:
- appwindows
- product-spec
- winui
- sqlite
- autoescuela
---

# AppWindows Product Specification

Especificacion funcional y tecnica de AppWindows, migrada desde `docs/app_spec.md` el 2026-05-23 para que Basic Memory sea la fuente persistente de contexto del proyecto.

## Proposito

AppWindows es una aplicacion de escritorio para Windows creada con WinUI 3 y Windows App SDK para gestionar una autoescuela. El alcance funcional actual es el modulo `Alumnos`, con persistencia local en SQLite.

## Publico Objetivo

Personal de una autoescuela que necesita registrar y consultar alumnos, carnets asociados y datos basicos de contacto.

## Requisitos Funcionales

- La aplicacion debe abrir una ventana de escritorio.
- La pantalla principal debe mostrar el modulo `Alumnos`.
- El usuario debe poder crear alumnos con nombre, DNI/NIE, telefono, fecha de alta automatica, observaciones y tags/carnets.
- Nombre, DNI/NIE y telefono son obligatorios.
- DNI/NIE debe ser unico, sin validar checksum en v1.
- El usuario debe poder buscar por nombre, DNI/NIE o telefono.
- El usuario debe poder filtrar por carnet/tag.
- El usuario debe poder abrir detalle y editar datos/tags.
- El usuario debe poder crear tags personalizados.
- No debe existir borrado de alumnos en v1.

Pendiente:

- Definir facturas, cobros, practicas, examenes y profesores.
- Definir importacion/exportacion entre versiones.

## Requisitos No Funcionales

- Plataforma objetivo inicial: Windows 10 version 1809 o superior.
- Framework inicial: `net8.0-windows10.0.19041.0`.
- UI: WinUI 3.
- SDK de plataforma: Windows App SDK.
- Persistencia local: SQLite en `%LocalAppData%\AppWindows\appwindows.db`.
- Esquema versionado con `AppMetadata.schema_version`.

Pendiente:

- Requisitos de rendimiento.
- Requisitos de seguridad y privacidad.
- Requisitos de accesibilidad.
- Requisitos de internacionalizacion/localizacion.
- Requisitos de observabilidad, logging o auditoria.

## Arquitectura

Arquitectura inicial:

- Solucion: `AppWindows.sln`.
- Proyecto principal: `src/AppWindows/AppWindows.csproj`.
- Tipo de salida: `WinExe`.
- Tipo de empaquetado inicial: unpackaged con `WindowsPackageType=None`.
- UI declarativa en XAML: `App.xaml`, `MainWindow.xaml`.
- Codigo C# code-behind: `App.xaml.cs`, `MainWindow.xaml.cs`.
- Dependencia principal: `Microsoft.WindowsAppSDK` version `2.0.1`.
- Dependencia de datos: `Microsoft.Data.Sqlite` version `10.0.6`.
- Configuracion NuGet local: `NuGet.Config`.
- Plataforma de build recomendada por defecto: x64.
- Capas internas iniciales: modelos simples `Student` y `Tag`, `DatabaseService`, `StudentRepository`.
- Tablas SQLite v1: `Students`, `Tags`, `StudentTags`, `AppMetadata`.

Pendiente:

- Definir estrategia de configuracion por entorno.
- Definir estructura de modulos cuando exista funcionalidad real.

## Diseno Y UX

- Interfaz grafica de escritorio con WinUI 3.
- Pantalla principal de `Alumnos` con tema oscuro tipo panel administrativo.
- Menu lateral izquierdo con `Inicio` y `Alumnos`.
- Cabecera superior con textura lineal sutil.
- Buscador, filtro por carnet/tag y accion `Nuevo`.
- Tabla principal de alumnos con descripcion, telefono, tags y fecha de alta.
- Panel derecho de detalle/edicion.

Pendiente:

- Definir principios visuales y ergonomicos del producto real.
- Definir estados vacios, carga, error y exito.

## Integraciones Externas

Pendiente de definir.

Estado actual:

- Solo se conoce el remoto Git `origin` en GitHub.
- No hay APIs, servicios externos o credenciales documentadas.
- Dependencia externa de plataforma: `Microsoft.WindowsAppSDK`.
- Dependencia SQLite: `Microsoft.Data.Sqlite`.

## Riesgos Conocidos

- El alcance funcional posterior a alumnos todavia no esta especificado.
- La terminal actual puede necesitar refrescar PATH tras instalar .NET/Visual Studio Build Tools.
- No hay pruebas automatizadas todavia.
- La estrategia de importacion/exportacion compatible entre versiones esta pendiente.

## Criterios De Aceptacion

Criterios del modulo `Alumnos`:

- La solucion `AppWindows.sln` existe y compila.
- La app arranca una ventana WinUI 3 con el modulo `Alumnos`.
- La base SQLite local se crea automaticamente.
- El usuario puede crear un alumno con nombre, DNI/NIE, telefono y carnet `B`.
- El usuario no puede crear dos alumnos con el mismo DNI/NIE.
- El usuario puede crear un tag personalizado y asignarlo.
- El usuario puede buscar por nombre, DNI/NIE o telefono.
- El usuario puede filtrar por carnet/tag.
- El usuario puede editar datos y tags de un alumno.
- La pantalla principal sigue la direccion visual oscura acordada: sidebar, cabecera, tabla y panel de detalle.
- Los datos persisten tras cerrar y abrir la app.
- `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore` compila sin errores despues de restaurar dependencias.

Criterios pendientes:

- Definir criterios de aceptacion del producto real.

## Observations

- [purpose] AppWindows gestiona una autoescuela desde una aplicacion de escritorio Windows #product
- [audience] El publico objetivo es personal de autoescuela que registra y consulta alumnos #product
- [requirement] La pantalla principal debe mostrar el modulo `Alumnos` #students
- [requirement] Nombre, DNI/NIE y telefono son obligatorios para crear alumnos #students
- [requirement] DNI/NIE debe ser unico sin validar checksum en v1 #students
- [requirement] El usuario puede buscar por nombre, DNI/NIE o telefono #students
- [requirement] El usuario puede filtrar por carnet o tag #students
- [requirement] El usuario puede crear tags personalizados #students
- [constraint] No debe existir borrado de alumnos en v1 #students
- [architecture] La app usa WinUI 3, Windows App SDK, C# y XAML #architecture
- [architecture] La persistencia local usa SQLite con esquema versionado #data
- [dependency] `Microsoft.WindowsAppSDK` version `2.0.1` #nuget
- [dependency] `Microsoft.Data.Sqlite` version `10.0.6` #nuget
- [acceptance] El build Debug x64 debe compilar sin errores despues de restaurar dependencias #qa
- [pending] Definir facturas, cobros, practicas, examenes y profesores #roadmap
- [pending] Definir importacion/exportacion compatible entre versiones #roadmap

## Relations

- informs [[AppWindows Operational Memory]]
- tracked_by [[AppWindows Roadmap]]
- migrated_by [[AppWindows Memory Migration - 2026-05-23]]