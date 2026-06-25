from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_77_owner_sleep_status_icon_candidates_2026-06-24"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "owner_sleep_status_icons" / BATCH_SLUG
ICON_DIRS = {
    256: CANDIDATE_DIR / "icons_256",
    64: CANDIDATE_DIR / "icons_64",
    32: CANDIDATE_DIR / "icons_32",
}

SOURCE_PATH = CANDIDATE_DIR / "thecat_ui_owner_sleep_status_icons_batch77_chromakey_source_v001.png"
ALPHA_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_owner_sleep_status_icons_batch77_alpha_sheet_v001.png"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_owner_sleep_status_icons_batch77_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_owner_sleep_status_icons_batch77_review_sheet_v001.png"
MANIFEST_PATH = CANDIDATE_DIR / "owner_sleep_status_icons_batch77_manifest.csv"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "owner_sleep_status_icons_batch77_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "owner_sleep_status_icons_batch77_process_note.md"
PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_77_owner_sleep_status_icon_candidates.md"

ICON_STATES = (
    ("deep_sleep", "Deep sleep / stable hypnosis", "crescent moon, calm breathing wave, blue motes"),
    ("half_awake", "Half-awake warning", "closed eye, cracked dream bubble, purple consciousness orb"),
    ("near_awake", "Near-awake critical", "almost-open eye, amber alarm pulse, widening dream cracks"),
    ("wake_failure", "Awakened failure", "open eye, returning orb, broken dream shards"),
)

FIELD_NAMES = (
    "asset_id",
    "state_id",
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
    write_process_note(rows, alpha_sheet.size)

    print(f"Wrote {len(rows)} Batch 77 manifest row(s).")
    print(to_repo_path(MANIFEST_PATH))


def split_and_normalize_icons(alpha_sheet: Image.Image) -> list[dict[str, object]]:
    icon_sets: list[dict[str, object]] = []

    for index, (state_id, display_name, review) in enumerate(ICON_STATES):
        left = round(index * alpha_sheet.width / 4)
        right = round((index + 1) * alpha_sheet.width / 4)
        raw_cell = alpha_sheet.crop((left, 0, right, alpha_sheet.height))
        base_icon = normalize_to_square(raw_cell, 256, 24)
        variants: dict[int, Path] = {}

        for size in (256, 64, 32):
            icon = base_icon if size == 256 else base_icon.resize((size, size), Image.Resampling.LANCZOS)
            asset_id = f"thecat_ui_owner_sleep_status_{state_id}_{size}_candidate_v001"
            path = ICON_DIRS[size] / f"{asset_id}.png"
            icon.save(path)
            variants[size] = path

        icon_sets.append(
            {
                "state_id": state_id,
                "display_name": display_name,
                "review": review,
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
        for size in (256, 64, 32):
            path = Path(variants[size])
            rows.append(
                {
                    "asset_id": path.stem,
                    "state_id": str(icon_set["state_id"]),
                    "display_name": str(icon_set["display_name"]),
                    "size_variant": str(size),
                    "batch_slug": BATCH_SLUG,
                    "asset_type": "owner_sleep_status_icon_candidate",
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
    canvas = Image.new("RGBA", (1500, 720), (33, 29, 42, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(36)
    label_font = load_font(20)
    small_font = load_font(15)

    draw.text((36, 28), "P0 Batch 77 - Owner Sleep Status Icon Candidates", fill=(245, 238, 220), font=title_font)
    draw.text((36, 78), "Candidate-only UI icons. 4 states x 3 sizes. Do not import until Unity HUD review.", fill=(238, 193, 106), font=small_font)

    x_positions = (74, 428, 782, 1136)
    for icon_set, x in zip(icon_sets, x_positions):
        state_id = str(icon_set["state_id"])
        variants = icon_set["variants"]
        draw.text((x, 128), state_id.replace("_", " "), fill=(211, 224, 255), font=label_font)

        icon_256 = Image.open(variants[256]).convert("RGBA")
        panel = checkerboard((256, 256))
        panel.alpha_composite(icon_256)
        canvas.alpha_composite(panel, (x, 168))
        draw.rectangle((x, 168, x + 256, 424), outline=(126, 136, 172, 255), width=2)
        draw.text((x, 434), "256 candidate", fill=(245, 238, 220), font=small_font)

        icon_64 = Image.open(variants[64]).convert("RGBA")
        icon_32 = Image.open(variants[32]).convert("RGBA")
        small_panel = checkerboard((160, 96), square=8)
        small_panel.alpha_composite(icon_64, (14, 16))
        small_panel.alpha_composite(icon_32, (104, 32))
        canvas.alpha_composite(small_panel, (x, 482))
        draw.rectangle((x, 482, x + 160, 578), outline=(126, 136, 172, 255), width=2)
        draw.text((x, 588), "64 / 32 review", fill=(245, 238, 220), font=small_font)

        for line_index, line in enumerate(wrap(str(icon_set["review"]), 34)):
            draw.text((x, 626 + line_index * 20), line, fill=(225, 220, 205), font=small_font)

    canvas.save(CONTACT_SHEET_PATH)


def write_review_sheet(icon_sets: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1600, 1000), (248, 244, 236, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(20)
    body_font = load_font(16)

    draw.text((36, 28), "P0 Batch 77 - owner sleep-state status icon candidate review", fill=(44, 38, 36), font=title_font)
    draw.text((36, 76), "Decision surface: imagegen source, chroma alpha cleanup, split into 4 symbolic status icons with 64/32 derivatives.", fill=(126, 54, 45), font=body_font)

    source = Image.open(SOURCE_PATH).convert("RGBA")
    alpha = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    draw_panel(canvas, draw, fit(source, (700, 260)), (36, 124), "imagegen chroma source", label_font)
    draw_panel(canvas, draw, fit(flat_composite(alpha, (32, 33, 44, 255)), (700, 260)), (836, 124), "alpha sheet after chroma removal", label_font)

    contact = Image.open(CONTACT_SHEET_PATH).convert("RGBA")
    draw_panel(canvas, draw, fit(contact, (760, 365)), (36, 450), "split icon contact sheet", label_font)

    x = 850
    y = 450
    draw.text((x, y), "Visual Review", fill=(44, 38, 36), font=label_font)
    y += 34
    bullets = [
        "Pass: four owner sleep-state icons map to Batch 76 and the source inventory states.",
        "Pass: no cats, paws, tails, starter-cat costume motifs, text, letters, or numbers are present.",
        "Pass: icons use the established sleep / warning / dream-crack palette and remain symbolic rather than character portraits.",
        "Watch: wake-failure may read like a purple eye/mark sigil at 32px; Unity HUD review must confirm it does not collide with the existing Mark icon language.",
        "Watch: half-awake is slightly intense for a first warning and should be compared against Batch 76 timing before approval.",
        "Unity gate: import settings, 64px/32px readability, dark/light HUD backgrounds, cooldown overlays, and Console status remain required.",
    ]
    for bullet in bullets:
        for line in wrap(bullet, 78):
            draw.text((x, y), line, fill=(44, 38, 36), font=body_font)
            y += 22
        y += 8

    y += 12
    draw.text((x, y), "State Mapping", fill=(44, 38, 36), font=label_font)
    y += 34
    for state_id, display_name, review in ICON_STATES:
        for line in wrap(f"- {state_id}: {display_name}; {review}", 76):
            draw.text((x, y), line, fill=(44, 38, 36), font=body_font)
            y += 22

    canvas.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Owner Sleep Status Icons Batch 77 Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "This batch fills the P0 UI inventory gap for four owner sleep-state HUD/settlement status icons. The icons synchronize with the Batch 76 owner sleep-state animation packet.",
        "",
        "## Outputs",
        "",
        f"- Chroma source: `{to_repo_path(SOURCE_PATH)}`",
        f"- Alpha sheet: `{to_repo_path(ALPHA_SHEET_PATH)}`",
        f"- 256px icons: `{to_repo_path(ICON_DIRS[256])}`",
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
        "- Pass: four states are present: deep sleep, half awake, near awake, wake failure.",
        "- Pass: no cat body, fur markings, paws, tails, starter-cat costumes, colored-turnaround crops, text, letters, or numbers are present.",
        "- Pass: icons are symbolic UI assets rather than owner portraits or character art.",
        "- Pass: the state progression escalates from calm blue sleep to amber warning and purple wake failure.",
        "- Watch: `wake_failure` may read like a purple eye/mark sigil at 32px; Unity HUD review must confirm it does not collide with existing Mark icon language.",
        "- Watch: `half_awake` is visually intense for a first warning and should be compared against the subtler Batch 76 half-awake timing before import approval.",
        "",
        "## Unity Gate",
        "",
        "- Import is blocked until Unity validates Sprite import settings, 64px and 32px readability, dark/light HUD backgrounds, cooldown overlays, scene/prefab binding, and Console status.",
        "- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['state_id']}` `{row['size_variant']}px` -> `{row['candidate_path']}`")

    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]], alpha_size: tuple[int, int]) -> None:
    lines = [
        "# Owner Sleep Status Icons Batch 77 Process Note",
        "",
        "Process: built-in image_gen generation, workspace source copy, local chroma-key alpha removal with the imagegen helper, deterministic alpha-bounds splitting, 256px normalization, 64px/32px derivative generation, contact sheet creation, manifest generation, and candidate review.",
        "",
        "Generation prompt summary:",
        "",
        "- Four symbolic owner sleep-state status icons in one horizontal row.",
        "- State order: deep sleep, half awake, near awake, wake failure.",
        "- Flat `#00ff00` chroma-key background, no cats, no text, no starter-cat motifs.",
        "- UI icon language: dream glow, crescent/breathing wave, cracked dream bubble, amber alarm pulse, returning consciousness orb.",
        "",
        "Chroma-key result:",
        "",
        "- Key color sampled by helper: `#04f203`.",
        "- Transparent pixels: 1066315 / 1572519.",
        "- Partially transparent pixels: 10063 / 1572519.",
        f"- Alpha sheet size: {alpha_size[0]}x{alpha_size[1]}.",
        "",
        f"Manifest rows: {len(rows)}.",
        "",
        "No Unity import was performed.",
    ]
    PROCESS_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def draw_panel(canvas: Image.Image, draw: ImageDraw.ImageDraw, image: Image.Image, xy: tuple[int, int], label: str, font: ImageFont.ImageFont) -> None:
    x, y = xy
    width, height = image.size
    draw.rectangle((x - 8, y - 8, x + width + 8, y + height + 38), fill=(255, 252, 245, 255), outline=(187, 168, 138, 255), width=2)
    canvas.alpha_composite(image, (x, y))
    draw.text((x, y + height + 12), label, fill=(44, 38, 36), font=font)


def checkerboard(size: tuple[int, int], square: int = 16) -> Image.Image:
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
