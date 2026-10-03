# AMJ Environment Coordination

This file is the authoritative coordination surface for **Ancient & Medieval Japan: Environment**.

## Working rule

1. Read `AGENTS.md`.
2. Read this file from `main`.
3. Check OPEN / IN PROGRESS items before starting new work.
4. Put durable decisions into `Docs/Design.md` and implementation files.
5. Use this file only for handoff, status, blockers, and cross-repository coordination.

## Source of truth

Primary design source:

`Docs/Design.md`

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — being investigated or implemented
- **BLOCKED** — waiting on a prerequisite
- **DONE** — completed and reflected in the proper source of truth
- **ARCHIVED** — retained for history only

## Current coordination items

### ENV-001 — Initial Japan-style world generation

**Requested by:** Environment/design  
**Owner:** Environment/worldgen  
**Status:** IN PROGRESS

Establish the first playable world-generation baseline for a Japan-like environment:
- bounded Japan-oriented temperature range;
- Japan-like elevation and hilliness distribution;
- increased small/medium river frequency;
- no naturally generated Large/Huge rivers;
- more coastline through more indented coasts, bays, peninsulas and islands rather than simply reducing land area.

The authoritative Alpha targets are in `Docs/Design.md`.

**Next action:** implement the river XML patch first, then prototype the minimal C# world-generation layer required for temperature/elevation/hilliness/coastline generation. Climate validation must use actual temperature samples against the CCTO/AMJ 10/8/5/0 C growth thresholds and -1/-4/-8 C cold-death reference thresholds, not annual mean alone.

**Result / references:** initial world-generation design `9f5fd58c77b8e9ae5bad00851189d0127a122925`; CCTO-calibrated climate revision `e93da687fcd543f6d3ec94d5398fc604c0559749`.
