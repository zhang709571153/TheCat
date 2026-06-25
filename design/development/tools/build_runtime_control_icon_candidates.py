from __future__ import annotations

import csv
import hashlib
import textwrap
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


PROJECT_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_62_runtime_control_icon_candidates_2026-06-15"
BATCH_DIR = PROJECT_ROOT / "design" / "development" / "asset_candidates" / "ui" / "runtime_controls" / BATCH_SLUG
PROMPT_PATH = "design/development/agent_prompts/p0_asset_batch_62_runtime_control_icon_candidates.md"


@dataclass(frozen=True)
class IconSpec:
    subject_id: str
    asset_id: str
    binding_hint: str
    description: str
    accent: tuple[int, int, int, int]
    secondary: tuple[int, int, int, int]


SPECS = (
    IconSpec(
        "runtime_pause_icon",
        "thecat_ui_runtime_pause_icon_128_candidate_v001",
        "runtime_control.pause",
        "Pause battle updates; keyboard P/Esc.",
        (255, 224, 120, 255),
        (95, 206, 255, 230),
    ),
    IconSpec(
        "runtime_resume_icon",
        "thecat_ui_runtime_resume_icon_128_candidate_v001",
        "runtime_control.resume",
        "Resume battle updates after pause.",
        (102, 232, 190, 255),
        (255, 224, 120, 230),
    ),
    IconSpec(
        "runtime_speed_half_icon",
        "thecat_ui_runtime_speed_half_icon_128_candidate_v001",
        "runtime_control.speed_half",
        "Set battle speed to 0.5x via F1.",
        (137, 190, 255, 255),
        (255, 224, 120, 220),
    ),
    IconSpec(
        "runtime_speed_normal_icon",
        "thecat_ui_runtime_speed_normal_icon_128_candidate_v001",
        "runtime_control.speed_normal",
        "Set battle speed to 1x via F2.",
        (102, 232, 190, 255),
        (255, 224, 120, 220),
    ),
    IconSpec(
        "runtime_speed_fast_icon",
        "thecat_ui_runtime_speed_fast_icon_128_candidate_v001",
        "runtime_control.speed_fast",
        "Set battle speed to 1.5x via F3.",
        (255, 128, 104, 255),
        (95, 206, 255, 230),
    ),
    IconSpec(
        "runtime_restart_icon",
        "thecat_ui_runtime_restart_icon_128_candidate_v001",
        "runtime_control.restart_run",
        "Restart the current run; keyboard N.",
        (255, 224, 120, 255),
        (102, 232, 190, 230),
    ),
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)
    rows = []
    for spec in SPECS:
        output_path = BATCH_DIR / f"{spec.asset_id}.png"
        image = render_icon(spec)
        image.save(output_path)
        rows.append(
            {
                "subject_id": spec.subject_id,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "runtime_control_icon_candidate",
                "candidate_path": to_project_path(output_path),
                "candidate_sha256": sha256(output_path),
                "candidate_size": "128x128",
                "source_references": "thecat_ui_button_primary_384x96_v001|thecat_ui_panel_dreamglass_512x256_v001|P0RuntimeSettingsPresenter|P0KeyboardInputMap",
                "intended_runtime_binding": spec.binding_hint,
                "recommendation": "candidate_review_only_do_not_import",
                "notes": "non_cat_runtime_control_ui_icon_candidate_no_unity_meta",
            }
        )

    manifest_path = BATCH_DIR / "runtime_control_icons_batch62_manifest.csv"
    write_manifest(manifest_path, rows)
    review_sheet_path = BATCH_DIR / "thecat_ui_runtime_controls_batch62_review_sheet.png"
    write_review_sheet(review_sheet_path, rows)
    write_review_note(BATCH_DIR / "runtime_control_icons_batch62_candidate_review.md", rows, review_sheet_path)
    write_process_note(BATCH_DIR / "runtime_control_icons_batch62_process_note.md", rows)
    print(f"Wrote {to_project_path(manifest_path)}")
    print(f"Wrote {to_project_path(review_sheet_path)}")


def render_icon(spec: IconSpec) -> Image.Image:
    image = Image.new("RGBA", (128, 128), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    draw_soft_disc(draw, (64, 64), 54, (30, 26, 45, 230), (130, 112, 170, 160))
    draw.rounded_rectangle((18, 18, 110, 110), radius=24, outline=(255, 246, 210, 170), width=3)
    draw.arc((28, 28, 100, 100), 200, 340, fill=spec.secondary, width=4)
    draw.arc((32, 32, 96, 96), 20, 160, fill=(255, 246, 210, 110), width=2)

    if spec.subject_id == "runtime_pause_icon":
        draw.rounded_rectangle((47, 39, 57, 89), radius=4, fill=spec.accent)
        draw.rounded_rectangle((71, 39, 81, 89), radius=4, fill=spec.accent)
        draw_crescent(draw, (64, 64), 38, spec.secondary)
    elif spec.subject_id == "runtime_resume_icon":
        draw.polygon([(50, 38), (50, 90), (88, 64)], fill=spec.accent)
        draw_crescent(draw, (62, 64), 38, spec.secondary)
    elif spec.subject_id == "runtime_speed_half_icon":
        draw_chevrons(draw, 1, spec.accent, spec.secondary)
        draw.ellipse((36, 57, 46, 67), fill=spec.secondary)
    elif spec.subject_id == "runtime_speed_normal_icon":
        draw_chevrons(draw, 2, spec.accent, spec.secondary)
    elif spec.subject_id == "runtime_speed_fast_icon":
        draw_chevrons(draw, 3, spec.accent, spec.secondary)
        draw.arc((25, 25, 103, 103), 210, 315, fill=spec.secondary, width=5)
    elif spec.subject_id == "runtime_restart_icon":
        draw.arc((33, 32, 95, 94), 35, 330, fill=spec.accent, width=7)
        draw.polygon([(92, 33), (102, 51), (81, 49)], fill=spec.accent)
        draw.ellipse((55, 55, 73, 73), fill=spec.secondary)

    draw_stars(draw, spec.secondary)
    return image


def draw_soft_disc(
    draw: ImageDraw.ImageDraw,
    center: tuple[int, int],
    radius: int,
    fill: tuple[int, int, int, int],
    outline: tuple[int, int, int, int],
) -> None:
    x, y = center
    draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=fill)
    for inset in (0, 5, 10):
        alpha = max(30, outline[3] - inset * 8)
        draw.ellipse(
            (x - radius + inset, y - radius + inset, x + radius - inset, y + radius - inset),
            outline=(outline[0], outline[1], outline[2], alpha),
            width=2,
        )


def draw_crescent(
    draw: ImageDraw.ImageDraw,
    center: tuple[int, int],
    radius: int,
    color: tuple[int, int, int, int],
) -> None:
    x, y = center
    draw.arc((x - radius, y - radius, x + radius, y + radius), 225, 315, fill=color, width=5)
    draw.arc((x - radius + 8, y - radius + 8, x + radius - 8, y + radius - 8), 225, 315, fill=color, width=2)


def draw_chevrons(
    draw: ImageDraw.ImageDraw,
    count: int,
    accent: tuple[int, int, int, int],
    secondary: tuple[int, int, int, int],
) -> None:
    start_x = 43 - (count - 1) * 10
    for index in range(count):
        x = start_x + index * 19
        draw.polygon([(x, 42), (x + 25, 64), (x, 86), (x + 7, 64)], fill=accent)
        draw.line([(x + 7, 64), (x + 25, 64)], fill=secondary, width=2)


def draw_stars(draw: ImageDraw.ImageDraw, color: tuple[int, int, int, int]) -> None:
    for x, y, size in ((34, 35, 3), (94, 34, 2), (32, 91, 2), (96, 91, 3)):
        draw.line((x - size, y, x + size, y), fill=color, width=1)
        draw.line((x, y - size, x, y + size), fill=color, width=1)


def write_manifest(path: Path, rows: list[dict[str, str]]) -> None:
    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(path: Path, rows: list[dict[str, str]]) -> None:
    width, height = 1820, 860
    sheet = Image.new("RGB", (width, height), (28, 26, 36))
    draw = ImageDraw.Draw(sheet)
    title_font = font(34, bold=True)
    body_font = font(18)
    meta_font = font(14)
    draw.text((36, 32), "P0 Batch 62 - runtime control icon candidates", fill=(255, 246, 210), font=title_font)
    draw.text(
        (36, 82),
        "Candidate-only non-cat controls for pause, resume, speed, and restart. No Unity import or cat-body art.",
        fill=(236, 226, 202),
        font=body_font,
    )

    card_w, card_h = 560, 330
    gap = 28
    for index, row in enumerate(rows):
        column = index % 3
        card_row = index // 3
        x = 36 + column * (card_w + gap)
        y = 132 + card_row * (card_h + gap)
        draw.rounded_rectangle((x, y, x + card_w, y + card_h), radius=8, fill=(247, 243, 234), outline=(146, 126, 166), width=2)
        draw.text((x + 18, y + 16), row["subject_id"], fill=(42, 37, 45), font=body_font)
        icon = Image.open(PROJECT_ROOT / row["candidate_path"]).convert("RGBA")
        checker = make_checkerboard(176, 176)
        checker.alpha_composite(icon.resize((176, 176), Image.Resampling.LANCZOS), (24, 24))
        sheet.paste(checker.convert("RGB"), (x + 18, y + 54))
        draw.rectangle((x + 18, y + 54, x + 193, y + 229), outline=(150, 136, 124), width=1)
        draw_wrapped_text(draw, (x + 218, y + 56), row["asset_id"], meta_font, (64, 54, 70), width=36, line_height=18)
        draw.text((x + 218, y + 114), row["intended_runtime_binding"], fill=(64, 54, 70), font=meta_font)
        draw.text((x + 218, y + 148), row["candidate_size"], fill=(64, 54, 70), font=meta_font)
        draw.text((x + 18, y + 248), row["recommendation"], fill=(92, 76, 104), font=meta_font)
    sheet.save(path)


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
        "# P0 Batch 62 - Runtime Control Icon Candidate Review",
        "",
        "## Decision",
        "",
        "- Candidate pack complete pending Unity review.",
        "- Do not import into `Assets` until runtime settings HUD scale, Console, and screenshot checks pass.",
        "- Non-cat UI only; no starter-cat body, fur, costume, or turnaround crop is included.",
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
            "- Pause/resume control readability in battle HUD.",
            "- Speed half, normal, and fast control readability beside F1/F2/F3 labels.",
            "- Restart affordance readability wherever the N shortcut is surfaced.",
            "- Console has no missing texture or IMGUI layout errors.",
        ]
    )
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(path: Path, rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 62 Runtime Control Icon Candidate Process Note",
        "",
        "- Generated deterministic transparent PNG candidates with `build_runtime_control_icon_candidates.py`.",
        "- Source references: Batch 08 UI shell, runtime settings presenter, and keyboard input map.",
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
