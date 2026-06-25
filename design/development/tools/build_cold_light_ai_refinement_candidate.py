from __future__ import annotations

import csv
import hashlib
import shutil
import sys
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_41_cold_light_ai_refinement_candidate_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "cold_light_shadow" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "cold_light_batch41_ai_refinement_manifest.csv"
PROMPT_PATH = CANDIDATE_DIR / "cold_light_batch41_ai_refinement_prompt.md"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "cold_light_batch41_ai_refinement_candidate_review.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_enemy_cold_light_batch41_ai_refinement_review_sheet.png"
SOURCE_CONCEPT_PATTERN = "design/*/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png"
SOURCE_ANIMATION_PATTERN = "design/*/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png"
BATCH38_COMBAT = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / "batch_38_p0_enemy_source_reference_pack_2026-06-15" / "cold_light_shadow" / "thecat_enemy_cold_light_shadow_batch38_combat_sprite_reference_512_512x512_candidate_v001.png"
BATCH38_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / "batch_38_p0_enemy_source_reference_pack_2026-06-15" / "thecat_enemy_p0_core_batch38_source_reference_review_sheet.png"
SOURCE_LOCK_IDS = "cold_light_concept;cold_light_animation"
ACTIVE_SCREENSHOT = "08-active-enemy-cold-light.png"

RAW_NAME = "thecat_enemy_cold_light_shadow_ai_refinement_raw_codex_v001.png"
CANDIDATE_NAME = "thecat_enemy_cold_light_shadow_ai_refinement_combat_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_enemy_cold_light_shadow_ai_refinement_combat_512_preview_v001.png"

PROMPT_TEXT = """# Cold Light Shadow Batch 41 AI Refinement Prompt

Use case: stylized-concept
Asset type: game enemy combat sprite candidate for TheCat P0 enemy Cold Light Shadow
Primary request: Create one polished full-body combat sprite candidate for Cold Light Shadow, strictly preserving the provided reference image identity.
Input images: the visible Cold Light Shadow concept image is the hard visual authority.
Subject: corrupted mechanical desk-lamp nightmare enemy, thin floating lamp-shadow silhouette, angled metal lamp arm with springs and joints, cold cyan lamp head glow, one hostile red eye inside the pale cyan light, black mud dripping from the arm and shade, black mud base pooling around the foot, ranged beam pressure cue.
Style/medium: hand-painted dream-fantasy 2D game sprite, readable combat silhouette for a top/angled camera, soft ink linework, painterly metal and glossy mud texture, consistent with the provided reference.
Composition/framing: centered single enemy, square 1024x1024, three-quarter front view, full body including base and lamp head, generous padding, neutral warm parchment background, no UI frame.
Color palette: lock to dark gunmetal, black glossy mud, pale cyan cold light, small saturated red eye, subtle cold blue highlights, neutral parchment ground.
Constraints: preserve cold lamp silhouette, cyan beam/light language, mechanical arm, black mud base, single red eye, long shadow-limb feel, and ranged-pressure read. Keep it clearly the same enemy as the reference, not a redesign.
Avoid: ordinary clean desk lamp, warm candle or fire palette, cute robot, humanoid body, human hands, extra phone/alarm/toy motifs, black mud removed, missing red eye, missing cyan light, missing mechanical spring arm, missing mud base, gore, realistic horror anatomy, text, logo, watermark, UI frame, heavy scene background.
"""


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("Usage: build_cold_light_ai_refinement_candidate.py <generated_image_path>")

    generated_path = Path(sys.argv[1]).expanduser().resolve()
    if not generated_path.exists():
        raise FileNotFoundError(generated_path)

    concept_path = resolve_one(SOURCE_CONCEPT_PATTERN)
    animation_path = resolve_one(SOURCE_ANIMATION_PATTERN)
    require_file(BATCH38_COMBAT)
    require_file(BATCH38_REVIEW)
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
    write_review_note(concept_path, animation_path, generated_path, raw_path, candidate_path, preview_path)
    write_review_sheet(concept_path, candidate_path, preview_path)
    write_manifest(concept_path, animation_path, generated_path, raw_path, candidate_path, preview_path)
    write_summary(concept_path, animation_path, candidate_path, preview_path)

    print("Wrote Cold Light AI refinement candidate pack.")
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


def write_review_sheet(concept_path: Path, candidate_path: Path, preview_path: Path) -> None:
    concept = Image.open(concept_path).convert("RGBA")
    batch38_combat = Image.open(BATCH38_COMBAT).convert("RGBA")
    candidate = Image.open(candidate_path).convert("RGBA")
    preview = Image.open(preview_path).convert("RGBA")

    sheet = Image.new("RGBA", (1600, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(28)
    label_font = load_font(16)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 41 - Cold Light AI refinement candidate", fill=(42, 36, 32), font=title_font)
    draw.text((36, 68), "Candidate review only. Stored outside Assets. Formal import remains blocked until active-enemy screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_preview = fit_to_canvas(concept, (680, 398), (250, 246, 237, 255))
    sheet.alpha_composite(source_preview, (36, 112))
    draw.text((36, 522), "Source authority: Cold Light concept", fill=(42, 36, 32), font=label_font)

    batch38_panel = fit_to_canvas(batch38_combat, (320, 320), (250, 246, 237, 255))
    candidate_panel = fit_to_canvas(candidate, (430, 430), (250, 246, 237, 255))
    preview_panel = fit_to_canvas(preview, (220, 220), (250, 246, 237, 255))

    draw_panel(sheet, draw, batch38_panel, (760, 112), "Batch 38 combat reference", label_font)
    draw_panel(sheet, draw, candidate_panel, (1098, 112), "AI candidate 1024", label_font)
    draw_panel(sheet, draw, preview_panel, (760, 510), "512 preview", label_font)

    checks = (
        "Pass candidate traits to inspect: cold lamp silhouette, cyan light language, mechanical spring arm, black mud base, single red eye.",
        "Watch item: candidate remains a single-view AI refinement; cast and beam timing still comes from cold_light_animation.",
        "Import status: blocked until Play Mode active screenshot 08-active-enemy-cold-light.png is reviewed.",
    )
    y = 590
    for check in checks:
        y = draw_wrapped_text(draw, "- " + check, (36, y), body_font, 84, fill=(42, 36, 32)) + 12

    REVIEW_SHEET_PATH.parent.mkdir(parents=True, exist_ok=True)
    sheet.save(REVIEW_SHEET_PATH)


def draw_panel(sheet: Image.Image, draw: ImageDraw.ImageDraw, image: Image.Image, pos: tuple[int, int], label: str, font: ImageFont.ImageFont) -> None:
    x, y = pos
    draw.rounded_rectangle((x - 10, y - 10, x + image.width + 10, y + image.height + 42), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
    sheet.alpha_composite(image, pos)
    draw.text((x, y + image.height + 14), label, fill=(42, 36, 32), font=font)


def draw_wrapped_text(draw: ImageDraw.ImageDraw, text: str, pos: tuple[int, int], font: ImageFont.ImageFont, width: int, fill: tuple[int, int, int]) -> int:
    x, y = pos
    line_height = 22
    for line in textwrap.wrap(text, width=width):
        draw.text((x, y), line, fill=fill, font=font)
        y += line_height
    return y


def fit_to_canvas(image: Image.Image, size: tuple[int, int], background: tuple[int, int, int, int]) -> Image.Image:
    canvas = Image.new("RGBA", size, background)
    work = image.copy()
    work.thumbnail((size[0] - 20, size[1] - 20), Image.Resampling.LANCZOS)
    x = (size[0] - work.width) // 2
    y = (size[1] - work.height) // 2
    canvas.alpha_composite(work, (x, y))
    return canvas


def write_review_note(concept_path: Path, animation_path: Path, generated_path: Path, raw_path: Path, candidate_path: Path, preview_path: Path) -> None:
    lines = [
        "# Cold Light Batch 41 AI Refinement Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "Formal Unity import remains blocked until active-enemy Play Mode screenshot review passes.",
        "",
        "## Source Authority",
        "",
        "- Enemy: `Cold Light Shadow`",
        f"- Source concept: `{to_repo_path(concept_path)}`",
        f"- Source animation: `{to_repo_path(animation_path)}`",
        f"- Batch 38 source reference sheet: `{to_repo_path(BATCH38_REVIEW)}`",
        f"- Batch 38 combat crop reference: `{to_repo_path(BATCH38_COMBAT)}`",
        f"- Source lock ids: `{SOURCE_LOCK_IDS}`",
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
        "- Pass: cold lamp silhouette, pale cyan light, mechanical spring arm, black mud base, single red eye, and ranged-pressure read are preserved.",
        "- Pass: the candidate stays close to the source concept and avoids warm fire palette, clean ordinary desk lamp, humanoid, or cute robot drift.",
        "- Watch: this is a single-view AI refinement only; cast timing and beam identity remain governed by the source animation sheet and Batch 38 reference pack.",
        "- Watch: final runtime sprite import still needs transparent or cutout treatment and Play Mode screenshot comparison.",
        "",
        "## Rejection Rules",
        "",
        "- Reject if future iterations lose the cold lamp silhouette, cyan beam/light language, mechanical arm, black mud base, single red eye, long shadow-limb feel, or ranged-pressure read.",
        "- Reject ordinary clean desk lamp, warm candle or fire palette, cute robot styling, humanoid body, black mud removal, missing red eye, missing cyan light, missing spring arm, or missing mud base.",
        "- Reject if the candidate is imported into Unity before active-enemy screenshot review.",
        "",
    ]
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(concept_path: Path, animation_path: Path, generated_path: Path, raw_path: Path, candidate_path: Path, preview_path: Path) -> None:
    rows = [
        build_row("ai_refinement_raw_codex", raw_path, concept_path, animation_path, generated_path, "source_raw"),
        build_row("ai_refinement_combat_1024", candidate_path, concept_path, animation_path, generated_path, "1024x1024"),
        build_row("ai_refinement_combat_512_preview", preview_path, concept_path, animation_path, generated_path, "512x512"),
    ]
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def build_row(asset_type: str, candidate_path: Path, concept_path: Path, animation_path: Path, generated_path: Path, candidate_size: str) -> dict[str, str]:
    return {
        "enemy_id": "cold_light_shadow",
        "display_name": "Cold Light Shadow",
        "combat_role": "P0 ranged harasser and bed beam pressure",
        "batch_slug": BATCH_SLUG,
        "asset_type": asset_type,
        "candidate_path": to_repo_path(candidate_path),
        "candidate_sha256": sha256(candidate_path),
        "candidate_size": actual_size(candidate_path) if candidate_size == "source_raw" else candidate_size,
        "concept_source_path": to_repo_path(concept_path),
        "concept_source_sha256": sha256(concept_path),
        "animation_source_path": to_repo_path(animation_path),
        "animation_source_sha256": sha256(animation_path),
        "source_lock_ids": SOURCE_LOCK_IDS,
        "generated_source_path": str(generated_path),
        "generated_source_sha256": sha256(generated_path),
        "prompt_record": to_repo_path(PROMPT_PATH),
        "batch38_review_sheet": to_repo_path(BATCH38_REVIEW),
        "batch38_combat_reference": to_repo_path(BATCH38_COMBAT),
        "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
        "review_note": to_repo_path(REVIEW_NOTE_PATH),
        "active_screenshot": ACTIVE_SCREENSHOT,
        "recommendation": "candidate_review_only_pending_playmode_screenshot",
    }


def write_summary(concept_path: Path, animation_path: Path, candidate_path: Path, preview_path: Path) -> None:
    summary = BATCH_DIR / "cold_light_batch41_ai_refinement_summary.md"
    lines = [
        "# Cold Light Batch 41 AI Refinement Candidate",
        "",
        "This batch records the first Codex built-in image-generation Cold Light Shadow candidate. The candidate remains outside Unity and is not approved for import.",
        "",
        f"- Source concept: `{to_repo_path(concept_path)}`",
        f"- Source animation: `{to_repo_path(animation_path)}`",
        f"- Batch 38 reference sheet: `{to_repo_path(BATCH38_REVIEW)}`",
        f"- Candidate: `{to_repo_path(candidate_path)}`",
        f"- Preview: `{to_repo_path(preview_path)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Review note: `{to_repo_path(REVIEW_NOTE_PATH)}`",
        "- Unity import: blocked pending active-enemy Play Mode screenshot review.",
        "",
    ]
    summary.write_text("\n".join(lines), encoding="utf-8")


def actual_size(path: Path) -> str:
    image = Image.open(path)
    try:
        return f"{image.width}x{image.height}"
    finally:
        image.close()


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
