---
title: AppWindows Memory Migration - 2026-05-23
type: migration_record
permalink: appwindows/projects/appwindows/app-windows-memory-migration-2026-05-23
migration_date: '2026-05-23'
repository: D:/AppWindows
source_files:
- AGENTS.md
- docs/agent_memory.md
- docs/app_spec.md
- docs/roadmap.md
tags:
- appwindows
- memory-migration
- basic-memory
---

# AppWindows Memory Migration - 2026-05-23

El usuario pidio migrar la memoria del proyecto AppWindows a Basic Memory, abandonar el flujo anterior basado en documentos locales dentro de `docs/`, y despues alojar la memoria de Basic Memory dentro del propio repositorio para que Git la versionara.

## Alcance Migrado

Se migraron los tres documentos del flujo anterior a notas estructuradas de Basic Memory:

- `docs/agent_memory.md` -> [[AppWindows Operational Memory]]
- `docs/app_spec.md` -> [[AppWindows Product Specification]]
- `docs/roadmap.md` -> [[AppWindows Roadmap]]

`AGENTS.md` dejo de exigir la lectura de esos archivos y paso a exigir la consulta de Basic Memory como fuente principal de continuidad operativa.

## Nueva Arquitectura De Memoria

- Basic Memory es la fuente de verdad para memoria operativa, especificacion funcional/tecnica, roadmap, decisiones, riesgos y pendientes.
- La memoria de Basic Memory vive dentro del repositorio en `memory/`, bajo el proyecto `appwindows`, para versionarse con Git.
- El repositorio conserva `AGENTS.md` como indice operativo para indicar como consultar y actualizar Basic Memory.
- Al iniciar trabajo, el agente debe construir contexto desde `memory://appwindows/projects/appwindows/app-windows-operational-memory` y revisar tambien las notas de especificacion y roadmap.
- Al cerrar sesion, el agente debe actualizar las notas correspondientes en Basic Memory y dejar bloqueos o informacion pendiente claramente registrados.

## Observations

- [decision] AppWindows abandona el flujo de memoria basado en `docs/agent_memory.md`, `docs/app_spec.md` y `docs/roadmap.md` #memory
- [decision] Basic Memory es la nueva fuente persistente principal para memoria del proyecto #memory
- [decision] Versionar los archivos markdown de Basic Memory dentro del repositorio #memory
- [migration] `docs/agent_memory.md` fue migrado a [[AppWindows Operational Memory]] #memory
- [migration] `docs/app_spec.md` fue migrado a [[AppWindows Product Specification]] #memory
- [migration] `docs/roadmap.md` fue migrado a [[AppWindows Roadmap]] #memory
- [migration] El 2026-05-23 se creo el proyecto Basic Memory `appwindows` en `D:\AppWindows\memory` #memory
- [instruction] `AGENTS.md` debe describir la nueva arquitectura de memoria con Basic Memory #agents
- [cleanup] Los archivos antiguos de memoria local deben eliminarse al terminar la migracion #cleanup

## Relations
- created [[AppWindows Operational Memory]]
- created [[AppWindows Product Specification]]
- created [[AppWindows Roadmap]]
- supersedes [[docs/agent_memory.md]]
- supersedes [[docs/app_spec.md]]
- supersedes [[docs/roadmap.md]]