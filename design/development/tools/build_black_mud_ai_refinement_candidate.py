from __future__ import annotations

import csv
import hashlib
import shutil
import sys
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_39_black_mud_ai_refinement_candidate_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "black_mud_nightmare" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "black_mud_batch39_ai_refinement_manifest.csv"
PROMPT_PATH = CANDIDATE_DIR / "black_mud_batch39_ai_refinement_prompt.md"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "black_mud_batch39_ai_refinement_candidate_review.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_enemy_black_mud_batch39_ai_refinement_review_sheet.png"
SOURCE_CONCEPT_PATTERN = "design/*/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png"
SOURCE_ANIMATION_PATTERN = "design/*/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png"
BATCH38_COMBAT = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / "batch_38_p0_enemy_source_reference_pack_2026-06-15" / "black_mud_nightmare" / "thecat_enemy_black_mud_nightmare_batch38_combat_sprite_reference_512_512x512_candidate_v001.png"
BATCH38_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / "batch_38_p0_enemy_source_reference_pack_2026-06-15" / "thecat_enemy_p0_core_batch38_source_reference_review_sheet.png"
SOURCE_LOCK_IDS = "black_mud_concept;black_mud_animation"
ACTIVE_SCREENSHOT = "07-active-enemy-black-mud.png"

RAW_NAME = "thecat_enemy_black_mud_nightmare_ai_refinement_raw_codex_v001.png"
CANDIDATE_NAME = "thecat_enemy_black_mud_nightmare_ai_refinement_combat_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_enemy_black_mud_nightmare_ai_refinement_combat_512_preview_v001.png"

PROMPT_TEXT = """# Black Mud Nightmare Batch 39 AI Refinement Prompt

Use case: stylized-concept
Asset type: game enemy combat sprite candidate for TheCat P0 enemy Black Mud Nightmare
Primary request: Create one polished full-body combat sprite candidate for Black Mud Nightmare, strictly preserving the provided reference image identity.
Input images: the visible Black Mud Nightmare concept image is the hard visual authority.
Subject: low crawling black-violet sludge mass, glossy soft mud body, squat dome silhouette, two bright hostile red eyes, dark sleepy face imprint inside the mud, small dripping curl of sludge on top, puddled edges spreading along the floor, readable near-bed pressure threat.
Style/medium: hand-painted dream-fantasy 2D game sprite, clean readable silhouette, soft ink linework, painterly highlights, consistent with the provided reference.
Composition/framing: centered single enemy, square 1024x1024, three-quarter front view suitable for top/angled combat camera, generous padding, neutral warm parchment background, no UI frame.
Color palette: lock to black and deep violet mud, glossy indigo highlights, dim gray sleeping-face imprint, saturated red glowing eyes, subtle warm parchment ground.
Constraints: preserve black sludge body, red eyes, soft-mud monster silhouette, crawling pressure, bed-contact threat, glossy pooled mud, and low squat shape. Keep it clearly the same enemy as the reference, not a redesign.
Avoid: cute pet styling, generic ghost shape, humanoid body, limbs with anatomy, gore, realistic horror anatomy, extra characters, phone/lamp/alarm motifs, cat features, text, logo, watermark, UI frame, heavy scene background.
"""


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("Usage: build_black_mud_ai_refinement_candidate.py <generated_image_path>")

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

    print("Wrote Black Mud AI refinement candidate pack.")
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

    draw.text((36, 28), "P0 Batch 39 - Black Mud AI refinement candidate", fill=(42, 36, 32), font=title_font)
    draw.text((36, 68), "Candidate review only. Stored outside Assets. Formal import remains blocked until active-enemy screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_preview = fit_to_canvas(concept, (680, 398), (250, 246, 237, 255))
    sheet.alpha_composite(source_preview, (36, 112))
    draw.text((36, 522), "Source authority: Black Mud concept", fill=(42, 36, 32), font=label_font)

    batch38_panel = fit_to_canvas(batch38_combat, (320, 320), (250, 246, 237, 255))
    candidate_panel = fit_to_canvas(candidate, (430, 430), (250, 246, 237, 255))
    preview_panel = fit_to_canvas(preview, (220, 220), (250, 246, 237, 255))

    draw_panel(sheet, draw, batch38_panel, (760, 112), "Batch 38 combat reference", label_font)
    draw_panel(sheet, draw, candidate_panel, (1098, 112), "AI candidate 1024", label_font)
    draw_panel(sheet, draw, preview_panel, (760, 510), "512 preview", label_font)

    checks = (
        "Pass candidate traits: low black sludge body, red eyes, glossy mud, sleepy face imprint, top drip, puddled crawl edges.",
        "Watch item: candidate remains a single-view AI refinement; animation authority still comes from black_mud_animation.",
        "Import status: blocked until Play Mode active screenshot 07-active-enemy-black-mud.png is reviewed.",
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
        "# Black Mud Batch 39 AI Refinement Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "Formal Unity import remains blocked until active-enemy Play Mode screenshot review passes.",
        "",
        "## Source Authority",
        "",
        "- Enemy: `Black Mud Nightmare`",
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
        "- Pass: black sludge body, red eyes, soft-mud monster silhouette, glossy pooled mud, sleepy face imprint, and low crawling shape are preserved.",
        "- Pass: the candidate stays close to the source concept and avoids phone, lamp, alarm, humanoid, or gore motifs.",
        "- Watch: this is a single-view AI refinement only; animation identity remains governed by the source animation sheet and Batch 38 reference pack.",
        "- Watch: final runtime sprite import still needs transparent or cutout treatment and Play Mode screenshot comparison.",
        "",
        "## Rejection Rules",
        "",
        "- Reject if future iterations lose the black sludge body, red eyes, soft-mud monster silhouette, crawling pressure, bed-contact threat, glossy pooled mud, or low squat shape.",
        "- Reject cute pet styling, generic ghost shape, humanoid body, gore, realistic horror anatomy, extra dream interruption objects, cat features, or palette drift.",
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
        "enemy_id": "black_mud_nightmare",
        "display_name": "Black Mud Nightmare",
        "combat_role": "P0 melee bed-pressure swarmer",
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
    summary = BATCH_DIR / "black_mud_batch39_ai_refinement_summary.md"
    lines = [
        "# Black Mud Batch 39 AI Refinement Candidate",
        "",
        "This batch records the first Codex built-in image-generation Black Mud Nightmare candidate. The candidate remains outside Unity and is not approved for import.",
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
