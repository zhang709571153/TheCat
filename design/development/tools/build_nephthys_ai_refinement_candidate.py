from __future__ import annotations

import csv
import hashlib
import shutil
import sys
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_36_nephthys_ai_refinement_candidate_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "nephthys_batch36_ai_refinement_manifest.csv"
PROMPT_PATH = CANDIDATE_DIR / "nephthys_batch36_ai_refinement_prompt.md"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "nephthys_batch36_ai_refinement_candidate_review.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_cat_nephthys_batch36_ai_refinement_review_sheet.png"
SOURCE_PATTERN = "design/*/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
BATCH32_FRONT = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / "batch_32_nephthys_strict_turnaround_derivatives_2026-06-14" / "thecat_cat_nephthys_front_view_source_reference_512_candidate_v001.png"
BATCH32_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / "batch_32_nephthys_strict_turnaround_derivatives_2026-06-14" / "thecat_cat_nephthys_batch32_strict_turnaround_review_sheet.png"
SOURCE_LOCK_ID = "nephthys_turnaround_colored"
ACTIVE_SCREENSHOT = "05-active-cat-nephthys.png"

RAW_NAME = "thecat_cat_nephthys_ai_refinement_raw_codex_v001.png"
CANDIDATE_NAME = "thecat_cat_nephthys_ai_refinement_combat_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_cat_nephthys_ai_refinement_combat_512_preview_v001.png"

PROMPT_TEXT = """# Nephthys Batch 36 AI Refinement Prompt

Use case: stylized-concept
Asset type: game character combat sprite candidate for TheCat P0 starter cat Nephthys
Primary request: Create one polished full-body front-facing combat sprite candidate for Nephthys, the non-human gold-brown tabby moon-sand agent cat shown in the provided reference images. The candidate must preserve the reference identity strictly.
Input images: the visible Nephthys colored three-view turnaround is the hard visual authority; the visible Batch 32 review sheet is the production reference.
Subject: compact non-human cat body, large cat head, upright ears, golden eyes, gold-brown tabby face stripes, small cat muzzle, short paws and compact legs, no human face, no human proportions.
Costume and props: deep navy hood and cloak, crescent moon hood ornament with blue tear gem, sand-gold script border, blue gemstone chest ornament, winged gold collar, ankh emblem, layered navy cloak panels, wrapped cream trousers, blue-gold bracelets, striped gold-brown tail, floating pyramid/obelisk prop with eye markings and cyan magic particles.
Style/medium: hand-painted dream-fantasy game sprite, clean readable silhouette, soft ink linework, painterly texture matching the turnaround reference, high-quality 2D illustration.
Composition/framing: centered full-body front view, square 1024x1024, generous padding, neutral warm parchment background, no shadow-heavy scene, no extra characters.
Color palette: lock to gold-brown fur, dark tabby stripes, deep navy hood and cloak, sand-gold trim and symbols, bright blue gems, cyan magic particles, warm cream trousers.
Constraints: preserve hood, gold script trim, crescent ornament, blue tear gem, golden eyes, tabby face, blue gemstone chest ornament, winged gold collar, ankh emblem, floating pyramid prop, striped tail, compact non-human cat posture, and moon-sand controller palette. Keep the character visibly the same as the reference, not a redesign.
Avoid: human Egyptian priest body, Cleopatra costume cliche, long legs, human torso, human hands, generic Egyptian fantasy cat, missing hood, missing crescent, missing blue gem, missing floating pyramid prop, missing ankh, missing striped tail, palette drift, extra weapons, text, logo, watermark, UI frame.
"""


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("Usage: build_nephthys_ai_refinement_candidate.py <generated_image_path>")

    generated_path = Path(sys.argv[1]).expanduser().resolve()
    if not generated_path.exists():
        raise FileNotFoundError(generated_path)

    source_path = resolve_one(SOURCE_PATTERN)
    require_file(BATCH32_FRONT)
    require_file(BATCH32_REVIEW)
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

    print("Wrote Nephthys AI refinement candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda p: p.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected exactly one match for {pattern}, found {len(matches)}")
    return matches[0]


def require_file(path: Path) -> None:
    if not path.exists():
        raise FileNotFoundError(path)


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
    batch32 = Image.open(BATCH32_FRONT).convert("RGBA")
    candidate = Image.open(candidate_path).convert("RGBA")
    preview = Image.open(preview_path).convert("RGBA")

    sheet = Image.new("RGBA", (1600, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(28)
    label_font = load_font(16)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 36 - Nephthys AI refinement candidate", fill=(42, 36, 32), font=title_font)
    draw.text((36, 68), "Candidate review only. Stored outside Assets. Formal import remains blocked until active-cat screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_preview = fit_to_canvas(source, (720, 398), (250, 246, 237, 255))
    sheet.alpha_composite(source_preview, (36, 112))
    draw.text((36, 522), "Source authority: colored three-view turnaround", fill=(42, 36, 32), font=label_font)

    batch32_panel = fit_to_canvas(batch32, (320, 320), (250, 246, 237, 255))
    candidate_panel = fit_to_canvas(candidate, (430, 430), (250, 246, 237, 255))
    preview_panel = fit_to_canvas(preview, (220, 220), (250, 246, 237, 255))

    draw_panel(sheet, draw, batch32_panel, (820, 112), "Batch 32 front reference", label_font)
    draw_panel(sheet, draw, candidate_panel, (1158, 112), "AI candidate 1024", label_font)
    draw_panel(sheet, draw, preview_panel, (820, 510), "512 preview", label_font)

    checks = (
        "Pass candidate traits: non-human tabby cat, gold eyes, navy hood, crescent, blue gems, ankh, pyramid prop, striped tail.",
        "Watch item: AI candidate is front-view only; side/back identity still comes from Batch 32 and must remain locked.",
        "Import status: blocked until Play Mode active screenshot 05-active-cat-nephthys.png is reviewed.",
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
        "# Nephthys Batch 36 AI Refinement Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "formal import remains blocked until active-cat Play Mode screenshot review passes.",
        "",
        "## Source Authority",
        "",
        f"- Colored three-view turnaround: `{to_repo_path(source_path)}`",
        f"- Batch 32 review sheet: `{to_repo_path(BATCH32_REVIEW)}`",
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
        "- Pass: compact non-human cat body, oversized cat head, upright ears, golden eyes, and gold-brown tabby face markings are preserved.",
        "- Pass: deep navy hood and cloak, crescent ornament, blue tear gem, sand-gold script trim, blue gemstone chest ornament, winged gold collar, ankh emblem, floating pyramid/obelisk prop, and striped tail are present.",
        "- Pass: no text, logo, watermark, extra character, Cleopatra costume cliche, or obvious human Egyptian priest body.",
        "- Watch: this is a front-view AI refinement only; side and back anchors remain governed by the colored turnaround and Batch 32 source-derived references.",
        "- Watch: final runtime sprite import still needs transparent or cutout treatment and Play Mode screenshot comparison.",
        "",
        "## Rejection Rules",
        "",
        "- Reject if future iterations lose hood, gold script trim, crescent ornament, blue tear gem, golden eyes, tabby face, blue gemstone chest ornament, winged gold collar, ankh emblem, floating pyramid prop, or striped tail.",
        "- Reject if future iterations introduce Cleopatra costume cliche, human Egyptian priest proportions, long legs, human hands, generic Egyptian fantasy drift, or generic controller costume.",
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
        "cat_id": "nephthys",
        "display_name": "Nephthys / Moon-Sand Agent",
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
    summary = BATCH_DIR / "nephthys_batch36_ai_refinement_summary.md"
    lines = [
        "# Nephthys Batch 36 AI Refinement Candidate",
        "",
        "This batch records the first Codex built-in image-generation Nephthys candidate. The candidate remains outside Unity and is not approved for import.",
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
