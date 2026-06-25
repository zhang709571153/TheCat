from __future__ import annotations

import csv
import hashlib
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


PROJECT_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_75_chinese_ui_scale_evidence_templates_2026-06-20"
BATCH_DIR = (
    PROJECT_ROOT
    / "design"
    / "development"
    / "asset_candidates"
    / "ui"
    / "chinese_ui_scale_evidence"
    / BATCH_SLUG
)
PROMPT_PATH = "design/development/agent_prompts/p0_asset_batch_75_chinese_ui_scale_evidence_templates.md"


SURFACES = (
    ("main_menu_character_select", "Main Menu / Character Select"),
    ("route_map", "10 Layer Route Map"),
    ("battle_hud", "Bedroom Guard Battle HUD"),
    ("skill_enemy_hud", "Skill / Enemy HUD"),
    ("result_pause_settings", "Result / Pause Settings"),
)

RESOLUTIONS = (
    ("compact_4_3", "1024x768"),
    ("baseline_16_9", "1280x720"),
    ("desktop_16_9", "1600x900"),
    ("wide_1080p", "1920x1080"),
)

ACCEPTANCE_CHECKS = (
    "Chinese-facing text, except necessary HP/shortcut/technical tokens",
    "No panel overlap or crowding",
    "No clipped long text",
    "Long panels remain scrollable",
    "Narrow-width controls stack cleanly",
    "HUD labels remain readable",
    "Console has no errors",
)


@dataclass(frozen=True)
class TemplateSpec:
    subject_id: str
    asset_id: str
    size: tuple[int, int]
    asset_type: str
    description: str
    intended_use: str


SPECS = (
    TemplateSpec(
        "ui_scale_capture_matrix_sheet",
        "thecat_ui_chinese_scale_capture_matrix_batch75_1920x1080_v001",
        (1920, 1080),
        "ui_validation_evidence_template",
        "Full screenshot matrix checklist for P0 Chinese UI responsive review.",
        "validation.capture_matrix",
    ),
    TemplateSpec(
        "ui_scale_safe_area_overlay",
        "thecat_ui_chinese_scale_safe_area_overlay_batch75_1920x1080_v001",
        (1920, 1080),
        "ui_validation_safe_area_overlay",
        "Transparent safe-area and overlap ruler for comparing Unity screenshots.",
        "validation.safe_area_overlay",
    ),
    TemplateSpec(
        "ui_scale_surface_note_card",
        "thecat_ui_chinese_scale_surface_note_card_batch75_1280x720_v001",
        (1280, 720),
        "ui_validation_note_card",
        "Per-surface evidence note card for screenshot, Console, and reviewer notes.",
        "validation.surface_note_card",
    ),
    TemplateSpec(
        "ui_scale_resolution_strip",
        "thecat_ui_chinese_scale_resolution_strip_batch75_1600x320_v001",
        (1600, 320),
        "ui_validation_resolution_strip",
        "Resolution checklist strip for the four required UI scale captures.",
        "validation.resolution_strip",
    ),
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)

    rows: list[dict[str, str]] = []
    for spec in SPECS:
        output_path = BATCH_DIR / f"{spec.asset_id}.png"
        image = render_template(spec)
        image.save(output_path)
        rows.append(
            {
                "subject_id": spec.subject_id,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": spec.asset_type,
                "template_path": to_project_path(output_path),
                "template_sha256": sha256(output_path),
                "template_size": f"{spec.size[0]}x{spec.size[1]}",
                "source_references": "P0ChineseUiScaleValidationPlan|P0ChineseUiCoverage|P0ImGuiLayout|UNITY_VALIDATION_BACKLOG item 188",
                "intended_use": spec.intended_use,
                "recommendation": "validation_template_only_do_not_import",
                "notes": "non_cat_ui_validation_template_no_runtime_binding",
            }
        )

    manifest_path = BATCH_DIR / "chinese_ui_scale_batch75_manifest.csv"
    write_manifest(manifest_path, rows)

    capture_matrix_path = BATCH_DIR / "chinese_ui_scale_batch75_capture_matrix.csv"
    write_capture_matrix(capture_matrix_path)

    review_sheet_path = BATCH_DIR / "thecat_ui_chinese_scale_batch75_review_sheet.png"
    write_review_sheet(review_sheet_path, rows)

    write_review_note(
        BATCH_DIR / "chinese_ui_scale_batch75_candidate_review.md",
        rows,
        review_sheet_path,
        capture_matrix_path,
    )
    write_process_note(BATCH_DIR / "chinese_ui_scale_batch75_process_note.md", rows)

    print(f"Wrote {to_project_path(manifest_path)}")
    print(f"Wrote {to_project_path(review_sheet_path)}")
    print(f"Wrote {to_project_path(capture_matrix_path)}")


def render_template(spec: TemplateSpec) -> Image.Image:
    if spec.subject_id == "ui_scale_safe_area_overlay":
        return render_safe_area_overlay(spec.size)
    if spec.subject_id == "ui_scale_surface_note_card":
        return render_surface_note_card(spec.size)
    if spec.subject_id == "ui_scale_resolution_strip":
        return render_resolution_strip(spec.size)
    return render_capture_matrix_sheet(spec.size)


def render_capture_matrix_sheet(size: tuple[int, int]) -> Image.Image:
    width, height = size
    image = Image.new("RGB", size, (24, 23, 31))
    draw = ImageDraw.Draw(image)
    title = font(42, bold=True)
    body = font(22)
    small = font(17)

    draw.text((48, 42), "P0 Chinese UI Responsive Validation Matrix", fill=(255, 244, 212), font=title)
    draw.text(
        (48, 102),
        "Capture every P0 UI surface at the four required resolutions before final visual acceptance.",
        fill=(217, 222, 235),
        font=body,
    )

    grid_x = 48
    grid_y = 184
    label_w = 400
    cell_w = 342
    cell_h = 118
    header_h = 72

    draw.rounded_rectangle(
        (grid_x, grid_y, grid_x + label_w + cell_w * len(RESOLUTIONS), grid_y + header_h + cell_h * len(SURFACES)),
        radius=10,
        fill=(35, 32, 48),
        outline=(139, 162, 205),
        width=2,
    )

    draw.text((grid_x + 22, grid_y + 22), "Surface", fill=(255, 244, 212), font=body)
    for index, (_, resolution) in enumerate(RESOLUTIONS):
        x = grid_x + label_w + index * cell_w
        draw.text((x + 22, grid_y + 22), resolution, fill=(255, 244, 212), font=body)

    for row_index, (surface_id, surface_name) in enumerate(SURFACES):
        y = grid_y + header_h + row_index * cell_h
        draw.line((grid_x, y, grid_x + label_w + cell_w * len(RESOLUTIONS), y), fill=(90, 92, 118), width=2)
        draw.text((grid_x + 22, y + 22), surface_name, fill=(237, 238, 246), font=body)
        draw.text((grid_x + 22, y + 56), surface_id, fill=(163, 176, 205), font=small)
        for col_index, _ in enumerate(RESOLUTIONS):
            x = grid_x + label_w + col_index * cell_w
            draw.line((x, grid_y, x, grid_y + header_h + cell_h * len(SURFACES)), fill=(90, 92, 118), width=2)
            draw.rounded_rectangle((x + 22, y + 26, x + 64, y + 68), radius=8, outline=(122, 232, 199), width=3)
            draw.text((x + 78, y + 26), "shot", fill=(217, 222, 235), font=small)
            draw.text((x + 78, y + 52), "console", fill=(217, 222, 235), font=small)

    checklist_x = 48
    checklist_y = 900
    draw.text((checklist_x, checklist_y), "Acceptance checks", fill=(255, 244, 212), font=body)
    for index, item in enumerate(ACCEPTANCE_CHECKS):
        x = checklist_x + (index % 2) * 890
        y = checklist_y + 44 + (index // 2) * 34
        draw.rounded_rectangle((x, y + 2, x + 24, y + 26), radius=5, outline=(255, 198, 108), width=2)
        draw.text((x + 36, y), item, fill=(217, 222, 235), font=small)

    return image


def render_safe_area_overlay(size: tuple[int, int]) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")

    margin = 48
    panel_w = int(width * 0.38)
    draw.rectangle((margin, margin, width - margin, height - margin), outline=(102, 232, 190, 180), width=4)
    draw.rectangle((margin, margin, panel_w, height - margin), outline=(255, 224, 120, 190), width=4)
    draw.line((width // 2, margin, width // 2, height - margin), fill=(137, 190, 255, 130), width=3)
    draw.line((margin, height // 2, width - margin, height // 2), fill=(137, 190, 255, 130), width=3)

    for x in range(margin, width - margin + 1, 160):
        draw.line((x, margin, x, height - margin), fill=(255, 255, 255, 34), width=1)
    for y in range(margin, height - margin + 1, 120):
        draw.line((margin, y, width - margin, y), fill=(255, 255, 255, 34), width=1)

    label_font = font(28, bold=True)
    draw.rounded_rectangle((66, 66, 520, 122), radius=14, fill=(24, 23, 31, 190))
    draw.text((86, 80), "Safe UI / scroll / stack ruler", fill=(255, 244, 212, 240), font=label_font)
    return image


def render_surface_note_card(size: tuple[int, int]) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    title = font(38, bold=True)
    body = font(24)
    small = font(18)

    draw.rounded_rectangle((26, 26, width - 26, height - 26), radius=28, fill=(31, 29, 43, 238), outline=(255, 244, 212, 145), width=4)
    draw.rounded_rectangle((56, 88, width - 56, 168), radius=18, fill=(47, 43, 62, 235), outline=(137, 190, 255, 150), width=2)
    draw.text((72, 48), "P0 UI Scale Evidence Note", fill=(255, 244, 212, 255), font=title)
    draw.text((78, 112), "surface:", fill=(217, 222, 235, 255), font=body)
    draw.text((420, 112), "resolution:", fill=(217, 222, 235, 255), font=body)
    draw.text((800, 112), "result:", fill=(217, 222, 235, 255), font=body)

    y = 218
    for item in ACCEPTANCE_CHECKS:
        draw.rounded_rectangle((72, y, 112, y + 40), radius=8, outline=(102, 232, 190, 200), width=3)
        draw.text((132, y + 4), item, fill=(226, 230, 242, 255), font=small)
        y += 54

    draw.rounded_rectangle((72, height - 132, width - 72, height - 64), radius=16, fill=(47, 43, 62, 220), outline=(255, 224, 120, 150), width=2)
    draw.text((92, height - 111), "Console / reviewer notes:", fill=(255, 244, 212, 255), font=body)
    return image


def render_resolution_strip(size: tuple[int, int]) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    title = font(34, bold=True)
    body = font(28)
    small = font(17)

    draw.rounded_rectangle((16, 16, width - 16, height - 16), radius=30, fill=(31, 29, 43, 238), outline=(137, 190, 255, 140), width=3)
    draw.text((46, 42), "Required UI scale captures", fill=(255, 244, 212, 255), font=title)
    card_w = 340
    start_x = 46
    for index, (res_id, resolution) in enumerate(RESOLUTIONS):
        x = start_x + index * (card_w + 36)
        y = 116
        accent = (102, 232, 190, 230) if index % 2 == 0 else (255, 224, 120, 230)
        draw.rounded_rectangle((x, y, x + card_w, y + 154), radius=18, fill=(47, 43, 62, 230), outline=accent, width=3)
        draw.text((x + 28, y + 28), resolution, fill=(255, 244, 212, 255), font=body)
        draw.text((x + 28, y + 76), res_id, fill=(217, 222, 235, 255), font=small)
        draw.rounded_rectangle((x + card_w - 64, y + 42, x + card_w - 28, y + 78), radius=8, outline=accent, width=3)
    return image


def write_manifest(path: Path, rows: list[dict[str, str]]) -> None:
    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_capture_matrix(path: Path) -> None:
    rows: list[dict[str, str]] = []
    for surface_id, surface_name in SURFACES:
        for resolution_id, resolution in RESOLUTIONS:
            rows.append(
                {
                    "surface_id": surface_id,
                    "surface_name": surface_name,
                    "resolution_id": resolution_id,
                    "resolution": resolution,
                    "required_checks": "; ".join(ACCEPTANCE_CHECKS),
                    "unity_evidence_status": "pending_unity_mcp_or_editor_capture",
                    "screenshot_path": "",
                    "console_note": "",
                }
            )

    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(path: Path, rows: list[dict[str, str]]) -> None:
    width, height = 1920, 1080
    sheet = Image.new("RGB", (width, height), (24, 23, 31))
    draw = ImageDraw.Draw(sheet)
    title = font(38, bold=True)
    body = font(20)
    small = font(15)
    draw.text((42, 34), "P0 Batch 75 - Chinese UI scale evidence templates", fill=(255, 244, 212), font=title)
    draw.text(
        (42, 86),
        "Validation-only non-cat templates for screenshot capture, safe-area review, and Console evidence. Do not import into Assets.",
        fill=(217, 222, 235),
        font=body,
    )

    positions = ((42, 146), (980, 146), (42, 606), (980, 606))
    card_w, card_h = 880, 390
    for row, (x, y) in zip(rows, positions):
        draw.rounded_rectangle((x, y, x + card_w, y + card_h), radius=10, fill=(247, 243, 234), outline=(146, 126, 166), width=2)
        draw.text((x + 18, y + 16), row["subject_id"], fill=(42, 37, 45), font=body)
        draw.text((x + 18, y + 46), row["template_size"], fill=(91, 80, 102), font=small)
        asset = Image.open(PROJECT_ROOT / row["template_path"]).convert("RGBA")
        preview = fit_image(asset, (card_w - 48, 250))
        checker = make_checkerboard(card_w - 48, 250)
        checker.alpha_composite(preview, ((card_w - 48 - preview.width) // 2, (250 - preview.height) // 2))
        sheet.paste(checker.convert("RGB"), (x + 24, y + 82))
        draw.text((x + 18, y + 346), row["intended_use"], fill=(42, 37, 45), font=small)

    return sheet.save(path)


def write_review_note(
    path: Path,
    rows: list[dict[str, str]],
    review_sheet_path: Path,
    capture_matrix_path: Path,
) -> None:
    lines = [
        "# P0 Batch 75 - Chinese UI Scale Evidence Template Review",
        "",
        "## Decision",
        "",
        "- Validation template only pending Unity review.",
        "- Do not import into `Assets`; these files are evidence templates, not runtime UI art.",
        "- Non-cat UI validation only; no starter-cat body, fur, costume, prop, or colored-turnaround crop is included.",
        "- Unity MCP is currently required for final screenshots, but manual Unity Editor capture may fill the matrix while MCP approval is revoked.",
        "",
        "## Review Sheet",
        "",
        f"- `{to_project_path(review_sheet_path)}`",
        "",
        "## Capture Matrix",
        "",
        f"- `{to_project_path(capture_matrix_path)}`",
        "- Required resolutions: `1024x768`, `1280x720`, `1600x900`, `1920x1080`.",
        "- Required surfaces: main menu / character select, route map, battle HUD, skill/enemy HUD, result / pause settings.",
        "",
        "## Template Rows",
        "",
    ]

    for row in rows:
        lines.append(f"- `{row['asset_id']}` -> `{row['template_path']}` use `{row['intended_use']}`")

    lines.extend(
        [
            "",
            "## Pending Unity Checks",
            "",
            "- Capture all 20 surface/resolution screenshots.",
            "- Verify Chinese-facing text, except necessary HP/shortcut/technical tokens.",
            "- Verify no overlap, no clipping, scrollable long panels, narrow stacking, and readable HUD labels.",
            "- Record Console status for every reviewed surface.",
            "- Keep this batch outside runtime catalogs unless it is explicitly repurposed as tooling art.",
        ]
    )
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(path: Path, rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 75 Chinese UI Scale Evidence Template Process Note",
        "",
        f"- Batch slug: `{BATCH_SLUG}`",
        f"- Output directory: `{to_project_path(BATCH_DIR)}`",
        "- Production method: deterministic Pillow generation from the P0 Chinese UI scale validation plan.",
        "- Template count: " + str(len(rows)),
        "- Capture matrix rows: 20 surface/resolution pairs.",
        "- Candidate-only status: true; validation templates only.",
        "- Unity import status: blocked; do not import into `Assets`.",
        "- Cat consistency impact: none. This batch does not read, generate, crop, recolor, or route starter-cat body art.",
        "- No Unity `.meta` files should exist in this candidate folder.",
        "- Follow-up: fill the matrix from Unity Play Mode once MCP approval or manual Editor screenshots are available.",
    ]
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def fit_image(image: Image.Image, box: tuple[int, int]) -> Image.Image:
    max_w, max_h = box
    ratio = min(max_w / image.width, max_h / image.height)
    return image.resize((max(1, int(image.width * ratio)), max(1, int(image.height * ratio))), Image.Resampling.LANCZOS)


def make_checkerboard(width: int, height: int) -> Image.Image:
    image = Image.new("RGBA", (width, height), (238, 235, 229, 255))
    draw = ImageDraw.Draw(image)
    cell = 24
    for y in range(0, height, cell):
        for x in range(0, width, cell):
            if (x // cell + y // cell) % 2 == 0:
                draw.rectangle((x, y, x + cell - 1, y + cell - 1), fill=(210, 206, 200, 255))
    return image


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = (
        "C:/Windows/Fonts/msyhbd.ttc" if bold else "C:/Windows/Fonts/msyh.ttc",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    )
    for candidate in candidates:
        try:
            return ImageFont.truetype(candidate, size)
        except OSError:
            continue
    return ImageFont.load_default()


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for chunk in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def to_project_path(path: Path) -> str:
    return path.resolve().relative_to(PROJECT_ROOT).as_posix()


if __name__ == "__main__":
    main()
