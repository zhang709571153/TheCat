from __future__ import annotations

import csv
import hashlib
import math
import textwrap
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


PROJECT_ROOT = Path(__file__).resolve().parents[3]
BATCH_SLUG = "batch_64_secondary_enemy_warning_candidates_2026-06-15"
BATCH_DIR = PROJECT_ROOT / "design" / "development" / "asset_candidates" / "enemies" / "secondary_warning_vfx" / BATCH_SLUG
PROMPT_PATH = "design/development/agent_prompts/p0_asset_batch_64_secondary_enemy_warning_candidates.md"


@dataclass(frozen=True)
class WarningSpec:
    subject_id: str
    display_name: str
    asset_id: str
    binding_hint: str
    source_reference: str
    palette: tuple[tuple[int, int, int, int], tuple[int, int, int, int], tuple[int, int, int, int]]
    notes: str


SPECS = (
    WarningSpec(
        "dream_rail_train_track_warning",
        "Dream Rail Train Track Warning",
        "thecat_vfx_dreamrail_track_warning_256_candidate_v001",
        "warning.dream_rail_track",
        "dream_rail_train_concept|dream_rail_train_animation",
        ((255, 221, 112, 255), (255, 82, 72, 230), (77, 215, 255, 210)),
        "straight toy-track charge warning for line dash readability",
    ),
    WarningSpec(
        "red_eye_alarm_shockring_warning",
        "Red Eye Alarm Shock Ring Warning",
        "thecat_vfx_redeye_alarm_shockring_256_candidate_v001",
        "warning.red_eye_alarm_shock_ring",
        "red_eye_alarm_concept|red_eye_alarm_animation",
        ((255, 74, 70, 255), (255, 212, 100, 230), (98, 218, 255, 180)),
        "area shock ring warning for mid-range stand-position pressure",
    ),
    WarningSpec(
        "unread_red_dot_swarm_warning",
        "Unread Red Dot Swarm Warning",
        "thecat_vfx_unread_swarm_attach_warning_256_candidate_v001",
        "warning.unread_red_dot_swarm_attach",
        "unread_red_dot_event|red_eye_alarm_style_reference",
        ((255, 68, 78, 255), (255, 235, 126, 220), (110, 224, 255, 160)),
        "swarm orbit and attach warning for flying bed-pressure enemies",
    ),
    WarningSpec(
        "falling_dream_teddy_slam_warning",
        "Falling Dream Teddy Slam Warning",
        "thecat_vfx_fallingteddy_slam_marker_256_candidate_v001",
        "warning.falling_dream_teddy_slam",
        "falling_dream_teddy_concept|falling_dream_teddy_animation",
        ((165, 116, 255, 245), (255, 94, 92, 235), (255, 220, 128, 210)),
        "large landing marker for elite jump-slam avoidance",
    ),
)


def main() -> None:
    BATCH_DIR.mkdir(parents=True, exist_ok=True)
    rows: list[dict[str, str]] = []
    for spec in SPECS:
        output_path = BATCH_DIR / f"{spec.asset_id}.png"
        image = render_warning(spec)
        image.save(output_path)
        rows.append(
            {
                "subject_id": spec.subject_id,
                "display_name": spec.display_name,
                "batch_slug": BATCH_SLUG,
                "asset_id": spec.asset_id,
                "asset_type": "secondary_enemy_warning_vfx_candidate",
                "candidate_path": to_project_path(output_path),
                "candidate_sha256": sha256(output_path),
                "candidate_size": "256x256",
                "source_references": spec.source_reference,
                "intended_runtime_binding": spec.binding_hint,
                "recommendation": "candidate_review_only_do_not_import",
                "notes": spec.notes + "; non_cat_warning_vfx_candidate_no_unity_meta",
            }
        )

    manifest_path = BATCH_DIR / "secondary_enemy_warning_batch64_manifest.csv"
    write_manifest(manifest_path, rows)
    review_sheet_path = BATCH_DIR / "thecat_vfx_secondary_enemy_warnings_batch64_review_sheet.png"
    write_review_sheet(review_sheet_path, rows)
    write_review_note(BATCH_DIR / "secondary_enemy_warning_batch64_candidate_review.md", rows, review_sheet_path)
    write_process_note(BATCH_DIR / "secondary_enemy_warning_batch64_process_note.md", rows)
    print(f"Wrote {to_project_path(manifest_path)}")
    print(f"Wrote {to_project_path(review_sheet_path)}")


def render_warning(spec: WarningSpec) -> Image.Image:
    image = Image.new("RGBA", (256, 256), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    accent, danger, cyan = spec.palette
    draw_warning_base(draw, danger, cyan)

    if spec.subject_id == "dream_rail_train_track_warning":
        draw_dream_rail(draw, accent, danger, cyan)
    elif spec.subject_id == "red_eye_alarm_shockring_warning":
        draw_red_eye_alarm(draw, accent, danger, cyan)
    elif spec.subject_id == "unread_red_dot_swarm_warning":
        draw_unread_swarm(draw, accent, danger, cyan)
    elif spec.subject_id == "falling_dream_teddy_slam_warning":
        draw_teddy_slam(draw, accent, danger, cyan)

    return image


def draw_warning_base(draw: ImageDraw.ImageDraw, danger: tuple[int, int, int, int], cyan: tuple[int, int, int, int]) -> None:
    draw.ellipse((28, 28, 228, 228), outline=with_alpha(danger, 70), width=10)
    draw.ellipse((44, 44, 212, 212), outline=with_alpha(cyan, 45), width=4)
    for angle in range(0, 360, 45):
        x1, y1 = point_on_circle(128, 128, 90, angle)
        x2, y2 = point_on_circle(128, 128, 108, angle)
        draw.line((x1, y1, x2, y2), fill=with_alpha(danger, 110), width=5)


def draw_dream_rail(draw: ImageDraw.ImageDraw, accent, danger, cyan) -> None:
    for offset in (-30, 0, 30):
        draw.line((34, 128 + offset, 222, 128 + offset), fill=with_alpha(danger, 130), width=8)
        draw.line((42, 128 + offset, 214, 128 + offset), fill=with_alpha(accent, 190), width=3)
    for x in range(58, 206, 28):
        draw.line((x, 88, x + 18, 168), fill=with_alpha(cyan, 205), width=5)
    draw.polygon(((204, 128), (176, 108), (176, 148)), fill=with_alpha(danger, 215))
    draw.rectangle((54, 108, 94, 148), outline=with_alpha(accent, 230), width=5)


def draw_red_eye_alarm(draw: ImageDraw.ImageDraw, accent, danger, cyan) -> None:
    for size, alpha in ((154, 180), (116, 145), (78, 110)):
        inset = (256 - size) // 2
        draw.ellipse((inset, inset, inset + size, inset + size), outline=with_alpha(danger, alpha), width=7)
    draw.ellipse((89, 90, 167, 166), fill=(42, 33, 52, 215), outline=with_alpha(accent, 220), width=5)
    draw.ellipse((104, 112, 152, 138), fill=with_alpha(danger, 238))
    draw.ellipse((118, 118, 138, 132), fill=(255, 245, 216, 235))
    for angle in (20, 80, 145, 210, 285, 330):
        x1, y1 = point_on_circle(128, 128, 50, angle)
        x2, y2 = point_on_circle(128, 128, 84, angle)
        draw.line((x1, y1, x2, y2), fill=with_alpha(cyan, 170), width=4)


def draw_unread_swarm(draw: ImageDraw.ImageDraw, accent, danger, cyan) -> None:
    draw.arc((48, 52, 208, 204), 25, 340, fill=with_alpha(cyan, 145), width=7)
    draw.arc((64, 68, 192, 188), 200, 515, fill=with_alpha(danger, 120), width=5)
    for index, angle in enumerate(range(0, 360, 30)):
        radius = 70 + (index % 3) * 7
        x, y = point_on_circle(128, 128, radius, angle)
        size = 13 if index % 4 == 0 else 9
        draw.ellipse((x - size, y - size, x + size, y + size), fill=with_alpha(danger, 225), outline=with_alpha(accent, 160), width=2)
    draw.ellipse((92, 92, 164, 164), outline=with_alpha(accent, 130), width=5)
    draw.line((128, 70, 128, 186), fill=with_alpha(cyan, 95), width=3)
    draw.line((70, 128, 186, 128), fill=with_alpha(cyan, 95), width=3)


def draw_teddy_slam(draw: ImageDraw.ImageDraw, accent, danger, cyan) -> None:
    draw.ellipse((42, 156, 214, 206), fill=with_alpha(danger, 58), outline=with_alpha(danger, 205), width=8)
    draw.ellipse((72, 172, 184, 202), outline=with_alpha(accent, 185), width=5)
    draw.polygon(((128, 42), (166, 112), (145, 112), (174, 164), (82, 164), (111, 112), (90, 112)), fill=with_alpha(accent, 155))
    draw.line((128, 46, 128, 164), fill=with_alpha(danger, 220), width=7)
    draw.line((96, 74, 128, 42, 160, 74), fill=with_alpha(cyan, 170), width=5)
    for x in (82, 110, 146, 174):
        draw.line((x, 202, x - 10, 222), fill=with_alpha(danger, 150), width=4)


def point_on_circle(cx: int, cy: int, radius: int, angle_degrees: float) -> tuple[float, float]:
    radians = math.radians(angle_degrees)
    return cx + math.cos(radians) * radius, cy + math.sin(radians) * radius


def with_alpha(color: tuple[int, int, int, int], alpha: int) -> tuple[int, int, int, int]:
    return color[0], color[1], color[2], alpha


def write_manifest(path: Path, rows: list[dict[str, str]]) -> None:
    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


def write_review_sheet(path: Path, rows: list[dict[str, str]]) -> None:
    width, height = 1560, 860
    sheet = Image.new("RGB", (width, height), (29, 27, 38))
    draw = ImageDraw.Draw(sheet)
    title_font = font(34, bold=True)
    body_font = font(18)
    meta_font = font(14)
    draw.text((36, 32), "P0 Batch 64 - secondary enemy warning candidates", fill=(255, 246, 210), font=title_font)
    draw.text(
        (36, 82),
        "Candidate-only non-cat warning VFX for post-core enemy attacks. No Unity import, no runtime binding change.",
        fill=(236, 226, 202),
        font=body_font,
    )

    card_w, card_h = 720, 330
    for index, row in enumerate(rows):
        column = index % 2
        card_row = index // 2
        x = 36 + column * (card_w + 36)
        y = 132 + card_row * (card_h + 36)
        draw.rounded_rectangle((x, y, x + card_w, y + card_h), radius=8, fill=(247, 243, 234), outline=(146, 126, 166), width=2)
        draw.text((x + 18, y + 16), row["display_name"], fill=(42, 37, 45), font=body_font)
        candidate = Image.open(PROJECT_ROOT / row["candidate_path"]).convert("RGBA")
        checker = make_checkerboard(208, 208)
        checker.alpha_composite(candidate.resize((208, 208), Image.Resampling.LANCZOS), (0, 0))
        sheet.paste(checker.convert("RGB"), (x + 18, y + 58))
        draw.rectangle((x + 18, y + 58, x + 225, y + 265), outline=(150, 136, 124), width=1)
        draw_wrapped_text(draw, (x + 250, y + 58), row["asset_id"], meta_font, (64, 54, 70), width=44, line_height=18)
        draw.text((x + 250, y + 132), row["intended_runtime_binding"], fill=(64, 54, 70), font=meta_font)
        draw_wrapped_text(draw, (x + 250, y + 164), row["notes"], meta_font, (64, 54, 70), width=54, line_height=18)
        draw.text((x + 18, y + 286), row["recommendation"], fill=(92, 76, 104), font=meta_font)
    sheet.save(path)


def write_review_note(path: Path, rows: list[dict[str, str]], review_sheet_path: Path) -> None:
    lines = [
        "# P0 Batch 64 - Secondary Enemy Warning Candidate Review",
        "",
        "## Decision",
        "",
        "- Candidate pack complete pending Unity review.",
        "- Do not import into `Assets` until enemy warning readability, Console, and gameplay-scale screenshot checks pass.",
        "- Non-cat warning VFX only; no starter-cat body, fur, costume, or colored-turnaround crop is included.",
        "- Batch 10 remains the installed P0 warning VFX baseline; Batch 64 is for post-core enemy expansion.",
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
            "- Dream Rail track line warning readability before dash.",
            "- Red Eye Alarm shock-ring readability around mid-range enemies.",
            "- Unread Red Dot swarm orbit and attach warning readability near the bed.",
            "- Falling Dream Teddy slam marker readability before jump impact.",
            "- Console has no missing texture, import, or IMGUI layout errors after any future install.",
        ]
    )
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def write_process_note(path: Path, rows: list[dict[str, str]]) -> None:
    lines = [
        "# P0 Batch 64 Secondary Enemy Warning Candidate Process Note",
        "",
        "- Generated deterministic transparent PNG candidates with `build_secondary_enemy_warning_candidates.py`.",
        "- Source references: secondary enemy design docs and existing Batch 10 warning VFX language.",
        "- Candidate-only batch. No Unity `.meta` files were created and no runtime catalog count changed.",
        "",
        "## Outputs",
        "",
    ]
    for row in rows:
        lines.append(f"- `{row['candidate_path']}` sha256 `{row['candidate_sha256']}`")
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


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
