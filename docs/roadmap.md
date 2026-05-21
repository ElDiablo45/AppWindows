# Roadmap operativo

## Historico de trabajo

### 2026-05-21

- Se inspecciono el repositorio inicial.
- Se detecto que el arbol de trabajo no contiene archivos de aplicacion.
- Se creo el sistema de memoria operativa persistente solicitado.

## Estado actual

Sistema de memoria operativa inicial creado. El producto y la arquitectura permanecen pendientes de definicion.

## Checklist completado

- [x] Crear `AGENTS.md`.
- [x] Crear `docs/agent_memory.md`.
- [x] Crear `docs/app_spec.md`.
- [x] Crear `docs/roadmap.md`.
- [x] Documentar orden obligatorio de lectura.
- [x] Documentar reglas de cierre de sesion.
- [x] Documentar reglas de versionado.
- [x] Dejar placeholders accionables donde falta contexto.

## Checklist en curso

- [ ] Confirmar si `AppWindows` es el nombre final del producto o solo el nombre del repositorio.
- [ ] Confirmar proposito, publico objetivo y alcance funcional.
- [ ] Confirmar stack tecnico y estrategia de build/test/deploy.

## Checklist pendiente

- [ ] Definir requisitos funcionales del producto.
- [ ] Definir requisitos no funcionales.
- [ ] Definir arquitectura inicial.
- [ ] Definir criterios de aceptacion del producto.
- [ ] Crear estructura base de aplicacion cuando el alcance este confirmado.
- [ ] Definir comandos de validacion local.
- [ ] Configurar CI/CD si aplica.

## Riesgos abiertos

- Falta contexto de producto para tomar decisiones tecnicas importantes.
- No hay codigo ni pruebas para validar el estado funcional del proyecto.
- El remoto `origin/main` aparece como inexistente o no sincronizado desde el estado local inicial.
