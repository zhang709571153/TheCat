from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_79_system_icon_candidates_2026-06-24"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "system_icons" / BATCH_SLUG
ICON_DIRS = {
    128: CANDIDATE_DIR / "icons_128",
    64: CANDIDATE_DIR / "icons_64",
    32: CANDIDATE_DIR / "icons_32",
}

SOURCE_PATH = CANDIDATE_DIR / "thecat_ui_system_icons_batch79_chromakey_source_v001.png"
ALPHA_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_system_icons_batch79_alpha_sheet_v001.png"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_system_icons_batch79_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_system_icons_batch79_review_sheet_v001.png"
MANIFEST_PATH = CANDIDATE_DIR / "system_icons_batch79_manifest.csv"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "system_icons_batch79_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "system_icons_batch79_process_note.md"
PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_79_system_icon_candidates.md"

ICON_SPECS = (
    ("settings", "Settings gear", "cog with moon-blue glass center"),
    ("sound", "Sound on", "speaker with two sound arcs"),
    ("mute", "Sound muted", "speaker with diagonal slash"),
    ("back", "Back", "left arrow with rounded dreamglass tail"),
    ("close", "Close", "X cross in rounded frame"),
    ("pause", "Pause", "two vertical bars in circular button"),
    ("continue", "Continue / play", "right-facing triangle in circular button"),
    ("retry", "Retry", "circular arrow loop"),
    ("lock", "Locked", "closed padlock with keyhole glow"),
    ("warning", "Warning", "triangular warning sigil without exclamation text"),
)

FIELD_NAMES = (
    "asset_id",
    "icon_id",
    "display_name",
    "size_variant",
    "batch_slug",
    "asset_type",
    "candidate_path",
    "candidate_sha256",
    "candidate_size",
    "source_prompt_path",
    "source_image_path",
    "source_image_sha256",
    "alpha_sheet_path",
    "alpha_sheet_sha256",
    "contact_sheet",
    "review_sheet",
    "review_note",
    "process_note",
    "recommendation",
    "visual_review",
)


def main() -> None:
    CANDIDATE_DIR.mkdir(parents=True, exist_ok=True)
    for directory in ICON_DIRS.values():
        directory.mkdir(parents=True, exist_ok=True)

    alpha_sheet = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    icon_sets = split_and_normalize_icons(alpha_sheet)
    rows = build_manifest_rows(icon_sets)
    write_manifest(rows)
    write_contact_sheet(icon_sets)
    write_review_sheet(icon_sets)
    write_review_note(rows)
    write_process_note(rows, alpha_sheet)

    print(f"Wrote {len(rows)} Batch 79 manifest row(s).")
    print(to_repo_path(MANIFEST_PATH))


def split_and_normalize_icons(alpha_sheet: Image.Image) -> list[dict[str, object]]:
    icon_sets: list[dict[str, object]] = []
    cell_width = alpha_sheet.width / 5
    cell_height = alpha_sheet.height / 2

    for index, (icon_id, display_name, review) in enumerate(ICON_SPECS):
        row = index // 5
        column = index % 5
        left = round(column * cell_width)
        top = round(row * cell_height)
        right = round((column + 1) * cell_width)
        bottom = round((row + 1) * cell_height)
        raw_cell = alpha_sheet.crop((left, top, right, bottom))
        base_icon = normalize_to_square(raw_cell, 128, 10)
        variants: dict[int, Path] = {}

        for size in (128, 64, 32):
            icon = base_icon if size == 128 else base_icon.resize((size, size), Image.Resampling.LANCZOS)
            asset_id = f"thecat_ui_system_icon_{icon_id}_{size}_candidate_v001"
            path = ICON_DIRS[size] / f"{asset_id}.png"
            icon.save(path)
            variants[size] = path

        icon_sets.append(
            {
                "icon_id": icon_id,
                "display_name": display_name,
                "review": review,
                "cell": f"{left},{top},{right},{bottom}",
                "variants": variants,
            }
        )

    return icon_sets


def normalize_to_square(image: Image.Image, size: int, margin: int) -> Image.Image:
    alpha_mask = image.getchannel("A").point(lambda value: 255 if value > 16 else 0)
    bbox = alpha_mask.getbbox()
    if bbox is None:
        raise ValueError("Source icon cell has no visible alpha content")

    cropped = image.crop(bbox)
    cropped.thumbnail((size - margin * 2, size - margin * 2), Image.Resampling.LANCZOS)
    result = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    result.alpha_composite(cropped, ((size - cropped.width) // 2, (size - cropped.height) // 2))
    return result


def build_manifest_rows(icon_sets: list[dict[str, object]]) -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    source_hash = sha256(SOURCE_PATH)
    alpha_hash = sha256(ALPHA_SHEET_PATH)

    for icon_set in icon_sets:
        variants = icon_set["variants"]
        for size in (128, 64, 32):
            path = Path(variants[size])
            rows.append(
                {
                    "asset_id": path.stem,
                    "icon_id": str(icon_set["icon_id"]),
                    "display_name": str(icon_set["display_name"]),
                    "size_variant": str(size),
                    "batch_slug": BATCH_SLUG,
                    "asset_type": "system_icon_candidate",
                    "candidate_path": to_repo_path(path),
                    "candidate_sha256": sha256(path),
                    "candidate_size": image_size(path),
                    "source_prompt_path": to_repo_path(PROMPT_PATH),
                    "source_image_path": to_repo_path(SOURCE_PATH),
                    "source_image_sha256": source_hash,
                    "alpha_sheet_path": to_repo_path(ALPHA_SHEET_PATH),
                    "alpha_sheet_sha256": alpha_hash,
                    "contact_sheet": to_repo_path(CONTACT_SHEET_PATH),
                    "review_sheet": to_repo_path(REVIEW_SHEET_PATH),
                    "review_note": to_repo_path(REVIEW_NOTE_PATH),
                    "process_note": to_repo_path(PROCESS_NOTE_PATH),
                    "recommendation": "candidate_review_only_do_not_import",
                    "visual_review": str(icon_set["review"]),
                }
            )

    return rows


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_contact_sheet(icon_sets: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1700, 1030), (32, 30, 43, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(18)
    small_font = load_font(14)

    draw.text((36, 28), "P0 Batch 79 - System Icon Candidates", fill=(245, 238, 220), font=title_font)
    draw.text((36, 78), "Candidate-only UI icons. 10 icons x 3 sizes. Do not import until UI screenshot review.", fill=(238, 193, 106), font=small_font)

    for index, icon_set in enumerate(icon_sets):
        row = index // 5
        column = index % 5
        x = 72 + column * 318
        y = 132 + row * 332
        variants = icon_set["variants"]

        icon_128 = Image.open(variants[128]).convert("RGBA")
        panel = checkerboard((160, 160))
        panel.alpha_composite(icon_128, (16, 16))
        canvas.alpha_composite(panel, (x, y))
        draw.rectangle((x, y, x + 160, y + 160), outline=(126, 136, 172, 255), width=2)
        draw.text((x, y + 172), str(icon_set["icon_id"]).replace("_", " "), fill=(211, 224, 255), font=label_font)
        draw.text((x, y + 196), "128 candidate", fill=(245, 238, 220), font=small_font)

        icon_64 = Image.open(variants[64]).convert("RGBA")
        icon_32 = Image.open(variants[32]).convert("RGBA")
        small_panel = checkerboard((128, 80), square=8)
        small_panel.alpha_composite(icon_64, (8, 8))
        small_panel.alpha_composite(icon_32, (88, 24))
        canvas.alpha_composite(small_panel, (x, y + 226))
        draw.rectangle((x, y + 226, x + 128, y + 306), outline=(126, 136, 172, 255), width=2)
        draw.text((x, y + 312), "64 / 32 review", fill=(245, 238, 220), font=small_font)

    y = 824
    draw.text((44, y), "Dark / Light Background Check", fill=(245, 238, 220), font=label_font)
    y += 36
    dark_panel = Image.new("RGBA", (780, 120), (20, 24, 38, 255))
    light_panel = Image.new("RGBA", (780, 120), (238, 232, 220, 255))
    arrange_icons_on_panel(dark_panel, icon_sets)
    arrange_icons_on_panel(light_panel, icon_sets)
    canvas.alpha_composite(dark_panel, (44, y))
    canvas.alpha_composite(light_panel, (868, y))
    draw.rectangle((44, y, 824, y + 120), outline=(126, 136, 172, 255), width=2)
    draw.rectangle((868, y, 1648, y + 120), outline=(126, 136, 172, 255), width=2)
    draw.text((44, y + 132), "dark panel readability", fill=(225, 220, 205), font=small_font)
    draw.text((868, y + 132), "light panel readability", fill=(225, 220, 205), font=small_font)

    canvas.save(CONTACT_SHEET_PATH)


def arrange_icons_on_panel(panel: Image.Image, icon_sets: list[dict[str, object]]) -> None:
    x = 18
    for icon_set in icon_sets:
        icon = Image.open(icon_set["variants"][64]).convert("RGBA")
        panel.alpha_composite(icon, (x, 28))
        x += 76


def write_review_sheet(icon_sets: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1600, 1320), (248, 244, 236, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(20)
    body_font = load_font(16)

    draw.text((36, 28), "P0 Batch 79 - system icon candidate review", fill=(44, 38, 36), font=title_font)
    draw.text((36, 76), "Decision surface: imagegen source, chroma alpha cleanup, 10 symbolic system icons with 128/64/32 derivatives.", fill=(126, 54, 45), font=body_font)

    source = Image.open(SOURCE_PATH).convert("RGBA")
    alpha = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    draw_panel(canvas, draw, fit(source, (720, 270)), (36, 124), "imagegen chroma source", label_font)
    draw_panel(canvas, draw, fit(flat_composite(alpha, (32, 33, 44, 255)), (720, 270)), (840, 124), "alpha sheet after chroma removal", label_font)

    contact = Image.open(CONTACT_SHEET_PATH).convert("RGBA")
    draw_panel(canvas, draw, fit(contact, (760, 470)), (36, 470), "system icon contact sheet", label_font)

    x = 850
    y = 470
    draw.text((x, y), "Visual Review", fill=(44, 38, 36), font=label_font)
    y += 34
    bullets = [
        "Pass: all 10 requested system icons are present: settings, sound, mute, back, close, pause, continue, retry, lock, and warning.",
        "Pass: there is no text, label, number, watermark, cat body, paw, tail, fur marking, character face, or starter-cat costume motif.",
        "Pass: icons share moon-blue dreamglass surfaces, consistent rim-light, and restrained fish-gold accents.",
        "Watch: mute uses both slash and remaining sound arcs; Unity review should verify it still reads as mute at 32px.",
        "Watch: warning is intentionally a symbolic triangle without text; confirm it reads as warning in the final UI.",
        "Unity gate: Sprite import settings, 64px/32px readability, dark/light backgrounds, scene/prefab binding, and Console status remain required.",
    ]
    for bullet in bullets:
        for line in wrap(bullet, 78):
            draw.text((x, y), line, fill=(44, 38, 36), font=body_font)
            y += 22
        y += 8

    y += 12
    draw.text((x, y), "Icon Mapping", fill=(44, 38, 36), font=label_font)
    y += 34
    for icon_id, display_name, review in ICON_SPECS:
        for line in wrap(f"- {icon_id}: {display_name}; {review}", 76):
            draw.text((x, y), line, fill=(44, 38, 36), font=body_font)
            y += 22

    canvas.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# System Icons Batch 79 Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "This batch fills the P0 UI inventory gap for general system icons: settings, sound, mute, back, close, pause, continue, retry, lock, and warning. It is a non-cat symbolic UI icon packet and does not modify runtime prefabs, scenes, or `Assets` files.",
        "",
        "## Outputs",
        "",
        f"- Chroma source: `{to_repo_path(SOURCE_PATH)}`",
        f"- Alpha sheet: `{to_repo_path(ALPHA_SHEET_PATH)}`",
        f"- 128px icons: `{to_repo_path(ICON_DIRS[128])}`",
        f"- 64px icons: `{to_repo_path(ICON_DIRS[64])}`",
        f"- 32px icons: `{to_repo_path(ICON_DIRS[32])}`",
        f"- Contact sheet: `{to_repo_path(CONTACT_SHEET_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(PROMPT_PATH)}`",
        "",
        "## Visual Decision",
        "",
        "- Pass: ten system icons are present: settings, sound, mute, back, close, pause, continue, retry, lock, and warning.",
        "- Pass: no cat body, fur markings, paws, tails, starter-cat costume motifs, colored-turnaround crops, text, letters, numbers, or watermarks are present.",
        "- Pass: icons are symbolic UI assets rather than character art.",
        "- Pass: the icon language is consistent with Batch 78 settings controls and the dreamglass UI direction.",
        "- Watch: `mute` keeps sound arcs behind the slash and must be checked at 32px to avoid reading as sound-on.",
        "- Watch: `warning` avoids text/exclamation marks and must be checked in context to ensure it still reads as a warning state.",
        "",
        "## Independent Review Findings",
        "",
        "- P0: three independent review lanes found no visual/source-lock, tooling, or tracking blocker for candidate-complete status.",
        "- P1: `mute` remains readable at 32px, but because sound arcs remain behind the slash, final UI review must check it against `sound` to avoid a sound-on read.",
        "- P1: `warning` is readable as a triangle, but without an exclamation/text cue it can read as a dream sigil; warning-state recognition must be confirmed in context.",
        "- P1: the validator hashes candidate PNGs plus source/alpha sheets, but only existence-checks the contact sheet, review sheet, review note, and process note.",
        "- P1: the builder uses deterministic 2x5 grid splitting, but source cell coordinates are not preserved in the manifest or independently rechecked by the validator.",
        "- P1: the validator does not token-check process-note content.",
        "- Tracking: keep Batch 79 as `candidate_complete_pending_unity_review`.",
        "",
        "## Unity Gate",
        "",
        "- Import is blocked until Unity validates Sprite import settings, 64px/32px readability, dark/light panel readability, scene/prefab binding, and Console status.",
        "- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['icon_id']}` `{row['size_variant']}px` -> `{row['candidate_path']}`")

    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]], alpha_sheet: Image.Image) -> None:
    transparent, partial, total = alpha_stats(alpha_sheet)
    lines = [
        "# System Icons Batch 79 Process Note",
        "",
        "Process: built-in image_gen generation, workspace source copy, local chroma-key alpha removal with the imagegen helper, deterministic 2x5 grid splitting, 128px normalization, 64px/32px derivative generation, contact sheet creation, manifest generation, and candidate review.",
        "",
        "Generation prompt summary:",
        "",
        "- Ten system UI icons in a strict 2 rows x 5 columns grid.",
        "- Icon order: settings, sound, mute, back, close, pause, continue, retry, lock, warning.",
        "- Flat `#00ff00` chroma-key background, no cats, no text, no starter-cat motifs.",
        "- UI language: moon-blue glass, navy dreamglass shadows, lavender rim light, restrained fish-gold accents.",
        "",
        "Chroma-key result:",
        "",
        "- Key color sampled by helper: `#05f809`.",
        f"- Transparent pixels: {transparent} / {total}.",
        f"- Partially transparent pixels: {partial} / {total}.",
        f"- Alpha sheet size: {alpha_sheet.width}x{alpha_sheet.height}.",
        "",
        f"Manifest rows: {len(rows)}.",
        "",
        "No Unity import was performed.",
        "",
        "Known validation limits from independent review:",
        "",
        "- Candidate PNGs, source image, and alpha sheet are hash-checked by the validator.",
        "- Contact sheet, review sheet, review note, and process note are existence-checked but not hash-checked.",
        "- Builder grid splitting is deterministic and sorted left-to-right / top-to-bottom, but source cell coordinates are not persisted in the manifest.",
        "- Process-note content is reviewed by humans but not token-checked by the validator.",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def alpha_stats(image: Image.Image) -> tuple[int, int, int]:
    alpha = image.getchannel("A")
    transparent = 0
    partial = 0
    total = image.width * image.height
    for value in alpha.tobytes():
        if value == 0:
            transparent += 1
        elif value < 255:
            partial += 1
    return transparent, partial, total


def draw_panel(canvas: Image.Image, draw: ImageDraw.ImageDraw, image: Image.Image, xy: tuple[int, int], label: str, font: ImageFont.ImageFont) -> None:
    x, y = xy
    width, height = image.size
    draw.rectangle((x - 8, y - 8, x + width + 8, y + height + 38), fill=(255, 252, 245, 255), outline=(187, 168, 138, 255), width=2)
    canvas.alpha_composite(image, (x, y))
    draw.text((x, y + height + 12), label, fill=(44, 38, 36), font=font)


def checkerboard(size: tuple[int, int], square: int = 8) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (238, 238, 238, 255))
    draw = ImageDraw.Draw(image)
    for y in range(0, height, square):
        for x in range(0, width, square):
            if (x // square + y // square) % 2:
                draw.rectangle((x, y, min(x + square, width), min(y + square, height)), fill=(197, 197, 197, 255))
    return image


def flat_composite(image: Image.Image, color: tuple[int, int, int, int]) -> Image.Image:
    background = Image.new("RGBA", image.size, color)
    background.alpha_composite(image.convert("RGBA"))
    return background


def fit(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    image = image.convert("RGBA")
    image.thumbnail(size, Image.Resampling.LANCZOS)
    result = Image.new("RGBA", size, (0, 0, 0, 0))
    result.alpha_composite(image, ((size[0] - image.width) // 2, (size[1] - image.height) // 2))
    return result


def wrap(text: str, width: int) -> list[str]:
    words = text.split()
    lines: list[str] = []
    current: list[str] = []
    current_length = 0
    for word in words:
        next_length = current_length + len(word) + (1 if current else 0)
        if current and next_length > width:
            lines.append(" ".join(current))
            current = [word]
            current_length = len(word)
        else:
            current.append(word)
            current_length = next_length
    if current:
        lines.append(" ".join(current))
    return lines


def load_font(size: int) -> ImageFont.ImageFont:
    for font_path in (
        "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/arial.ttf",
    ):
        if Path(font_path).exists():
            return ImageFont.truetype(font_path, size=size)
    return ImageFont.load_default()


def image_size(path: Path) -> str:
    with Image.open(path) as image:
        return f"{image.width}x{image.height}"


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_repo_path(path: Path) -> str:
    return path.resolve().relative_to(REPO_ROOT).as_posix()


if __name__ == "__main__":
    main()
