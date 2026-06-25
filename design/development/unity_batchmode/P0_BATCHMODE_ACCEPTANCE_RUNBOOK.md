# P0 Batchmode Acceptance Runbook

Date: 2026-06-14

This runbook defines the Unity command-line acceptance path for the P0 build.
It is the fallback when Unity MCP editor tools are unavailable, and it also
gives CI or future agents a deterministic way to reproduce editor-side gates.

## Offline Gates

Use this when validating code, data, scenes, imports, and asset review packets
without requiring fresh Play Mode screenshots:

```powershell
& 'D:\SoftWares\6000.4.10f1\Editor\Unity.exe' `
  -batchmode `
  -quit `
  -projectPath 'D:\Unity Workspace\TheCat' `
  -executeMethod TheCat.EditorTools.P0BatchmodeAcceptanceRunner.RunOfflineP0GatesForBatchmode `
  -logFile 'D:\Unity Workspace\TheCat\Temp\Logs\P0OfflineAcceptance.log'
```

Expected report:

`design/development/unity_batchmode/P0_OFFLINE_ACCEPTANCE_REPORT.md`

Offline gates currently include:

- P0 Code Smoke Suite
- P0 Playable Readiness
- P0 Scene Setup
- P0 Asset Imports
- P0 Asset Review Packet
- P0 Offline Asset Production Readiness

## Play Mode Acceptance Smoke

Use this when capturing Play Mode runtime evidence and screenshots. Do not add
`-batchmode` for this visual pass: batchmode can run the logic smoke, but it can
produce blank frame-buffer captures. The screenshot evidence gate now rejects
blank or single-color PNGs, so the visual pass should run in normal Editor mode
and let the runner exit Unity when complete.

```powershell
$unity = 'D:\SoftWares\6000.4.10f1\Editor\Unity.exe'
$log = 'D:\Unity Workspace\TheCat\Logs\P0PlayModeAcceptanceVisual.log'
$arguments = '-projectPath "D:\Unity Workspace\TheCat" -executeMethod TheCat.EditorTools.P0PlayModeEvidenceBatchmodeRunner.RunPlayModeAcceptanceSmokeForBatchmode -logFile "' + $log + '"'
$p = Start-Process -FilePath $unity -ArgumentList $arguments -PassThru -WindowStyle Hidden
$p.WaitForExit()
$p.ExitCode
```

Expected report:

`design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md`

Expected screenshots:

`design/development/screenshots/p0-playmode-smoke`

Current successful evidence from 2026-06-25:

- Play Mode acceptance result: `passed`.
- Evidence gate: `8/8` checks passed, `0` warnings.
- Screenshot file evidence: `11/11` expected validated captures, including
  `02-cat-room.png`.
- Screenshot dimensions: `1920x1080`.
- Runtime visual binding check: `111/111` resolved runtime bindings.
- Route-flow smoke: `10/10` nodes, `5` battle wins, Boss observed, route
  settlement cleared.
- Defeat-flow smoke: first battle defeat path reaches failed settlement.

## Full Acceptance

Use this after Play Mode screenshot evidence has been captured:

```powershell
& 'D:\SoftWares\6000.4.10f1\Editor\Unity.exe' `
  -batchmode `
  -quit `
  -projectPath 'D:\Unity Workspace\TheCat' `
  -executeMethod TheCat.EditorTools.P0BatchmodeAcceptanceRunner.RunFullP0AcceptanceForBatchmode `
  -logFile 'D:\Unity Workspace\TheCat\Temp\Logs\P0FullAcceptance.log'
```

Expected report:

`design/development/unity_batchmode/P0_FULL_ACCEPTANCE_REPORT.md`

Full acceptance includes all offline gates plus:

- P0 Play Mode Evidence

The Play Mode Evidence gate is stricter than the offline evidence readout. It
passes only when every Play Mode evidence check is complete:

- screenshot capture plan present
- runtime visual screenshot plan present
- runtime visual contact sheet present
- screenshot file evidence complete: 11/11 expected captures, every capture
  decodes as a non-blank PNG, and zero unexpected PNG files
- screenshot smoke passed with all 11 captures
- route-flow smoke passed
- defeat-flow smoke passed
- zero pending warnings

Pending `Idle` or `Running` smoke checks are treated as incomplete full
acceptance evidence even when they have no blocking failures.

## Important Limitation

Unity cannot run batchmode against the project while another Unity instance has
the same project open. If the command exits before writing a report and the log
contains:

```text
It looks like another Unity instance is running with this project open.
Multiple Unity instances cannot open the same project.
```

close the open `TheCat` editor instance or run the command on a clean copied
workspace.

## Current Attempt

Attempted command:

`TheCat.EditorTools.P0BatchmodeAcceptanceRunner.RunOfflineP0GatesForBatchmode`

Result:

- Unity batchmode reached licensing and project path setup.
- Unity exited before executing the method because the project was already open.
- Log path:
  `Temp/Logs/P0OfflineAcceptance.log`
- No offline report was generated in `design/development/unity_batchmode`.

Update 2026-06-14:

- Offline gates now also include `P0 Offline Asset Production Readiness`.
- This gate combines manifest coverage, generation batch ordering, PNG/meta
  import readiness, current runtime visual bindings, asset review packet counts,
  hard reference locks, starter cat colored-turnaround locks, starter cat
  visual checklist, starter cat asset-production spec, starter cat formal
  import readiness, starter cat review contact sheet, and runtime visual
  contact sheet.
- The starter cat formal import gate may pass in `Blocked` state. That means
  systematic asset production can continue, but starter cat candidates must not
  be copied into Unity import paths until the same gate reports `Approved`.
