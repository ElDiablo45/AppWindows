# Indice operativo de agentes

Este repositorio usa un sistema de memoria operativa persistente para mantener continuidad entre sesiones de desarrollo.

## Orden obligatorio de lectura

Antes de continuar cualquier desarrollo, leer y aplicar estos documentos en este orden:

1. `docs/agent_memory.md`
2. `docs/app_spec.md`
3. `docs/roadmap.md`

## Reglas de uso de cada documento

### `docs/agent_memory.md`

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

### `docs/app_spec.md`

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

### `docs/roadmap.md`

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

1. Actualizar `docs/agent_memory.md` con nuevas decisiones, validaciones o riesgos.
2. Actualizar `docs/roadmap.md` con lo terminado y lo pendiente.
3. Ajustar `docs/app_spec.md` si cambia el alcance funcional o tecnico del producto.
4. Dejar claro cualquier bloqueo o informacion pendiente de confirmar con el usuario.

## Reglas de versionado

- Crear un commit en cada progreso resenable, no solo al final.
- Hacer push tras cada commit relevante para dejar el estado respaldado en remoto cuando haya remoto configurado.
- Crear ramas nuevas, fusionarlas o reorganizar el trabajo si eso mejora la limpieza del historial.
- Priorizar un historial claro y reversible.
- No inventar decisiones importantes de producto: si falta informacion, marcarla como pendiente.
