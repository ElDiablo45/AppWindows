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

## Estado actual

Sistema de memoria operativa inicial creado. Aplicacion WinUI 3 Hello World creada. Restore y build Debug x64 validados correctamente con .NET CLI.

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

## Checklist en curso

- [ ] Confirmar si `AppWindows` es el nombre final del producto o solo el nombre del repositorio.
- [ ] Confirmar proposito, publico objetivo y alcance funcional.
- [ ] Reiniciar terminales abiertas antes del cambio de PATH o refrescar PATH manualmente.

## Checklist pendiente

- [ ] Definir requisitos funcionales del producto.
- [ ] Definir requisitos no funcionales.
- [ ] Definir arquitectura inicial.
- [ ] Definir criterios de aceptacion del producto.
- [ ] Definir comandos de validacion local.
- [ ] Configurar CI/CD si aplica.

## Riesgos abiertos

- Falta contexto de producto para tomar decisiones tecnicas importantes.
- No hay pruebas automatizadas todavia.
- Las terminales ya abiertas antes del cambio pueden no tener `dotnet` en PATH hasta reiniciarse o refrescar `$env:Path`.
- El remoto `origin/main` aparece como inexistente o no sincronizado desde el estado local inicial.
