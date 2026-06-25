from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw

from build_saiban_ai_generation_pilot import (
    alpha_mask,
    checkerboard_composite,
    draw_panel,
    fit_image,
    flat_composite,
    load_font,
    resize_square,
    wrap,
)


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_50_nephthys_strict_ai_generation_candidate_2026-06-15"
CAT_ID = "nephthys"
DISPLAY_NAME = "Nephthys / Moon-Sand Agent"
SOURCE_LOCK_ID = "nephthys_turnaround_colored"
ACTIVE_SCREENSHOT = "05-active-cat-nephthys.png"

CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / CAT_ID / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "nephthys_batch50_strict_ai_generation_manifest.csv"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_cat_nephthys_batch50_strict_ai_generation_review_sheet.png"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "nephthys_batch50_strict_ai_generation_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "nephthys_batch50_strict_ai_generation_process_note.md"
AGENT_PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_50_nephthys_strict_ai_generation_candidate.md"

SOURCE_TURNAROUND = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
BATCH47_JSON = "design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_nephthys_batch47_strict_generation_spec_v001.json"
BATCH47_CARD = "design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_nephthys_batch47_strict_generation_spec_card_v001.png"
BATCH37_ALPHA = "design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_1024_candidate_v001.png"
BATCH37_REVIEW = "design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/nephthys_batch37_cutout_candidate_review.md"
UNITY_SPRITE = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"

CHROMA_SOURCE_NAME = "thecat_cat_nephthys_batch50_strict_ai_chromakey_source_v001.png"
ALPHA_NAME = "thecat_cat_nephthys_batch50_strict_ai_alpha_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_cat_nephthys_batch50_strict_ai_alpha_512_preview_v001.png"
CHECKER_NAME = "thecat_cat_nephthys_batch50_strict_ai_checkerboard_512_review_v001.png"
DARK_NAME = "thecat_cat_nephthys_batch50_strict_ai_darkfield_512_review_v001.png"
WARM_NAME = "thecat_cat_nephthys_batch50_strict_ai_warmfield_512_review_v001.png"
MASK_NAME = "thecat_cat_nephthys_batch50_strict_ai_alpha_mask_512_review_v001.png"

FIELD_NAMES = (
    "cat_id",
    "display_name",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "source_turnaround_path",
    "source_turnaround_sha256",
    "source_lock_id",
    "batch47_json_path",
    "batch47_json_sha256",
    "batch47_card_path",
    "batch47_card_sha256",
    "batch37_alpha_path",
    "batch37_alpha_sha256",
    "batch37_review_path",
    "batch37_review_sha256",
    "unity_sprite_path",
    "unity_sprite_sha256",
    "generated_source_path",
    "generated_source_sha256",
    "alpha_candidate_path",
    "alpha_candidate_sha256",
    "active_screenshot",
    "review_sheet",
    "review_note",
    "process_note",
    "agent_prompt",
    "recommendation",
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    chroma_source = CANDIDATE_DIR / CHROMA_SOURCE_NAME
    alpha_candidate = CANDIDATE_DIR / ALPHA_NAME
    if not chroma_source.exists():
        raise FileNotFoundError(chroma_source)
    if not alpha_candidate.exists():
        raise FileNotFoundError(alpha_candidate)

    alpha = Image.open(alpha_candidate).convert("RGBA")
    resize_square(alpha, 512).save(CANDIDATE_DIR / PREVIEW_NAME)
    checkerboard_composite(resize_square(alpha, 512)).save(CANDIDATE_DIR / CHECKER_NAME)
    flat_composite(resize_square(alpha, 512), (27, 29, 38, 255)).save(CANDIDATE_DIR / DARK_NAME)
    flat_composite(resize_square(alpha, 512), (247, 239, 224, 255)).save(CANDIDATE_DIR / WARM_NAME)
    alpha_mask(alpha).resize((512, 512), Image.Resampling.LANCZOS).save(CANDIDATE_DIR / MASK_NAME)

    write_review_sheet()
    rows = build_manifest_rows()
    write_review_note(rows)
    write_process_note(rows)
    write_manifest(rows)
    print("Wrote Nephthys Batch 50 strict AI generation candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def build_manifest_rows() -> list[dict[str, str]]:
    source_turnaround = resolve_repo_path(SOURCE_TURNAROUND)
    batch47_json = resolve_repo_path(BATCH47_JSON)
    batch47_card = resolve_repo_path(BATCH47_CARD)
    batch37_alpha = resolve_repo_path(BATCH37_ALPHA)
    batch37_review = resolve_repo_path(BATCH37_REVIEW)
    unity_sprite = resolve_repo_path(UNITY_SPRITE)
    chroma_source = CANDIDATE_DIR / CHROMA_SOURCE_NAME
    alpha_candidate = CANDIDATE_DIR / ALPHA_NAME
    assets = (
        ("chromakey_source_1024", chroma_source, "1024x1024"),
        ("alpha_candidate_1024", alpha_candidate, "1024x1024"),
        ("alpha_preview_512", CANDIDATE_DIR / PREVIEW_NAME, "512x512"),
        ("checkerboard_review_512", CANDIDATE_DIR / CHECKER_NAME, "512x512"),
        ("darkfield_review_512", CANDIDATE_DIR / DARK_NAME, "512x512"),
        ("warmfield_review_512", CANDIDATE_DIR / WARM_NAME, "512x512"),
        ("alpha_mask_review_512", CANDIDATE_DIR / MASK_NAME, "512x512"),
    )

    rows: list[dict[str, str]] = []
    for asset_type, path, size in assets:
        rows.append(
            {
                "cat_id": CAT_ID,
                "display_name": DISPLAY_NAME,
                "batch_slug": BATCH_SLUG,
                "asset_type": asset_type,
                "candidate_path": to_repo_path(path),
                "candidate_sha256": sha256(path),
                "candidate_size": size,
                "source_turnaround_path": SOURCE_TURNAROUND,
                "source_turnaround_sha256": sha256(source_turnaround),
                "source_lock_id": SOURCE_LOCK_ID,
                "batch47_json_path": BATCH47_JSON,
                "batch47_json_sha256": sha256(batch47_json),
                "batch47_card_path": BATCH47_CARD,
                "batch47_card_sha256": sha256(batch47_card),
                "batch37_alpha_path": BATCH37_ALPHA,
                "batch37_alpha_sha256": sha256(batch37_alpha),
                "batch37_review_path": BATCH37_REVIEW,
                "batch37_review_sha256": sha256(batch37_review),
                "unity_sprite_path": UNITY_SPRITE,
                "unity_sprite_sha256": sha256(unity_sprite),
                "generated_source_path": to_repo_path(chroma_source),
                "generated_source_sha256": sha256(chroma_source),
                "alpha_candidate_path": to_repo_path(alpha_candidate),
                "alpha_candidate_sha256": sha256(alpha_candidate),
                "active_screenshot": ACTIVE_SCREENSHOT,
                "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                "review_note": to_repo_path(REVIEW_NOTE_PATH),
                "process_note": to_repo_path(PROCESS_NOTE_PATH),
                "agent_prompt": to_repo_path(AGENT_PROMPT_PATH),
                "recommendation": "candidate_review_only_do_not_import",
            }
        )
    return rows


def write_review_sheet() -> None:
    source = Image.open(resolve_repo_path(SOURCE_TURNAROUND)).convert("RGBA")
    batch47 = Image.open(resolve_repo_path(BATCH47_CARD)).convert("RGBA")
    unity = Image.open(resolve_repo_path(UNITY_SPRITE)).convert("RGBA")
    batch37 = Image.open(resolve_repo_path(BATCH37_ALPHA)).convert("RGBA")
    chroma = Image.open(CANDIDATE_DIR / CHROMA_SOURCE_NAME).convert("RGBA")
    alpha = Image.open(CANDIDATE_DIR / ALPHA_NAME).convert("RGBA")
    checker = Image.open(CANDIDATE_DIR / CHECKER_NAME).convert("RGBA")
    dark = Image.open(CANDIDATE_DIR / DARK_NAME).convert("RGBA")
    warm = Image.open(CANDIDATE_DIR / WARM_NAME).convert("RGBA")
    mask = Image.open(CANDIDATE_DIR / MASK_NAME).convert("RGBA")

    sheet = Image.new("RGBA", (2200, 1450), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(34)
    label_font = load_font(17)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 50 - Nephthys strict AI generation candidate", fill=(42, 36, 32), font=title_font)
    draw.text(
        (36, 76),
        "Candidate review only. Source-locked to colored three-view + Batch 47 strict spec. Do not import into Unity.",
        fill=(116, 47, 43),
        font=body_font,
    )

    draw_panel(sheet, draw, source, (36, 126), (520, 310), "locked colored three-view source", label_font)
    draw_panel(sheet, draw, batch47, (590, 126), (360, 310), "Batch 47 strict spec card", label_font)
    draw_panel(sheet, draw, unity, (984, 126), (250, 250), "current Unity sprite", label_font)
    draw_panel(sheet, draw, batch37, (1270, 126), (250, 250), "Batch 37 cutout baseline", label_font)
    draw_panel(sheet, draw, chroma, (1556, 126), (285, 285), "Batch 50 chroma source", label_font)
    draw_panel(sheet, draw, alpha, (1877, 126), (285, 285), "Batch 50 alpha candidate", label_font)

    draw_panel(sheet, draw, checker, (36, 526), (300, 300), "checkerboard review", label_font)
    draw_panel(sheet, draw, dark, (372, 526), (300, 300), "dark-field review", label_font)
    draw_panel(sheet, draw, warm, (708, 526), (300, 300), "warm-field review", label_font)
    draw_panel(sheet, draw, mask, (1044, 526), (300, 300), "alpha mask review", label_font)

    x = 1390
    y = 526
    draw.text((x, y), "Manual review notes", fill=(42, 36, 32), font=label_font)
    notes = [
        "PASS: non-human hooded cat body, exposed ears, crescent ornament, blue tear gem, ankh, and floating pyramid are present.",
        "PASS: navy, sand-gold, brown tabby, pale cloth, and blue gem palette remains close to Batch 47 palette guard.",
        "WATCH: Batch 50 is more close-up and hero-polished than Batch 37; game-scale readability must be checked.",
        "WATCH: front-only candidate still cannot prove side/back identity anchors.",
        "BLOCKED: no Unity import before active-cat screenshot comparison, Console clean, Sprite import settings, and prefab/scene binding.",
    ]
    y += 30
    for note in notes:
        for line in wrap(note, 74):
            draw.text((x, y), line, fill=(42, 36, 32), font=body_font)
            y += 19
        y += 8

    draw.text((36, 1384), f"Required active screenshot: {ACTIVE_SCREENSHOT}", fill=(116, 47, 43), font=body_font)
    draw.text((36, 1408), "Batch 50 does not supersede Batch 37 until active-cat Unity review proves it reads better without source drift.", fill=(78, 68, 60), font=body_font)
    sheet.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Nephthys Batch 50 Strict AI Generation Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "This candidate was generated after the Batch 47 strict generation spec so it can be compared against the existing Batch 37 cutout baseline.",
        "",
        "## Output Summary",
        "",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(AGENT_PROMPT_PATH)}`",
        "",
        "## Identity Review",
        "",
        "- Present: non-human cat body, visible ears, deep navy hood, gold-brown tabby face, crescent ornament, blue tear gem, blue gemstone chest ornament, ankh emblem, winged collar, striped tail, and floating pyramid prop.",
        "- Present: navy, sand-gold, brown tabby, pale cloth, and blue gem palette remains close to the Batch 47 palette guard.",
        "- Watch item: Batch 50 is more close-up and hero-polished than the Batch 37 cutout baseline.",
        "- Watch item: this is one front combat pose and cannot prove side/back identity anchors from the colored three-view turnaround.",
        f"- Blocker: active-cat Play Mode screenshot comparison is still missing: `{ACTIVE_SCREENSHOT}`.",
        "",
        "## Safety",
        "",
        "- The built-in image_gen output was copied into the workspace candidate folder.",
        "- Chroma-key removal was done locally with the imagegen skill helper.",
        "- No candidate file was copied into `Assets`.",
        "- No Unity `.meta` files were created.",
        "- Formal Unity import remains blocked pending generated candidate review and active-cat Play Mode screenshot review.",
        f"- Active-cat screenshot gate: `{ACTIVE_SCREENSHOT}`.",
        "- Batch 50 does not supersede Batch 37 until active-cat review proves the more polished close-up read is better without source drift.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['asset_type']}` -> `{row['candidate_path']}`")
    lines.append("")
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Nephthys Batch 50 Strict AI Generation Process Note",
        "",
        "Process: built-in image_gen strict source-locked generation, workspace copy, 1024 normalization, local chroma-key alpha removal, deterministic review-pack generation.",
        "",
        "Prompt goal: keep non-human hooded cat body, crescent ornament, blue tear gem, ankh, winged collar, floating pyramid prop, and Batch 47 navy/gold/brown/blue palette while avoiding human Cleopatra drift.",
        "",
        "Chroma-key helper result: key color `#0bf10d`, transparent pixels `757320/1048576`, partially transparent pixels `4037/1048576`.",
        "",
        "Rows: " + str(len(rows)),
        "",
        "No Unity import was performed.",
        "",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def resolve_repo_path(relative_path: str) -> Path:
    return REPO_ROOT / relative_path.replace("/", "\\")


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


if __name__ == "__main__":
    main()
