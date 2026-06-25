from __future__ import annotations

import argparse
import csv
import hashlib
import textwrap
import uuid
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
SOURCE_BATCH_SLUG = "batch_70_source_turnaround_reference_plates_2026-06-15"
SOURCE_PLATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / SOURCE_BATCH_SLUG
UNITY_REFERENCE_ROOT = REPO_ROOT / "Assets" / "TheCat" / "Art" / "Characters" / "References"
META_TEMPLATE_PATH = REPO_ROOT / "Assets" / "TheCat" / "Art" / "Characters" / "Sprites" / "thecat_cat_saiban_combat_sprite_512_v001.png.meta"
DESIGN_ROOT = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets"
RECOMMENDATION = "installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke"
VIEWS = ("front", "side", "back")

FIELD_NAMES = (
    "cat_id",
    "batch_slug",
    "asset_id",
    "asset_type",
    "unity_import_path",
    "unity_meta_path",
    "installed_sha256",
    "installed_size",
    "source_lock_id",
    "source_turnaround_path",
    "source_turnaround_sha256",
    "locked_combat_sprite_path",
    "locked_combat_sprite_sha256",
    "front_reference_plate_path",
    "front_reference_plate_sha256",
    "side_reference_plate_path",
    "side_reference_plate_sha256",
    "back_reference_plate_path",
    "back_reference_plate_sha256",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
)


@dataclass(frozen=True)
class CatInstallSpec:
    cat_id: str
    display_name: str
    batch_number: int
    batch_slug: str
    source_lock_id: str
    source_turnaround_path: str
    locked_combat_sprite_path: str
    hud_avatar_id: str
    motif_summary: str

    @property
    def asset_id(self) -> str:
        return f"thecat_cat_{self.cat_id}_turnaround_reference_atlas_2304x768_v001"

    @property
    def batch_directory(self) -> Path:
        return REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / self.cat_id / self.batch_slug

    @property
    def unity_import_path(self) -> str:
        return "Assets/TheCat/Art/Characters/References/" + self.asset_id + ".png"

    @property
    def unity_meta_path(self) -> str:
        return self.unity_import_path + ".meta"

    @property
    def unity_atlas_path(self) -> Path:
        return repo_path(self.unity_import_path)

    @property
    def manifest_path(self) -> Path:
        return self.batch_directory / f"{self.cat_id}_batch{self.batch_number}_unity_reference_install_manifest.csv"

    @property
    def review_sheet_path(self) -> Path:
        return self.batch_directory / f"thecat_cat_{self.cat_id}_batch{self.batch_number}_unity_reference_install_review_sheet.png"

    @property
    def review_note_path(self) -> Path:
        return self.batch_directory / f"{self.cat_id}_batch{self.batch_number}_unity_reference_install_review.md"

    @property
    def process_note_path(self) -> Path:
        return self.batch_directory / f"{self.cat_id}_batch{self.batch_number}_unity_reference_install_process_note.md"

    @property
    def agent_prompt_path(self) -> Path:
        return REPO_ROOT / "design" / "development" / "agent_prompts" / f"p0_asset_batch_{self.batch_number}_{self.cat_id}_unity_reference_install.md"

    def source_plate_path(self, view: str) -> Path:
        return SOURCE_PLATE_ROOT / f"thecat_cat_{self.cat_id}_turnaround_{view}_reference_plate_768_batch70_v001.png"


CATS = {
    "nephthys": CatInstallSpec(
        cat_id="nephthys",
        display_name="Nephthys",
        batch_number=72,
        batch_slug="batch_72_nephthys_unity_reference_install_2026-06-15",
        source_lock_id="nephthys_turnaround_colored",
        source_turnaround_path=DESIGN_ROOT + "/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png",
        locked_combat_sprite_path="Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png",
        hud_avatar_id="thecat_cat_nephthys_hud_avatar_256_v001",
        motif_summary="hooded moon-sand control cat, gold-blue Egyptian dream motifs, pyramid/obelisk prop silhouette",
    ),
    "suzune": CatInstallSpec(
        cat_id="suzune",
        display_name="Suzune",
        batch_number=73,
        batch_slug="batch_73_suzune_unity_reference_install_2026-06-15",
        source_lock_id="suzune_turnaround_colored",
        source_turnaround_path=DESIGN_ROOT + "/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png",
        locked_combat_sprite_path="Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png",
        hud_avatar_id="thecat_cat_suzune_hud_avatar_256_v001",
        motif_summary="calico sleep-shrine healer cat, bells, wand/branch silhouette, vermilion warm-white moon-blue palette",
    ),
}


def main() -> None:
    parser = argparse.ArgumentParser(description="Build starter-cat Unity reference atlas install packets.")
    parser.add_argument(
        "--cats",
        nargs="+",
        choices=sorted(CATS.keys()),
        default=sorted(CATS.keys()),
        help="Cat ids to build. Defaults to Nephthys and Suzune.",
    )
    args = parser.parse_args()

    UNITY_REFERENCE_ROOT.mkdir(parents=True, exist_ok=True)
    write_folder_meta(UNITY_REFERENCE_ROOT.with_suffix(".meta"))

    for cat_id in args.cats:
        build_cat(CATS[cat_id])


def build_cat(spec: CatInstallSpec) -> None:
    spec.batch_directory.mkdir(parents=True, exist_ok=True)

    source_plate_paths = {view: spec.source_plate_path(view) for view in VIEWS}
    for path in source_plate_paths.values():
        if not path.exists():
            raise FileNotFoundError(f"Missing Batch 70 source plate: {to_repo_path(path)}")

    source_turnaround = repo_path(spec.source_turnaround_path)
    locked_combat_sprite = repo_path(spec.locked_combat_sprite_path)
    if not source_turnaround.exists():
        raise FileNotFoundError(f"Missing {spec.display_name} colored turnaround source: {spec.source_turnaround_path}")
    if not locked_combat_sprite.exists():
        raise FileNotFoundError(f"Missing {spec.display_name} locked combat sprite: {spec.locked_combat_sprite_path}")

    atlas = Image.new("RGBA", (2304, 768), (250, 246, 236, 255))
    for index, view in enumerate(VIEWS):
        plate = Image.open(source_plate_paths[view]).convert("RGBA")
        try:
            atlas.alpha_composite(plate, (768 * index, 0))
        finally:
            plate.close()
    atlas.save(spec.unity_atlas_path, "PNG")
    write_sprite_meta(Path(str(spec.unity_atlas_path) + ".meta"))

    row = {
        "cat_id": spec.cat_id,
        "batch_slug": spec.batch_slug,
        "asset_id": spec.asset_id,
        "asset_type": "reference_atlas",
        "unity_import_path": spec.unity_import_path,
        "unity_meta_path": spec.unity_meta_path,
        "installed_sha256": sha256(spec.unity_atlas_path),
        "installed_size": "2304x768",
        "source_lock_id": spec.source_lock_id,
        "source_turnaround_path": spec.source_turnaround_path,
        "source_turnaround_sha256": sha256(source_turnaround),
        "locked_combat_sprite_path": spec.locked_combat_sprite_path,
        "locked_combat_sprite_sha256": sha256(locked_combat_sprite),
        "front_reference_plate_path": to_repo_path(source_plate_paths["front"]),
        "front_reference_plate_sha256": sha256(source_plate_paths["front"]),
        "side_reference_plate_path": to_repo_path(source_plate_paths["side"]),
        "side_reference_plate_sha256": sha256(source_plate_paths["side"]),
        "back_reference_plate_path": to_repo_path(source_plate_paths["back"]),
        "back_reference_plate_sha256": sha256(source_plate_paths["back"]),
        "review_sheet": to_repo_path(spec.review_sheet_path),
        "review_note": to_repo_path(spec.review_note_path),
        "process_note": to_repo_path(spec.process_note_path),
        "agent_prompt": to_repo_path(spec.agent_prompt_path),
        "recommendation": RECOMMENDATION,
    }

    write_manifest(spec, row)
    write_review_sheet(spec, row, source_plate_paths)
    write_review_note(spec, row)
    write_process_note(spec, row)
    write_agent_prompt(spec, row)
    print(f"Wrote Batch {spec.batch_number} {spec.display_name} Unity reference install packet.")
    print(to_repo_path(spec.unity_atlas_path))
    print(to_repo_path(spec.manifest_path))


def write_manifest(spec: CatInstallSpec, row: dict[str, str]) -> None:
    with spec.manifest_path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerow(row)


def write_review_sheet(spec: CatInstallSpec, row: dict[str, str], source_plate_paths: dict[str, Path]) -> None:
    sheet = Image.new("RGBA", (2200, 1180), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(36)
    label_font = load_font(22)
    body_font = load_font(16)
    small_font = load_font(13)

    draw.text((44, 28), f"P0 Batch {spec.batch_number} - {spec.display_name} Unity reference atlas install", fill=(42, 36, 32), font=title_font)
    draw.text(
        (44, 78),
        "Source-derived debug reference only. The installed atlas does not replace the combat sprite and is not runtime-bound.",
        fill=(112, 49, 43),
        font=body_font,
    )

    headings = ("Front plate", "Side plate", "Back plate")
    x_positions = (56, 736, 1416)
    for view, heading, x in zip(VIEWS, headings, x_positions):
        draw.rounded_rectangle((x - 12, 132, x + 620, 812), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
        draw.text((x, 146), heading, fill=(42, 36, 32), font=label_font)
        plate = Image.open(source_plate_paths[view]).convert("RGBA")
        try:
            preview = contain(plate, (560, 560))
            sheet.alpha_composite(preview, (x + 30 + (560 - preview.width) // 2, 202 + (560 - preview.height) // 2))
        finally:
            plate.close()

    atlas = Image.open(spec.unity_atlas_path).convert("RGBA")
    try:
        preview = contain(atlas, (1480, 170))
        draw.rounded_rectangle((44, 850, 1584, 1130), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
        draw.text((64, 868), "Installed Unity atlas: " + spec.asset_id, fill=(42, 36, 32), font=label_font)
        sheet.alpha_composite(preview, (74 + (1480 - preview.width) // 2, 928 + (160 - preview.height) // 2))
    finally:
        atlas.close()

    notes = [
        "Scope",
        f"- Uses only Batch 70 {spec.display_name} front/side/back reference plates.",
        f"- Keeps the existing {spec.display_name} combat sprite unchanged.",
        "- Installed for Unity-side inspection and screenshot review.",
        "- Formal cat body-art import remains blocked.",
        "- No AI-generated cat body art was imported.",
        "",
        "Unity validation still pending",
        "- Refresh AssetDatabase.",
        "- Inspect Sprite Single import settings.",
        f"- Capture active-cat {spec.display_name} Play Mode screenshot before promotion.",
    ]
    y = 848
    draw.rounded_rectangle((1622, 850, 2140, 1130), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
    for line in notes:
        font = label_font if line in ("Scope", "Unity validation still pending") else small_font
        draw.text((1644, y), line, fill=(42, 36, 32), font=font)
        y += 28 if font is label_font else 22

    draw.text((44, 1144), row["unity_import_path"], fill=(112, 49, 43), font=small_font)
    sheet.save(spec.review_sheet_path)


def write_review_note(spec: CatInstallSpec, row: dict[str, str]) -> None:
    lines = [
        f"# Batch {spec.batch_number} - {spec.display_name} Unity Reference Install Review",
        "",
        "## Decision",
        "",
        f"Installed one source-derived {spec.display_name} front/side/back reference atlas into Unity as a debug reference asset.",
        f"This is not a runtime-bound combat sprite and does not replace the existing {spec.display_name} combat sprite.",
        "Formal starter-cat body-art import remains blocked until active-cat Play Mode screenshots pass source-lock review.",
        "",
        "## Installed Asset",
        "",
        f"- Asset id: `{row['asset_id']}`",
        f"- Unity import path: `{row['unity_import_path']}`",
        f"- Unity meta path: `{row['unity_meta_path']}`",
        f"- Installed size: `{row['installed_size']}`",
        f"- Source lock: `{row['source_lock_id']}`",
        f"- Recommendation: `{row['recommendation']}`",
        "",
        "## Source Evidence",
        "",
        f"- Colored turnaround: `{row['source_turnaround_path']}`",
        f"- Locked combat sprite retained: `{row['locked_combat_sprite_path']}`",
        f"- Front plate: `{row['front_reference_plate_path']}`",
        f"- Side plate: `{row['side_reference_plate_path']}`",
        f"- Back plate: `{row['back_reference_plate_path']}`",
        "",
        "## Consistency Notes",
        "",
        f"- The atlas is a direct left-to-right concatenation of the Batch 70 {spec.display_name} reference plates.",
        f"- It preserves the source motif: {spec.motif_summary}.",
        "- No AI-generated cat body candidate was imported.",
        "- No existing combat sprite, HUD avatar, source turnaround, or Batch 70 plate was overwritten.",
        "- Unity visual smoke remains pending: refresh AssetDatabase, inspect import settings, and capture active-cat screenshots.",
    ]
    spec.review_note_path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(spec: CatInstallSpec, row: dict[str, str]) -> None:
    lines = [
        f"# Batch {spec.batch_number} Process Note",
        "",
        "No image generation was performed.",
        "The installed Unity atlas was built deterministically from Batch 70 source-turnaround reference plates.",
        "The atlas is source-derived, debug-reference-only, and not runtime-bound.",
        "",
        "Inputs:",
        f"- `{row['front_reference_plate_path']}`",
        f"- `{row['side_reference_plate_path']}`",
        f"- `{row['back_reference_plate_path']}`",
        "",
        "Output:",
        f"- `{row['unity_import_path']}`",
        "",
        "Validation intent:",
        "- Keep source-lock lineage visible inside Unity.",
        f"- Avoid replacing {spec.display_name} combat art before active-cat Play Mode screenshot review.",
        "- Provide hard visual input for future Codex image generation or manual paintover review.",
    ]
    spec.process_note_path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_agent_prompt(spec: CatInstallSpec, row: dict[str, str]) -> None:
    text = f"""# P0 Asset Batch {spec.batch_number} - {spec.display_name} Unity Reference Install

Date: 2026-06-15

## Task Scope

Install a Unity-side {spec.display_name} source-reference atlas built only from
the Batch 70 front, side, and back reference plates. This batch is an
installation and audit bridge: it gives Unity reviewers a hard in-project visual
reference while the current {spec.display_name} combat sprite remains unchanged.

This is not a new cat-body generation task. Do not use AI-generated
{spec.display_name} body art, do not replace the existing combat sprite, and do
not approve formal starter-cat body-art import.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- {spec.display_name} colored turnaround:
  `{spec.source_turnaround_path}`
- Batch 70 {spec.display_name} reference plates:
  - `{row['front_reference_plate_path']}`
  - `{row['side_reference_plate_path']}`
  - `{row['back_reference_plate_path']}`

## Project Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetMetaImportSettingsReadiness.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatUnityReferenceInstallEvidence.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatSourceLockPacketEvidence.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetReviewPacketTests.cs`
- `Assets/TheCat/Tests/EditMode/P0StarterCatSourceLockPacketEvidenceTests.cs`
- `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/starter_cat_turnaround_reference_plates_batch70_manifest.csv`

## Expected Outputs

- Installed Unity atlas:
  `{row['unity_import_path']}`
- Matching `.png.meta` with Sprite Single import settings, disabled mipmaps,
  alpha transparency, max texture size at least 4096, and
  `TheCatP0ImportSettings:v1`.
- Batch {spec.batch_number} manifest and review packet under:
  `{to_repo_path(spec.batch_directory)}`
- Runtime review evidence that marks this as:
  `{RECOMMENDATION}`.

## Forbidden Changes

- Do not overwrite `{spec.locked_combat_sprite_path}`.
- Do not overwrite the {spec.display_name} HUD avatar `{spec.hud_avatar_id}`.
- Do not edit the colored turnaround source PNG.
- Do not edit or regenerate Batch 70 reference plates.
- Do not copy any AI body-art candidate into `Assets`.
- Do not add a runtime visual binding for this debug reference atlas.

## Acceptance

- The installed atlas is exactly `2304x768`.
- The atlas is a direct left-to-right concatenation of the Batch 70 front,
  side, and back {spec.display_name} reference plates.
- The manifest records the {spec.display_name} colored-turnaround source hash,
  current combat sprite hash, all three Batch 70 plate hashes, Unity import
  path, and meta path.
- `P0AssetManifestCatalog` includes the atlas as a `reference_atlas` generated
  asset with `{spec.source_lock_id}` source lock.
- `P0AssetReviewPacket` reports starter-cat Unity reference installs as ready
  while keeping the assets non-runtime-bound and pending Unity visual smoke.
- Review notes explicitly state that no AI cat body art was imported and that
  formal starter-cat body-art import remains blocked.

## Validation

```powershell
& 'C:\\Users\\PC\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\python\\python.exe' design/development/tools/build_starter_cat_unity_reference_install_assets.py --cats {spec.cat_id}
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_unity_reference_installs.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_turnaround_reference_plates.ps1
& 'C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity MCP validation, when available:

- Refresh AssetDatabase.
- Inspect the reference atlas import settings.
- Open the atlas in Unity and compare it against the Batch 70 review sheet.
- Capture active-cat {spec.display_name} Play Mode screenshot and compare
  against the atlas before any future formal cat-body import decision.
"""
    spec.agent_prompt_path.write_text(textwrap.dedent(text), encoding="utf-8")


def write_sprite_meta(path: Path) -> None:
    template = META_TEMPLATE_PATH.read_text(encoding="utf-8")
    guid = uuid.uuid5(uuid.NAMESPACE_URL, to_repo_path(path.with_suffix(""))).hex
    sprite_id = uuid.uuid5(uuid.NAMESPACE_DNS, to_repo_path(path.with_suffix(""))).hex[:24] + "00000000"
    lines = []
    for line in template.splitlines():
        if line.startswith("guid: "):
            lines.append("guid: " + guid)
        elif "spriteID:" in line:
            indent = line[: line.index("spriteID:")]
            lines.append(indent + "spriteID: " + sprite_id)
        elif "maxTextureSize: 2048" in line:
            lines.append(line.replace("2048", "4096"))
        else:
            lines.append(line)
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_folder_meta(path: Path) -> None:
    if path.exists():
        return
    guid = uuid.uuid5(uuid.NAMESPACE_URL, to_repo_path(path.with_suffix("")) + ".folder").hex
    path.write_text(
        "\n".join(
            [
                "fileFormatVersion: 2",
                "guid: " + guid,
                "folderAsset: yes",
                "DefaultImporter:",
                "  externalObjects: {}",
                "  userData: ",
                "  assetBundleName: ",
                "  assetBundleVariant: ",
                "",
            ]
        ),
        encoding="utf-8",
    )


def contain(image: Image.Image, box: tuple[int, int]) -> Image.Image:
    copy = image.copy()
    copy.thumbnail(box, Image.Resampling.LANCZOS)
    return copy


def repo_path(relative_path: str) -> Path:
    return REPO_ROOT / relative_path.replace("/", "\\")


def load_font(size: int) -> ImageFont.ImageFont:
    for candidate in (
        Path("C:/Windows/Fonts/msyh.ttc"),
        Path("C:/Windows/Fonts/msyh.ttf"),
        Path("C:/Windows/Fonts/segoeui.ttf"),
        Path("C:/Windows/Fonts/arial.ttf"),
    ):
        if candidate.exists():
            return ImageFont.truetype(str(candidate), size=size)
    return ImageFont.load_default()


def sha256(path: Path) -> str:
    hasher = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            hasher.update(chunk)
    return hasher.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
