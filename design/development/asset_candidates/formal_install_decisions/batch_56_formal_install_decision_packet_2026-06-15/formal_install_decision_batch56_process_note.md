# Formal Install Decision Batch 56 Process Note

Process: generated a blocked decision packet from current candidate manifests and review records.

The packet intentionally records install blockers instead of approving assets, because Unity MCP editor tools are not exposed in the current Codex tool surface.

Local MCP setup command run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File C:/Users/PC/.codex/skills/unity-mcp-smoke-test/scripts/check-unity-mcp-local.ps1 -ProjectPath D:/Unity Workspace/TheCat
```

Observed local setup summary: Unity `6000.4.10f1`, Unity AI Assistant `2.12.0-pre.1`, relay exists, Codex config contains Unity, connection registry exists with approved records and older capacity/plan-limit records.

Rows: 8

No Unity import was performed.
