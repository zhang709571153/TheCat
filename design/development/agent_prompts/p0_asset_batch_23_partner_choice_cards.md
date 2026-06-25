# Agent Prompt: P0 Asset Batch 23 Partner Choice Cards

## Task Scope

Produce and integrate the P0 partner choice card assets for:

- `partner_shadowmaru_preview`
- `partner_preview_duplicate_supply`

This is a non-cat UI-symbol batch. It must not create Shadowmaru body art and
must not modify existing starter-cat, candidate-cat, or turnaround-derived
assets.

## Design Sources

- `D:\Unity Workspace\TheCat\design\梦境支配者核心玩法\docs`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md`
- `design/development/prompts/p0_partner_choice_cards.md`

## Code Files To Read

- `Assets/TheCat/Scripts/Runtime/Roguelite/P0RouteRewardResolver.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Gameplay/P0RouteMapPresenter.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetManifestCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RouteMapSurfaceCoverage.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0RouteMapSurfaceCoverageTests.cs`

## Expected Output

- Two `384x160` PNGs in `Assets/TheCat/Art/UI/Cards`.
- Matching Unity `.png.meta` files with sprite importer settings and batch
  userData.
- Manifest rows, runtime bindings, batch catalog entries, coverage checks, and
  EditMode tests for both choice cards.
- Updated runtime visual contact sheet and asset review packet.
- Development log entry with validation commands and Unity MCP follow-up.

## Forbidden Modifications

- Do not replace or regenerate Saiban, Nephthys, Suzune, or any starter-cat
  files.
- Do not create cat silhouettes, body parts, markings, costumes, or character
  portraits.
- Do not modify Unity scene or prefab files in this batch.
- Do not loosen starter-cat source-lock or formal import readiness gates.

## Acceptance Standards

- `P0AssetManifestCatalog.P0ManifestAssetCount == 88`.
- `P0VisualAssetCatalog.P0RuntimeVisualBindingCount == 84`.
- `P0AssetGenerationBatchCatalog.PartnerChoiceCardBatchId` exists after Batch
  22.
- `GetRouteChoiceCard` resolves specific partner choice card assets for both
  `partner_shadowmaru_preview` and `partner_preview_duplicate_supply`.
- Runtime binding coverage includes both `partner_choice.*` entries.
- Route map surface coverage confirms both first partner recruit and duplicate
  partner fallback card states.
- Generated images are visibly non-cat symbolic UI cards.

## Validation

Run:

```powershell
& 'D:\Unity Workspace\TheCat\design\development\tools\build_partner_choice_cards.ps1'
& 'D:\Unity Workspace\TheCat\design\development\tools\build_runtime_visual_contact_sheet.ps1'
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.Runtime.csproj' /p:OutputPath='Temp\bin\RuntimeBatch23PartnerCards\' /p:BaseIntermediateOutputPath='Temp\obj\RuntimeBatch23PartnerCards\' /v:minimal /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.EditModeTests.csproj' /p:OutputPath='Temp\bin\EditModeBatch23PartnerCards\' /p:BaseIntermediateOutputPath='Temp\obj\EditModeBatch23PartnerCards\' /v:minimal /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' 'TheCat.Editor.csproj' /p:OutputPath='Temp\bin\EditorBatch23PartnerCards\' /p:BaseIntermediateOutputPath='Temp\obj\EditorBatch23PartnerCards\' /v:minimal /nologo
git diff --check
```

Unity MCP follow-up when available:

- Reimport the two card PNGs and inspect TextureImporter sprite settings.
- Open route map, navigate to partner node, and screenshot both first-recruit
  and duplicate-fallback reward states.
- Confirm Console has no new errors.
