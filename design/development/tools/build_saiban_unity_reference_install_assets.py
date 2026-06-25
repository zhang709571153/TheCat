from __future__ import annotations

import csv
import hashlib
import textwrap
import uuid
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_71_saiban_unity_reference_install_2026-06-15"
SOURCE_BATCH_SLUG = "batch_70_source_turnaround_reference_plates_2026-06-15"
CANDIDATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "saiban" / BATCH_SLUG
SOURCE_PLATE_ROOT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / SOURCE_BATCH_SLUG
UNITY_REFERENCE_ROOT = REPO_ROOT / "Assets" / "TheCat" / "Art" / "Characters" / "References"

ASSET_ID = "thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001"
UNITY_IMPORT_PATH = "Assets/TheCat/Art/Characters/References/" + ASSET_ID + ".png"
UNITY_META_PATH = UNITY_IMPORT_PATH + ".meta"
UNITY_ATLAS_PATH = REPO_ROOT / UNITY_IMPORT_PATH
MANIFEST_PATH = CANDIDATE_ROOT / "saiban_batch71_unity_reference_install_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_ROOT / "thecat_cat_saiban_batch71_unity_reference_install_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_ROOT / "saiban_batch71_unity_reference_install_review.md"
PROCESS_NOTE_PATH = CANDIDATE_ROOT / "saiban_batch71_unity_reference_install_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_71_saiban_unity_reference_install.md"
META_TEMPLATE_PATH = REPO_ROOT / "Assets" / "TheCat" / "Art" / "Characters" / "Sprites" / "thecat_cat_saiban_combat_sprite_512_v001.png.meta"
DESIGN_ROOT = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets"
SOURCE_TURNAROUND_PATH = DESIGN_ROOT + "/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
SOURCE_LOCK_ID = "saiban_turnaround_colored"
LOCKED_COMBAT_SPRITE_PATH = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"

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


def main() -> None:
    CANDIDATE_ROOT.mkdir(parents=True, exist_ok=True)
    UNITY_REFERENCE_ROOT.mkdir(parents=True, exist_ok=True)
    write_folder_meta(UNITY_REFERENCE_ROOT.with_suffix(".meta"))

    source_plate_paths = {view: source_plate_path(view) for view in VIEWS}
    for path in source_plate_paths.values():
        if not path.exists():
            raise FileNotFoundError(f"Missing Batch 70 source plate: {to_repo_path(path)}")

    source_turnaround = repo_path(SOURCE_TURNAROUND_PATH)
    locked_combat_sprite = repo_path(LOCKED_COMBAT_SPRITE_PATH)
    if not source_turnaround.exists():
        raise FileNotFoundError(f"Missing Saiban colored turnaround source: {SOURCE_TURNAROUND_PATH}")
    if not locked_combat_sprite.exists():
        raise FileNotFoundError(f"Missing Saiban locked combat sprite: {LOCKED_COMBAT_SPRITE_PATH}")

    atlas = Image.new("RGBA", (2304, 768), (250, 246, 236, 255))
    for index, view in enumerate(VIEWS):
        plate = Image.open(source_plate_paths[view]).convert("RGBA")
        try:
            atlas.alpha_composite(plate, (768 * index, 0))
        finally:
            plate.close()
    atlas.save(UNITY_ATLAS_PATH, "PNG")
    write_sprite_meta(Path(str(UNITY_ATLAS_PATH) + ".meta"))

    row = {
        "cat_id": "saiban",
        "batch_slug": BATCH_SLUG,
        "asset_id": ASSET_ID,
        "asset_type": "reference_atlas",
        "unity_import_path": UNITY_IMPORT_PATH,
        "unity_meta_path": UNITY_META_PATH,
        "installed_sha256": sha256(UNITY_ATLAS_PATH),
        "installed_size": "2304x768",
        "source_lock_id": SOURCE_LOCK_ID,
        "source_turnaround_path": SOURCE_TURNAROUND_PATH,
        "source_turnaround_sha256": sha256(source_turnaround),
        "locked_combat_sprite_path": LOCKED_COMBAT_SPRITE_PATH,
        "locked_combat_sprite_sha256": sha256(locked_combat_sprite),
        "front_reference_plate_path": to_repo_path(source_plate_paths["front"]),
        "front_reference_plate_sha256": sha256(source_plate_paths["front"]),
        "side_reference_plate_path": to_repo_path(source_plate_paths["side"]),
        "side_reference_plate_sha256": sha256(source_plate_paths["side"]),
        "back_reference_plate_path": to_repo_path(source_plate_paths["back"]),
        "back_reference_plate_sha256": sha256(source_plate_paths["back"]),
        "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
        "review_note": to_repo_path(REVIEW_NOTE_PATH),
        "process_note": to_repo_path(PROCESS_NOTE_PATH),
        "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
        "recommendation": "installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke",
    }

    write_manifest(row)
    write_review_sheet(row, source_plate_paths)
    write_review_note(row)
    write_process_note(row)
    print("Wrote Batch 71 Saiban Unity reference install packet.")
    print(to_repo_path(UNITY_ATLAS_PATH))
    print(to_repo_path(MANIFEST_PATH))


def source_plate_path(view: str) -> Path:
    return SOURCE_PLATE_ROOT / f"thecat_cat_saiban_turnaround_{view}_reference_plate_768_batch70_v001.png"


def write_manifest(row: dict[str, str]) -> None:
    with MANIFEST_PATH.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerow(row)


def write_review_sheet(row: dict[str, str], source_plate_paths: dict[str, Path]) -> None:
    sheet = Image.new("RGBA", (2200, 1180), (246, 241, 232, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(36)
    label_font = load_font(22)
    body_font = load_font(16)
    small_font = load_font(13)

    draw.text((44, 28), "P0 Batch 71 - Saiban Unity reference atlas install", fill=(42, 36, 32), font=title_font)
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

    atlas = Image.open(UNITY_ATLAS_PATH).convert("RGBA")
    try:
        preview = contain(atlas, (1480, 170))
        draw.rounded_rectangle((44, 850, 1584, 1130), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
        draw.text((64, 868), "Installed Unity atlas: " + ASSET_ID, fill=(42, 36, 32), font=label_font)
        sheet.alpha_composite(preview, (74 + (1480 - preview.width) // 2, 928 + (160 - preview.height) // 2))
    finally:
        atlas.close()

    notes = [
        "Scope",
        "- Uses only Batch 70 Saiban front/side/back reference plates.",
        "- Keeps the existing Saiban combat sprite unchanged.",
        "- Installed for Unity-side inspection and screenshot review.",
        "- Formal cat body-art import remains blocked.",
        "- No AI-generated cat body art was imported.",
        "",
        "Unity validation still pending",
        "- Refresh AssetDatabase.",
        "- Inspect Sprite Single import settings.",
        "- Capture active-cat Play Mode screenshot before promotion.",
    ]
    y = 848
    draw.rounded_rectangle((1622, 850, 2140, 1130), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
    for line in notes:
        font = label_font if line in ("Scope", "Unity validation still pending") else small_font
        draw.text((1644, y), line, fill=(42, 36, 32), font=font)
        y += 28 if font is label_font else 22

    draw.text((44, 1144), row["unity_import_path"], fill=(112, 49, 43), font=small_font)
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(row: dict[str, str]) -> None:
    lines = [
        "# Batch 71 - Saiban Unity Reference Install Review",
        "",
        "## Decision",
        "",
        "Installed one source-derived Saiban front/side/back reference atlas into Unity as a debug reference asset.",
        "This is not a runtime-bound combat sprite and does not replace the existing Saiban combat sprite.",
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
        "- The atlas is a direct left-to-right concatenation of the Batch 70 Saiban reference plates.",
        "- No AI-generated cat body candidate was imported.",
        "- No existing combat sprite, HUD avatar, source turnaround, or Batch 70 plate was overwritten.",
        "- Unity visual smoke remains pending: refresh AssetDatabase, inspect import settings, and capture active-cat screenshots.",
    ]
    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(row: dict[str, str]) -> None:
    lines = [
        "# Batch 71 Process Note",
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
        "- Avoid replacing Saiban combat art before active-cat Play Mode screenshot review.",
        "- Provide hard visual input for future Codex image generation or manual paintover review.",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


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
