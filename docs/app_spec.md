# Especificacion del producto

## Proposito

AppWindows es una aplicacion de escritorio para Windows creada con WinUI 3 y Windows App SDK para gestionar una autoescuela. El alcance funcional actual es el modulo `Alumnos`, con persistencia local en SQLite.

## Publico objetivo

Personal de una autoescuela que necesita registrar y consultar alumnos, carnets asociados y datos basicos de contacto.

## Requisitos funcionales

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

## Requisitos no funcionales

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
- UI declarativa en XAML:
  - `App.xaml`
  - `MainWindow.xaml`
- Codigo C# code-behind:
  - `App.xaml.cs`
  - `MainWindow.xaml.cs`
- Dependencia principal: `Microsoft.WindowsAppSDK` version `2.0.1`.
- Dependencia de datos: `Microsoft.Data.Sqlite` version `10.0.6`.
- Configuracion NuGet local: `NuGet.Config`.
- Plataforma de build recomendada por defecto: x64.
- Capas internas iniciales:
  - Modelos simples `Student` y `Tag`.
  - `DatabaseService` para ruta, inicializacion, esquema v1 y seed de tags.
  - `StudentRepository` para listar, buscar, filtrar, crear, editar y crear tags.
- Tablas SQLite v1:
  - `Students`
  - `Tags`
  - `StudentTags`
  - `AppMetadata`

Pendiente:

- Definir estrategia de configuracion por entorno.
- Definir estructura de modulos cuando exista funcionalidad real.

## Diseno/UX

- Interfaz grafica de escritorio con WinUI 3.
- Pantalla principal de `Alumnos`:
  - Cabecera con accion `Nuevo alumno`.
  - Buscador y filtro por carnet/tag.
  - Lista de alumnos.
  - Panel de detalle/edicion.

Pendiente:

- Definir principios visuales y ergonomicos del producto real.
- Definir estados vacios, carga, error y exito.

## Integraciones externas

Pendiente de definir.

Estado actual:

- Solo se conoce el remoto Git `origin` en GitHub.
- No hay APIs, servicios externos o credenciales documentadas.
- La dependencia externa de plataforma es el paquete NuGet `Microsoft.WindowsAppSDK`.
- La dependencia SQLite es el paquete NuGet `Microsoft.Data.Sqlite`.

## Riesgos conocidos

- El alcance funcional posterior a alumnos todavia no esta especificado.
- La terminal actual puede necesitar refrescar PATH tras instalar .NET/Visual Studio Build Tools.
- No hay pruebas automatizadas todavia.
- La estrategia de importacion/exportacion compatible entre versiones esta pendiente.

## Criterios de aceptacion

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
- Los datos persisten tras cerrar y abrir la app.
- `dotnet build AppWindows.sln -c Debug -p:Platform=x64 --no-restore` compila sin errores despues de restaurar dependencias.

Criterios pendientes:

- Definir criterios de aceptacion del producto real.
