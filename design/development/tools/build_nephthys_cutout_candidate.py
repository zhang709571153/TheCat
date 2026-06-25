from __future__ import annotations

import csv
import hashlib
from collections import deque
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_37_nephthys_cutout_candidate_2026-06-15"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / BATCH_SLUG
BATCH_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / BATCH_SLUG
MANIFEST_PATH = BATCH_DIR / "nephthys_batch37_cutout_manifest.csv"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "nephthys_batch37_cutout_candidate_review.md"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_cat_nephthys_batch37_cutout_review_sheet.png"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "nephthys_batch37_cutout_process_note.md"
SOURCE_PATTERN = "design/*/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
BATCH32_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / "batch_32_nephthys_strict_turnaround_derivatives_2026-06-14" / "thecat_cat_nephthys_batch32_strict_turnaround_review_sheet.png"
BATCH36_CANDIDATE = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / "batch_36_nephthys_ai_refinement_candidate_2026-06-15" / "thecat_cat_nephthys_ai_refinement_combat_1024_candidate_v001.png"
BATCH36_REVIEW = REPO_ROOT / "design" / "development" / "asset_candidates" / "starter_cats" / "nephthys" / "batch_36_nephthys_ai_refinement_candidate_2026-06-15" / "nephthys_batch36_ai_refinement_candidate_review.md"
AGENT_PROMPT = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_37_nephthys_cutout_candidate.md"
SOURCE_LOCK_ID = "nephthys_turnaround_colored"
ACTIVE_SCREENSHOT = "05-active-cat-nephthys.png"

CUTOUT_NAME = "thecat_cat_nephthys_cutout_alpha_1024_candidate_v001.png"
PREVIEW_NAME = "thecat_cat_nephthys_cutout_alpha_512_preview_v001.png"
CHECKER_NAME = "thecat_cat_nephthys_cutout_checkerboard_512_review_v001.png"
MASK_NAME = "thecat_cat_nephthys_cutout_alpha_mask_512_review_v001.png"


def main() -> None:
    source_path = resolve_one(SOURCE_PATTERN)
    if not BATCH36_CANDIDATE.exists():
        raise FileNotFoundError(BATCH36_CANDIDATE)

    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    source_image = Image.open(BATCH36_CANDIDATE).convert("RGBA")
    cutout, mask, stats = build_cutout(source_image)

    cutout_path = CANDIDATE_DIR / CUTOUT_NAME
    preview_path = CANDIDATE_DIR / PREVIEW_NAME
    checker_path = CANDIDATE_DIR / CHECKER_NAME
    mask_path = CANDIDATE_DIR / MASK_NAME

    cutout.save(cutout_path)
    resize_square(cutout, 512).save(preview_path)
    checkerboard_composite(resize_square(cutout, 512)).save(checker_path)
    mask.resize((512, 512), Image.Resampling.LANCZOS).save(mask_path)

    write_review_note(source_path, cutout_path, preview_path, checker_path, mask_path, stats)
    write_process_note(source_path, stats)
    write_review_sheet(source_path, cutout_path, preview_path, checker_path, mask_path, stats)
    write_manifest(source_path, cutout_path, preview_path, checker_path, mask_path)
    write_summary(source_path, cutout_path, preview_path, checker_path, mask_path, stats)

    print("Wrote Nephthys cutout candidate pack.")
    print(to_repo_path(MANIFEST_PATH))


def resolve_one(pattern: str) -> Path:
    matches = sorted(REPO_ROOT.glob(pattern), key=lambda p: p.as_posix())
    if len(matches) != 1:
        raise FileNotFoundError(f"Expected exactly one match for {pattern}, found {len(matches)}")
    return matches[0]


def build_cutout(image: Image.Image) -> tuple[Image.Image, Image.Image, dict[str, str]]:
    if image.size != (1024, 1024):
        image = resize_square(image, 1024)

    rgb = image.convert("RGB")
    width, height = rgb.size
    pixels = rgb.load()
    background = estimate_background(rgb)
    threshold = 48
    visited = bytearray(width * height)
    queue: deque[tuple[int, int]] = deque()

    def maybe_add(x: int, y: int) -> None:
        index = y * width + x
        if visited[index]:
            return
        r, g, b = pixels[x, y]
        if color_distance((r, g, b), background) <= threshold:
            visited[index] = 1
            queue.append((x, y))

    for x in range(width):
        maybe_add(x, 0)
        maybe_add(x, height - 1)
    for y in range(height):
        maybe_add(0, y)
        maybe_add(width - 1, y)

    while queue:
        x, y = queue.popleft()
        for nx, ny in ((x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)):
            if 0 <= nx < width and 0 <= ny < height:
                maybe_add(nx, ny)

    alpha = Image.new("L", (width, height), 255)
    alpha_pixels = alpha.load()
    transparent_count = 0
    for y in range(height):
        row = y * width
        for x in range(width):
            if visited[row + x]:
                alpha_pixels[x, y] = 0
                transparent_count += 1

    alpha = alpha.filter(ImageFilter.GaussianBlur(0.65))
    cutout = image.copy()
    cutout.putalpha(alpha)
    mask = alpha.convert("RGBA")
    opaque_count = width * height - transparent_count
    coverage = opaque_count / float(width * height)
    stats = {
        "background_rgb": f"{background[0]},{background[1]},{background[2]}",
        "flood_threshold": str(threshold),
        "transparent_pixels": str(transparent_count),
        "opaque_pixels": str(opaque_count),
        "opaque_coverage_percent": f"{coverage * 100.0:.2f}",
    }
    return cutout, mask, stats


def estimate_background(image: Image.Image) -> tuple[int, int, int]:
    width, height = image.size
    sample = []
    border = 12
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
    base = Image.new("RGBA", image.size, (33, 33, 42, 255))
    base.alpha_composite(image)
    return base


def write_review_sheet(
    source_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    mask_path: Path,
    stats: dict[str, str],
) -> None:
    source = Image.open(source_path).convert("RGBA")
    batch36 = Image.open(BATCH36_CANDIDATE).convert("RGBA")
    cutout = Image.open(cutout_path).convert("RGBA")
    checker = Image.open(checker_path).convert("RGBA")
    mask = Image.open(mask_path).convert("RGBA")
    preview = Image.open(preview_path).convert("RGBA")

    sheet = Image.new("RGBA", (1600, 900), (248, 244, 236, 255))
    draw = ImageDraw.Draw(sheet)
    title_font = load_font(28)
    label_font = load_font(16)
    body_font = load_font(14)

    draw.text((36, 28), "P0 Batch 37 - Nephthys transparent cutout candidate", fill=(42, 36, 32), font=title_font)
    draw.text((36, 68), "Candidate review only. Stored outside Assets. Formal import remains blocked until active-cat screenshots pass.", fill=(92, 48, 42), font=body_font)

    source_panel = fit_to_canvas(source, (500, 300), (250, 246, 237, 255))
    batch36_panel = fit_to_canvas(batch36, (300, 300), (250, 246, 237, 255))
    checker_panel = fit_to_canvas(checker, (300, 300), (250, 246, 237, 255))
    dark_panel = fit_to_canvas(dark_composite(resize_square(cutout, 512)), (300, 300), (250, 246, 237, 255))
    mask_panel = fit_to_canvas(mask, (220, 220), (250, 246, 237, 255))
    preview_panel = fit_to_canvas(preview, (220, 220), (250, 246, 237, 255))

    draw_panel(sheet, draw, source_panel, (36, 116), "Source authority: colored turnaround", label_font)
    draw_panel(sheet, draw, batch36_panel, (560, 116), "Batch 36 source candidate", label_font)
    draw_panel(sheet, draw, checker_panel, (880, 116), "Cutout on checkerboard", label_font)
    draw_panel(sheet, draw, dark_panel, (1200, 116), "Cutout on dark UI field", label_font)
    draw_panel(sheet, draw, mask_panel, (560, 486), "Alpha mask review", label_font)
    draw_panel(sheet, draw, preview_panel, (810, 486), "512 alpha preview", label_font)

    checks = (
        "Candidate traits to keep: non-human tabby cat, gold eyes, navy hood, crescent, blue gems, ankh, pyramid prop, striped tail.",
        "Cutout method: border flood-fill against parchment background, soft alpha edge; no source identity repaint.",
        "Watch item: ears, pyramid particles, cloak tips, and tail edges need Unity active-cat screenshot QA before any formal install.",
        f"Opaque coverage: {stats['opaque_coverage_percent']}%; background RGB: {stats['background_rgb']}; threshold: {stats['flood_threshold']}.",
    )
    y = 770
    for check in checks:
        draw.text((36, y), "- " + check, fill=(42, 36, 32), font=body_font)
        y += 30

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
    canvas.alpha_composite(work, ((size[0] - work.width) // 2, (size[1] - work.height) // 2))
    return canvas


def write_review_note(
    source_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    mask_path: Path,
    stats: dict[str, str],
) -> None:
    lines = [
        "# Nephthys Batch 37 Cutout Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity yet.",
        "",
        "formal import remains blocked until active-cat Play Mode screenshot review passes.",
        "",
        "## Source Authority",
        "",
        f"- Colored three-view turnaround: `{to_repo_path(source_path)}`",
        f"- Batch 32 review sheet: `{to_repo_path(BATCH32_REVIEW)}`",
        f"- Batch 36 candidate review: `{to_repo_path(BATCH36_REVIEW)}`",
        f"- Source lock id: `{SOURCE_LOCK_ID}`",
        f"- Active screenshot required before import: `{ACTIVE_SCREENSHOT}`",
        "",
        "## Cutout Outputs",
        "",
        f"- Alpha 1024 candidate: `{to_repo_path(cutout_path)}`",
        f"- Alpha 512 preview: `{to_repo_path(preview_path)}`",
        f"- Checkerboard review composite: `{to_repo_path(checker_path)}`",
        f"- Alpha mask review: `{to_repo_path(mask_path)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        "",
        "## Cutout Metrics",
        "",
        f"- Background RGB sampled from border: `{stats['background_rgb']}`",
        f"- Flood threshold: `{stats['flood_threshold']}`",
        f"- Transparent pixels: `{stats['transparent_pixels']}`",
        f"- Opaque pixels: `{stats['opaque_pixels']}`",
        f"- Opaque coverage: `{stats['opaque_coverage_percent']}%`",
        "",
        "## Visual Review",
        "",
        "- Pass: output has an alpha channel and transparent corners for Unity sprite review.",
        "- Pass: core Nephthys identity remains inherited from Batch 36: non-human cat body, gold-brown tabby markings, golden eyes, deep navy hood and cloak, crescent ornament, blue tear gem, sand-gold script trim, blue gemstone chest ornament, winged gold collar, ankh emblem, floating pyramid/obelisk prop, and striped tail.",
        "- Watch: local flood-fill transparency can leave edge residue or over-soften pale fur, cloak tips, pyramid particles, and tail edges; verify in Unity against dark and warm HUD fields.",
        "- Watch: this is still a front-view candidate only; side and back anchors remain governed by the colored turnaround and Batch 32 source-derived references.",
        "",
        "## Rejection Rules",
        "",
        "- Reject if future cutout iterations clip ears, hood, crescent ornament, blue tear gem, face markings, floating pyramid prop, cyan particles, ankh, cloak panels, paws, or striped tail.",
        "- Reject if future iterations introduce human body proportions, long legs, human hands, Cleopatra costume cliche, generic Egyptian fantasy drift, or generic controller costume.",
        "- Reject if alpha edges show obvious parchment halos in Unity active-cat screenshot review.",
        "- Reject if the candidate is imported into Unity before active-cat screenshot review.",
        "",
    ]
    REVIEW_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_process_note(source_path: Path, stats: dict[str, str]) -> None:
    lines = [
        "# Nephthys Batch 37 Cutout Process Note",
        "",
        "This batch is a deterministic local post-processing pass over the Batch 36 Nephthys candidate. No new image generation was invoked.",
        "",
        f"- Source turnaround authority: `{to_repo_path(source_path)}`",
        f"- Input candidate: `{to_repo_path(BATCH36_CANDIDATE)}`",
        f"- Output directory: `{to_repo_path(CANDIDATE_DIR)}`",
        "- Method: sample the 12-pixel image border, flood-fill connected parchment background pixels, convert that region to alpha, and apply a 0.65px soft matte.",
        f"- Background RGB: `{stats['background_rgb']}`",
        f"- Flood threshold: `{stats['flood_threshold']}`",
        f"- Opaque coverage: `{stats['opaque_coverage_percent']}%`",
        "- Safety: files remain outside `Assets`; formal import remains blocked until active-cat Play Mode screenshot review passes.",
        "",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines), encoding="utf-8")


def write_manifest(source_path: Path, cutout_path: Path, preview_path: Path, checker_path: Path, mask_path: Path) -> None:
    rows = [
        build_row("cutout_alpha_1024", cutout_path, source_path, "1024x1024"),
        build_row("cutout_alpha_512_preview", preview_path, source_path, "512x512"),
        build_row("cutout_checkerboard_512_review", checker_path, source_path, "512x512"),
        build_row("cutout_alpha_mask_512_review", mask_path, source_path, "512x512"),
    ]
    MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def build_row(asset_type: str, candidate_path: Path, source_path: Path, candidate_size: str) -> dict[str, str]:
    return {
        "cat_id": "nephthys",
        "display_name": "Nephthys / Moon-Sand Agent",
        "batch_slug": BATCH_SLUG,
        "asset_type": asset_type,
        "candidate_path": to_repo_path(candidate_path),
        "candidate_sha256": sha256(candidate_path),
        "candidate_size": candidate_size,
        "source_turnaround_path": to_repo_path(source_path),
        "source_turnaround_sha256": sha256(source_path),
        "source_lock_id": SOURCE_LOCK_ID,
        "input_candidate_path": to_repo_path(BATCH36_CANDIDATE),
        "input_candidate_sha256": sha256(BATCH36_CANDIDATE),
        "agent_prompt": to_repo_path(AGENT_PROMPT),
        "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
        "review_note": to_repo_path(REVIEW_NOTE_PATH),
        "process_note": to_repo_path(PROCESS_NOTE_PATH),
        "active_screenshot": ACTIVE_SCREENSHOT,
        "recommendation": "candidate_review_only_pending_playmode_screenshot",
    }


def write_summary(
    source_path: Path,
    cutout_path: Path,
    preview_path: Path,
    checker_path: Path,
    mask_path: Path,
    stats: dict[str, str],
) -> None:
    summary = BATCH_DIR / "nephthys_batch37_cutout_summary.md"
    lines = [
        "# Nephthys Batch 37 Cutout Candidate",
        "",
        "This batch converts the Batch 36 Nephthys candidate into an alpha PNG candidate for Unity sprite review. It remains outside Unity and is not approved for import.",
        "",
        f"- Source authority: `{to_repo_path(source_path)}`",
        f"- Cutout candidate: `{to_repo_path(cutout_path)}`",
        f"- Preview: `{to_repo_path(preview_path)}`",
        f"- Checkerboard review: `{to_repo_path(checker_path)}`",
        f"- Alpha mask review: `{to_repo_path(mask_path)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Review note: `{to_repo_path(REVIEW_NOTE_PATH)}`",
        f"- Opaque coverage: `{stats['opaque_coverage_percent']}%`",
        "- Unity import: blocked pending active-cat Play Mode screenshot review.",
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
