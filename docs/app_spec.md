# Especificacion del producto

## Proposito

AppWindows es una aplicacion de escritorio para Windows creada con WinUI 3 y Windows App SDK. El alcance inicial confirmado es mostrar una ventana "Hello world".

## Publico objetivo

Pendiente de definir con el usuario. De momento se asume un usuario de escritorio Windows solo para validar el arranque tecnico.

## Requisitos funcionales

- La aplicacion debe abrir una ventana de escritorio.
- La ventana principal debe mostrar el texto `Hello world`.
- La ventana debe indicar que usa WinUI 3 + Windows App SDK.

Pendiente:

- Definir el problema principal que resolvera la aplicacion despues del Hello World.
- Definir usuarios o roles principales.
- Definir flujos principales de uso.
- Definir datos que la aplicacion debe crear, leer, actualizar o eliminar.
- Definir comportamiento esperado fuera del camino feliz.

## Requisitos no funcionales

- Plataforma objetivo inicial: Windows 10 version 1809 o superior.
- Framework inicial: `net8.0-windows10.0.19041.0`.
- UI: WinUI 3.
- SDK de plataforma: Windows App SDK.

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

Pendiente:

- Definir almacenamiento local o remoto si el producto lo necesita.
- Definir estrategia de configuracion por entorno.
- Definir estructura de modulos cuando exista funcionalidad real.

## Diseno/UX

- Interfaz grafica de escritorio con WinUI 3.
- Pantalla inicial: ventana centrada con texto `Hello world` y subtitulo `WinUI 3 + Windows App SDK`.

Pendiente:

- Definir principios visuales y ergonomicos del producto real.
- Definir vistas o pantallas principales.
- Definir estados vacios, carga, error y exito.

## Integraciones externas

Pendiente de definir.

Estado actual:

- Solo se conoce el remoto Git `origin` en GitHub.
- No hay APIs, servicios externos o credenciales documentadas.
- La dependencia externa de plataforma es el paquete NuGet `Microsoft.WindowsAppSDK`.

## Riesgos conocidos

- El alcance funcional posterior al Hello World todavia no esta especificado.
- El entorno actual no tiene `dotnet` disponible y MSBuild no puede resolver `Microsoft.NET.Sdk`.
- No hay contrato de datos ni arquitectura de persistencia.

## Criterios de aceptacion

Criterios iniciales del Hello World:

- La solucion `AppWindows.sln` existe.
- El proyecto `src/AppWindows/AppWindows.csproj` referencia Windows App SDK.
- La app arranca una ventana WinUI 3.
- La ventana principal muestra `Hello world`.
- El README documenta como ejecutar el proyecto cuando el entorno tenga `dotnet`/Visual Studio configurado.

Criterios pendientes:

- Compilar localmente cuando se instale/configure `Microsoft.NET.Sdk`.
- Definir criterios de aceptacion del producto real.
