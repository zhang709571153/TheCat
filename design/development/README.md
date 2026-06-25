# Cat Somnium Development Docs

This folder is the local development control room for the Unity implementation of
`TheCat`.

The design source of truth remains under:

- `design/梦境支配者核心玩法/docs`
- `design/梦境支配者核心玩法/assets`

The files here translate those design documents into engineering scope,
architecture, task prompts, review criteria, validation plans, and running
development history.

## Files

- `P0_DEVELOPMENT_ARCHITECTURE.md`: Current stable architecture entry point.
  Use this before dated snapshots when assigning or implementing P0 work.
- `P0_IMPLEMENTATION_TASK_BREAKDOWN.md`: Current stable task graph, milestone
  map, and first code-slice contract.
- `P0_DEVELOPMENT_ARCHITECTURE_2026-06-25.md`: Evidence snapshot after
  re-reading `Qr1`, mapping `MDr` to the local synced copy, and keeping `IZp`
  blocked until access is granted.
- `P0_AGENT_DISPATCH_AND_TASK_GRAPH_2026-06-25.md`: 2026-06-25 agent-lane,
  workstream, near-term WBS, and first worker prompt candidate snapshot.
- `P0_CURRENT_ARCHITECTURE_2026-06-24.md`: Historical implemented-route and
  battle architecture snapshot for the single-bedroom route implementation.
  Superseded by the stable architecture entry point for live-source P0 scope.
- `P0_DEMO_EXECUTION_PLAN_2026-06-24.md`: Historical WBS snapshot from source
  sync through playable loop, battle, roguelite content, UI, assets, and
  verification.
- `P0_BLOCKERS_AND_RETIREMENT_LOG_2026-06-24.md`: Active blockers, stale
  guidance retirement, and current authoritative entry points.
- `P0_DEVELOPMENT_BLUEPRINT.md`: Superseded initial 2026-06-13 blueprint. Keep
  for history; do not use its "current Unity facts" as live state.
- `AGENT_WORKFLOW.md`: Delegation rules, worker prompt templates, review prompt templates, and phase gates.
- `DEVELOPMENT_LOG.md`: Local record of completed work, decisions, test results, gaps, and next tasks.

## Current Phase

Current target: a design-faithful P0 demo with entry/menu, cat room hub,
10-layer route, bedroom defense combat, Egypt dream theme target, three starter
cats, four core values, interactables, authority blessings, Boss, settlement,
return-to-cat-room feedback, and validation evidence.

Current architecture caveat: the project now has substantial runtime,
roguelite, gameplay, UI-presenter, validation, and asset-catalog code. Treat
older blank-skeleton assumptions as retired.

Current external blocker: `IZpFdIwtboEzzrx4ZFlcZLD2npe` is not readable by the
current Feishu user and must be granted before its content can influence
implementation. Live access to `MDrSdEoaToB5cnxZgrOcAE34nof` is also blocked,
but the synced 2026-06-13 local copy remains the working gameplay reference.

Current Unity evidence state: Unity MCP approval may still be unavailable, but
normal Editor Play Mode acceptance has current passing baseline evidence. Treat
MCP connection notes as tooling status, not as a reason to discard the current
Play Mode reports. This baseline is smoke/screenshot evidence only; it is not
candidate import approval or final art acceptance.

Next code slice:

- Keep `P0_DEVELOPMENT_ARCHITECTURE.md` and
  `P0_IMPLEMENTATION_TASK_BREAKDOWN.md` as the current entry points.
- F1 candidate UI asset review gate is complete for Batch 83-88 as docs-only
  import-order guidance. G1 has refreshed the current 11-capture Play Mode
  baseline for Batch 87, Batch 88, Batch 86, and Batch 84 review context, but
  batch-specific import evidence is still pending.
- H1 added code-side hooks for Batch 85 full settings and Batch 83
  loading/start. They are hook-ready only; fresh Unity-rendered screenshots,
  import settings, binding proof, and Console evidence are still required
  before full import review.
- Treat E1 as code/readiness complete and G1 as screenshot-baseline refreshed,
  but do not claim final visual acceptance for the battle view until the
  battle-world label safe-area / overlay hierarchy debt is fixed or explicitly
  accepted.
- Keep the player main path centered on cat room -> bedroom dream while Egypt
  remains a visible P0 placeholder target.
- Keep the new 11-capture Play Mode screenshot plan current as UI surfaces
  change, including `02-cat-room.png`. H1 did not add dedicated loading or
  settings captures to this plan.
- Keep candidate assets outside runtime until Unity evidence approves formal
  install.
