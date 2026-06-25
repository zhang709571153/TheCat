from __future__ import annotations

import csv
import hashlib
import textwrap
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_74_runtime_combat_sprite_source_audit_2026-06-15"
BATCH_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
SOURCE_PLATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "batch_70_source_turnaround_reference_plates_2026-06-15"
MANIFEST_PATH = BATCH_ROOT / "starter_cat_runtime_combat_sprite_source_audit_batch74_manifest.csv"
REVIEW_SHEET_PATH = BATCH_ROOT / "thecat_cat_starter_runtime_combat_sprite_source_audit_batch74_review_sheet.png"
REVIEW_NOTE_PATH = BATCH_ROOT / "starter_cat_runtime_combat_sprite_source_audit_batch74_review.md"
PROCESS_NOTE_PATH = BATCH_ROOT / "starter_cat_runtime_combat_sprite_source_audit_batch74_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_74_starter_cat_runtime_combat_sprite_source_audit.md"
DESIGN_ROOT = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets"
RECOMMENDATION = "runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot"


@dataclass(frozen=True)
class RuntimeSpriteAuditSpec:
    cat_id: str
    display_name: str
    source_lock_id: str
    source_turnaround_path: str
    runtime_sprite_path: str
    runtime_binding_id: str
    visual_catalog_constant: str
    required_traits: tuple[str, ...]

    @property
    def asset_id(self) -> str:
        return Path(self.runtime_sprite_path).stem

    @property
    def front_plate_path(self) -> Path:
        return SOURCE_PLATE_ROOT / f"thecat_cat_{self.cat_id}_turnaround_front_reference_plate_768_batch70_v001.png"


CATS: tuple[RuntimeSpriteAuditSpec, ...] = (
    RuntimeSpriteAuditSpec(
        "saiban",
        "Saiban / Sword Saint",
        "saiban_turnaround_colored",
        DESIGN_ROOT + "/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png",
        "cat.combat.saiban",
        "SaibanCombatSpriteId",
        (
            "silver-gray tabby face markings",
            "round sun shield and sword silhouettes",
            "silver-gold armor, helm, and red cape",
            "non-human cat body proportions",
        ),
    ),
    RuntimeSpriteAuditSpec(
        "nephthys",
        "Nephthys / Moon-Sand Agent",
        "nephthys_turnaround_colored",
        DESIGN_ROOT + "/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png",
        "cat.combat.nephthys",
        "NephthysCombatSpriteId",
        (
            "gold-brown tabby face and golden eyes",
            "deep navy hood with crescent marker",
            "floating pyramid/obelisk controller prop",
            "gold-blue moon-sand trim and non-human cat body",
        ),
    ),
    RuntimeSpriteAuditSpec(
        "suzune",
        "Suzune / Sleep Shrine Healer",
        "suzune_turnaround_colored",
        DESIGN_ROOT + "/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png",
        "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png",
        "cat.combat.suzune",
        "SuzuneCombatSpriteId",
        (
            "calico orange, black, and white face markings",
            "vermilion and white shrine outfit",
            "bell ornaments and wand/branch healer prop",
            "soft healer palette on non-human cat body",
        ),
    ),
)


FIELD_NAMES = (
    "cat_id",
    "display_name",
    "batch_slug",
    "asset_id",
    "asset_type",
    "runtime_binding_id",
    "visual_catalog_constant",
    "source_lock_id",
    "source_turnaround_path",
    "source_turnaround_sha256",
    "front_reference_plate_path",
    "front_reference_plate_sha256",
    "runtime_sprite_path",
    "runtime_sprite_sha256",
    "runtime_sprite_meta_path",
    "runtime_sprite_size",
    "runtime_sprite_has_alpha",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
)


def main() -> None:
    BATCH_ROOT.mkdir(parents=True, exist_ok=True)
    rows: list[dict[str, str]] = []

    for spec in CATS:
        source_turnaround = repo_path(spec.source_turnaround_path)
        front_plate = spec.front_plate_path
        runtime_sprite = repo_path(spec.runtime_sprite_path)
        runtime_meta = Path(str(runtime_sprite) + ".meta")
        for path in (source_turnaround, front_plate, runtime_sprite, runtime_meta):
            if not path.exists():
                raise FileNotFoundError(to_repo_path(path))

        runtime_image = Image.open(runtime_sprite).convert("RGBA")
        try:
            runtime_size = f"{runtime_image.width}x{runtime_image.height}"
            runtime_has_alpha = has_alpha(runtime_image)
        finally:
            runtime_image.close()

        rows.append(
            {
                "cat_id": spec.cat_id,
                "display_name": spec.display_name,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "runtime_combat_sprite",
                "runtime_binding_id": spec.runtime_binding_id,
                "visual_catalog_constant": spec.visual_catalog_constant,
                "source_lock_id": spec.source_lock_id,
                "source_turnaround_path": spec.source_turnaround_path,
                "source_turnaround_sha256": sha256(source_turnaround),
                "front_reference_plate_path": to_repo_path(front_plate),
                "front_reference_plate_sha256": sha256(front_plate),
                "runtime_sprite_path": spec.runtime_sprite_path,
                "runtime_sprite_sha256": sha256(runtime_sprite),
                "runtime_sprite_meta_path": spec.runtime_sprite_path + ".meta",
                "runtime_sprite_size": runtime_size,
                "runtime_sprite_has_alpha": "yes" if runtime_has_alpha else "no",
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": RECOMMENDATION,
            }
        )

    write_manifest(rows)
    write_review_sheet(rows)
    write_review_note(rows)
    write_process_note(rows)
    write_agent_prompt(rows)
    print("Wrote Batch 74 starter-cat runtime combat sprite source audit.")
    print(to_repo_path(MANIFEST_PATH))
    print(to_repo_path(REVIEW_SHEET_PATH))


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(rows: list[dict[str, str]]) -> None:
    sheet = Image.new("RGBA", (2200, 1320), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(20)
    body_font = load_font(14)
    small_font = load_font(12)

    draw.text((42, 28), "P0 Batch 74 - runtime combat sprite source audit", fill=(42, 36, 32), font=title_font)
    draw.text(
        (42, 76),
        "Runtime-bound sprite vs Batch 70 front plate. No image generation, no Unity sprite replacement, no AI cat-body approval.",
        fill=(112, 49, 43),
        font=body_font,
    )

    for index, (spec, row) in enumerate(zip(CATS, rows)):
        y = 124 + index * 380
        draw.rounded_rectangle((42, y, 2158, y + 340), radius=9, fill=(255, 252, 246), outline=(178, 158, 130))
        draw.text((66, y + 18), spec.display_name, fill=(42, 36, 32), font=label_font)
        draw.text((66, y + 46), f"{row['runtime_binding_id']} -> {row['asset_id']}", fill=(112, 49, 43), font=small_font)

        front_plate = Image.open(repo_path(row["front_reference_plate_path"])).convert("RGBA")
        runtime_sprite = Image.open(repo_path(row["runtime_sprite_path"])).convert("RGBA")
        try:
            draw_panel(sheet, draw, front_plate, (70, y + 78), (330, 220), "Batch 70 front reference plate", small_font)
            draw_panel(sheet, draw, runtime_sprite, (430, y + 78), (220, 220), "runtime sprite on paper", small_font, paper=True)
            draw_panel(sheet, draw, runtime_sprite, (682, y + 78), (220, 220), "runtime sprite on checker", small_font, checker=True)
        finally:
            front_plate.close()
            runtime_sprite.close()

        notes_x = 944
        notes_y = y + 82
        draw.text((notes_x, notes_y), "Source lock and runtime evidence", fill=(42, 36, 32), font=label_font)
        notes_y += 32
        evidence_lines = (
            f"source lock: {row['source_lock_id']}",
            f"runtime sprite: {row['runtime_sprite_path']}",
            f"sprite sha256: {row['runtime_sprite_sha256'][:18]}...",
            f"front plate sha256: {row['front_reference_plate_sha256'][:18]}...",
            f"size: {row['runtime_sprite_size']}; alpha: {row['runtime_sprite_has_alpha']}",
            f"recommendation: {row['recommendation']}",
        )
        for line in evidence_lines:
            for wrapped in textwrap.wrap(line, width=68, break_long_words=False):
                draw.text((notes_x, notes_y), wrapped, fill=(42, 36, 32), font=body_font)
                notes_y += 21

        traits_x = 1514
        traits_y = y + 82
        draw.text((traits_x, traits_y), "Must remain visually readable", fill=(42, 36, 32), font=label_font)
        traits_y += 32
        for trait in spec.required_traits:
            for wrapped in textwrap.wrap("- " + trait, width=54, break_long_words=False):
                draw.text((traits_x + 10, traits_y), wrapped, fill=(42, 36, 32), font=body_font)
                traits_y += 21
        draw.text((traits_x, y + 300), "Unity active-cat screenshot comparison remains pending.", fill=(112, 49, 43), font=body_font)

    draw.text((42, 1280), to_repo_path(MANIFEST_PATH), fill=(112, 49, 43), font=small_font)
    sheet.save(REVIEW_SHEET_PATH)


def draw_panel(
    sheet: Image.Image,
    draw: ImageDraw.ImageDraw,
    image: Image.Image,
    origin: tuple[int, int],
    size: tuple[int, int],
    label: str,
    font: ImageFont.ImageFont,
    *,
    paper: bool = False,
    checker: bool = False,
) -> None:
    x, y = origin
    width, height = size
    draw.rounded_rectangle((x, y, x + width, y + height + 24), radius=7, fill=(242, 236, 226), outline=(186, 168, 140))
    if checker:
        background = checkerboard((width - 12, height - 28))
    else:
        color = (250, 246, 236, 255) if paper else (255, 252, 246, 255)
        background = Image.new("RGBA", (width - 12, height - 28), color)
    preview = image.copy()
    preview.thumbnail((background.width - 8, background.height - 8), Image.Resampling.LANCZOS)
    background.alpha_composite(preview, ((background.width - preview.width) // 2, (background.height - preview.height) // 2))
    sheet.alpha_composite(background, (x + 6, y + 6))
    draw.text((x + 6, y + height + 3), label, fill=(42, 36, 32), font=font)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 74 Starter Cat Runtime Combat Sprite Source Audit",
        "",
        "Decision: runtime sprite source audit ready; Unity active-cat Play Mode screenshot comparison remains pending.",
        "",
        "This packet makes the current runtime-bound starter-cat combat sprites auditable against the strict colored three-view source chain. It does not generate new cat body art, does not replace any Unity sprite, and does not approve AI-generated or repainted cat-body imports.",
        "",
        "## Scope",
        "",
        "- Covers the existing runtime-bound Saiban, Nephthys, and Suzune combat sprites.",
        "- Compares each runtime sprite against the Batch 70 front reference plate.",
        "- Records source turnaround hashes, front plate hashes, runtime sprite hashes, Unity `.png.meta` paths, runtime binding ids, and visual catalog constants.",
        "- Keeps `P0StarterCatFormalImportReadiness` blocked until active-cat Play Mode screenshots are captured and reviewed.",
        "",
        "## Outputs",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Runtime Sprite Rows",
        "",
    ]
    for row in rows:
        lines.extend(
            [
                f"### {row['display_name']}",
                "",
                f"- Runtime binding: `{row['runtime_binding_id']}`",
                f"- Visual catalog constant: `{row['visual_catalog_constant']}`",
                f"- Asset id: `{row['asset_id']}`",
                f"- Runtime sprite: `{row['runtime_sprite_path']}`",
                f"- Runtime sprite SHA-256: `{row['runtime_sprite_sha256']}`",
                f"- Runtime sprite meta: `{row['runtime_sprite_meta_path']}`",
                f"- Source lock: `{row['source_lock_id']}`",
                f"- Source turnaround: `{row['source_turnaround_path']}`",
                f"- Front reference plate: `{row['front_reference_plate_path']}`",
                f"- Recommendation: `{row['recommendation']}`",
                "",
            ]
        )
    lines.extend(
        [
            "## Unity Gate",
            "",
            "- Refresh Unity AssetDatabase.",
            "- Inspect the three sprite import settings as Sprite / Single, alpha transparency enabled, mipmaps disabled.",
            "- Capture active-cat Saiban, Nephthys, and Suzune Play Mode screenshots.",
            "- Compare screenshot scale, pose, markings, props, costume, and palette against the Batch 74 review sheet and Batch 70 front plates.",
            "- Keep AI/repainted cat-body import blocked until that review explicitly approves it.",
            "",
        ]
    )
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Batch 74 Process Note",
        "",
        "- Production mode: deterministic local review-packet generation with Pillow.",
        "- Image generation model: not used.",
        "- Unity runtime assets changed: none.",
        "- Existing runtime-bound combat sprites audited: 3.",
        "- New Unity `.png.meta` files created: none.",
        "- Purpose: preserve a hard baseline before any future AI, paintover, animation-frame, or sprite replacement work.",
        "- Unity visual smoke remains pending.",
        "",
        "## Audited Runtime Sprites",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['runtime_binding_id']}` -> `{row['runtime_sprite_path']}` sha256 `{row['runtime_sprite_sha256']}`")
    lines.append("")
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_agent_prompt(rows: list[dict[str, str]]) -> None:
    text = """# P0 Asset Batch 74 - Starter Cat Runtime Combat Sprite Source Audit

Date: 2026-06-15

## Task Scope

Audit the currently runtime-bound Saiban, Nephthys, and Suzune combat sprites
against the strict colored-turnaround source chain. This task does not generate
new cat body art and does not replace any Unity sprite. Its purpose is to make
the current runtime baseline explicit before the next formal animation,
paintover, or image-generation pass.

## Required Design Sources

- `design/梦境支配者核心玩法/docs/03_characters/character_design.md`
- `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md`
- Batch 70 front/side/back reference plates:
  `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15`

## Project Files To Read

- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0RuntimeVisualBindingCoverage.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs`
- `Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatFormalImportReadiness.cs`
- `Assets/TheCat/Tests/EditMode/P0VisualAssetCatalogTests.cs`
- `Assets/TheCat/Tests/EditMode/P0RuntimeVisualBindingCoverageTests.cs`
- `Assets/TheCat/Tests/EditMode/P0AssetReviewPacketTests.cs`

## Runtime Rows

"""
    for row in rows:
        text += f"- `{row['runtime_binding_id']}` -> `{row['runtime_sprite_path']}`; source lock `{row['source_lock_id']}`; front plate `{row['front_reference_plate_path']}`\n"
    text += """

## Forbidden Changes

- Do not overwrite the three runtime combat sprites.
- Do not import Batch 49, 50, or 51 AI candidates into `Assets`.
- Do not change `P0StarterCatFormalImportReadiness` from blocked to approved.
- Do not remove the active-cat Play Mode screenshot requirement.
- Do not edit the original colored turnaround sources or Batch 70 reference plates.

## Expected Outputs

- Batch 74 manifest, review sheet, review note, process note, and agent prompt.
- Runtime evidence class and EditMode test coverage proving that all three
  runtime-bound combat sprites have source-lock, front-plate, hash, meta, and
  runtime-binding evidence.
- `P0AssetReviewPacket` section showing Batch 74 is ready while formal
  cat-body import remains blocked pending active-cat Play Mode screenshots.

## Validation

```powershell
& 'C:\\Users\\PC\\.cache\\codex-runtimes\\codex-primary-runtime\\dependencies\\python\\python.exe' design/development/tools/build_starter_cat_runtime_combat_sprite_source_audit.py
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_runtime_combat_sprite_source_audit.ps1
& 'C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe' TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe' TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity MCP validation, when available:

- Refresh AssetDatabase.
- Inspect all three runtime sprite import settings.
- Capture active-cat Play Mode screenshots for Saiban, Nephthys, and Suzune.
- Compare runtime screenshots against the Batch 74 review sheet and Batch 70
  front reference plates.
"""
    AGENT_PROMPT_PATH.write_text(textwrap.dedent(text), encoding="utf-8")


def checkerboard(size: tuple[int, int], cell: int = 16) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    for y in range(0, height, cell):
        for x in range(0, width, cell):
            color = (225, 218, 206, 255) if ((x // cell) + (y // cell)) % 2 == 0 else (250, 246, 236, 255)
            draw.rectangle((x, y, x + cell - 1, y + cell - 1), fill=color)
    return image


def has_alpha(image: Image.Image) -> bool:
    alpha = image.getchannel("A")
    min_alpha, _ = alpha.getextrema()
    return min_alpha < 255


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
