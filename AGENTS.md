# Indice operativo de agentes

Este repositorio usa Basic Memory como memoria operativa persistente para mantener continuidad entre sesiones de desarrollo. Los antiguos documentos de memoria en `docs/` fueron migrados y ya no son la fuente de verdad.

La memoria de Basic Memory vive dentro del repositorio en `memory/` para que Git la versione junto con el codigo.

## Fuente de verdad de memoria

Proyecto Basic Memory: `appwindows`.

Ruta local versionable: `memory/`.

Notas principales:

1. `AppWindows Operational Memory`
2. `AppWindows Product Specification`
3. `AppWindows Roadmap`
4. `AppWindows Memory Migration - 2026-05-23`

URLs de referencia:

- `memory://appwindows/projects/appwindows/app-windows-operational-memory`
- `memory://appwindows/projects/appwindows/app-windows-product-specification`
- `memory://appwindows/projects/appwindows/app-windows-roadmap`
- `memory://appwindows/projects/appwindows/app-windows-memory-migration-2026-05-23`

## Orden obligatorio de lectura

Antes de continuar cualquier desarrollo, consultar Basic Memory en este orden:

1. Construir contexto desde `AppWindows Operational Memory`.
2. Leer `AppWindows Product Specification`.
3. Leer `AppWindows Roadmap`.
4. Revisar notas relacionadas si `build_context` devuelve relaciones relevantes.

Usar preferentemente las herramientas MCP de Basic Memory (`build_context`, `read_note`, `search_notes`, `edit_note`, `write_note`) en lugar de crear nuevos archivos locales de memoria.

## Reglas de uso de cada nota

### `AppWindows Operational Memory`

Mantiene el contexto acordado con el usuario y la memoria viva del trabajo.

Debe incluir:

- Contexto acordado con el usuario.
- Decisiones activas.
- Restricciones.
- Riesgos.
- Notas importantes.
- Estado del entorno.
- Implementado hasta ahora.
- Notas de build/deploy.

### `AppWindows Product Specification`

Define el producto/proyecto desde el punto de vista funcional y tecnico.

Debe incluir:

- Proposito.
- Publico objetivo.
- Requisitos funcionales.
- Requisitos no funcionales.
- Arquitectura.
- Diseno/UX si aplica.
- Integraciones externas.
- Riesgos conocidos.
- Criterios de aceptacion.

### `AppWindows Roadmap`

Registra el historico operativo del proyecto y el estado actual del trabajo.

Debe incluir:

- Historico de trabajo.
- Estado actual.
- Checklist completado.
- Checklist en curso.
- Checklist pendiente.
- Riesgos abiertos.

## Reglas de cierre de sesion

Al cerrar una sesion de trabajo:

1. Actualizar `AppWindows Operational Memory` con nuevas decisiones, validaciones, riesgos o cambios de entorno.
2. Actualizar `AppWindows Roadmap` con lo terminado y lo pendiente.
3. Ajustar `AppWindows Product Specification` si cambia el alcance funcional o tecnico del producto.
4. Dejar claro cualquier bloqueo o informacion pendiente de confirmar con el usuario.
5. No recrear `docs/agent_memory.md`, `docs/app_spec.md` ni `docs/roadmap.md`; la memoria vive en Basic Memory dentro de `memory/`.
6. Versionar los cambios relevantes de `memory/` junto con los cambios de codigo o documentacion.

## Reglas de versionado

- Crear un commit en cada progreso resenable, no solo al final.
- Hacer push tras cada commit relevante para dejar el estado respaldado en remoto cuando haya remoto configurado.
- Crear ramas nuevas, fusionarlas o reorganizar el trabajo si eso mejora la limpieza del historial.
- Priorizar un historial claro y reversible.
- No inventar decisiones importantes de producto: si falta informacion, marcarla como pendiente.
