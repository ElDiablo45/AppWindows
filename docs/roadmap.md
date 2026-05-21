# Roadmap operativo

## Historico de trabajo

### 2026-05-21

- Se inspecciono el repositorio inicial.
- Se detecto que el arbol de trabajo no contiene archivos de aplicacion.
- Se creo el sistema de memoria operativa persistente solicitado.
- Se creo una aplicacion inicial WinUI 3 + Windows App SDK con una ventana "Hello world".
- Se intento validar con MSBuild, pero el entorno no pudo resolver `Microsoft.NET.Sdk`.

## Estado actual

Sistema de memoria operativa inicial creado. Aplicacion WinUI 3 Hello World creada. La compilacion local esta bloqueada por falta de .NET SDK/configuracion WinUI en el entorno actual.

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

## Checklist en curso

- [ ] Confirmar si `AppWindows` es el nombre final del producto o solo el nombre del repositorio.
- [ ] Confirmar proposito, publico objetivo y alcance funcional.
- [ ] Instalar/configurar .NET SDK y workload/soporte WinUI para compilar localmente.

## Checklist pendiente

- [ ] Definir requisitos funcionales del producto.
- [ ] Definir requisitos no funcionales.
- [ ] Definir arquitectura inicial.
- [ ] Definir criterios de aceptacion del producto.
- [ ] Definir comandos de validacion local.
- [ ] Ejecutar `dotnet restore` y `dotnet build` cuando `dotnet` este disponible.
- [ ] Configurar CI/CD si aplica.

## Riesgos abiertos

- Falta contexto de producto para tomar decisiones tecnicas importantes.
- No hay pruebas automatizadas todavia.
- El entorno actual no tiene `dotnet` en PATH y MSBuild no resuelve `Microsoft.NET.Sdk`.
- El remoto `origin/main` aparece como inexistente o no sincronizado desde el estado local inicial.
