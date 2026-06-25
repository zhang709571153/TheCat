from __future__ import annotations

import argparse
import csv
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


VERSION = "v002_light"
STATES = ["ready", "selected", "cooldown_99", "disabled"]
FOCUS_SET = [
    ("saiban", "shield_barrier", "selected-vs-ready cyan shield"),
    ("saiban", "battle_flag_rally", "red/gold density watch"),
    ("nephthys", "sandstorm_swirl", "gold swirl contrast"),
    ("nephthys", "sand_tornado_column", "dense motion plus cooldown digit"),
    ("suzune", "ice_talisman_guard", "cyan selected-vs-ready"),
    ("suzune", "team_heal_ice_enchant", "lightframe replacement watch"),
]


def read_csv(path: Path) -> list[dict[str, str]]:
    with path.open("r", newline="", encoding="utf-8") as handle:
        return list(csv.DictReader(handle))


def load_font(size: int, bold: bool = False) -> ImageFont.ImageFont:
    candidates = [
        "arialbd.ttf" if bold else "arial.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
        "C:/Windows/Fonts/msyhbd.ttc" if bold else "C:/Windows/Fonts/msyh.ttc",
    ]
    for candidate in candidates:
        try:
            return ImageFont.truetype(candidate, size)
        except OSError:
            continue
    return ImageFont.load_default()


def fit(image: Image.Image, max_size: int) -> Image.Image:
    image = image.convert("RGBA")
    scale = min(max_size / image.width, max_size / image.height)
    size = (max(1, round(image.width * scale)), max(1, round(image.height * scale)))
    return image.resize(size, Image.Resampling.LANCZOS)


def paste_center(base: Image.Image, overlay: Image.Image, center: tuple[int, int]) -> None:
    base.alpha_composite(overlay, (center[0] - overlay.width // 2, center[1] - overlay.height // 2))


def draw_digit(image: Image.Image, digit: str, size: int) -> None:
    draw = ImageDraw.Draw(image)
    font_size = 12 if size == 64 else 20
    font = load_font(font_size, bold=True)
    center = (round(size * 0.80), round(size * 0.20))
    bbox = draw.textbbox((0, 0), digit, font=font, stroke_width=1)
    width = bbox[2] - bbox[0]
    height = bbox[3] - bbox[1]
    position = (center[0] - width / 2, center[1] - height / 2 - 1)
    draw.text(position, digit, font=font, fill=(255, 239, 168, 255), stroke_width=1, stroke_fill=(19, 28, 47, 255))


def compose_slot(frame_path: Path, icon_path: Path, state: str, size: int) -> Image.Image:
    frame = Image.open(frame_path).convert("RGBA").resize((size, size), Image.Resampling.LANCZOS)
    icon = fit(Image.open(icon_path).convert("RGBA"), round(size * 0.68))
    if state == "disabled":
        alpha = icon.getchannel("A").point(lambda value: round(value * 0.55))
        icon.putalpha(alpha)
    paste_center(frame, icon, (size // 2, round(size * 0.52)))
    if state == "cooldown_99":
        draw_digit(frame, "99", size)
    return frame


def draw_background(size: tuple[int, int], title: str) -> Image.Image:
    width, height = size
    image = Image.new("RGBA", size, (10, 16, 28, 255))
    draw = ImageDraw.Draw(image)
    draw.rectangle((0, 0, width, 72), fill=(25, 38, 61, 255))
    title_size = 18 if width < 900 else 22
    draw.text((32, 22), title, fill=(226, 238, 255, 255), font=load_font(title_size, bold=True))
    draw.rectangle((0, height - 150, width, height), fill=(13, 22, 38, 238))
    for x in range(0, width, 96):
        color = (24, 88, 128, 34) if (x // 96) % 2 == 0 else (120, 92, 38, 28)
        draw.line((x, 72, max(0, x - height // 3), height), fill=color, width=2)
    return image


def add_status_gauges(image: Image.Image) -> int:
    draw = ImageDraw.Draw(image)
    width, _ = image.size
    labels = [("SLEEP", (108, 191, 255)), ("HP", (105, 232, 160)), ("POOP", (234, 181, 92)), ("HUNGER", (245, 213, 108))]
    gauge_w = 240
    gauge_h = 34
    margin = 36
    gap = 28

    if width < 900:
        for index, (label, color) in enumerate(labels):
            col = index % 2
            row = index // 2
            x = margin + col * (gauge_w + gap)
            y = 94 + row * 46
            draw.rounded_rectangle((x, y, x + gauge_w, y + gauge_h), radius=10, fill=(18, 30, 48, 230), outline=(64, 104, 132, 255), width=1)
            draw.rectangle((x + 76, y + 11, x + 218, y + 23), fill=(29, 43, 68, 255))
            draw.rectangle((x + 76, y + 11, x + 172, y + 23), fill=color + (255,))
            draw.text((x + 12, y + 9), label, fill=(226, 238, 255, 255), font=load_font(12, bold=True))
        pause_y = 186
        draw.rounded_rectangle((width - 168, pause_y, width - 36, pause_y + 38), radius=10, fill=(18, 30, 48, 230), outline=(86, 125, 154, 255), width=1)
        draw.text((width - 136, pause_y + 12), "PAUSE", fill=(226, 238, 255, 255), font=load_font(13, bold=True))
        return pause_y + 38

    x = margin
    for label, color in labels:
        draw.rounded_rectangle((x, 94, x + gauge_w, 128), radius=10, fill=(18, 30, 48, 230), outline=(64, 104, 132, 255), width=1)
        draw.rectangle((x + 76, 105, x + 218, 117), fill=(29, 43, 68, 255))
        draw.rectangle((x + 76, 105, x + 172, 117), fill=color + (255,))
        draw.text((x + 12, 103), label, fill=(226, 238, 255, 255), font=load_font(12, bold=True))
        x += gauge_w + gap
    draw.rounded_rectangle((width - 168, 92, width - 36, 130), radius=10, fill=(18, 30, 48, 230), outline=(86, 125, 154, 255), width=1)
    draw.text((width - 136, 104), "PAUSE", fill=(226, 238, 255, 255), font=load_font(13, bold=True))
    return 130


def ellipsize(text: str, max_chars: int) -> str:
    if len(text) <= max_chars:
        return text
    return text[: max(1, max_chars - 3)] + "..."


def make_focus_matrix(slot_rows: dict[tuple[str, str, str], Path], icon_rows: dict[tuple[str, str, str], Path], out_path: Path, size: tuple[int, int], slot_size: int) -> None:
    image = draw_background(size, f"Batch 81 {VERSION} local HUD mockup - focus set at {slot_size}px")
    gauge_bottom = add_status_gauges(image)
    draw = ImageDraw.Draw(image)
    label_font = load_font(12 if size[0] < 900 else 13)
    header_font = load_font(12, bold=True)
    width, height = size
    margin_x = 48 if width < 900 else 52
    label_w = 152 if width < 900 else 300
    grid_w = width - margin_x * 2 - label_w
    cell_w = max(slot_size + 42, grid_w // 4)
    start_y = gauge_bottom + (42 if width < 900 else 40)
    row_h = max(slot_size + 12, (height - start_y - 64) // len(FOCUS_SET))
    state_header_y = start_y - 25
    for col_index, state in enumerate(STATES):
        x = margin_x + label_w + col_index * cell_w + max(0, (cell_w - slot_size) // 2)
        draw.text((x, state_header_y), state, fill=(174, 202, 228, 255), font=header_font)
    for row_index, (cat, skill, _) in enumerate(FOCUS_SET):
        icon_path = icon_rows[(cat, skill, f"{slot_size}x{slot_size}")]
        y = start_y + row_index * row_h
        label = ellipsize(f"{cat}/{skill}", 22 if width < 900 else 38)
        draw.text((margin_x, y + max(0, slot_size // 2 - 8)), label, fill=(226, 238, 255, 255), font=label_font)
        for col_index, state in enumerate(STATES):
            frame_state = "cooldown" if state == "cooldown_99" else state
            slot = compose_slot(slot_rows[(frame_state, f"{slot_size}x{slot_size}")], icon_path, state, slot_size)
            x = margin_x + label_w + col_index * cell_w + max(0, (cell_w - slot_size) // 2)
            image.alpha_composite(slot, (x, y))
    out_path.parent.mkdir(parents=True, exist_ok=True)
    image.save(out_path)


def make_fullbar(slot_rows: dict[tuple[str, str], Path], icon_rows: dict[tuple[str, str, str], Path], ordered: list[tuple[str, str]], out_path: Path, size: tuple[int, int]) -> None:
    slot_size = 64
    image = draw_background(size, f"Batch 81 {VERSION} local HUD mockup - all 18 icons at 64px")
    add_status_gauges(image)
    draw = ImageDraw.Draw(image)
    width, height = size
    gap = max(8, min(18, (width - 120 - len(ordered) * slot_size) // max(1, len(ordered) - 1)))
    total_w = len(ordered) * slot_size + (len(ordered) - 1) * gap
    x = max(48, (width - total_w) // 2)
    y = height - 112
    states = ["ready", "selected", "cooldown_99", "disabled"]
    for index, (cat, skill) in enumerate(ordered):
        state = states[index % len(states)]
        frame_state = "cooldown" if state == "cooldown_99" else state
        icon_path = icon_rows[(cat, skill, "64x64")]
        slot = compose_slot(slot_rows[(frame_state, "64x64")], icon_path, state, slot_size)
        image.alpha_composite(slot, (x, y))
        if index % 6 == 0:
            draw.text((x, y - 20), cat, fill=(174, 202, 228, 255), font=load_font(11, bold=True))
        x += slot_size + gap
    out_path.parent.mkdir(parents=True, exist_ok=True)
    image.save(out_path)


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch80-dir", required=True, type=Path)
    parser.add_argument("--batch81-dir", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    args = parser.parse_args()

    icon_manifest = read_csv(args.batch80_dir / "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv")
    frame_manifest = read_csv(args.batch81_dir / f"thecat_ui_skill_slot_frames_batch81_manifest_{VERSION}.csv")
    icon_rows = {(row["cat"], row["skill"], row["size"]): Path(row["path"]) for row in icon_manifest}
    frame_rows = {(row["state"], row["size"]): Path(row["path"]) for row in frame_manifest if row["shape"] == "square"}

    ordered: list[tuple[str, str]] = []
    seen = set()
    for row in icon_manifest:
        if row["size"] == "64x64":
            key = (row["cat"], row["skill"])
            if key not in seen:
                seen.add(key)
                ordered.append(key)

    boards_dir = args.out_dir / "boards"
    slot_dir = args.out_dir / "slots_64"
    slot_dir.mkdir(parents=True, exist_ok=True)
    rows: list[dict[str, str]] = []

    for cat, skill in ordered:
        for state in STATES:
            frame_state = "cooldown" if state == "cooldown_99" else state
            path = slot_dir / f"{cat}_{skill}_{state}_64_hudmock_{VERSION}.png"
            compose_slot(frame_rows[(frame_state, "64x64")], icon_rows[(cat, skill, "64x64")], state, 64).save(path)
            rows.append(
                {
                    "cat": cat,
                    "skill": skill,
                    "state": state,
                    "slot_size": "64x64",
                    "path": path.as_posix(),
                    "status": "local_actual_scale_mockup_not_unity_screenshot",
                }
            )

    make_fullbar(frame_rows, icon_rows, ordered, boards_dir / "thecat_batch81_v002_light_hud_1920x1080_64px_fullbar_v001.png", (1920, 1080))
    make_fullbar(frame_rows, icon_rows, ordered, boards_dir / "thecat_batch81_v002_light_hud_1280x720_64px_fullbar_v001.png", (1280, 720))
    make_focus_matrix(frame_rows, icon_rows, boards_dir / "thecat_batch81_v002_light_hud_1920x1080_64px_focus_matrix_v001.png", (1920, 1080), 64)
    make_focus_matrix(frame_rows, icon_rows, boards_dir / "thecat_batch81_v002_light_hud_1280x720_64px_focus_matrix_v001.png", (1280, 720), 64)
    make_focus_matrix(frame_rows, icon_rows, boards_dir / "thecat_batch81_v002_light_hud_720x1280_64px_focus_matrix_v001.png", (720, 1280), 64)
    make_focus_matrix(frame_rows, icon_rows, boards_dir / "thecat_batch81_v002_light_hud_1920x1080_128px_focus_matrix_v001.png", (1920, 1080), 128)

    board_rows = []
    for board_path in sorted(boards_dir.glob("*.png")):
        with Image.open(board_path) as image:
            board_rows.append(
                {
                    "board": board_path.name,
                    "size": f"{image.width}x{image.height}",
                    "path": board_path.as_posix(),
                    "status": "local_hud_mockup_not_unity_screenshot",
                }
            )

    with (args.out_dir / "thecat_batch81_v002_light_actual_scale_hud_mockup_slots.csv").open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=["cat", "skill", "state", "slot_size", "path", "status"])
        writer.writeheader()
        writer.writerows(rows)
    with (args.out_dir / "thecat_batch81_v002_light_actual_scale_hud_mockup_boards.csv").open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=["board", "size", "path", "status"])
        writer.writeheader()
        writer.writerows(board_rows)

    note = args.out_dir / "thecat_batch81_v002_light_actual_scale_hud_mockup_note.md"
    note.write_text(
        "\n".join(
            [
                "# Batch 81 V002 Light Actual-Scale HUD Mockups",
                "",
                "Status: `local_mockup_not_unity_screenshot`",
                "",
                "These boards compose Batch 80 recommended skill icons with Batch 81 `v002_light` square slot frames at 64 px and 128 px. They are local visual evidence only and do not replace Unity Battle HUD screenshots.",
                "",
                "## Outputs",
                "",
                "- `slots_64/`: 18 icons x 4 states = 72 local slot composites.",
                "- `boards/`: full-bar and focus-matrix mockups at 1920x1080, 1280x720, and 720x1280.",
                "",
                "## Watch Items",
                "",
                "- Ready vs selected clarity on cyan-heavy icons.",
                "- Cooldown `99` near the upper-right badge at 64 px.",
                "- Disabled vs cooldown distinction.",
                "- No round slots, rejected icons, or starter-cat body art are used.",
            ]
        ),
        encoding="utf-8",
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
