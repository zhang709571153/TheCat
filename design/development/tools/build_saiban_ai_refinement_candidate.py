from __future__ import annotations

import csv
import hashlib
import shutil
import sys
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_30_ai_refinement_candidate_2026-06-14"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "saiban" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "saiban_batch30_ai_refinement_manifest.csv"
PROMPT_PATH = CANDIDATE_DIR / "saiban_batch30_ai_refinement_prompt.md"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "saiban_batch30_ai_refinement_candidate_review.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_cat_saiban_batch30_ai_refinement_review_sheet.png"
SOURCE_PATTERN = "design/*/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
BATCH29_FRONT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "saiban" / "batch_29_strict_turnaround_derivatives_2026-06-14" / "thecat_cat_saiban_front_view_source_reference_512_candidate_v001.png"
BATCH29_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "saiban" / "batch_29_strict_turnaround_derivatives_2026-06-14" / "thecat_cat_saiban_batch29_strict_turnaround_review_sheet.png"
SOURCE_LOCK_ID = "saiban_turnaround_colored"
ACTIVE_SCREENSHOT = "04-active-cat-saiban.png"

RAW_NAME = "thecat_cat_saiban_ai_refinement_raw_codex_v001.png"
CANDIDATE_NAME = "thecat_cat_saiban_ai_refinement_combat_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_cat_saiban_ai_refinement_combat_512_preview_v001.png"

PROMPT_TEXT = """# Saiban Batch 30 AI Refinement Prompt

Use case: stylized-concept
Asset type: game character combat sprite candidate for TheCat P0 starter cat Saiban
Primary request: Create one polished full-body front-facing combat sprite candidate for Saiban, the non-human silver-gray tabby cat sword saint shown in the provided reference images. The candidate must preserve the reference identity strictly.
Input images: the visible Saiban colored three-view turnaround is the hard visual authority; the visible Batch 29 review sheet is the production reference.
Subject: compact non-human cat body, large cat head, upright ears, pale green eyes, silver-gray fur, clear dark tabby forehead and cheek stripes, small cat muzzle, no human face, no human proportions.
Costume and props: silver plate armor with gold trim and blue gem accents, deep red torn cape and collar, small helm/crown detail, round oath sun shield on the cat's left side, single sword on the cat's right side, short paws and compact legs.
Style/medium: hand-painted dream-fantasy game sprite, clean readable silhouette, soft ink linework, painterly texture matching the turnaround reference, high-quality 2D illustration.
Composition/framing: centered full-body front view, square 1024x1024, generous padding, neutral warm parchment background, no shadow-heavy scene, no extra characters.
Color palette: lock to silver-gray fur, dark gray tabby markings, deep red cape, silver armor, gold trim, blue gems, warm sun shield tones.
Constraints: preserve shield, sword, tabby face, cape, helm, silver-gray fur, red cape, silver armor, gold trim, blue gems, compact non-human cat posture. Keep the character visibly the same as the reference, not a redesign.
Avoid: human knight body, long legs, human torso, human hands, generic armored cat, missing shield, missing sword, missing cape, palette drift, extra weapons, text, logo, watermark, UI frame.
"""


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("Usage: build_saiban_ai_refinement_candidate.py <generated_image_path>")

    generated_path = Path(sys.argv[1]).expanduser().resolve()
    if not generated_path.exists():
        raise FileNotFoundError(generated_path)

    source_path = resolve_one(SOURCE_PATTERN)
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    raw_path = CANDIDATE_DIR / RAW_NAME
    candidate_path = CANDIDATE_DIR / CANDIDATE_NAME
    preview_path = CANDIDATE_DIR / PREVIEW_NAME

    shutil.copy2(generated_path, raw_path)
    raw_image = Image.open(raw_path).convert("RGBA")
    normalize_square(raw_image, 1024).save(candidate_path)
    normalize_square(raw_image, 512).save(preview_path)

    PROMPT_PATH.write_text(PROMPT_TEXT, encoding="utf-8")
    write_review_note(source_path, generated_path, raw_path, candidate_path, preview_path)
    write_review_sheet(source_path, candidate_path, preview_path)
    write_manifest(source_path, generated_path, raw_path, candidate_path, preview_path)
    write_summary(source_path, candidate_path, preview_path)

    print("Wrote Saiban AI refinement candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda p: p.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected exactly one match for {pattern}, found {len(matches)}")
    return matches[0]


def normalize_square(image: Image.Image, size: int) -> Image.Image:
    canvas = Image.new("RGBA", (size, size), (250, 246, 237, 255))
    work = image.copy()
    work.thumbnail((size, size), Image.Resampling.LANCZOS)
    x = (size - work.width) // 2
    y = (size - work.height) // 2
    canvas.alpha_composite(work, (x, y))
    return canvas


def write_review_sheet(source_path: Path, candidate_path: Path, preview_path: Path) -> None:
    source = Image.open(source_path).convert("RGBA")
    batch29 = Image.open(BATCH29_FRONT).convert("RGBA")
    candidate = Image.open(candidate_path).convert("RGBA")
    preview = Image.open(preview_path).convert("RGBA")

    sheet = Image.new("RGBA", (1600, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(28)
    label_font = load_font(16)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 30 - Saiban AI refinement candidate", fill=(42, 36, 32), font=title_font)
    draw.text((36, 68), "Candidate review only. Stored outside Assets. Formal import remains blocked until active-cat screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_preview = fit_to_canvas(source, (720, 398), (250, 246, 237, 255))
    sheet.alpha_composite(source_preview, (36, 112))
    draw.text((36, 522), "Source authority: colored three-view turnaround", fill=(42, 36, 32), font=label_font)

    batch29_panel = fit_to_canvas(batch29, (320, 320), (250, 246, 237, 255))
    candidate_panel = fit_to_canvas(candidate, (430, 430), (250, 246, 237, 255))
    preview_panel = fit_to_canvas(preview, (220, 220), (250, 246, 237, 255))

    draw_panel(sheet, draw, batch29_panel, (820, 112), "Batch 29 front reference", label_font)
    draw_panel(sheet, draw, candidate_panel, (1158, 112), "AI candidate 1024", label_font)
    draw_panel(sheet, draw, preview_panel, (820, 510), "512 preview", label_font)

    checks = (
        "Pass candidate traits: non-human cat, shield, sword, red cape, tabby face, silver armor, gold trim, blue gems.",
        "Watch item: AI candidate is front-view only; side/back identity still comes from Batch 29 and must remain locked.",
        "Import status: blocked until Play Mode active screenshot 04-active-cat-saiban.png is reviewed.",
    )
    y = 590
    for check in checks:
        draw.text((36, y), "- " + check, fill=(42, 36, 32), font=body_font)
        y += 30

    REVIEW_SHEET_PATH.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(REVIEW_SHEET_PATH)


def draw_panel(sheet: Image.Image, draw: ImageDraw.ImageDraw, image: Image.Image, pos: tuple[int, int], label: str, font: ImageFont.ImageFont) -> None:
    x, y = pos
    draw.rounded_rectangle((x - 10, y - 10, x + image.width + 10, y + image.height + 42), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
    sheet.alpha_composite(image, pos)
    draw.text((x, y + image.height + 14), label, fill=(42, 36, 32), font=font)


def fit_to_canvas(image: Image.Image, size: tuple[int, int], background: tuple[int, int, int, int]) -> Image.Image:
    canvas = Image.new("RGBA", size, background)
    work = image.copy()
    work.thumbnail((size[0] - 20, size[1] - 20), Image.Resampling.LANCZOS)
    x = (size[0] - work.width) // 2
    y = (size[1] - work.height) // 2
    canvas.alpha_composite(work, (x, y))
    return canvas


def write_review_note(source_path: Path, generated_path: Path, raw_path: Path, candidate_path: Path, preview_path: Path) -> None:
    lines = [
        "# Saiban Batch 30 AI Refinement Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "formal import remains blocked until active-cat Play Mode screenshot review passes.",
        "",
        "## Source Authority",
        "",
        f"- Colored three-view turnaround: `{to_repo_path(source_path)}`",
        f"- Batch 29 review sheet: `{to_repo_path(BATCH29_REVIEW)}`",
        f"- Source lock id: `{SOURCE_LOCK_ID}`",
        f"- Active screenshot required before import: `{ACTIVE_SCREENSHOT}`",
        "",
        "## Generated Candidate",
        "",
        f"- Built-in image generation source: `{generated_path}`",
        f"- Raw project copy: `{to_repo_path(raw_path)}`",
        f"- Standardized candidate: `{to_repo_path(candidate_path)}`",
        f"- Preview candidate: `{to_repo_path(preview_path)}`",
        f"- Prompt record: `{to_repo_path(PROMPT_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        "",
        "## Visual Review",
        "",
        "- Pass: compact non-human cat body, oversized cat head, upright ears, pale green eyes, and tabby face markings are preserved.",
        "- Pass: round oath shield, single sword, deep red cape, silver armor, gold trim, and blue gems are present.",
        "- Pass: no text, logo, watermark, extra character, or obvious human knight body.",
        "- Watch: this is a front-view AI refinement only; side and back anchors remain governed by the colored turnaround and Batch 29 source-derived references.",
        "- Watch: final runtime sprite import still needs transparent or cutout treatment and Play Mode screenshot comparison.",
        "",
        "## Rejection Rules",
        "",
        "- Reject if future iterations lose shield, sword, tabby face, red cape, helm/crown detail, silver-gray fur, silver armor, gold trim, or blue gems.",
        "- Reject if future iterations introduce human body proportions, long legs, human hands, or generic knight-cat drift.",
        "- Reject if palette drifts away from the locked colored three-view turnaround.",
        "- Reject if the candidate is imported into Unity before active-cat screenshot review.",
        "",
    ]
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(source_path: Path, generated_path: Path, raw_path: Path, candidate_path: Path, preview_path: Path) -> None:
    rows = [
        build_row("ai_refinement_raw_codex", raw_path, source_path, generated_path, "source_raw"),
        build_row("ai_refinement_combat_1024", candidate_path, source_path, generated_path, "1024x1024"),
        build_row("ai_refinement_combat_512_preview", preview_path, source_path, generated_path, "512x512"),
    ]
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def build_row(asset_type: str, candidate_path: Path, source_path: Path, generated_path: Path, candidate_size: str) -> dict[str, str]:
    return {
        "cat_id": "saiban",
        "display_name": "Saiban / Sword Saint",
        "batch_slug": BATCH_SLUG,
        "asset_type": asset_type,
        "candidate_path": to_repo_path(candidate_path),
        "candidate_sha256": sha256(candidate_path),
        "candidate_size": actual_size(candidate_path) if candidate_size == "source_raw" else candidate_size,
        "source_turnaround_path": to_repo_path(source_path),
        "source_turnaround_sha256": sha256(source_path),
        "source_lock_id": SOURCE_LOCK_ID,
        "generated_source_path": str(generated_path),
        "generated_source_sha256": sha256(generated_path),
        "prompt_record": to_repo_path(PROMPT_PATH),
        "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
        "review_note": to_repo_path(REVIEW_NOTE_PATH),
        "active_screenshot": ACTIVE_SCREENSHOT,
        "recommendation": "candidate_review_only_pending_playmode_screenshot",
    }


def write_summary(source_path: Path, candidate_path: Path, preview_path: Path) -> None:
    summary = BATCH_DIR / "saiban_batch30_ai_refinement_summary.md"
    lines = [
        "# Saiban Batch 30 AI Refinement Candidate",
        "",
        "This batch records the first Codex built-in image-generation Saiban candidate. The candidate remains outside Unity and is not approved for import.",
        "",
        f"- Source authority: `{to_repo_path(source_path)}`",
        f"- Candidate: `{to_repo_path(candidate_path)}`",
        f"- Preview: `{to_repo_path(preview_path)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Review note: `{to_repo_path(REVIEW_NOTE_PATH)}`",
        "- Unity import: blocked pending active-cat Play Mode screenshot review.",
        "",
    ]
    summary.write_text("\n".join(lines), encoding="utf-8")


def actual_size(path: Path) -> str:
    image = Image.open(path)
    return f"{image.width}x{image.height}"


def load_font(size: int) -> ImageFont.ImageFont:
    candidates = (
        Path("C:/Windows/Fonts/arial.ttf"),
        Path("C:/Windows/Fonts/segoeui.ttf"),
    )
    for candidate in candidates:
        if candidate.exists():
            return ImageFont.truetype(str(candidate), size=size)
    return ImageFont.load_default()


def sha256(path: Path) -> str:
    hasher = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(65536), b""):
            hasher.update(chunk)
    return hasher.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
