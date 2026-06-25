# Unity MCP Smoke Report - 2026-06-20

Project: `D:\Unity Workspace\TheCat`
Time: `2026-06-20 15:15:21 +08:00`

## Result

Status: blocked by current Unity MCP approval state.

Current Codex session exposes Unity MCP tools, but the first live tool call
returned:

`Connection revoked. Go to Unity Editor > Project Settings > AI > Unity MCP to change approval.`

## Local Setup Check

- Unity version file found: `6000.4.10f1`.
- `com.unity.ai.assistant` package present: `2.12.0-pre.1`.
- Relay exists: `C:\Users\PC\.unity\relay\relay_win.exe`.
- Codex config contains Unity MCP entries.
- Connection registry exists.
- Registry includes both approved historical records and stale failed records:
  - `codex-mcp-client`: `status 1`, `Approved by user`.
  - `codex-mcp-client`: `status 1`, `Auto-approved: previously accepted`.
  - older `status 4` records mention connection limit or plan/seat limits.
- Latest bridge status file observed:
  `C:\Users\PC\.unity\mcp\connections\bridge-b7c6d43b-17920.json`
  with timestamp `2026-06-20T15:11:49+08:00`.

## Live Tool Check

- `Unity_GetConsoleLogs(logTypes: All, maxEntries: 20)` failed with
  `success: false` and `Connection revoked`.
- Because Console access failed at the first required smoke step, no scene
  command, transient object, camera capture, asset model query, or screenshot
  validation was claimed.

## Required Remediation

1. In Unity Editor, open `Project Settings > AI > Unity MCP`.
2. Re-approve the current Codex MCP client.
3. Close extra MCP clients if the Unity plan is limited to one connection.
4. Re-run the smoke sequence:
   - Console logs.
   - asset generation model list.
   - read-only `Unity_RunCommand`.
   - transient object create/destroy.
   - camera or scene capture.
   - final Console check.

Until then, Unity Console, Test Runner, Scene/Game View screenshots,
AssetDatabase refresh, import settings, prefab binding, and runtime visual
acceptance remain pending.
