# Roadmap operativo

## Historico de trabajo

### 2026-05-21

- Se inspecciono el repositorio inicial.
- Se detecto que el arbol de trabajo no contiene archivos de aplicacion.
- Se creo el sistema de memoria operativa persistente solicitado.
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

## Estado actual

Aplicacion WinUI 3 convertida en una primera herramienta de autoescuela con modulo `Alumnos` y persistencia SQLite local. Restore y build Debug x64 validados correctamente con .NET CLI.

## Checklist completado

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

## Checklist en curso

- [ ] Reiniciar terminales abiertas antes del cambio de PATH o refrescar PATH manualmente.
- [ ] Realizar prueba manual completa de persistencia abriendo la app interactiva.
- [ ] Revisar visualmente la nueva UI en ventana real con el usuario.

## Checklist pendiente

- [ ] Definir modulo de facturas/cobros.
- [ ] Definir gestion de practicas y examenes.
- [ ] Definir importacion/exportacion compatible entre versiones.
- [ ] Definir pruebas automatizadas.
- [ ] Configurar CI/CD si aplica.

## Riesgos abiertos

- No hay pruebas automatizadas todavia.
- Las terminales ya abiertas antes del cambio pueden no tener `dotnet` en PATH hasta reiniciarse o refrescar `$env:Path`.
- Falta validar manualmente el flujo completo en una ventana interactiva.
- Falta validar visualmente proporciones, legibilidad y ergonomia en ventana real.
- La estrategia de importacion/exportacion entre versiones esta pendiente.
