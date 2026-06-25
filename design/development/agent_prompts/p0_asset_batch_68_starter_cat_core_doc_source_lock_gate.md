# Agent Prompt - P0 Asset Batch 68 Starter Cat Core Document Source-Lock Gate

## Task Scope

Tighten the starter-cat asset consistency gate so Saiban, Nephthys, and Suzune
asset production can only proceed from the real colored three-view turnaround
paths. This is a code-and-documentation gate, not an image-generation batch and
not a Unity import approval.

Do not generate new cat art. Do not install or replace Unity cat sprites.

## Required Reading

Design source paths:

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\assets\characters\ch01_saiban_swordsaint\turnaround\saiban_turnaround_colored_2026-06-03.png`
- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\assets\characters\ch02_nephthys_moonsand_agent\turnaround\nephthys_turnaround_colored_2026-06-03.png`
- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\assets\characters\ch03_suzune_sleep_shrine_maiden\turnaround\suzune_turnaround_colored_2026-06-03.png`

Local source-lock docs:

- `D:\Unity Workspace\TheCat\design\development\asset_review\p0_starter_cat_source_lock_packet_2026-06-14.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\p0_starter_cat_strict_reference_pack_2026-06-14.md`
- `D:\Unity Workspace\TheCat\design\development\asset_review\p0_starter_cat_candidate_gate_2026-06-14.md`

Relevant code and tests:

- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0StarterCatSourceLockPacketEvidence.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0StarterCatTurnaroundSourceLocks.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Scripts\Runtime\Tools\P0AssetReviewPacket.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode\P0StarterCatSourceLockPacketEvidenceTests.cs`
- `D:\Unity Workspace\TheCat\Assets\TheCat\Tests\EditMode\P0AssetReviewPacketTests.cs`

## Expected Output

- Extend the starter-cat source-lock packet evidence gate so it verifies the
  three core source-lock documents:
  - source-lock packet
  - turnaround conformance spec
  - strict reference pack
- Require every core document to repeat all three exact colored-turnaround
  source paths.
- Require every core document to keep formal starter-cat Unity import blocked.
- Reject mojibake/stale encoded design path text in the core source-lock docs.
- Expose the new evidence in the generated P0 asset review packet markdown.
- Add negative tests for mojibake core docs and missing exact source paths.
- Update local development records with what changed and what remains pending.

## Forbidden Modification Scope

Do not modify:

- any PNG/JPEG image files
- `Assets/TheCat/Art/Characters/Sprites/*.png`
- starter-cat source turnaround images
- Unity `.meta` files
- runtime cat sprite bindings
- source-lock SHA-256 values
- formal starter-cat import state

## Acceptance Criteria

- `P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket()` passes.
- The report counts:
  - 3 core source-lock documents
  - 3 core documents with all exact source paths
  - 3 core documents with import block language
  - 0 core document mojibake mentions
- A mojibake path in any core source-lock document fails readiness.
- A missing exact colored-turnaround path in any core source-lock document
  fails readiness.
- The P0 asset review packet lists the new core-document evidence.
- `TheCat.Runtime.csproj` and `TheCat.EditModeTests.csproj` compile.
- `git diff --check` passes.

## Unity MCP / Editor Validation Later

No Unity install is authorized by this batch. When Unity MCP/editor tools are
available, use the existing formal import gate to capture and review:

- `05-active-cat-saiban.png`
- `06-active-cat-nephthys.png`
- `07-active-cat-suzune.png`

These screenshots must be compared against the colored three-view turnarounds
before any runtime cat sprite replacement is approved.
