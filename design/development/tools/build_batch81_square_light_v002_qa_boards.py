from __future__ import annotations

import argparse
import csv
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


DIGITS = ["1", "12", "99"]
VERSION = "v002_light"


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


def read_csv(path: Path) -> list[dict[str, str]]:
    with path.open("r", newline="", encoding="utf-8") as handle:
        return list(csv.DictReader(handle))


def fit(image: Image.Image, max_size: int) -> Image.Image:
    image = image.convert("RGBA")
    scale = min(max_size / image.width, max_size / image.height)
    size = (max(1, round(image.width * scale)), max(1, round(image.height * scale)))
    return image.resize(size, Image.Resampling.LANCZOS)


def paste_center(base: Image.Image, overlay: Image.Image, center: tuple[int, int]) -> None:
    base.alpha_composite(overlay, (center[0] - overlay.width // 2, center[1] - overlay.height // 2))


def compose(frame_path: Path, icon_path: Path, size: int, state: str) -> Image.Image:
    frame = Image.open(frame_path).convert("RGBA").resize((size, size), Image.Resampling.LANCZOS)
    icon_alpha = 0.56 if state == "disabled" else 1.0
    icon = fit(Image.open(icon_path).convert("RGBA"), round(size * 0.70))
    if icon_alpha < 1.0:
        alpha = icon.getchannel("A").point(lambda value: round(value * icon_alpha))
        icon.putalpha(alpha)
    paste_center(frame, icon, (size // 2, round(size * 0.52)))
    return frame


def draw_cooldown_digit(image: Image.Image, digit: str) -> None:
    draw = ImageDraw.Draw(image)
    font_size = 24 if len(digit) == 1 else 18
    font = load_font(font_size, bold=True)
    center = (103, 27)
    bbox = draw.textbbox((0, 0), digit, font=font, stroke_width=1)
    width = bbox[2] - bbox[0]
    height = bbox[3] - bbox[1]
    position = (center[0] - width / 2, center[1] - height / 2 - 1)
    draw.text(position, digit, font=font, fill=(255, 239, 168, 255), stroke_width=2, stroke_fill=(19, 28, 47, 255))


def row_index(rows: list[dict[str, str]], keys: tuple[str, ...]) -> dict[tuple[str, ...], dict[str, str]]:
    return {tuple(row[key] for key in keys): row for row in rows}


def ordered_icons(rec_rows: list[dict[str, str]]) -> list[tuple[str, str]]:
    ordered: list[tuple[str, str]] = []
    seen = set()
    for row in rec_rows:
        if row["size"] == "256x256":
            key = (row["cat"], row["skill"])
            if key not in seen:
                seen.add(key)
                ordered.append(key)
    return ordered


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--batch80-dir", required=True, type=Path)
    parser.add_argument("--batch81-dir", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    parser.add_argument("--digit-out-dir", required=True, type=Path)
    args = parser.parse_args()

    rec_rows = read_csv(args.batch80_dir / "recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv")
    v002_rows = read_csv(args.batch81_dir / f"thecat_ui_skill_slot_frames_batch81_manifest_{VERSION}.csv")
    rec = row_index(rec_rows, ("cat", "skill", "size"))
    frames = row_index(v002_rows, ("shape", "state", "size"))
    ordered = ordered_icons(rec_rows)

    args.out_dir.mkdir(parents=True, exist_ok=True)
    for subdir in ("ready", "cooldown", "disabled", "selected"):
        (args.out_dir / subdir).mkdir(parents=True, exist_ok=True)

    qa_rows: list[dict[str, str]] = []
    board = Image.new("RGBA", (6 * 216 + 64, 3 * 242 + 104), (13, 19, 31, 255))
    draw = ImageDraw.Draw(board)
    draw.rectangle((0, 0, board.width, 58), fill=(29, 43, 68, 255))
    draw.text((24, 19), "Batch 81 v002_light square slot fit: ready / cooldown / disabled / selected", fill=(226, 238, 255, 255), font=load_font(16))
    small = load_font(11)

    for index, (cat, skill) in enumerate(ordered):
        icon_path = Path(rec[(cat, skill, "256x256")]["path"])
        version = rec[(cat, skill, "256x256")]["selected_source_version"]
        outputs = {}
        for state in ("ready", "cooldown", "disabled", "selected"):
            frame_path = Path(frames[("square", state, "128x128")]["path"])
            composite = compose(frame_path, icon_path, 128, state)
            outputs[state] = composite
            directory = args.out_dir / state
            path = directory / f"{cat}_{skill}_{state}_{VERSION}.png"
            composite.save(path)
            qa_rows.append(
                {
                    "cat": cat,
                    "skill": skill,
                    "selected_icon_version": version,
                    "slot_state": f"square_{state}",
                    "path": path.as_posix(),
                    "status": "slot_fit_pending_visual_review",
                    "notes": "Batch 81 square-only lighter ornament v002 candidate; not a Unity screenshot.",
                }
            )

        col = index % 6
        row = index // 6
        x = 32 + col * 216
        y = 82 + row * 242
        panel = (25, 38, 61, 255) if row != 1 else (220, 232, 238, 255)
        label = (226, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 168, y + 194), radius=8, fill=panel, outline=(80, 112, 142, 255), width=1)
        positions = {
            "ready": (x, y + 4),
            "cooldown": (x + 86, y + 4),
            "disabled": (x, y + 88),
            "selected": (x + 86, y + 88),
        }
        for state, pos in positions.items():
            board.alpha_composite(outputs[state].resize((72, 72), Image.Resampling.LANCZOS), pos)
        draw.text((x, y + 166), f"{cat}/{version}", fill=label, font=small)

    board.save(args.out_dir / f"thecat_ui_skill_slot_frames_batch81_icon_fit_board_{VERSION}.png")
    with (args.out_dir / f"thecat_ui_skill_slot_frames_batch81_icon_fit_report_{VERSION}.csv").open(
        "w", newline="", encoding="utf-8"
    ) as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["cat", "skill", "selected_icon_version", "slot_state", "path", "status", "notes"],
        )
        writer.writeheader()
        writer.writerows(qa_rows)

    args.digit_out_dir.mkdir(parents=True, exist_ok=True)
    for digit in DIGITS:
        (args.digit_out_dir / f"digit_{digit}").mkdir(parents=True, exist_ok=True)

    digit_rows: list[dict[str, str]] = []
    cooldown_slot = Image.open(frames[("square", "cooldown", "128x128")]["path"]).convert("RGBA")
    digit_board = Image.new("RGBA", (6 * 216 + 64, 3 * 242 + 104), (13, 19, 31, 255))
    draw = ImageDraw.Draw(digit_board)
    draw.rectangle((0, 0, digit_board.width, 58), fill=(29, 43, 68, 255))
    draw.text((24, 19), "Batch 81 v002_light square cooldown digit test: 1 / 12 / 99", fill=(226, 238, 255, 255), font=load_font(16))

    for index, (cat, skill) in enumerate(ordered):
        icon_path = Path(rec[(cat, skill, "256x256")]["path"])
        icon = Image.open(icon_path).convert("RGBA")
        version = rec[(cat, skill, "256x256")]["selected_source_version"]
        composites = []
        for digit in DIGITS:
            composite = cooldown_slot.copy().resize((128, 128), Image.Resampling.LANCZOS)
            icon_img = fit(icon, 84)
            paste_center(composite, icon_img, (64, 66))
            draw_cooldown_digit(composite, digit)
            path = args.digit_out_dir / f"digit_{digit}" / f"{cat}_{skill}_cd{digit}_{VERSION}.png"
            composite.save(path)
            composites.append((digit, composite, path))
            digit_rows.append(
                {
                    "cat": cat,
                    "skill": skill,
                    "selected_icon_version": version,
                    "digit": digit,
                    "path": path.as_posix(),
                    "status": "cooldown_digit_mockup_pending_visual_review",
                    "notes": "Batch 81 v002_light candidate-only cooldown digit mockup; not a Unity screenshot.",
                }
            )

        col = index % 6
        row = index // 6
        x = 32 + col * 216
        y = 82 + row * 242
        panel = (25, 38, 61, 255) if row != 1 else (220, 232, 238, 255)
        label = (226, 238, 255, 255) if row != 1 else (20, 34, 45, 255)
        draw.rounded_rectangle((x - 8, y - 8, x + 168, y + 194), radius=8, fill=panel, outline=(80, 112, 142, 255), width=1)
        for n, (digit, composite, _) in enumerate(composites):
            digit_board.alpha_composite(composite.resize((62, 62), Image.Resampling.LANCZOS), (x + n * 52, y + 10))
            draw.text((x + n * 52 + 23, y + 76), digit, fill=label, font=small)
        draw.text((x, y + 154), f"{cat}/{version}", fill=label, font=small)
        draw.text((x, y + 170), skill[:24], fill=label, font=small)

    digit_board.save(args.digit_out_dir / f"thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_{VERSION}.png")
    with (args.digit_out_dir / f"thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_report_{VERSION}.csv").open(
        "w", newline="", encoding="utf-8"
    ) as handle:
        writer = csv.DictWriter(
            handle,
            fieldnames=["cat", "skill", "selected_icon_version", "digit", "path", "status", "notes"],
        )
        writer.writeheader()
        writer.writerows(digit_rows)

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
