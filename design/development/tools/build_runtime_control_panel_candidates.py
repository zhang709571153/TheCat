from __future__ import annotations

import csv
import hashlib
import textwrap
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


PROJECT_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_63_runtime_control_panel_candidates_2026-06-15"
BATCH_DIR = PROJECT_ROOT / "design" / "development" / "asset_candidates" / "ui" / "runtime_controls" / BATCH_SLUG
PROMPT_PATH = "design/development/agent_prompts/p0_asset_batch_63_runtime_control_panel_candidates.md"


@dataclass(frozen=True)
class PanelSpec:
    subject_id: str
    asset_id: str
    size: tuple[int, int]
    binding_hint: str
    description: str
    accent: tuple[int, int, int, int]
    secondary: tuple[int, int, int, int]


SPECS = (
    PanelSpec(
        "runtime_pause_overlay_panel",
        "thecat_ui_runtime_pause_overlay_panel_768x432_candidate_v001",
        (768, 432),
        "runtime_control.pause_overlay_panel",
        "Pause overlay surface for battle HUD.",
        (255, 224, 120, 255),
        (95, 206, 255, 230),
    ),
    PanelSpec(
        "runtime_speed_segmented_control",
        "thecat_ui_runtime_speed_segmented_control_512x128_candidate_v001",
        (512, 128),
        "runtime_control.speed_segmented_control",
        "Segmented speed selector frame for 0.5x, 1x, and 1.5x.",
        (102, 232, 190, 255),
        (255, 224, 120, 230),
    ),
    PanelSpec(
        "runtime_restart_confirm_plate",
        "thecat_ui_runtime_restart_confirm_plate_512x256_candidate_v001",
        (512, 256),
        "runtime_control.restart_confirm_plate",
        "Restart confirmation plate for run reset affordance.",
        (255, 128, 104, 255),
        (95, 206, 255, 230),
    ),
    PanelSpec(
        "runtime_keyboard_hint_strip",
        "thecat_ui_runtime_keyboard_hint_strip_768x128_candidate_v001",
        (768, 128),
        "runtime_control.keyboard_hint_strip",
        "Keyboard hint strip for pause, speed, and restart shortcuts.",
        (137, 190, 255, 255),
        (255, 224, 120, 220),
    ),
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)
    rows = []
    for spec in SPECS:
        output_path = BATCH_DIR / f"{spec.asset_id}.png"
        image = render_panel(spec)
        image.save(output_path)
        rows.append(
            {
                "subject_id": spec.subject_id,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "runtime_control_panel_candidate",
                "candidate_path": to_project_path(output_path),
                "candidate_sha256": sha256(output_path),
                "candidate_size": f"{spec.size[0]}x{spec.size[1]}",
                "source_references": "thecat_ui_panel_dreamglass_512x256_v001|thecat_ui_button_primary_384x96_v001|batch_62_runtime_control_icon_candidates_2026-06-15|P0RuntimeSettingsPresenter",
                "intended_runtime_binding": spec.binding_hint,
                "recommendation": "candidate_review_only_do_not_import",
                "notes": "non_cat_runtime_control_panel_candidate_no_unity_meta",
            }
        )

    manifest_path = BATCH_DIR / "runtime_control_panels_batch63_manifest.csv"
    write_manifest(manifest_path, rows)
    review_sheet_path = BATCH_DIR / "thecat_ui_runtime_control_panels_batch63_review_sheet.png"
    write_review_sheet(review_sheet_path, rows)
    write_review_note(BATCH_DIR / "runtime_control_panels_batch63_candidate_review.md", rows, review_sheet_path)
    write_process_note(BATCH_DIR / "runtime_control_panels_batch63_process_note.md", rows)
    print(f"Wrote {to_project_path(manifest_path)}")
    print(f"Wrote {to_project_path(review_sheet_path)}")


def render_panel(spec: PanelSpec) -> Image.Image:
    image = Image.new("RGBA", spec.size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    width, height = spec.size

    draw_panel_base(draw, width, height, spec.accent, spec.secondary)

    if spec.subject_id == "runtime_pause_overlay_panel":
        draw_sleep_arc(draw, (width // 2, height // 2), min(width, height) // 3, spec.secondary)
        draw.rounded_rectangle((width // 2 - 48, height // 2 - 72, width // 2 - 20, height // 2 + 72), radius=10, fill=spec.accent)
        draw.rounded_rectangle((width // 2 + 20, height // 2 - 72, width // 2 + 48, height // 2 + 72), radius=10, fill=spec.accent)
        draw_moon_dust(draw, width, height, spec.secondary)
    elif spec.subject_id == "runtime_speed_segmented_control":
        segment_w = (width - 72) // 3
        for index in range(3):
            x1 = 36 + index * segment_w
            x2 = x1 + segment_w - 10
            fill = (34, 31, 48, 220) if index != 1 else (50, 43, 62, 245)
            draw.rounded_rectangle((x1, 28, x2, height - 28), radius=18, fill=fill, outline=(255, 246, 210, 120), width=2)
            draw_chevrons(draw, x1 + segment_w // 2 - 26, height // 2, index + 1, spec.accent, spec.secondary)
    elif spec.subject_id == "runtime_restart_confirm_plate":
        draw.arc((width // 2 - 80, height // 2 - 80, width // 2 + 80, height // 2 + 80), 35, 330, fill=spec.accent, width=14)
        draw.polygon([(width // 2 + 76, height // 2 - 82), (width // 2 + 106, height // 2 - 38), (width // 2 + 54, height // 2 - 42)], fill=spec.accent)
        draw.rounded_rectangle((58, height - 78, width - 58, height - 36), radius=18, fill=(44, 38, 58, 230), outline=spec.secondary, width=2)
        draw_star_line(draw, 94, height - 57, width - 94, spec.secondary)
    elif spec.subject_id == "runtime_keyboard_hint_strip":
        keys = ("Esc", "P", "F1", "F2", "F3", "N")
        key_w = 94
        start_x = (width - (len(keys) * key_w + (len(keys) - 1) * 16)) // 2
        for index, key in enumerate(keys):
            x1 = start_x + index * (key_w + 16)
            draw.rounded_rectangle((x1, 31, x1 + key_w, 91), radius=12, fill=(34, 31, 48, 235), outline=(255, 246, 210, 160), width=2)
            draw.text((x1 + key_w // 2, 61), key, fill=spec.accent, font=font(24, bold=True), anchor="mm")
        draw.line((34, 102, width - 34, 102), fill=spec.secondary, width=3)

    return image


def draw_panel_base(
    draw: ImageDraw.ImageDraw,
    width: int,
    height: int,
    accent: tuple[int, int, int, int],
    secondary: tuple[int, int, int, int],
) -> None:
    draw.rounded_rectangle((6, 6, width - 6, height - 6), radius=24, fill=(28, 25, 40, 235), outline=(255, 246, 210, 130), width=3)
    draw.rounded_rectangle((18, 18, width - 18, height - 18), radius=18, outline=(126, 108, 162, 130), width=2)
    draw.arc((28, 22, width - 28, height - 22), 205, 335, fill=secondary, width=4)
    draw.arc((38, 32, width - 38, height - 32), 25, 155, fill=(255, 246, 210, 85), width=2)
    for x, y, size in ((38, 38, 4), (width - 42, 42, 3), (42, height - 44, 3), (width - 48, height - 46, 4)):
        draw.line((x - size, y, x + size, y), fill=accent, width=1)
        draw.line((x, y - size, x, y + size), fill=accent, width=1)


def draw_sleep_arc(draw: ImageDraw.ImageDraw, center: tuple[int, int], radius: int, color: tuple[int, int, int, int]) -> None:
    x, y = center
    draw.arc((x - radius, y - radius, x + radius, y + radius), 208, 332, fill=color, width=8)
    draw.arc((x - radius + 22, y - radius + 22, x + radius - 22, y + radius - 22), 214, 326, fill=color, width=4)


def draw_moon_dust(draw: ImageDraw.ImageDraw, width: int, height: int, color: tuple[int, int, int, int]) -> None:
    for index in range(11):
        x = 92 + index * ((width - 184) // 10)
        y = height // 2 + (18 if index % 2 == 0 else -18)
        draw.ellipse((x - 4, y - 4, x + 4, y + 4), fill=color)


def draw_chevrons(
    draw: ImageDraw.ImageDraw,
    x: int,
    y: int,
    count: int,
    accent: tuple[int, int, int, int],
    secondary: tuple[int, int, int, int],
) -> None:
    start_x = x - (count - 1) * 15
    for index in range(count):
        px = start_x + index * 30
        draw.polygon([(px, y - 24), (px + 30, y), (px, y + 24), (px + 8, y)], fill=accent)
        draw.line((px + 8, y, px + 30, y), fill=secondary, width=2)


def draw_star_line(draw: ImageDraw.ImageDraw, x1: int, y: int, x2: int, color: tuple[int, int, int, int]) -> None:
    draw.line((x1, y, x2, y), fill=color, width=2)
    for x in range(x1, x2 + 1, 52):
        draw.line((x - 4, y, x + 4, y), fill=color, width=1)
        draw.line((x, y - 4, x, y + 4), fill=color, width=1)


def write_manifest(path: Path, rows: list[dict[str, str]]) -> None:
    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(path: Path, rows: list[dict[str, str]]) -> None:
    width, height = 1920, 1280
    sheet = Image.new("RGB", (width, height), (28, 26, 36))
    draw = ImageDraw.Draw(sheet)
    title_font = font(34, bold=True)
    body_font = font(18)
    meta_font = font(14)
    draw.text((36, 32), "P0 Batch 63 - runtime control panel candidates", fill=(255, 246, 210), font=title_font)
    draw.text(
        (36, 82),
        "Candidate-only non-cat panels for pause, speed selection, restart confirmation, and keyboard hints. No Unity import.",
        fill=(236, 226, 202),
        font=body_font,
    )

    card_w, card_h = 892, 520
    positions = ((36, 132), (992, 132), (36, 692), (992, 692))
    for row, (x, y) in zip(rows, positions):
        draw.rounded_rectangle((x, y, x + card_w, y + card_h), radius=8, fill=(247, 243, 234), outline=(146, 126, 166), width=2)
        draw.text((x + 18, y + 16), row["subject_id"], fill=(42, 37, 45), font=body_font)
        asset = Image.open(PROJECT_ROOT / row["candidate_path"]).convert("RGBA")
        preview = fit_image(asset, (420, 270))
        checker = make_checkerboard(440, 290)
        checker.alpha_composite(preview, ((440 - preview.width) // 2, (290 - preview.height) // 2))
        sheet.paste(checker.convert("RGB"), (x + 18, y + 58))
        draw.rectangle((x + 18, y + 58, x + 457, y + 347), outline=(150, 136, 124), width=1)
        draw_wrapped_text(draw, (x + 484, y + 62), row["asset_id"], meta_font, (64, 54, 70), width=42, line_height=18)
        draw.text((x + 484, y + 142), row["intended_runtime_binding"], fill=(64, 54, 70), font=meta_font)
        draw.text((x + 484, y + 176), row["candidate_size"], fill=(64, 54, 70), font=meta_font)
        draw_wrapped_text(draw, (x + 484, y + 216), row["source_references"], meta_font, (92, 76, 104), width=46, line_height=18)
        draw.text((x + 18, y + 374), row["recommendation"], fill=(92, 76, 104), font=meta_font)
    sheet.save(path)


def fit_image(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    output = image.copy()
    output.thumbnail(size, Image.Resampling.LANCZOS)
    return output


def draw_wrapped_text(
    draw: ImageDraw.ImageDraw,
    xy: tuple[int, int],
    text: str,
    font_obj: ImageFont.FreeTypeFont | ImageFont.ImageFont,
    fill: tuple[int, int, int],
    width: int,
    line_height: int,
) -> None:
    x, y = xy
    for index, line in enumerate(textwrap.wrap(text, width=width, break_long_words=True)):
        draw.text((x, y + index * line_height), line, fill=fill, font=font_obj)


def make_checkerboard(width: int, height: int) -> Image.Image:
    image = Image.new("RGBA", (width, height), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image)
    colors = ((244, 241, 232, 255), (218, 212, 203, 255))
    tile = 16
    for y in range(0, height, tile):
        for x in range(0, width, tile):
            draw.rectangle((x, y, x + tile - 1, y + tile - 1), fill=colors[((x // tile) + (y // tile)) % 2])
    return image


def write_review_note(path: Path, rows: list[dict[str, str]], review_sheet_path: Path) -> None:
    lines = [
        "# P0 Batch 63 - Runtime Control Panel Candidate Review",
        "",
        "## Decision",
        "",
        "- Candidate pack complete pending Unity review.",
        "- Do not import into `Assets` until pause overlay scale, speed selector readability, restart confirmation affordance, keyboard hint readability, Console, and screenshot checks pass.",
        "- Non-cat UI only; no starter-cat body, fur, costume, or colored-turnaround crop is included.",
        "",
        "## Review Sheet",
        "",
        f"- `{to_project_path(review_sheet_path)}`",
        "",
        "## Candidate Rows",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['asset_id']}` -> `{row['candidate_path']}` binding hint `{row['intended_runtime_binding']}`")
    lines.extend(
        [
            "",
            "## Pending Unity Checks",
            "",
            "- Pause overlay does not hide critical four-core values unintentionally.",
            "- Speed segmented control is readable near 0.5x / 1x / 1.5x labels.",
            "- Restart confirmation plate is visually distinct from normal pause controls.",
            "- Keyboard hint strip remains readable at battle HUD scale.",
            "- Console has no missing texture or IMGUI layout errors after any future install.",
        ]
    )
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(path: Path, rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 63 Runtime Control Panel Candidate Process Note",
        "",
        "- Generated deterministic transparent PNG candidates with `build_runtime_control_panel_candidates.py`.",
        "- Source references: Batch 08 UI shell, Batch 62 runtime control icons, runtime settings presenter, and keyboard input map.",
        "- Candidate-only batch. No Unity `.meta` files were created and no runtime catalog count changed.",
        "",
        "## Outputs",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['candidate_path']}` sha256 `{row['candidate_sha256']}`")
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = (
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/segoeuib.ttf" if bold else "C:/Windows/Fonts/segoeui.ttf",
    )
    for candidate in candidates:
        if Path(candidate).exists():
            return ImageFont.truetype(candidate, size)
    return ImageFont.load_default()


def sha256(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def to_project_path(path: Path) -> str:
    return path.relative_to(PROJECT_ROOT).as_posix()


if __name__ == "__main__":
    main()
