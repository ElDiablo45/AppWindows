# Memoria operativa del agente

## Nombre del producto/proyecto

Pendiente de definir. El repositorio remoto se llama `AppWindows`, pero no hay todavia archivos de aplicacion que confirmen el nombre publico del producto.

## Objetivo principal

Pendiente de definir con el usuario. En esta sesion se esta creando la memoria operativa persistente del repositorio para dar continuidad al trabajo con agentes.

## Decisiones activas

- Usar `AGENTS.md` como indice operativo principal del repositorio.
- Leer siempre la documentacion persistente en este orden antes de desarrollar:
  1. `docs/agent_memory.md`
  2. `docs/app_spec.md`
  3. `docs/roadmap.md`
- Mantener `agent_memory.md`, `app_spec.md` y `roadmap.md` actualizados al cierre de cada sesion.
- No inventar decisiones de producto importantes; marcar como pendiente todo contexto no confirmado.

## Contexto funcional

- No hay todavia funcionalidad de aplicacion visible en el arbol de trabajo.
- El alcance funcional del producto esta pendiente de especificacion.

## Restricciones y notas

- Repositorio inspeccionado el 2026-05-21.
- El arbol de trabajo inicial no contiene archivos de aplicacion rastreables.
- El repositorio Git existe y apunta al remoto `origin`.
- La rama local actual es `main`.
- El remoto configurado es `https://github.com/ElDiablo45/AppWindows.git`.
- El estado inicial indicaba `origin/main [gone]`, por lo que el remoto puede no tener todavia esa rama o puede requerir sincronizacion.

## Estado del entorno

- Sistema operativo del entorno de trabajo: Windows/PowerShell.
- Directorio de trabajo declarado por el usuario: `d:\AppWindows`.
- Sandbox de ejecucion activo con permisos de escritura en el workspace.
- Fecha de referencia de la sesion: 2026-05-21.

## Implementado hasta ahora

- Sistema de memoria operativa persistente creado en documentacion:
  - `AGENTS.md`
  - `docs/agent_memory.md`
  - `docs/app_spec.md`
  - `docs/roadmap.md`

## Notas de build/deploy

- Pendiente. No hay stack, comandos de build, pruebas o deploy definidos en el repositorio actual.

## Riesgos activos

- Falta definicion del producto, objetivo, publico, arquitectura y criterios de aceptacion.
- No hay codigo de aplicacion ni configuracion de build/test para validar comportamiento tecnico.
- El remoto `origin/main` aparece como inexistente o no sincronizado en el estado local inicial.
