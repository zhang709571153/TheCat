from __future__ import annotations

import csv
import hashlib
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


REPO_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_78_settings_control_candidates_2026-06-24"
CANDIDATE_DIR = REPO_ROOT / "design" / "development" / "asset_candidates" / "ui" / "settings_controls" / BATCH_SLUG
CONTROL_DIR = CANDIDATE_DIR / "controls"

SOURCE_PATH = CANDIDATE_DIR / "thecat_ui_settings_controls_batch78_chromakey_source_v001.png"
ALPHA_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_settings_controls_batch78_alpha_sheet_v001.png"
CONTACT_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_settings_controls_batch78_contact_sheet_v001.png"
REVIEW_SHEET_PATH = CANDIDATE_DIR / "thecat_ui_settings_controls_batch78_review_sheet_v001.png"
MANIFEST_PATH = CANDIDATE_DIR / "settings_controls_batch78_manifest.csv"
REVIEW_NOTE_PATH = CANDIDATE_DIR / "settings_controls_batch78_candidate_review.md"
PROCESS_NOTE_PATH = CANDIDATE_DIR / "settings_controls_batch78_process_note.md"
PROMPT_PATH = REPO_ROOT / "design" / "development" / "agent_prompts" / "p0_asset_batch_78_settings_control_candidates.md"

CONTROL_SPECS = (
    ("slider_track", "Slider track / empty value", (384, 64), 4, "long empty dreamglass volume/setting slider track"),
    ("slider_knob", "Slider knob", (96, 96), 8, "round moon-blue glass knob"),
    ("switch_off", "Switch toggle off", (192, 96), 6, "muted inactive switch with knob on the left"),
    ("switch_on", "Switch toggle on", (192, 96), 6, "active switch with blue-gold glow and knob on the right"),
    ("checkbox_unchecked", "Checkbox unchecked", (96, 96), 8, "empty square checkbox frame"),
    ("checkbox_checked", "Checkbox checked", (96, 96), 8, "checked square checkbox frame with clear tick"),
)

FIELD_NAMES = (
    "asset_id",
    "control_id",
    "display_name",
    "target_size",
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
    CONTROL_DIR.mkdir(parents=True, exist_ok=True)

    alpha_sheet = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    controls = split_and_normalize_controls(alpha_sheet)
    rows = build_manifest_rows(controls)
    write_manifest(rows)
    write_contact_sheet(controls)
    write_review_sheet(controls)
    write_review_note(rows)
    write_process_note(rows, alpha_sheet)

    print(f"Wrote {len(rows)} Batch 78 manifest row(s).")
    print(to_repo_path(MANIFEST_PATH))


def split_and_normalize_controls(alpha_sheet: Image.Image) -> list[dict[str, object]]:
    segments = find_horizontal_segments(alpha_sheet)
    if len(segments) != len(CONTROL_SPECS):
        raise ValueError(f"Expected {len(CONTROL_SPECS)} horizontal controls, found {len(segments)} segment(s): {segments}")

    controls: list[dict[str, object]] = []
    for (control_id, display_name, target_size, margin, review), segment in zip(CONTROL_SPECS, segments):
        left, right = segment
        raw_cell = alpha_sheet.crop((left, 0, right, alpha_sheet.height))
        normalized = normalize_to_size(raw_cell, target_size, margin)
        asset_id = f"thecat_ui_settings_{control_id}_{target_size[0]}x{target_size[1]}_candidate_v001"
        path = CONTROL_DIR / f"{asset_id}.png"
        normalized.save(path)
        controls.append(
            {
                "control_id": control_id,
                "display_name": display_name,
                "target_size": target_size,
                "source_segment": segment,
                "path": path,
                "review": review,
            }
        )

    return controls


def find_horizontal_segments(image: Image.Image) -> list[tuple[int, int]]:
    alpha = image.getchannel("A")
    active_columns: list[bool] = []
    for x in range(image.width):
        active = 0
        for y in range(image.height):
            if alpha.getpixel((x, y)) > 16:
                active += 1
        active_columns.append(active > 3)

    segments: list[tuple[int, int]] = []
    start: int | None = None
    gap = 0
    gap_tolerance = 36

    for x, is_active in enumerate(active_columns):
        if is_active:
            if start is None:
                start = x
            gap = 0
        elif start is not None:
            gap += 1
            if gap > gap_tolerance:
                end = x - gap + 1
                if end - start > 32:
                    segments.append((start, end))
                start = None
                gap = 0

    if start is not None:
        segments.append((start, image.width))

    return segments


def normalize_to_size(image: Image.Image, target_size: tuple[int, int], margin: int) -> Image.Image:
    alpha_mask = image.getchannel("A").point(lambda value: 255 if value > 16 else 0)
    bbox = alpha_mask.getbbox()
    if bbox is None:
        raise ValueError("Source control cell has no visible alpha content")

    cropped = image.crop(bbox)
    max_size = (target_size[0] - margin * 2, target_size[1] - margin * 2)
    cropped.thumbnail(max_size, Image.Resampling.LANCZOS)
    result = Image.new("RGBA", target_size, (0, 0, 0, 0))
    result.alpha_composite(cropped, ((target_size[0] - cropped.width) // 2, (target_size[1] - cropped.height) // 2))
    return result


def build_manifest_rows(controls: list[dict[str, object]]) -> list[dict[str, str]]:
    rows: list[dict[str, str]] = []
    source_hash = sha256(SOURCE_PATH)
    alpha_hash = sha256(ALPHA_SHEET_PATH)

    for control in controls:
        path = Path(control["path"])
        target_width, target_height = control["target_size"]
        rows.append(
            {
                "asset_id": path.stem,
                "control_id": str(control["control_id"]),
                "display_name": str(control["display_name"]),
                "target_size": f"{target_width}x{target_height}",
                "batch_slug": BATCH_SLUG,
                "asset_type": "settings_control_candidate",
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
                "visual_review": str(control["review"]),
            }
        )

    return rows


def write_manifest(rows: list[dict[str, str]]) -> None:
    with MANIFEST_PATH.open("w", encoding="utf-8", newline="") as handle:
        writer = csv.DictWriter(handle, fieldnames=FIELD_NAMES)
        writer.writeheader()
        writer.writerows(rows)


def write_contact_sheet(controls: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1700, 760), (32, 30, 43, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(20)
    small_font = load_font(15)

    draw.text((36, 28), "P0 Batch 78 - Settings Control Candidates", fill=(245, 238, 220), font=title_font)
    draw.text((36, 78), "Candidate-only sliders, switches, and checkboxes. Do not import until settings-screen review.", fill=(238, 193, 106), font=small_font)

    placements = (
        (54, 140),
        (540, 120),
        (720, 132),
        (1000, 132),
        (1300, 120),
        (1510, 120),
    )
    for control, (x, y) in zip(controls, placements):
        path = Path(control["path"])
        sprite = Image.open(path).convert("RGBA")
        panel = checkerboard(sprite.size)
        panel.alpha_composite(sprite)
        canvas.alpha_composite(panel, (x, y))
        draw.rectangle((x, y, x + sprite.width, y + sprite.height), outline=(126, 136, 172, 255), width=2)
        draw_control_label(draw, (x, y + sprite.height + 12), str(control["control_id"]), label_font, small_font)
        draw.text((x, y + sprite.height + 56), f"{sprite.width}x{sprite.height}", fill=(245, 238, 220), font=small_font)

    y = 326
    draw.text((44, y), "Dark / Light Settings Panel Check", fill=(245, 238, 220), font=label_font)
    y += 42
    dark_panel = Image.new("RGBA", (690, 250), (20, 24, 38, 255))
    light_panel = Image.new("RGBA", (690, 250), (238, 232, 220, 255))
    arrange_controls_on_panel(dark_panel, controls)
    arrange_controls_on_panel(light_panel, controls)
    canvas.alpha_composite(dark_panel, (44, y))
    canvas.alpha_composite(light_panel, (866, y))
    draw.rectangle((44, y, 734, y + 250), outline=(126, 136, 172, 255), width=2)
    draw.rectangle((866, y, 1556, y + 250), outline=(126, 136, 172, 255), width=2)
    draw.text((44, y + 262), "dark panel readability", fill=(225, 220, 205), font=small_font)
    draw.text((866, y + 262), "light panel readability", fill=(225, 220, 205), font=small_font)

    notes = [
        "6 transparent controls: slider track, slider knob, switch off/on, checkbox unchecked/checked.",
        "No cats, paws, tails, fur markings, starter-cat costume motifs, text, letters, or numbers.",
        "Unity gate still needs Sprite import settings, settings-screen screenshots, pointer hit target scale, and Console status.",
    ]
    for index, note in enumerate(notes):
        draw.text((44, 692 + index * 22), note, fill=(245, 238, 220), font=small_font)

    canvas.save(CONTACT_SHEET_PATH)


def draw_control_label(draw: ImageDraw.ImageDraw, xy: tuple[int, int], control_id: str, label_font: ImageFont.ImageFont, small_font: ImageFont.ImageFont) -> None:
    x, y = xy
    parts = control_id.split("_")
    if len(parts) > 2:
        first = " ".join(parts[:1])
        second = " ".join(parts[1:])
        draw.text((x, y), first, fill=(211, 224, 255), font=label_font)
        draw.text((x, y + 24), second, fill=(211, 224, 255), font=small_font)
    else:
        draw.text((x, y), control_id.replace("_", " "), fill=(211, 224, 255), font=label_font)


def arrange_controls_on_panel(panel: Image.Image, controls: list[dict[str, object]]) -> None:
    sprites = {str(control["control_id"]): Image.open(control["path"]).convert("RGBA") for control in controls}
    panel.alpha_composite(sprites["slider_track"], (40, 34))
    panel.alpha_composite(sprites["slider_knob"], (310, 18))
    panel.alpha_composite(sprites["switch_off"], (46, 134))
    panel.alpha_composite(sprites["switch_on"], (270, 134))
    panel.alpha_composite(sprites["checkbox_unchecked"], (506, 36))
    panel.alpha_composite(sprites["checkbox_checked"], (506, 134))


def write_review_sheet(controls: list[dict[str, object]]) -> None:
    canvas = Image.new("RGBA", (1600, 1280), (248, 244, 236, 255))
    draw = ImageDraw.Draw(canvas)
    title_font = load_font(34)
    label_font = load_font(20)
    body_font = load_font(16)

    draw.text((36, 28), "P0 Batch 78 - settings control candidate review", fill=(44, 38, 36), font=title_font)
    draw.text((36, 76), "Decision surface: built-in imagegen source, chroma alpha cleanup, six exact-size transparent UI controls.", fill=(126, 54, 45), font=body_font)

    source = Image.open(SOURCE_PATH).convert("RGBA")
    alpha = Image.open(ALPHA_SHEET_PATH).convert("RGBA")
    draw_panel(canvas, draw, fit(source, (720, 250)), (36, 124), "imagegen chroma source", label_font)
    draw_panel(canvas, draw, fit(flat_composite(alpha, (32, 33, 44, 255)), (720, 250)), (840, 124), "alpha sheet after chroma removal", label_font)

    contact = Image.open(CONTACT_SHEET_PATH).convert("RGBA")
    draw_panel(canvas, draw, fit(contact, (760, 390)), (36, 440), "settings-control contact sheet", label_font)

    x = 850
    y = 440
    draw.text((x, y), "Visual Review", fill=(44, 38, 36), font=label_font)
    y += 34
    bullets = [
        "Pass: all six requested controls are present: slider track, slider knob, switch off, switch on, unchecked box, checked box.",
        "Pass: there is no text, label, number, watermark, cat body, paw, tail, fur marking, or starter-cat costume motif.",
        "Pass: active controls use moon-blue and restrained fish-gold accents, while inactive controls stay muted gray-blue.",
        "Pass: controls read against both dark and light settings-panel backgrounds as candidate art.",
        "Watch: the slider knob is strongly ornamental; Unity review should confirm it remains readable when dragged over the long track.",
        "Watch: on/off color contrast must be checked in the actual settings screen and color-blind UI pass.",
        "Unity gate: Sprite import settings, click/pointer target scale, settings-screen screenshots, scene/prefab binding, and Console status remain required.",
    ]
    for bullet in bullets:
        for line in wrap(bullet, 78):
            draw.text((x, y), line, fill=(44, 38, 36), font=body_font)
            y += 22
        y += 8

    y += 12
    draw.text((x, y), "Control Mapping", fill=(44, 38, 36), font=label_font)
    y += 34
    for control in controls:
        width, height = control["target_size"]
        line = f"- {control['control_id']}: {control['display_name']} ({width}x{height}); {control['review']}"
        for wrapped in wrap(line, 76):
            draw.text((x, y), wrapped, fill=(44, 38, 36), font=body_font)
            y += 22

    canvas.save(REVIEW_SHEET_PATH)


def write_review_note(rows: list[dict[str, str]]) -> None:
    lines = [
        "# Settings Controls Batch 78 Candidate Review",
        "",
        "Decision: candidate review only; do not import into Unity.",
        "",
        "This batch fills the P0 UI inventory gap for settings sliders, switches, and checkboxes. It is a non-cat symbolic UI control packet and does not modify runtime prefabs, scenes, or `Assets` files.",
        "",
        "## Outputs",
        "",
        f"- Chroma source: `{to_repo_path(SOURCE_PATH)}`",
        f"- Alpha sheet: `{to_repo_path(ALPHA_SHEET_PATH)}`",
        f"- Controls: `{to_repo_path(CONTROL_DIR)}`",
        f"- Contact sheet: `{to_repo_path(CONTACT_SHEET_PATH)}`",
        f"- Review sheet: `{to_repo_path(REVIEW_SHEET_PATH)}`",
        f"- Manifest: `{to_repo_path(MANIFEST_PATH)}`",
        f"- Process note: `{to_repo_path(PROCESS_NOTE_PATH)}`",
        f"- Agent prompt: `{to_repo_path(PROMPT_PATH)}`",
        "",
        "## Visual Decision",
        "",
        "- Pass: six controls are present: slider track, slider knob, switch off, switch on, checkbox unchecked, and checkbox checked.",
        "- Pass: no cat body, fur markings, paws, tails, starter-cat costume motifs, colored-turnaround crops, text, letters, numbers, or watermarks are present.",
        "- Pass: controls are symbolic UI assets for settings screens rather than character art.",
        "- Pass: active/inactive state contrast is clear in candidate sheets.",
        "- Watch: slider knob readability over the long track must be checked in Unity at the actual settings scale.",
        "- Watch: switch on/off contrast must be checked against the final settings-panel background and accessibility color pass.",
        "",
        "## Independent Review Findings",
        "",
        "- P0: three independent review lanes found no visual/source-lock, tooling, or tracking blocker for candidate-complete status.",
        "- P1: Unity review must verify slider drag alignment, value-fill behavior, and pointer target scale because the knob is ornamental and large relative to the track.",
        "- P1: switch on/off contrast depends on the final settings panel and needs a color-blind accessibility pass in addition to the current color/knob-position distinction.",
        "- P1: the validator hashes candidate PNGs plus source/alpha sheets, but only existence-checks the contact sheet, review sheet, review note, and process note.",
        "- P1: alpha segmentation is deterministic in the builder, but segment coordinates are not preserved in the manifest or independently rechecked by the validator.",
        "- Tracking: keep Batch 78 as `candidate_complete_pending_unity_review`.",
        "",
        "## Unity Gate",
        "",
        "- Import is blocked until Unity validates Sprite import settings, settings-screen screenshots, click/pointer target scale, dark/light panel readability, scene/prefab binding, and Console status.",
        "- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.",
        "",
        "## Manifest Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['control_id']}` `{row['target_size']}` -> `{row['candidate_path']}`")

    REVIEW_NOTE_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(rows: list[dict[str, str]], alpha_sheet: Image.Image) -> None:
    transparent, partial, total = alpha_stats(alpha_sheet)
    lines = [
        "# Settings Controls Batch 78 Process Note",
        "",
        "Process: built-in image_gen generation, workspace source copy, local chroma-key alpha removal with the imagegen helper, deterministic horizontal alpha segmentation, exact-size transparent control normalization, contact sheet creation, manifest generation, and candidate review.",
        "",
        "Generation prompt summary:",
        "",
        "- Six settings UI controls in one horizontal row.",
        "- Control order: slider track, slider knob, switch off, switch on, checkbox unchecked, checkbox checked.",
        "- Flat `#00ff00` chroma-key background, no cats, no text, no starter-cat motifs.",
        "- UI language: dreamglass navy panels, moon-blue rim light, lavender glow, restrained fish-gold active accents, muted gray-blue inactive accents.",
        "",
        "Chroma-key result:",
        "",
        f"- Transparent pixels: {transparent} / {total}.",
        f"- Partially transparent pixels: {partial} / {total}.",
        f"- Alpha sheet size: {alpha_sheet.width}x{alpha_sheet.height}.",
        "",
        "Detected control segments:",
        "",
    ]

    for row in rows:
        lines.append(f"- `{row['control_id']}` -> `{row['candidate_path']}` ({row['target_size']})")

    lines.extend(
        [
            "",
            f"Manifest rows: {len(rows)}.",
            "",
            "No Unity import was performed.",
            "",
            "Known validation limits from independent review:",
            "",
            "- Candidate PNGs, source image, and alpha sheet are hash-checked by the validator.",
            "- Contact sheet, review sheet, review note, and process note are existence-checked but not hash-checked.",
            "- Builder segmentation is deterministic and sorted left-to-right, but segment coordinates are not persisted in the manifest.",
        ]
    )
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
