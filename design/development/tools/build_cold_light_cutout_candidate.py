from __future__ import annotations

import csv
import hashlib
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_42_cold_light_cutout_candidate_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "cold_light_shadow" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "cold_light_batch42_cutout_manifest.csv"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "cold_light_batch42_cutout_candidate_review.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_enemy_cold_light_batch42_cutout_review_sheet.png"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "cold_light_batch42_cutout_process_note.md"
SOURCE_CONCEPT_PATTERN = "design/*/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png"
SOURCE_ANIMATION_PATTERN = "design/*/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png"
BATCH38_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / "batch_38_p0_enemy_source_reference_pack_2026-06-15" / "thecat_enemy_p0_core_batch38_source_reference_review_sheet.png"
BATCH38_COMBAT = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "p0_core" / "batch_38_p0_enemy_source_reference_pack_2026-06-15" / "cold_light_shadow" / "thecat_enemy_cold_light_shadow_batch38_combat_sprite_reference_512_512x512_candidate_v001.png"
BATCH41_CANDIDATE = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "cold_light_shadow" / "batch_41_cold_light_ai_refinement_candidate_2026-06-15" / "thecat_enemy_cold_light_shadow_ai_refinement_combat_1024_candidate_v001.png"
BATCH41_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "cold_light_shadow" / "batch_41_cold_light_ai_refinement_candidate_2026-06-15" / "cold_light_batch41_ai_refinement_candidate_review.md"
AGENT_PROMPT = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_42_cold_light_cutout_candidate.md"
SOURCE_LOCK_IDS = "cold_light_concept;cold_light_animation"
ACTIVE_SCREENSHOT = "08-active-enemy-cold-light.png"

CUTOUT_NAME = "thecat_enemy_cold_light_shadow_cutout_beam_alpha_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_enemy_cold_light_shadow_cutout_beam_alpha_512_preview_v001.png"
CHECKER_NAME = "thecat_enemy_cold_light_shadow_cutout_beam_checkerboard_512_review_v001.png"
DARK_NAME = "thecat_enemy_cold_light_shadow_cutout_beam_darkfield_512_review_v001.png"
WARM_NAME = "thecat_enemy_cold_light_shadow_cutout_beam_warmfield_512_review_v001.png"
MASK_NAME = "thecat_enemy_cold_light_shadow_cutout_beam_alpha_mask_512_review_v001.png"


def main() -> None:
    concept_path = resolve_one(SOURCE_CONCEPT_PATTERN)
    animation_path = resolve_one(SOURCE_ANIMATION_PATTERN)
    require_file(BATCH38_REVIEW)
    require_file(BATCH38_COMBAT)
    require_file(BATCH41_CANDIDATE)
    require_file(BATCH41_REVIEW)

    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    source_image = Image.open(BATCH41_CANDIDATE).convert("RGBA")
    cutout, mask, stats = build_cutout(source_image)

    cutout_path = CANDIDATE_DIR / CUTOUT_NAME
    preview_path = CANDIDATE_DIR / PREVIEW_NAME
    checker_path = CANDIDATE_DIR / CHECKER_NAME
    dark_path = CANDIDATE_DIR / DARK_NAME
    warm_path = CANDIDATE_DIR / WARM_NAME
    mask_path = CANDIDATE_DIR / MASK_NAME

    cutout.save(cutout_path)
    preview = resize_square(cutout, 512)
    preview.save(preview_path)
    checkerboard_composite(preview).save(checker_path)
    dark_composite(preview).save(dark_path)
    warm_hud_composite(preview).save(warm_path)
    mask.resize((512, 512), Image.Resampling.LANCZOS).save(mask_path)

    write_review_note(concept_path, animation_path, cutout_path, preview_path, checker_path, dark_path, warm_path, mask_path, stats)
    write_process_note(concept_path, animation_path, stats)
    write_review_sheet(concept_path, cutout_path, preview_path, checker_path, dark_path, warm_path, mask_path, stats)
    write_manifest(concept_path, animation_path, cutout_path, preview_path, checker_path, dark_path, warm_path, mask_path)
    write_summary(concept_path, animation_path, cutout_path, preview_path, checker_path, dark_path, warm_path, mask_path, stats)

    print("Wrote Cold Light cutout candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda p: p.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected exactly one match for {pattern}, found {len(matches)}")
    return matches[0]


def require_file(path: Path) -> None:
    if not path.exists():
        raise FileNotFoundError(path)


def build_cutout(image: Image.Image) -> tuple[Image.Image, Image.Image, dict[str, str]]:
    if image.size != (1024, 1024):
        image = resize_square(image, 1024)

    rgb = image.convert("RGB")
    width, height = rgb.size
    pixels = rgb.load()
    background = estimate_background(rgb)
    alpha = Image.new("L", (width, height), 0)
    alpha_pixels = alpha.load()

    opaque_pixels = 0
    semi_pixels = 0
    cyan_semi_pixels = 0
    initial_foreground = 0

    for y in range(height):
        for x in range(width):
            r, g, b = pixels[x, y]
            luma = (0.2126 * r) + (0.7152 * g) + (0.0722 * b)
            distance = color_distance((r, g, b), background)
            cyan_score = min(g, b) - r
            is_red_eye = r >= 115 and r >= g + 30 and r >= b + 20 and max(g, b) <= 150
            is_black_mud = luma <= 105 and distance >= 28
            is_metal_shadow = luma <= 185 and distance >= 52
            is_cyan_lamp = b >= 120 and g >= 95 and b >= r + 16 and g >= r + 6
            is_pale_cyan_beam = b >= 145 and g >= 135 and r <= 220 and b >= r + 10 and g >= r + 4

            value = 0
            if is_red_eye or is_black_mud:
                value = 255
            elif is_metal_shadow:
                value = 235
            elif is_cyan_lamp:
                value = clamp_int(128 + (cyan_score * 2) + (distance * 0.35), 110, 225)
            elif is_pale_cyan_beam:
                value = clamp_int(52 + (cyan_score * 1.75) + (distance * 0.2), 42, 150)

            if value > 0:
                initial_foreground += 1
                alpha_pixels[x, y] = value
                if value >= 200:
                    opaque_pixels += 1
                else:
                    semi_pixels += 1
                    if b >= r + 10 and g >= r + 4:
                        cyan_semi_pixels += 1

    alpha = alpha.filter(ImageFilter.MaxFilter(3)).filter(ImageFilter.GaussianBlur(0.55))
    cutout = image.copy()
    cutout.putalpha(alpha)
    mask = alpha.convert("RGBA")

    total = width * height
    alpha_values = alpha.tobytes()
    transparent_pixels = sum(1 for value in alpha_values if value <= 8)
    semi_after = sum(1 for value in alpha_values if 8 < value < 200)
    opaque_after = sum(1 for value in alpha_values if value >= 200)
    stats = {
        "background_rgb": f"{background[0]},{background[1]},{background[2]}",
        "initial_foreground_pixels": str(initial_foreground),
        "opaque_pixels_before_matte": str(opaque_pixels),
        "semi_pixels_before_matte": str(semi_pixels),
        "cyan_semi_pixels_before_matte": str(cyan_semi_pixels),
        "transparent_pixels_after_matte": str(transparent_pixels),
        "semi_pixels_after_matte": str(semi_after),
        "opaque_pixels_after_matte": str(opaque_after),
        "transparent_coverage_percent": f"{transparent_pixels / total * 100.0:.2f}",
        "semi_coverage_percent": f"{semi_after / total * 100.0:.2f}",
        "opaque_coverage_percent": f"{opaque_after / total * 100.0:.2f}",
    }
    return cutout, mask, stats


def clamp_int(value: float, minimum: int, maximum: int) -> int:
    return max(minimum, min(maximum, int(round(value))))


def estimate_background(image: Image.Image) -> tuple[int, int, int]:
    width, height = image.size
    sample = []
    border = 16
    pixels = image.load()
    for x in range(width):
        for y in range(border):
            sample.append(pixels[x, y])
            sample.append(pixels[x, height - 1 - y])
    for y in range(height):
        for x in range(border):
            sample.append(pixels[x, y])
            sample.append(pixels[width - 1 - x, y])
    r_values = sorted(p[0] for p in sample)
    g_values = sorted(p[1] for p in sample)
    b_values = sorted(p[2] for p in sample)
    mid = len(sample) // 2
    return r_values[mid], g_values[mid], b_values[mid]


def color_distance(a: tuple[int, int, int], b: tuple[int, int, int]) -> float:
    return ((a[0] - b[0]) ** 2 + (a[1] - b[1]) ** 2 + (a[2] - b[2]) ** 2) ** 0.5


def resize_square(image: Image.Image, size: int) -> Image.Image:
    if image.size == (size, size):
        return image.copy()
    work = image.copy()
    work.thumbnail((size, size), Image.Resampling.LANCZOS)
    canvas = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    canvas.alpha_composite(work, ((size - work.width) // 2, (size - work.height) // 2))
    return canvas


def checkerboard_composite(image: Image.Image, cell: int = 24) -> Image.Image:
    board = Image.new("RGBA", image.size, (220, 220, 220, 255))
    draw = ImageDraw.Draw(board)
    for y in range(0, image.height, cell):
        for x in range(0, image.width, cell):
            color = (252, 252, 252, 255) if ((x // cell) + (y // cell)) % 2 == 0 else (180, 187, 196, 255)
            draw.rectangle((x, y, x + cell - 1, y + cell - 1), fill=color)
    board.alpha_composite(image)
    return board


def dark_composite(image: Image.Image) -> Image.Image:
    base = Image.new("RGBA", image.size, (31, 35, 44, 255))
    base.alpha_composite(image)
    return base


def warm_hud_composite(image: Image.Image) -> Image.Image:
    base = Image.new("RGBA", image.size, (244, 220, 174, 255))
    draw = ImageDraw.Draw(base)
    for y in range(0, image.height, 64):
        draw.rectangle((0, y, image.width, y + 31), fill=(235, 205, 158, 255))
    base.alpha_composite(image)
    return base


def write_review_sheet(
    concept_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    dark_path: Path,
    warm_path: Path,
    mask_path: Path,
    stats: dict[str, str],
) -> None:
    concept = Image.open(concept_path).convert("RGBA")
    input_candidate = Image.open(BATCH41_CANDIDATE).convert("RGBA")
    checker = Image.open(checker_path).convert("RGBA")
    dark = Image.open(dark_path).convert("RGBA")
    warm = Image.open(warm_path).convert("RGBA")
    mask = Image.open(mask_path).convert("RGBA")
    preview = Image.open(preview_path).convert("RGBA")

    sheet = Image.new("RGBA", (1600, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(28)
    label_font = load_font(16)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 42 - Cold Light beam-preserving cutout candidate", fill=(42, 36, 32), font=title_font)
    draw.text((36, 68), "Candidate review only. Stored outside Assets. Formal import remains blocked until active-enemy screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_panel = fit_to_canvas(concept, (500, 300), (250, 246, 237, 255))
    input_panel = fit_to_canvas(input_candidate, (300, 300), (250, 246, 237, 255))
    checker_panel = fit_to_canvas(checker, (300, 300), (250, 246, 237, 255))
    dark_panel = fit_to_canvas(dark, (300, 300), (250, 246, 237, 255))
    warm_panel = fit_to_canvas(warm, (300, 300), (250, 246, 237, 255))
    mask_panel = fit_to_canvas(mask, (220, 220), (250, 246, 237, 255))
    preview_panel = fit_to_canvas(preview, (220, 220), (250, 246, 237, 255))

    draw_panel(sheet, draw, source_panel, (36, 116), "Source authority: Cold Light concept", label_font)
    draw_panel(sheet, draw, input_panel, (560, 116), "Batch 41 input candidate", label_font)
    draw_panel(sheet, draw, checker_panel, (880, 116), "Cutout on checkerboard", label_font)
    draw_panel(sheet, draw, dark_panel, (1200, 116), "Cutout on dark UI field", label_font)
    draw_panel(sheet, draw, warm_panel, (560, 486), "Cutout on warm HUD field", label_font)
    draw_panel(sheet, draw, mask_panel, (890, 486), "Alpha mask review", label_font)
    draw_panel(sheet, draw, preview_panel, (1140, 486), "512 alpha preview", label_font)

    checks = (
        "Keep: cold lamp silhouette, cyan beam/light language, mechanical spring arm, black mud base, single red eye.",
        "Method: dark metal and mud are opaque; pale cyan lamp and beam are preserved as semi-transparent alpha.",
        "Watch: beam may need a separate Unity VFX later; this candidate is for sprite import review only.",
        f"Coverage: opaque {stats['opaque_coverage_percent']}%, semi {stats['semi_coverage_percent']}%, transparent {stats['transparent_coverage_percent']}%.",
    )
    y = 470
    for check in checks:
        y = draw_wrapped_text(draw, "- " + check, (36, y), body_font, 70, fill=(42, 36, 32)) + 8

    sheet.save(REVIEW_SHEET_PATH)


def draw_panel(sheet: Image.Image, draw: ImageDraw.ImageDraw, image: Image.Image, pos: tuple[int, int], label: str, font: ImageFont.ImageFont) -> None:
    x, y = pos
    draw.rounded_rectangle((x - 10, y - 10, x + image.width + 10, y + image.height + 42), radius=8, fill=(255, 252, 246), outline=(178, 158, 130))
    sheet.alpha_composite(image, pos)
    draw.text((x, y + image.height + 14), label, fill=(42, 36, 32), font=font)


def draw_wrapped_text(draw: ImageDraw.ImageDraw, text: str, pos: tuple[int, int], font: ImageFont.ImageFont, width: int, fill: tuple[int, int, int]) -> int:
    x, y = pos
    line_height = 20
    for line in textwrap.wrap(text, width=width):
        draw.text((x, y), line, fill=fill, font=font)
        y += line_height
    return y


def fit_to_canvas(image: Image.Image, size: tuple[int, int], background: tuple[int, int, int, int]) -> Image.Image:
    canvas = Image.new("RGBA", size, background)
    work = image.copy()
    work.thumbnail((size[0] - 20, size[1] - 20), Image.Resampling.LANCZOS)
    canvas.alpha_composite(work, ((size[0] - work.width) // 2, (size[1] - work.height) // 2))
    return canvas


def write_review_note(
    concept_path: Path,
    animation_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    dark_path: Path,
    warm_path: Path,
    mask_path: Path,
    stats: dict[str, str],
) -> None:
    lines = [
        "# Cold Light Batch 42 Cutout Candidate Review",
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
        f"- Batch 38 review sheet: `{to_repo_path(BATCH38_REVIEW)}`",
        f"- Batch 38 combat crop reference: `{to_repo_path(BATCH38_COMBAT)}`",
        f"- Batch 41 input candidate review: `{to_repo_path(BATCH41_REVIEW)}`",
        f"- Source lock ids: `{SOURCE_LOCK_IDS}`",
        f"- Active screenshot required before import: `{ACTIVE_SCREENSHOT}`",
        "",
        "## Cutout Outputs",
        "",
        f"- Beam-preserving alpha 1024 candidate: `{to_repo_path(cutout_path)}`",
        f"- Alpha 512 preview: `{to_repo_path(preview_path)}`",
        f"- Checkerboard review composite: `{to_repo_path(checker_path)}`",
        f"- Dark-field review composite: `{to_repo_path(dark_path)}`",
        f"- Warm-HUD review composite: `{to_repo_path(warm_path)}`",
        f"- Alpha mask review: `{to_repo_path(mask_path)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        "",
        "## Cutout Metrics",
        "",
        f"- Background RGB sampled from border: `{stats['background_rgb']}`",
        f"- Initial foreground pixels: `{stats['initial_foreground_pixels']}`",
        f"- Opaque pixels before matte: `{stats['opaque_pixels_before_matte']}`",
        f"- Semi-transparent pixels before matte: `{stats['semi_pixels_before_matte']}`",
        f"- Cyan semi-transparent pixels before matte: `{stats['cyan_semi_pixels_before_matte']}`",
        f"- Transparent pixels after matte: `{stats['transparent_pixels_after_matte']}`",
        f"- Semi-transparent pixels after matte: `{stats['semi_pixels_after_matte']}`",
        f"- Opaque pixels after matte: `{stats['opaque_pixels_after_matte']}`",
        f"- Transparent coverage: `{stats['transparent_coverage_percent']}%`",
        f"- Semi-transparent coverage: `{stats['semi_coverage_percent']}%`",
        f"- Opaque coverage: `{stats['opaque_coverage_percent']}%`",
        "",
        "## Visual Review",
        "",
        "- Pass: output has an alpha channel and transparent corners for Unity sprite review.",
        "- Pass: cold lamp silhouette, cyan beam/light language, mechanical spring arm, black mud base, single red eye, long shadow-limb feel, and ranged-pressure read remain visible.",
        "- Pass: pale cyan beam information is preserved as semi-transparent alpha instead of being fully clipped.",
        "- Watch: beam may be better split into a separate Unity warning VFX later; this batch intentionally keeps it for candidate identity review.",
        "- Watch: final runtime scale, hitbox readability, beam-warning readability, and prefab binding still need active-enemy screenshot review.",
        "",
        "## Rejection Rules",
        "",
        "- Reject if future cutout iterations clip the cold lamp silhouette, cyan beam/light language, mechanical spring arm, black mud base, single red eye, long shadow-limb feel, or ranged-pressure read.",
        "- Reject ordinary clean desk lamp, warm candle or fire palette, cute robot styling, humanoid body, black mud removal, missing red eye, missing cyan light, missing spring arm, missing mud base, or removal of all beam readability.",
        "- Reject if alpha edges show obvious parchment halos in Unity active-enemy screenshot review.",
        "- Reject if the candidate is imported into Unity before active-enemy screenshot review.",
        "",
    ]
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(concept_path: Path, animation_path: Path, stats: dict[str, str]) -> None:
    lines = [
        "# Cold Light Batch 42 Cutout Process Note",
        "",
        "This batch is a deterministic local post-processing pass over the Batch 41 Cold Light Shadow candidate. No new image generation was invoked.",
        "",
        f"- Source concept authority: `{to_repo_path(concept_path)}`",
        f"- Source animation authority: `{to_repo_path(animation_path)}`",
        f"- Input candidate: `{to_repo_path(BATCH41_CANDIDATE)}`",
        f"- Output directory: `{to_repo_path(CANDIDATE_DIR)}`",
        "- Method: sample the 16-pixel image border, keep dark metal/mud and red-eye pixels opaque, preserve pale cyan lamp and beam pixels as semi-transparent alpha, then apply a 0.55px soft matte.",
        f"- Background RGB: `{stats['background_rgb']}`",
        f"- Opaque coverage: `{stats['opaque_coverage_percent']}%`",
        f"- Semi-transparent coverage: `{stats['semi_coverage_percent']}%`",
        f"- Transparent coverage: `{stats['transparent_coverage_percent']}%`",
        "- Safety: files remain outside `Assets`; formal import remains blocked until active-enemy Play Mode screenshot review passes.",
        "",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(
    concept_path: Path,
    animation_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    dark_path: Path,
    warm_path: Path,
    mask_path: Path,
) -> None:
    rows = [
        build_row("cutout_beam_alpha_1024", cutout_path, concept_path, animation_path, "1024x1024"),
        build_row("cutout_beam_alpha_512_preview", preview_path, concept_path, animation_path, "512x512"),
        build_row("cutout_beam_checkerboard_512_review", checker_path, concept_path, animation_path, "512x512"),
        build_row("cutout_beam_darkfield_512_review", dark_path, concept_path, animation_path, "512x512"),
        build_row("cutout_beam_warmfield_512_review", warm_path, concept_path, animation_path, "512x512"),
        build_row("cutout_beam_alpha_mask_512_review", mask_path, concept_path, animation_path, "512x512"),
    ]
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def build_row(asset_type: str, candidate_path: Path, concept_path: Path, animation_path: Path, candidate_size: str) -> dict[str, str]:
    return {
        "enemy_id": "cold_light_shadow",
        "display_name": "Cold Light Shadow",
        "combat_role": "P0 ranged harasser and bed beam pressure",
        "batch_slug": BATCH_SLUG,
        "asset_type": asset_type,
        "candidate_path": to_repo_path(candidate_path),
        "candidate_sha256": sha256(candidate_path),
        "candidate_size": candidate_size,
        "concept_source_path": to_repo_path(concept_path),
        "concept_source_sha256": sha256(concept_path),
        "animation_source_path": to_repo_path(animation_path),
        "animation_source_sha256": sha256(animation_path),
        "source_lock_ids": SOURCE_LOCK_IDS,
        "input_candidate_path": to_repo_path(BATCH41_CANDIDATE),
        "input_candidate_sha256": sha256(BATCH41_CANDIDATE),
        "batch38_review_sheet": to_repo_path(BATCH38_REVIEW),
        "batch38_combat_reference": to_repo_path(BATCH38_COMBAT),
        "agent_prompt": to_repo_path(AGENT_PROMPT),
        "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
        "review_note": to_repo_path(REVIEW_NOTE_PATH),
        "process_note": to_repo_path(PROCESS_NOTE_PATH),
        "active_screenshot": ACTIVE_SCREENSHOT,
        "recommendation": "candidate_review_only_pending_playmode_screenshot",
    }


def write_summary(
    concept_path: Path,
    animation_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    dark_path: Path,
    warm_path: Path,
    mask_path: Path,
    stats: dict[str, str],
) -> None:
    summary = BATCH_DIR / "cold_light_batch42_cutout_summary.md"
    lines = [
        "# Cold Light Batch 42 Cutout Candidate",
        "",
        "This batch converts the Batch 41 Cold Light Shadow candidate into a beam-preserving alpha PNG candidate for Unity sprite review. It remains outside Unity and is not approved for import.",
        "",
        f"- Source concept: `{to_repo_path(concept_path)}`",
        f"- Source animation: `{to_repo_path(animation_path)}`",
        f"- Cutout candidate: `{to_repo_path(cutout_path)}`",
        f"- Preview: `{to_repo_path(preview_path)}`",
        f"- Checkerboard review: `{to_repo_path(checker_path)}`",
        f"- Dark-field review: `{to_repo_path(dark_path)}`",
        f"- Warm-HUD review: `{to_repo_path(warm_path)}`",
        f"- Alpha mask review: `{to_repo_path(mask_path)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Review note: `{to_repo_path(REVIEW_NOTE_PATH)}`",
        f"- Opaque coverage: `{stats['opaque_coverage_percent']}%`",
        f"- Semi-transparent coverage: `{stats['semi_coverage_percent']}%`",
        "- Unity import: blocked pending active-enemy Play Mode screenshot review.",
        "",
    ]
    summary.write_text("\n".join(lines), encoding="utf-8")


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
